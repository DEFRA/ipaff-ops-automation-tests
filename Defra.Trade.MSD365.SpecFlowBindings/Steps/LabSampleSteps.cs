// <copyright file="LabSampleSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.Specs.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Capgemini.PowerApps.SpecFlowBindings.Steps;
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
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using Reqnroll;

/// <summary>
/// LabSample Api Steps.
/// </summary>
[Binding]
public class LabSampleSteps : PowerAppsStepDefiner
{
    private readonly SessionContext sessionContext;
    private readonly ScenarioContext scenarioContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="LabSampleSteps"/> class.
    /// </summary>
    /// <param name="sessionContext">SessionContext.</param>
    /// <param name="scenarioContext">ScenarioContext.</param>
    public LabSampleSteps(SessionContext sessionContext, ScenarioContext scenarioContext)
    {
        this.sessionContext = sessionContext;
        this.scenarioContext = scenarioContext;
    }

    [Given(@"the system submits the lab sample")]
    [When(@"the system submits the lab sample")]
    public void WhenLabSampleSubmitted()
    {
        XrmApp.Entity.GetEntityName().Should().Be(trd_labsample.EntityLogicalName);
        var labSample = new EntityReference(trd_labsample.EntityLogicalName, XrmApp.Entity.GetObjectId());

        using (var plantsContext = new PlantsContext(SessionContext.GetServiceClient()))
        {
            var commodity = plantsContext.trd_labsampleSet.Where(st => st.Id == labSample.Id)
                .Select(st => new trd_labsample
                {
                    Id = st.Id,
                    statuscode = st.statuscode,
                }).Single();

            commodity.statuscode = trd_labsample_statuscode.Submitted;
            plantsContext.UpdateObject(commodity);
            plantsContext.SaveChanges();
        }

        ApplicationSteps.OpenEntity(labSample);
        new FormSteps(this.sessionContext, this.scenarioContext).WaitForValue(((int)trd_labsample_statuscode.Submitted).ToString(), "statuscode", 30);
    }

    /// <summary>
    /// Validates that the lab sample status is the expected status, with an alternative assertion for environments with no Fera integration.
    /// </summary>
    /// <param name="expectedStatus">Status to be asserted against.</param>
    [Then(@"labsample status is '(.*)'")]
    public void ThenLabsampleStatusIs(trd_labsample_statuscode expectedStatus)
    {
        XrmApp.Entity.GetEntityName().Should().Be("trd_labsample");
        new FormSteps(this.sessionContext, this.scenarioContext).WaitForValue(((int)expectedStatus).ToString(), "statuscode", 30);
    }

