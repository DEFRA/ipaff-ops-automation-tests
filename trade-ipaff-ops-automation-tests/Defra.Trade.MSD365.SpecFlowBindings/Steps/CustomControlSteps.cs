namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Reqnroll;
using Entity = Microsoft.Xrm.Sdk.Entity;

[Binding]
public class CustomControlSteps : PowerAppsStepDefiner
{
    private readonly SessionContext sessionContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomControlSteps"/> class.
    /// </summary>
    /// <param name="ctx">The test session context object.</param>
    public CustomControlSteps(SessionContext ctx)
    {
        this.sessionContext = ctx;
    }

    [When(@"I input drawing data into the '(.*)' pen control on the '(.*)' form")]
    public void WhenIInputDrawingDataIntoThePenControlOnTheForm(string fieldLogicalName, string formDisplayName)
    {
        var formContext = FormHelper.GetFormContext(Driver);
        CustomControlHelper.InputToPenControl(formContext, Driver, fieldLogicalName);
    }

    [Then(@"There (should|should not) be drawing data saved in the '(.*)' field for the (.*) record")]
    public void ThenThereShouldShouldNotBeDrawingDataSavedInTheField(string shouldShouldNot, string fieldLogicalName, string alias)
    {
        var recordId = this.sessionContext.GetEntityReference(alias).Id;

        using (var service = new CdsWebApiService(TestConfig.GetTestUrl(), AccessToken))
        {
            var response = service.Get("trd_inspectorprofiles", $"{fieldLogicalName}", $"trd_inspectorprofileid eq '{recordId}'");
            var content = (JArray)response["value"];
            var signatureValue = content[0][fieldLogicalName].ToString();

            if (shouldShouldNot == "should")
            {
                signatureValue.Should().NotBeNullOrEmpty();
            }
            else if (shouldShouldNot == "should not")
            {
                signatureValue.Should().BeNullOrEmpty();
            }

        }
    }
}
