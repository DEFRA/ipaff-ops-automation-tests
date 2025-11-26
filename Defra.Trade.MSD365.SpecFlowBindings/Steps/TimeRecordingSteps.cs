namespace Defra.Trade.Plants.Specs.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Steps;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Polly;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Reqnroll;

/// <summary>
/// Time Recording reusable steps.
/// </summary>
[Binding]
public class TimeRecordingSteps : PowerAppsStepDefiner
{
    private SessionContext ctx;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeRecordingSteps"/> class.
    /// </summary>
    /// <param name="context">SessionContext.</param>
    public TimeRecordingSteps(SessionContext context)
    {
        this.ctx = context;
    }

    /// <summary>
    /// Enters submitted time for all work order tasks.
    /// </summary>
    [Given(@"I enter and submit time for all service tasks")]
    public void WhenICreatePreInspectionTimeEntriesForWorkOrder()
    {
        if (XrmApp.Entity.GetEntityName() != "msdyn_workorder")
        {
            throw new Exception($"You must first navigate to the work order");
        }

        var id = XrmApp.Entity.GetObjectId();

        // TODO: auth as logged in user
        using (var context = new PlantsContext(SessionContext.GetServiceClient()))
        {
            var tasks = context.msdyn_workorderservicetaskSet.Where(x => x.msdyn_WorkOrder.Id == id);
            var workOrder = context.msdyn_workorderSet.Where(x => x.Id == id).First();

            tasks.ToList().ForEach(t =>
            {
                context.AddObject(new trd_timerecording
                {
                    Id = Guid.NewGuid(),
                    trd_TrainingProvidedtoTrade = 1,
                    trd_TrainingProvidedtoInspectors = 1,
                    trd_TrainingReceived = 1,
                    trd_Date = DateTime.UtcNow,
                    trd_Travel = 1,
                    statuscode = trd_timerecording_statuscode.Submitted,
                    statecode = trd_timerecordingState.Inactive,
                    trd_Admin = 1,
                    trd_Inspection = 1,
                    trd_msdyn_workorderservicetask_trd_timerecording_WorkOrderServiceTask = t,
                    trd_bookableresource_trd_timerecording_BookableResource = context.BookableResourceSet.First(),
                    trd_msdyn_workorder_trd_timerecording_WorkerOrder = workOrder,
                });
            });

            context.SaveChanges();
        }
    }

    [When("I enter time for (.*)")]
    [When("I create a time record for (.*)")]
    public void WhenIEnterTimeAgainst(string inspectorName)
    {
        inspectorName = this.GetInspectorName(inspectorName);
        var timeRecordCount = this.GetTimeRecordCount(inspectorName);
        Policy
            .Handle<Exception>()
            .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(1))
            .Execute(() =>
            {
                var act = new Actions(Driver);
                var inspector = Driver.WaitUntilAvailable(By.ClassName("InspectorPickerControl"));
                var selectInput = inspector.WaitUntilAvailable(By.XPath(".//input[starts-with(@id, 'react-select-')]"));
                selectInput.Clear();
                act.Click(selectInput).Build().Perform();
                selectInput.SendKeys(inspectorName);
                Driver.WaitForTransaction();
                selectInput.SendKeys(Keys.Enter);
                Driver.WaitForTransaction();
                this.WaitForTimeRecordCount(inspectorName, timeRecordCount);
                inspector.FindElement(By.TagName("svg")).Click();
                Driver.WaitForTransaction();
            });
    }

    public string GetInspectorName(string inspectorName)
    {
        if (inspectorName == "Customer Test User 4")
            return inspectorName;
        if (inspectorName == "any available resource")
        {
            var context = new PlantsContext(SessionContext.GetServiceClient());
            inspectorName = (from br in context.BookableResourceSet join u2 in context.SystemUserSet on br.OwningBusinessUnit.Id equals u2.BusinessUnitId.Id where u2.SystemUserId == this.ctx.UserId select br.Name).First();
        }
        else
        {
            try
            {
                // first try and resolve from test config
                var config = TestConfig.GetUser(inspectorName);
                var context = new PlantsContext(SessionContext.GetServiceClient());
                inspectorName = (from u in context.SystemUserSet where u.InternalEMailAddress == config.Username select u.FullName).FirstOrDefault();
            }
            catch
            {
                // else look for test data
                var entityRef = this.ctx.GetEntityReference(inspectorName);
                entityRef.Should().NotBeNull($"no test data setup exists for {inspectorName}");
                inspectorName = SessionContext.GetServiceClient()
                    .Retrieve(entityRef.LogicalName, entityRef.Id, new ColumnSet("name")).GetAttributeValue<string>("name");
            }
        }

        return inspectorName;
    }

    public void SubmitTimeEntry(string gridName, int recordIndex, bool submit, Table table)
    {
        foreach (var row in table.Rows)
        {
            if (row.ContainsKey("Inspector"))
            {
                this.WhenIEnterTimeAgainst(row["Inspector"]);
            }

            SharedSteps.WaitForScriptProcessing();

            for (var i = 0; i < table.Header.Count; i++)
            {
                var columnHeader = table.Header.ToList()[i];
                if (columnHeader == "Inspector")
                {
                    continue;
                }

                GridSteps.WhenISelectRowInTheGrid(recordIndex, gridName);
                GridSteps.WhenIEnterIntoTheCellForRowInTheGrid(row[i], columnHeader, recordIndex, gridName, string.Empty);
            }

            if (submit)
            {
                GridSteps.WhenISelectRowInTheGrid(recordIndex, gridName);
                new GridPageSteps().WhenIClickOnSubmitTimeOnSubgrid();
                DialogSteps.WhenIClickTheAlertButtonOnTheDialog("confirm");
                SharedSteps.WaitForScriptProcessing();
            }

            recordIndex++;
        }
    }

    private void WaitForTimeRecordCount(string inspectorName, int existingTimeRecordCount)
    {
        var timer = new Stopwatch();
        timer.Start();
        do
        {
            var newRecordCount = this.GetTimeRecordCount(inspectorName);
            if (newRecordCount > 0 || existingTimeRecordCount == 1)
            {
                return;
            }

            if (timer.Elapsed.Duration() > TimeSpan.FromSeconds(15))
            {
                throw new TimeoutException("Unable to find the time record entry");
            }

            Thread.Sleep(TimeSpan.FromSeconds(5));
        }
        while (true);
    }

    private int GetTimeRecordCount(string inspectorName)
    {
        var entityId = XrmApp.Entity.GetObjectId();
        using (var context = new PlantsContext(SessionContext.GetServiceClient()))
        {
            var timeRecordings = (from t in context.trd_timerecordingSet join br in context.BookableResourceSet on t.trd_BookableResource.Id equals br.BookableResourceId where (t.trd_WorkOrderServiceTask.Id == entityId || t.trd_PHIMActionId.Id == entityId || t.trd_PHIMNotificationId.Id == entityId) && br.Name == inspectorName select t).ToList();
            var count = timeRecordings.Count;
            Console.WriteLine($"Time record count for {inspectorName}: {count}");
            return count;
        }
    }
}