    /// <summary>
    /// create a Lab Sample Diagnosis record.
    /// </summary>
    /// <param name="labDiagnosisIdKey">labDiagnosisIdKey variableName.</param>
    [When(@"Fera responded lab sample diagnosis with a random generated Latest Diagnosis ID as '(.*)'")]
    public void WhenFeraRespondedLabSampleDiagnosis(string labDiagnosisIdKey)
    {
        const string labSampleEntityLogicalName = "trd_labsample";

        Policy
           .Handle<Exception>()
           .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(10))
           .Execute(() =>
           {
               // TODO: replace with S2S user used by Fera
               using (var plantsContext = new PlantsContext(
                   SessionContext.GetServiceClient()))
               {
                   var labDiagnosisId = string.Concat("E2E", DateTime.Now.Ticks);
                   var actionRecommendationId = this.sessionContext.RandomNumber();
                   var labSampleDiagnosis = new trd_labsamplediagnosis
                   {
                       trd_labdiagnosisid = labDiagnosisId,
                       trd_labsampleid = new EntityReference(labSampleEntityLogicalName,
                           XrmApp.Entity.GetObjectId()),
                       trd_diagnosistype = trd_labsamplediagnosis_trd_diagnosistype.Provisional,
                       trd_diagnosisdate = DateTime.Now,
                       trd_diagnostician = "Paul Taylor",
                       trd_diagnosissubteam = trd_labsamplediagnosis_trd_diagnosissubteam.Nematology,
                       trd_actionrecommendations =
                           trd_labsamplediagnosis_trd_actionrecommendations.StatutoryActionrequired,
                       trd_actionrecommendationid = actionRecommendationId,
                       trd_actionrecommendationdate = DateTime.Now,
                       trd_actionrecommendationauthor = "Test Author",
                       trd_actionrecommendation = "Test Action Recommendation Description",
                   };

                   var createSample = new CreateRequest
                   {
                       Target = labSampleDiagnosis,
                   };
                   plantsContext.Execute(createSample);
                   this.sessionContext.SetVariable(labDiagnosisIdKey, labDiagnosisId);
               }
           });
    }

    [Then(@"I can see a labsample message sent to Fera")]
    public void ThenICanSeeALabsampleMessageSentToFera()
    {
        RelatedGridSteps.WhenIOpenTheRelatedTab("Fera Outbound Messages");
        XrmApp.Grid.GetGridItems().Count.Should().Be(1, "Expected to have only one outbound message");
        Capgemini.PowerApps.SpecFlowBindings.Steps.GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(0);
        FormSteps.FieldContainsData("text", "field", "trd_message", "contains");
    }

    /// <summary>
    /// Given step to set all lab samples for an application to Submitted, or Ready To Submit if the given environment has no Fera integration.
    /// Note that this step requires lab samples to have been created.
    /// </summary>
    /// <param name="applicationAlias">The alias of the application to update the lab samples for.</param>
    [Given("the system has updated the status of the lab samples for '(.*)' to Submitted")]
    public void GivenSystemUpdatesLabSample(string applicationAlias)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);
        var expectedStatus = trd_labsample_statuscode.Submitted;
        using (var svc = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svc))
        {
            var workOrder = svc.WaitForFieldValue<EntityReference>(application.LogicalName, application.Id, "trd_workorderid", SpecflowBindingsConstants.DefaultWaitTime);
            var results = svc.WaitForRecords(
            new QueryByAttribute(trd_labsample.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(true),
                Attributes = { "trd_workorder", "statuscode" },
                Values = { workOrder.Id, (int)expectedStatus },
            },
            SpecFlowBindings.SpecflowBindingsConstants.DefaultWaitTime,
            SpecFlowBindings.SpecflowBindingsConstants.DefaultRetryInterval,
            false).Entities.Select(x => x.ToEntity<trd_labsample>()).ToList();
            context.ClearChanges();
        }
    }

    /// <summary>
    /// A given step for setting up lab samples for a given application to a certain status.
    /// Note this step requires inspection results on an application.
    /// </summary>
    /// <param name="samplesToComplete">Whether to set up a single lab sample or one for every inspection result.</param>
    /// <param name="applicationAlias">The alias of the application to set up lab samples for.</param>
    /// <param name="result">The status of the required lab samples.</param>
    [Given("(a lab sample|all lab samples) for '(.*)' have a status of '(.*)'")]
    public void GivenLabSamplesHaveStatus(string samplesToComplete, string applicationAlias, string result)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);

        using (var svc = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svc))
        {
            var workOrder = svc.WaitForFieldValue<EntityReference>(application.LogicalName, application.Id, "trd_workorderid", TimeSpan.FromSeconds(30));
            var results = svc.WaitForRecords(
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

            this.UpdateLabSampleStatus(context, results, samplesToComplete, result);
        }
    }

    /// <summary>
    /// Opens a related lab sample for a given application.
    /// </summary>
    /// <param name="applicationAlias">The alias of the related application.</param>
    [Given("I have opened a lab sample for '(.*)'")]
    public void GivenIOpenLabSample(string applicationAlias)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);

        using (var svc = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svc))
        {
            var workOrder = svc.WaitForFieldValue<EntityReference>(application.LogicalName, application.Id, "trd_workorderid", TimeSpan.FromSeconds(30));
            var sample = svc.WaitForRecords(
            new QueryByAttribute(trd_labsample.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(true),
                Attributes = { "trd_workorder" },
                Values = { workOrder.Id },
            },
            SpecFlowBindings.SpecflowBindingsConstants.DefaultWaitTime,
            SpecFlowBindings.SpecflowBindingsConstants.DefaultRetryInterval,
            false).Entities.Select(x => x.ToEntity<trd_labsample>()).ToList().FirstOrDefault();
            context.ClearChanges();
            XrmApp.Entity.OpenEntity("trd_labsample", sample.Id);
            SharedSteps.WaitForScriptProcessing();
        }
    }

    private void UpdateLabSampleStatus(PlantsContext context, List<trd_inspectionresult> results, string samplesToComplete, string status)
    {
        var osvResult = trd_labsample_statuscode.Draft;
        bool createAsDraft = false;
        var sampleState = trd_labsampleState.Active;
        var sampleCount = samplesToComplete == "all" ? results.Count : 1;

        switch (status)
        {
            case "Draft":
                osvResult = trd_labsample_statuscode.Draft;
                sampleState = trd_labsampleState.Active;
                break;

            case "Created":
                osvResult = trd_labsample_statuscode.Created;
                sampleState = trd_labsampleState.Inactive;
                break;

            case "Ready to Submit":
                osvResult = trd_labsample_statuscode.ReadytoSubmit;
                sampleState = trd_labsampleState.Inactive;
                break;

            case "Submitted":
                osvResult = trd_labsample_statuscode.Submitted;
                sampleState = trd_labsampleState.Inactive;
                createAsDraft = true;
                break;

            case "Sample Forwarded to Sub Team":
                osvResult = trd_labsample_statuscode.SampleForwardedtoSubTeam;
                sampleState = trd_labsampleState.Inactive;
                break;

            case "Sent Back Error":
                osvResult = trd_labsample_statuscode.SentBackErrors;
                sampleState = trd_labsampleState.Active;
                break;
        }

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
                trd_WorkOrder = results[0].trd_WorkOrder,
                trd_consignmentitemid = results[i].trd_ConsignmentItemId,
                trd_WorkOrderServiceTask = results[0].trd_WorkOrderServiceTask,
                trd_SuspectPestDescription = "Unknown Pests",
                trd_PestAliveDuringInspection = trd_labsample_trd_pestaliveduringinspection.Unknown,
                statuscode = createAsDraft ? trd_labsample_statuscode.Draft : osvResult,
                statecode = createAsDraft ? trd_labsampleState.Active : sampleState,
                OwnerId = results[0].OwnerId,
            };

            context.AddObject(sample);
            context.SaveChanges();

            var inspectionresult = new trd_inspectionresult
            {
                Id = results[i].Id,
                trd_LabSample = sample.ToEntityReference(),
                trd_QuantityInspected = 100,
            };

            if (!context.IsAttached(inspectionresult))
            {
                context.Attach(inspectionresult);
            }

            context.UpdateObject(inspectionresult);
            context.SaveChanges();

            if (status != "Draft")
            {
                List<EntityReference> pests = new List<EntityReference>();
                pests.Add(new EntityReference("trd_pest", Pest.DefaultTestPest));
                this.AddSuspectedPests(context, pests, sample);

                var updateSample = new trd_labsample
                {
                    Id = sample.Id,
                    statuscode = osvResult,
                    statecode = sampleState,
                };

                if (!context.IsAttached(updateSample))
                {
                    context.Attach(updateSample);
                }

                context.UpdateObject(updateSample);
                context.SaveChanges();
            }
        }

        context.ClearChanges();
    }

    private void AddSuspectedPests(PlantsContext context, List<EntityReference> pests, trd_labsample sample)
    {
        EntityReferenceCollection pestsToAssociate = new EntityReferenceCollection(pests);
        AssociateRequest associateRequest = new AssociateRequest()
        {
            Target = sample.ToEntityReference(),
            RelatedEntities = pestsToAssociate,
            Relationship = new Relationship("trd_trd_labsample_trd_pest"),
        };
        context.Execute(associateRequest);
    }
}