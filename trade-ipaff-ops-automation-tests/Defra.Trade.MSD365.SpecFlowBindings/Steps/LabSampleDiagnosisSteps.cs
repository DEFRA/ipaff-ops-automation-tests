namespace Defra.Trade.Plants.Specs.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.BusinessLogic.ReferenceData;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Extensions;
using Defra.Trade.Plants.SpecFlowBindings.Steps;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using Reqnroll;

/// <summary>
/// Lab Sample Diagnosis API steps.
/// </summary>
[Binding]
public class LabSampleDiagnosisSteps : PowerAppsStepDefiner
{
    private readonly SessionContext sessionContext;
    private readonly ScenarioContext scenarioContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="LabSampleDiagnosisSteps"/> class.
    /// </summary>
    /// <param name="sessionContext">SessionContext.</param>
    /// <param name="scenarioContext">ScenarioContext.</param>
    public LabSampleDiagnosisSteps(SessionContext sessionContext, ScenarioContext scenarioContext)
    {
        this.sessionContext = sessionContext;
        this.scenarioContext = scenarioContext;
    }

    [Given(@"I create and submit an import lab sample as '(.*)' for '(.*)' of '(.*)'")]
    public void GivenICreateAndSubmitAnImportLabSampleFor(string labSampleAlias, string commodityAlias, string application)
    {
        var appEntityReference = this.sessionContext.GetEntityReference(application);
        var workOrder = this.sessionContext.GetEntityReference(SpecflowBindingsConstants.WorkOrderAlias);
        var commodity = this.sessionContext.GetEntityReference(commodityAlias);
        var idAndPhysicalServiceTask = new ImportSteps(this.sessionContext).GetServiceTask(appEntityReference).Single(p => p.msdyn_TaskType.Id == ServiceTaskType.IdentityAndPhysicalCheck).ToEntityReference();
        using (var svc = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svc))
        {
            var id = this.CreateImportLabSample(workOrder, commodity, idAndPhysicalServiceTask, svc, context, labSampleAlias);
            this.CompleteImportLabSample(id, context);
            this.UpdateLabSampleStatus(id, trd_labsample_statuscode.ReadytoSubmit, context);
            this.UpdateLabSampleStatus(id, trd_labsample_statuscode.Submitted, context);
        }
    }

    [Given(@"Lab sample diagnosis information returned by Fera for '(.*)' as '(.*)'")]
    public void GivenLabSampleDiagnosisInformationReturnedByFeraForAs(string labSampleAlias, string labSampleDiagnosisAlias)
    {
        using (var svc = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svc))
        {
            var workOrder = this.sessionContext.GetEntityReference(SpecflowBindingsConstants.WorkOrderAlias);
            var ownerId = svc.Retrieve(workOrder.LogicalName, workOrder.Id, new ColumnSet("ownerid")).GetAttributeValue<EntityReference>("ownerid");
            var sample = new trd_labsample
            {
                Id = this.sessionContext.GetEntityReference(labSampleAlias).Id,
            };

            new LabSampleDiagnosisSteps(this.sessionContext, this.scenarioContext).CreateLabSampleDiagnosis(context, trd_labsamplediagnosis_statuscode.ActionRecommendationResponseReadyToSubmit, sample, workOrder, ownerId, labSampleDiagnosisAlias);
        }
    }

    /// <summary>
    /// A given step for setting up a lab sampe diagnosis for a given application.
    /// Note this step requires inspection results on an application.
    /// </summary>
    /// <param name="applicationAlias">The alias of the application to set up lab samples for.</param>
    [Given("a lab sample diagnosis has been received for '(.*)'")]
    public void WhenLabSampleDiagnosisCreated(string applicationAlias)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);
        using (var svc = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svc))
        {
            var workOrder = WorkOrderSteps.GetWorkOrder(svc, application);

            var inspectionResults = svc.WaitForRecords(
                new QueryByAttribute(trd_inspectionresult.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(true),
                    Attributes = { "trd_workorder" },
                    Values = { workOrder.Id },
                },
                SpecflowBindingsConstants.DefaultWaitTime,
                SpecflowBindingsConstants.DefaultRetryInterval,
                false).Entities.Select(x => x.ToEntity<trd_inspectionresult>()).ToList();

            context.ClearChanges();

            this.CreateExportLabSampleDiagnosis(context, inspectionResults, 1);
        }
    }

    [Given(@"I wait for the system to generate (.*) inspection results in the background for the workorder")]
    public void GivenIWaitForTheSystemToGenerateInspectionResultsInTheBackgroundFor(int expectedCount)
    {
        var workOrder = this.sessionContext.GetEntityReference(SpecflowBindingsConstants.WorkOrderAlias);
        Policy
            .Handle<Exception>()
            .WaitAndRetry(SpecflowBindingsConstants.ApiRetryAttempts, retryAttempt => SpecflowBindingsConstants.ApiSleepDuration)
            .Execute(() =>
            {
                using (var sv = SessionContext.GetServiceClient())
                using (var plantsContext = new PlantsContext(sv))
                {
                    var ownerId = sv.Retrieve(workOrder.LogicalName, workOrder.Id, new ColumnSet("ownerid")).GetAttributeValue<EntityReference>("ownerid");
                    var inspectionresults = (from t in plantsContext.trd_inspectionresultSet where t.trd_WorkOrder.Id == workOrder.Id select t).ToList();
                    inspectionresults.Count.Should().Be(expectedCount);
                }
            });
    }

    [Given(@"I wait for the system to assign (.*) inspection results to me")]
    public void GivenIWaitForTheSystemToAssignTheInspectionResultsToMe(int count)
    {
        var workOrder = this.sessionContext.GetEntityReference(SpecflowBindingsConstants.WorkOrderAlias);
        Policy
            .Handle<Exception>()
            .WaitAndRetry(SpecflowBindingsConstants.ApiRetryAttempts, retryAttempt => SpecflowBindingsConstants.ApiSleepDuration)
            .Execute(() =>
            {
                using (var sv = SessionContext.GetServiceClient())
                using (var plantsContext = new PlantsContext(sv))
                {
                    var inspectionresults = (from t in plantsContext.trd_inspectionresultSet where t.trd_WorkOrder.Id == workOrder.Id && t.OwnerId.Id == this.sessionContext.UserId select t).ToList();
                    inspectionresults.Count.Should().Be(count);
                }
            });
    }

    /// <summary>
    /// A given step for setting up a lab sampe diagnosis for a given application with a action recommendation.
    /// Note this step requires inspection results on an application.
    /// </summary>
    /// <param name="applicationAlias">The alias of the application to set up lab samples diagnosis for.</param>
    [Given("an action recommendation has been received for '(.*)'")]
    public void WhenAnActionRecommendationHasBeenReceived(string applicationAlias)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);
        var workOrder = this.sessionContext.GetEntityReference(SpecflowBindingsConstants.WorkOrderAlias);

        Policy
            .Handle<Exception>()
            .WaitAndRetry(SpecflowBindingsConstants.ApiRetryAttempts, retryAttempt => SpecflowBindingsConstants.ApiSleepDuration)
            .Execute(() =>
            {
                using (var sv = SessionContext.GetServiceClient())
                using (var plantsContext = new PlantsContext(sv))
                {
                    var ownerId = sv.Retrieve(workOrder.LogicalName, workOrder.Id, new ColumnSet("ownerid")).GetAttributeValue<EntityReference>("ownerid");
                    var inspectionresults = (from t in plantsContext.trd_inspectionresultSet where t.trd_WorkOrder.Id == workOrder.Id select t).ToList();
                    inspectionresults.Count.Should().BeGreaterThan(0);
                    var inspectionResultsOwnedByWorkOrder = inspectionresults.Count(p => p.OwnerId.Id != ownerId.Id);
                    inspectionResultsOwnedByWorkOrder.Should().Be(0, $"Expected unowned service tasks count should be 0");
                }
            });

        using (var svc = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svc))
        {
            var inspectionResults = svc.WaitForRecords(
                new QueryByAttribute(trd_inspectionresult.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(true),
                    Attributes = { "trd_workorder" },
                    Values = { workOrder.Id },
                },
                SpecflowBindingsConstants.DefaultWaitTime,
                SpecflowBindingsConstants.DefaultRetryInterval,
                false).Entities.Select(x => x.ToEntity<trd_inspectionresult>()).ToList();

            context.ClearChanges();

            CreateExportLabSampleDiagnosis(context, inspectionResults, 1, trd_labsamplediagnosis_statuscode.ActionRecommendationsReceived);
        }
    }

    /// <summary>
    /// A given step that opens a lab sample diagnosis record for a given application.
    /// </summary>
    /// <param name="applicationAlias">The alias of the application to set up lab samples for.</param>
    [Given("I have opened a lab sample diagnosis for '(.*)'")]
    [When("I have opened a lab sample diagnosis for '(.*)'")]
    public void GivenIOpenLabSampleDiagnosis(string applicationAlias)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);

        //if(application.LogicalName==trd)

        this.CreateLabSampleDiagForExportApplication(application);
    }

    private void CreateLabSampleDiagForExportApplication(EntityReference application)
    {
        using (var svc = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svc))
        {
            var workOrder = WorkOrderSteps.GetWorkOrder(svc, application);
            var sampleDiagnosis = svc.WaitForRecords(
                new QueryByAttribute(trd_labsamplediagnosis.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(false),
                    Attributes = { "trd_workorderid" },
                    Values = { workOrder.Id },
                },
                SpecflowBindingsConstants.DefaultWaitTime,
                SpecflowBindingsConstants.DefaultRetryInterval,
                false).Entities.Select(x => x.ToEntity<trd_labsamplediagnosis>()).ToList().FirstOrDefault();

            context.ClearChanges();

            XrmApp.Entity.OpenEntity("trd_labsamplediagnosis", sampleDiagnosis.Id);

            SharedSteps.WaitForScriptProcessing();
        }
    }

    private void CreateExportLabSampleDiagnosis(PlantsContext context, List<trd_inspectionresult> inspectionResults, int sampleCount, trd_labsamplediagnosis_statuscode status = trd_labsamplediagnosis_statuscode.ReadytoReview)
    {
        var workOrder = inspectionResults[0].trd_WorkOrder;
        var ownerId = inspectionResults[0].OwnerId;
        var osvResult = trd_labsample_statuscode.ReadytoSubmit;
        var sampleState = trd_labsampleState.Inactive;

        for (int i = 0; i < sampleCount; i++)
        {
            var sample = new trd_labsample
            {
                trd_PercentageShowingSymptoms = 100,
                trd_ChargeTypeCodeId = new EntityReference("trd_chargetypecode", ChargeTypeCode.ExportsLabSamples),
                trd_DateSampleSent = new DateTime(2020, 1, 1, 0, 0, 0),
                trd_LabSite = trd_labsample_trd_labsite.FeraLaboratoryOnly,
                trd_numberofphysicalsamples = 1,
                trd_sampleimportance = false,
                trd_SeedHealthTestRequired = false,
                trd_SeedOrobTestRequired = false,
                trd_SeedOtherTestRequired = false,
                trd_SeedWeedTestRequired = false,
                trd_WorkOrder = workOrder,
                trd_consignmentitemid = inspectionResults[i].trd_ConsignmentItemId,
                trd_WorkOrderServiceTask = inspectionResults[i].trd_WorkOrderServiceTask,
                trd_SuspectPestDescription = "Unknown Pests",
                trd_PestAliveDuringInspection = trd_labsample_trd_pestaliveduringinspection.Unknown,
                statuscode = osvResult,
                statecode = sampleState,
                OwnerId = ownerId,
            };

            context.AddObject(sample);
            context.SaveChanges();

            var inspectionresult = new trd_inspectionresult
            {
                Id = inspectionResults[i].Id,
                trd_LabSample = sample.ToEntityReference(),
                trd_QuantityInspected = 100,
            };

            if (!context.IsAttached(inspectionresult))
            {
                context.Attach(inspectionresult);
            }

            context.UpdateObject(inspectionresult);

            this.CreateLabSampleDiagnosis(context, status, sample, workOrder, ownerId);
        }

        context.ClearChanges();
    }

    public void CreateLabSampleDiagnosis(PlantsContext context, trd_labsamplediagnosis_statuscode status,
        trd_labsample sample, EntityReference workOrder, EntityReference ownerId, string alias = null)
    {
        var sampleDiagnosis = new trd_labsamplediagnosis
        {
            trd_labsampleid = sample.ToEntityReference(), 
            trd_WorkOrderId = workOrder,
            OwnerId = ownerId,
            statuscode = status,
            trd_labdiagnosisid = Faker.RandomNumber.Next(1000000, 214748364).ToString(),
            trd_diag_sample_reference = Faker.RandomNumber.Next(1000000, 214748364),
            trd_diagnosis =
                "Provisional - Tests on-going:  Provisional: This sample is currently undergoing extraction for plant-parasitic nematodes. Results to follow.",
            trd_diagnosisdate = DateTime.Now,
            trd_diagnosistype = trd_labsamplediagnosis_trd_diagnosistype.Final,
            trd_pestid = new EntityReference("trd_pest", Pest.UnknownPest),
            trd_diagnostician = Faker.Name.FullName(),
            trd_diagnosissubteam = trd_labsamplediagnosis_trd_diagnosissubteam.Nematology,
            trd_estimatedreturndate = DateTime.Now,
            trd_NonPestDiagnosisDescription = "No Pest Yet",
            trd_actionrecommendationid = Faker.RandomNumber.Next(1000000, 214748364),
            trd_actionrecommendations = trd_labsamplediagnosis_trd_actionrecommendations.StatutoryActionrequired,
            trd_actionrecommendation = "Test has been completdd and follow the Hand book Instruction",
            trd_actionrecommendationauthor = Faker.Name.FullName(),
            trd_actionrecommendationdate = DateTime.Now,
            trd_ActionRecommendationResponseDateWritten = DateTime.UtcNow,
            trd_actionrecommendationresponsewrittenby = ownerId,
        };
        context.AddObject(sampleDiagnosis);
        context.SaveChanges();

        if (alias != null)
        {
            this.sessionContext.EntityReferences.Add(alias + this.sessionContext.SessionId, sampleDiagnosis.ToEntityReference());
        }
    }

    private void UpdateLabSampleStatus(Guid id, trd_labsample_statuscode status, PlantsContext context)
    {
        var updateSample = new trd_labsample
        {
            Id = id,
            statuscode = status,
        };

        if (!context.IsAttached(updateSample))
        {
            context.Attach(updateSample);
        }

        context.UpdateObject(updateSample);
        context.SaveChanges();
    }

    private Guid CreateImportLabSample(EntityReference workOrder, EntityReference commodity, EntityReference idAndPhysicalServiceTask, CrmServiceClient svc, PlantsContext context, string sampleAlias)
    {
        var sample = new trd_labsample
        {
            trd_ChargeTypeCodeId = new EntityReference("trd_chargetypecode", ChargeTypeCode.ImportsLabSamples),
            trd_WorkOrder = workOrder,
            trd_ImportCommodityLineId = commodity,
            trd_WorkOrderServiceTask = idAndPhysicalServiceTask,
            trd_PestAliveDuringInspection = trd_labsample_trd_pestaliveduringinspection.Unknown,
            statuscode = trd_labsample_statuscode.Draft,
            statecode = trd_labsampleState.Active,
            OwnerId = svc.Retrieve(workOrder.LogicalName, workOrder.Id, new ColumnSet("ownerid")).GetAttributeValue<EntityReference>("ownerid"),
        };

        context.AddObject(sample);
        context.SaveChanges();
        this.sessionContext.EntityReferences.Add(sampleAlias + this.sessionContext.SessionId, sample.ToEntityReference());
        return sample.Id;
    }

    private void CompleteImportLabSample(Guid id, PlantsContext context)
    {
        var pests = new List<EntityReference>
        {
            new EntityReference("trd_pest", Pest.DefaultTestPest),
        };

        var pestsToAssociate = new EntityReferenceCollection(pests);
        var associateRequest = new AssociateRequest
        {
            Target = new EntityReference(trd_labsample.EntityLogicalName, id),
            RelatedEntities = pestsToAssociate,
            Relationship = new Relationship("trd_trd_labsample_trd_pest"),
        };

        context.Execute(associateRequest);
        var sample = new trd_labsample
        {
            Id = id,
            trd_PercentageShowingSymptoms = 100,
            trd_SampleReason = trd_labsample_trd_samplereason.SuspicionofHarmfulOrganism,
            trd_DateSampleSent = DateTime.Today.Date,
            trd_LabSite = trd_labsample_trd_labsite.FeraLaboratoryOnly,
            trd_numberofphysicalsamples = 1,
            trd_SuspectPestDescription = "Unknown Pests",
            trd_PestAliveDuringInspection = trd_labsample_trd_pestaliveduringinspection.Unknown,
            statuscode = trd_labsample_statuscode.Created,
            statecode = trd_labsampleState.Inactive,
        };

        if (!context.IsAttached(sample))
        {
            context.Attach(sample);
        }

        context.UpdateObject(sample);
        context.SaveChanges();
    }
}
