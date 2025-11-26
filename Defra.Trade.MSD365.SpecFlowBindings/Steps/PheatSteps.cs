// <copyright file="Pheats.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Capgemini.PowerApps.SpecFlowBindings.Steps;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using Polly;
using System;
using System.Linq;
using Reqnroll;

/// <summary>
/// Steps relating to pheats authorisation.
/// </summary>
[Binding]
public class PheatSteps : PowerAppsStepDefiner
{
    private readonly SessionContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="PheatSteps"/> class.
    /// </summary>
    /// <param name="context"></param>
    public PheatSteps(SessionContext context)
    {
        this.context = context;
    }

    [Given("Pheats Test Contacts have been created")]
    public void CreatePheatsContacts()
    {
        // Create Contacts for Pheats testing (with unique names, as used in lookups)
        using (var serviceClient = SessionContext.GetServiceClient())
        {
            using (var context = new PlantsContext(serviceClient))
            {
                for (int i = 1; i <= 2; i++)
                {
                    var contact = new Contact()
                    {
                        FirstName = this.context.SessionId + "-Test",
                        LastName = "User-" + i,
                    };

                    var id = (serviceClient.Execute(new CreateRequest() { Target = contact }) as CreateResponse).id;
                    var alias = contact.FirstName + contact.LastName;
                    this.context.Entities.Add(alias, new EntityHolder() { Alias = alias, EntityName = "contact", EntityCollectionName = "contacts", Id = id });
                }
            }
        }
    }

    [When("I select pheats authorised person '([^']+)' in the lookup dialog")]
    public static void WhenISelectPheatsAuthorisedPersonAndAddInTheLookupDialog(string searchTerm)
    {
        var fieldContainer = Driver.WaitUntilAvailable(By.CssSelector("div[id=\"lookupDialogContainer\"] div[id=\"lookupDialogLookup\"]"));
        var input = fieldContainer.ClickWhenAvailable(By.TagName("input"));
        input.SendKeys(searchTerm);
        input.SendKeys(Keys.Enter);

        Driver.WaitForTransaction();

        fieldContainer.ClickWhenAvailable(By.CssSelector("li[data-id*=\"MscrmControls.FieldControls.SimpleLookupControl-LookupResultsPopup_falseBoundLookup_resultsContainer\"]"));

        Driver.WaitForTransaction();

        Driver
            .WaitUntilAvailable(By.CssSelector("div[id=\"lookupDialogContainer\"]"))
            .ClickWhenAvailable(By.CssSelector("button[data-id*=\"lookupDialogSaveBtn\"]"));
    }

    [When("I add the person responsible and authorised people")]
    public void WhenICompletePheatsAuthorisation()
    {
        CommandSteps.WhenISelectTheCommand("PHEATS Authorisation");
        Driver.WaitForTransaction();
        EntitySteps.ISelectTab("General");
        Driver.WaitForTransaction();
        EntitySteps.WhenIEnterInTheField(this.context.SessionId + "-Test User-1", "trd_personresponsibleid", "lookup", "field");
        Driver.WaitForTransaction();
        FormSteps.SetDateTime("trd_firstinspectiondate", "yesterday", "midnight");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save");
        Driver.WaitForTransaction();
        EntitySubGridSteps.WhenISelectTheCommandOnTheSubgrid("Add Existing Contact", "authorised_people_subgrid");
        Driver.WaitForTransaction();
        WhenISelectPheatsAuthorisedPersonAndAddInTheLookupDialog(this.context.SessionId + "-Test User-2");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save");
    }

    [Then("I can see the pheats authorisation for person responsible")]
    public static void ThenIConfirmPheatsAuthorisationForPersonResponsible()
    {
        FormSteps.FieldContainsData("lookup", "field", "trd_personresponsibleid", "contains");
        FormSteps.FieldContainsData("datetime", "field", "trd_firstinspectiondate", "contains");
        EntitySubGridSteps.ThenICanSeeRecordsInTheSubgrid("exactly", 1, "authorised_people_subgrid");
        FormSteps.VerifyValue("434800001", "statuscode");
    }

    [When(@"I authorise inspection address")]
    public void WhenIAuthoriseInspectionAddress()
    {
        CommandSteps.WhenISelectTheCommand("PHEATS Authorisation");
        Driver.WaitForTransaction();
        EntitySteps.ISelectTab("General");
        Driver.WaitForTransaction();
        EntitySteps.WhenIEnterInTheField(this.context.SessionId + "-Test User-1", "trd_personresponsibleid", "lookup", "field");
        Driver.WaitForTransaction();
        FormSteps.SetDateTime("trd_firstinspectiondate", "yesterday-3", "midnight");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Authorise");
        Driver.WaitForTransaction();
        DialogSteps.WhenIClickTheAlertButtonOnTheDialog("confirm");
        Driver.WaitForTransaction();
        FormSteps.VerifyValue("434800001", "statuscode");
    }

    [When(@"I relate the contact with the inspection location")]
    public void WhenIrelateTheContactWithTheInspectionLocation()
    {
        EntitySteps.WhenIEnterInTheField(this.context.SessionId + "-Test User-2", "trd_legalowner", "lookup", "field");
        Driver.WaitForTransaction();
    }

    [When(@"I create the pheats audit")]
    public void WhenICreateThePheatsAudit()
    {
        if (XrmApp.Entity.GetEntityName() == "trd_pheatsaudit")
        {
            CommandSteps.WhenIgoBack();
        }

        CommandSteps.WhenISelectTheCommand("Create PHEATS Audit");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save");
        Driver.WaitForTransaction();
        FormSteps.SetDateTime("trd_dateofaudit", "today", "midnight");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save");
        Driver.WaitForTransaction();
    }

    [When(@"I create the previous pheats audit")]
    public void WhenICreateThePreviousPheatsAudit()
    {
        CommandSteps.WhenISelectTheCommand("Create PHEATS Audit");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save");
        Driver.WaitForTransaction();
        FormSteps.SetDateTime("trd_dateofaudit", "yesterday-1", "midnight");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save");
        Driver.WaitForTransaction();
    }

    [When(@"I passed the previous pheats audit")]
    public void WhenIPassedThePreviousPheatsAudit()
    {
        WhenIAuthoriseInspectionAddress();
        WhenICreateThePreviousPheatsAudit();
        CommandSteps.WhenISelectTheCommand("Pass");
        DialogSteps.WhenIClickTheAlertButtonOnTheDialog("confirm");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save & Close");
        Driver.WaitForTransaction();
    }

    [When(@"I failed the previous pheats audit")]
    public void WhenIFailedThePreviousPheatsAudit()
    {
        WhenIAuthoriseInspectionAddress();
        WhenICreateThePreviousPheatsAudit();
        CommandSteps.WhenISelectTheCommand("Fail");
        DialogSteps.WhenIClickTheAlertButtonOnTheDialog("confirm");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save & Close");
        Driver.WaitForTransaction();
    }

    [When(@"I authorise inspection address and person responsible")]
    [When(@"Contact has been pheats authorised")]
    public void ThenIAuthoriseInspectionAddressAndPersonResponsible()
    {
        CommandSteps.WhenISelectTheCommand("Authorise");
        Driver.WaitForTransaction();
        DialogSteps.WhenIClickTheAlertButtonOnTheDialog("confirm");
        Driver.WaitForTransaction();
    }

    [When(@"I failed the initial inspection")]
    public void WhenIFailedTheInitialInspection()
    {
        CommandSteps.WhenISelectTheCommand("PHEATS Authorisation");
        Driver.WaitForTransaction();
        EntitySteps.ISelectTab("General");
        Driver.WaitForTransaction();
        EntitySteps.WhenIEnterInTheField(this.context.SessionId + "-Test User-1", "trd_personresponsibleid", "lookup", "field");
        Driver.WaitForTransaction();
        FormSteps.SetDateTime("trd_firstinspectiondate", "yesterday", "midnight");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Deauthorise");
        Driver.WaitForTransaction();
        DialogSteps.WhenIClickTheAlertButtonOnTheDialog("confirm");
        Driver.WaitForTransaction();
        FormSteps.VerifyValue("434800002", "statuscode");
    }

    [When(@"I unauthorise inspection address")]
    public void ThenIUnauthoriseInspectionAddress()
    {
        this.ThenIAuthoriseInspectionAddressAndPersonResponsible();
        FormSteps.FieldContainsData("lookup", "field", "trd_personresponsibleid", "contains");
        FormSteps.FieldContainsData("datetime", "field", "trd_firstinspectiondate", "contains");
        EntitySubGridSteps.ThenICanSeeRecordsInTheSubgrid("exactly", 1, "authorised_people_subgrid");
        FormSteps.VerifyValue("434800001", "statuscode");
        CommandSteps.WhenISelectTheCommand("Revise");
        Driver.WaitForTransaction();
        DialogSteps.WhenIClickTheAlertButtonOnTheDialog("confirm");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Create PHEATS Audit");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save");
        Driver.WaitForTransaction();
        FormSteps.SetDateTime("trd_dateofaudit", "yesterday", "midnight");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save");
        Driver.WaitForTransaction();
        CommandSteps.WhenIgoBack();
        Driver.WaitForTransaction();
        CommandSteps.WhenIgoBack();
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("PHEATS Authorisation");
        Driver.WaitForTransaction();
        EntitySteps.ISelectTab("General");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Deauthorise");
        Driver.WaitForTransaction();
        DialogSteps.WhenIClickTheAlertButtonOnTheDialog("confirm");
        Driver.WaitForTransaction();
    }

    [When(@"I unauthorise pheats authorised people and create audit")]
    public void ThenIUnauthorisePheatsAuthorisedPeopleAndCreateAudit()
    {
        CommandSteps.WhenIgoBack();
        CommandSteps.WhenISelectTheCommand("PHEATS Authorisation");
        Driver.WaitForTransaction();
        EntitySteps.ISelectTab("General");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Authorise");
        Driver.WaitForTransaction();
        DialogSteps.WhenIClickTheAlertButtonOnTheDialog("confirm");
        Driver.WaitForTransaction();
        FormSteps.FieldContainsData("lookup", "field", "trd_personresponsibleid", "contains");
        FormSteps.FieldContainsData("datetime", "field", "trd_firstinspectiondate", "contains");
        EntitySubGridSteps.ThenICanSeeRecordsInTheSubgrid("exactly", 1, "authorised_people_subgrid");
        FormSteps.VerifyValue("434800001", "statuscode");
        CommandSteps.WhenISelectTheCommand("Revise");
        Driver.WaitForTransaction();
        DialogSteps.WhenIClickTheAlertButtonOnTheDialog("confirm");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Create PHEATS Audit");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save");
        Driver.WaitForTransaction();
        FormSteps.SetDateTime("trd_dateofaudit", "yesterday", "midnight");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save");
        Driver.WaitForTransaction();
        CommandSteps.WhenIgoBack();
        Driver.WaitForTransaction();
        CommandSteps.WhenIgoBack();
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("PHEATS Authorisation");
        Driver.WaitForTransaction();
        EntitySteps.ISelectTab("General");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Deauthorise");
        Driver.WaitForTransaction();
        DialogSteps.WhenIClickTheAlertButtonOnTheDialog("confirm");
        Driver.WaitForTransaction();
    }

    [Then("I can see the pheats record unauthorised for inspection address")]
    public static void ThenICanSeePheatsRecordUnauthorisedForInspectionAddress()
    {
        var subGrid = GridHelper.GetGrid(Driver, "authorised_people_subgrid");
        var subGridRows = GridHelper.GetRows(Driver, "authorised_people_subgrid");

        if (subGridRows.IsEmpty())
        {
            Policy.Handle<Exception>()
                .WaitAndRetry(5, retryCount => 5.Seconds())
                .Execute(() =>
                {
                    CommandHelper.ClickCommand(Driver, subGrid, "Refresh");
                    Driver.WaitForTransaction();

                    subGridRows = GridHelper.GetRows(Driver, "authorised_people_subgrid");
                    if (subGridRows.IsEmpty())
                    {
                        throw new Exception($"There is no data available within the subGrid 'authorised_people_subgrid'");
                    }
                });
        }

        EntitySubGridSteps.ThenICanSeeRecordsInTheSubgrid("exactly", 1, "authorised_people_subgrid");
        Driver.WaitForTransaction();
        FormSteps.VerifyValue("434800002", "statuscode");
    }

    [When(@"I revise pheats authorisation and create audit")]
    public void WhenIRevisePheatsAuthorisationAndCreateAudit()
    {
        this.ThenIAuthoriseInspectionAddressAndPersonResponsible();
        FormSteps.FieldContainsData("lookup", "field", "trd_personresponsibleid", "contains");
        FormSteps.FieldContainsData("datetime", "field", "trd_firstinspectiondate", "contains");
        EntitySubGridSteps.ThenICanSeeRecordsInTheSubgrid("exactly", 1, "authorised_people_subgrid");
        FormSteps.VerifyValue("434800001", "statuscode");
        CommandSteps.WhenISelectTheCommand("Revise");
        Driver.WaitForTransaction();
        DialogSteps.WhenIClickTheAlertButtonOnTheDialog("confirm");
        Driver.WaitForTransaction();
        GridSteps.WhenISelectRowInTheGrid(0, "authorised_people_subgrid");
        Driver.WaitForTransaction();

        var subGrid = GridHelper.GetGrid(Driver, "authorised_people_subgrid");
        CommandSteps.WhenISelectTheCommand("Create PHEATS Audit");

        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save");
        Driver.WaitForTransaction();
        FormSteps.SetDateTime("trd_dateofaudit", "yesterday", "midnight");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save");
        Driver.WaitForTransaction();
        CommandSteps.WhenIgoBack();
    }

    [Then(@"I view the pheats audits for the contact")]
    public void WhenIViewThePheatsAuditsForTheContact()
    {
        LookupSteps.WhenISelectARelatedLookupInTheForm("trd_personresponsibleid");
        RelatedGridSteps.WhenIOpenTheRelatedTab("PHEATS Audits");
    }

    [When(@"I view the pheats audits records for all audits")]
    public void WhenIViewThePheatsAuditsForAllAudits()
    {
        CommandSteps.WhenISelectTheCommand("Save & Close");
        Driver.WaitForTransaction();
        EntitySteps.ISelectTab("PHEATS Audits");
    }

    [When(@"I open the pheats authorisation")]
    public void WhenIOpenPheatsAuthorisation()
    {
        CommandSteps.WhenIgoBack();
        Driver.WaitForTransaction();
        EntitySteps.ISelectTab("General");
    }

    [When(@"I view the related charges for pheats first inspection")]
    public void WhenIViewThePheatsFirstInspectionCharges()
    {
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
    }

    [When(@"I view the pheats audit record")]
    public void WhenIViewThePheatsAuditRecord()
    {
        CommandSteps.WhenISelectTheCommand("Save & Close");
        EntitySteps.ISelectTab("PHEATS Audits");
        EntitySubGridSteps.ThenICanSeeRecordsInTheSubgrid("exactly", 2, "pheats_audit_subgrid"); // now includes initial inspection
    }

    [When(@"Contact has subsequent pheats audits")]
    public void WhenContactHasSubsequentPheatsAudits()
    {
        GridSteps.WhenISelectRowInTheGrid(0, "authorised_people_subgrid");
        var subGrid = GridHelper.GetGrid(Driver, "authorised_people_subgrid");
        CommandSteps.WhenISelectTheCommand("Create PHEATS Audit");
        CommandSteps.WhenISelectTheCommand("Save");
        FormSteps.SetDateTime("trd_dateofaudit", "yesterday", "midnight");
        CommandSteps.WhenISelectTheCommand("Save");
        CommandSteps.WhenIgoBack();
        CommandSteps.WhenIgoBack();
        CommandSteps.WhenISelectTheCommand("PHEATS Authorisation");
        EntitySteps.ISelectTab("General");
    }

    [When(@"I create pheats audit with active status")]
    public void WhenCreatePheatsAuditActiveStatus()
    {
        GridSteps.WhenISelectRowInTheGrid(0, "authorised_people_subgrid");
        var subGrid = GridHelper.GetGrid(Driver, "authorised_people_subgrid");
        CommandSteps.WhenISelectTheCommand("Create PHEATS Audit");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save");
        FormSteps.SetDateTime("trd_dateofaudit", "yesterday", "midnight");
        CommandSteps.WhenISelectTheCommand("Save");
    }

    [When(@"I create pheats audit with passed status")]
    public void WhenCreatePheatsAuditPassStatus()
    {
        GridSteps.WhenISelectRowInTheGrid(0, "authorised_people_subgrid");
        var subGrid = GridHelper.GetGrid(Driver, "authorised_people_subgrid");
        CommandSteps.WhenISelectTheCommand("Create PHEATS Audit");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save");
        FormSteps.SetDateTime("trd_dateofaudit", "yesterday", "midnight");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Pass");
        DialogSteps.WhenIClickTheAlertButtonOnTheDialog("confirm");
        Driver.WaitForTransaction();
    }

    [When(@"I create pheats audit with failed status")]
    public void WhenCreatePheatsAuditWithFailStatus()
    {
        GridSteps.WhenISelectRowInTheGrid(0, "authorised_people_subgrid");
        var subGrid = GridHelper.GetGrid(Driver, "authorised_people_subgrid");
        CommandSteps.WhenISelectTheCommand("Create PHEATS Audit");
        Driver.WaitForTransaction();
        CommandSteps.WhenISelectTheCommand("Save");
        FormSteps.SetDateTime("trd_dateofaudit", "yesterday", "midnight");
        CommandSteps.WhenISelectTheCommand("Fail");
        Driver.WaitForTransaction();
        DialogSteps.WhenIClickTheAlertButtonOnTheDialog("confirm");
        Driver.WaitForTransaction();
    }

    [When(@"I store the value for the audit id")]
    public void WhenIStoreTheValueForAuditId()
    {
        FormSteps.StoreFormValueInVariable("trd_auditid", "text", "field", "AuditId", this.context);
    }

    [Then(@"I can see the stored audit id value")]
    public void ThenICanSeeTheStoredAuditIdValue()
    {
        FormSteps.ValueInFieldMatchesVariable("trd_auditid", "text", "field", "matches", "AuditId", this.context);
    }

    [When(@"I store the value for the first inspection id")]
    public void WhenIStoreTheValueForInspectionId()
    {
        FormSteps.StoreFormValueInVariable("trd_firstinspectionid", "lookup", "field", "PheatsAuditId", this.context);
    }

    [Then(@"I can see the stored pheats audit id value")]
    public void ThenICanSeeTheStoredPheatsAuditId()
    {
        FormSteps.ValueInFieldMatchesVariable("trd_pheatsauditid", "text", "field", "matches", "PheatsAuditId", this.context);
    }

    [Then(@"Charge record is created for the initial inspection with the charge exempt field blank")]
    public void ThenChargeRecordIsCreatedForTheInitialInspectionWithTheChargeExemptFieldBlank()
    {
        CommandSteps.WhenIgoBack();
        EntitySteps.ISelectTab("PHEATS Audits");
        Driver.WaitForTransaction();
        GridSteps.CheckGridRowCount(2);
        Driver.WaitForTransaction();
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        Driver.WaitForTransaction();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(0);
        Driver.WaitForTransaction();
        FormSteps.FieldContainsData("optionset", "field", "trd_chargeexempt", "does not contain");
    }

    [Then(@"I can see inspection address is linked to contact through legal owner")]
    public void ThenICanSeeInspectionAddressIsLinkedToContactThroughLegalOwner()
    {
        LookupSteps.WhenISelectARelatedLookupInTheForm("trd_inspectionaddressid");
        LookupSteps.WhenISelectARelatedLookupInTheForm("trd_legalowner");
        FormSteps.ICanSeeAForm("editable", "contact");
    }

    [Then(@"Charge record is created for the failed initial inspection with the charge exempt field blank")]
    public void ThenChargeRecordIsCreatedForTheFailedInitialInspectionWithTheChargeExemptFieldBlank()
    {
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        Driver.WaitForTransaction();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(0);
        Driver.WaitForTransaction();
        FormSteps.FieldContainsData("optionset", "field", "trd_chargeexempt", "does not contain");
    }

    [Then(@"Charge records created for the failed first audit with the charge exempt field blank")]
    public void ThenChargeRecordIsCreatedForTheFailedFirstAuditWithTheChargeExemptFieldBlank()
    {
        CommandSteps.WhenIgoBack();
        EntitySteps.ISelectTab("PHEATS Audits");
        Driver.WaitForTransaction();
        GridSteps.CheckGridRowCount(2);
        Driver.WaitForTransaction();
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        Driver.WaitForTransaction();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(0);
        Driver.WaitForTransaction();
        FormSteps.FieldContainsData("optionset", "field", "trd_chargeexempt", "does not contain");
        CommandSteps.WhenIgoBack();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(1);
        Driver.WaitForTransaction();
        FormSteps.FieldContainsData("optionset", "field", "trd_chargeexempt", "does not contain");
    }

    [Then(@"Charge record not created for the passed initial inspection")]
    public void ThenChargeRecordNotCreatedForThePassedInitialInspection()
    {
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        Driver.WaitForTransaction();
        GridSteps.CheckGridRowCount(0);
        Driver.WaitForTransaction();
    }

    [Then(@"Charge record created for the passed previous audit with the charge exempt field blank when current audit passed")]
    public void ThenChargeRecordCreatedForThePassedPreviousAuditWithTheChargeExemptFieldBlankWhenCurrentAuditPassed()
    {
        CommandSteps.WhenIgoBack();
        EntitySteps.ISelectTab("PHEATS Audits");
        Driver.WaitForTransaction();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(0);
        Driver.WaitForTransaction();
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        Driver.WaitForTransaction();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(0);
        Driver.WaitForTransaction();
        FormSteps.FieldContainsData("optionset", "field", "trd_chargeexempt", "does not contain");
    }

    [Then(@"Charge records created with the charge exempt field blank for the passed previous audit and failed current audit")]
    public void ThenChargeRecordsCreatedWithTheChargeExemptFieldBlankForThePassedPreviousAuditAndFailedCurrentAudit()
    {
        CommandSteps.WhenIgoBack();
        EntitySteps.ISelectTab("PHEATS Audits");
        Driver.WaitForTransaction();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(0);
        Driver.WaitForTransaction();
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        Driver.WaitForTransaction();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(0);
        Driver.WaitForTransaction();
        FormSteps.FieldContainsData("optionset", "field", "trd_chargeexempt", "does not contain");
        CommandSteps.WhenIgoBack();
        CommandSteps.WhenIgoBack();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(1);
        Driver.WaitForTransaction();
        FormSteps.FieldContainsData("optionset", "field", "trd_chargeexempt", "does not contain");
    }

    [Then(@"Charge records created with the charge exempt field blank for the failed previous and current audit")]
    public void ThenChargeRecordsCreatedWithTheChargeExemptFieldBlankForTheFailedPreviousAndCurrentAudit()
    {
        CommandSteps.WhenIgoBack();
        EntitySteps.ISelectTab("PHEATS Audits");
        Driver.WaitForTransaction();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(0);
        Driver.WaitForTransaction();
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        Driver.WaitForTransaction();
        GridSteps.CheckGridRowCount(1);
        Driver.WaitForTransaction();
        CommandSteps.WhenIgoBack();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(1);
        Driver.WaitForTransaction();
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        Driver.WaitForTransaction();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(0);
        Driver.WaitForTransaction();
        FormSteps.FieldContainsData("optionset", "field", "trd_chargeexempt", "does not contain");
    }

    [Then(@"Charge record not created for the passed current audit when previous audit failed")]
    public void ThenChargeRecordNotCreatedForThePassedCurrentAuditWhenPreviousAuditFailed()
    {
        CommandSteps.WhenIgoBack();
        EntitySteps.ISelectTab("PHEATS Audits");
        Driver.WaitForTransaction();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(1);
        Driver.WaitForTransaction();
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        Driver.WaitForTransaction();
        GridSteps.CheckGridRowCount(1);
        Driver.WaitForTransaction();
        CommandSteps.WhenIgoBack();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(2);
        Driver.WaitForTransaction();
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        Driver.WaitForTransaction();
        GridSteps.CheckGridRowCount(0);
    }

    [Then(@"Charge record created for the passed initial inspection when first audit failed")]
    public void ThenChargeRecordCreatedForThePassedInitialInspectionWhenFirstAuditFailed()
    {
        CommandSteps.WhenIgoBack();
        EntitySteps.ISelectTab("PHEATS Audits");
        Driver.WaitForTransaction();
        GridSteps.CheckGridRowCount(2); /* now includes initial */
        Driver.WaitForTransaction();
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        Driver.WaitForTransaction();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(0);
        Driver.WaitForTransaction();
        FormSteps.FieldContainsData("optionset", "field", "trd_chargeexempt", "does not contain");
        CommandSteps.WhenIgoBack();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(1);
    }

    [Then(@"Charge record created for the passed initial inspection when first audit passed")]
    public void ThenChargeRecordCreatedForThePassedInitialInspectionWhenFirstAuditPassed()
    {
        CommandSteps.WhenIgoBack();
        EntitySteps.ISelectTab("PHEATS Audits");
        Driver.WaitForTransaction();
        GridSteps.CheckGridRowCount(2); /* includes initial inspection */
        Driver.WaitForTransaction();
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        Driver.WaitForTransaction();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(0);
    }

    [Then(@"Charge record created for the failed current audit when previous audit passed")]
    public void ThenChargeRecordCreatedForTheFailedCurrentAuditWhenPreviousAuditPassed()
    {
        CommandSteps.WhenIgoBack();
        EntitySteps.ISelectTab("PHEATS Audits");
        Driver.WaitForTransaction();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(2);
        Driver.WaitForTransaction();
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        Driver.WaitForTransaction();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(0);
        Driver.WaitForTransaction();
        FormSteps.FieldContainsData("optionset", "field", "trd_chargeexempt", "does not contain");
        CommandSteps.WhenIgoBack();
        CommandSteps.WhenIgoBack();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(1);
        Driver.WaitForTransaction();
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        Driver.WaitForTransaction();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(0);
        Driver.WaitForTransaction();
    }

    [Then(@"Charge record created for the passed previous audit when current audit passed")]
    public void ThenChargeRecordCreatedForThePassedPreviousAuditWhenCurrentAuditPassed()
    {
        CommandSteps.WhenIgoBack();
        EntitySteps.ISelectTab("PHEATS Audits");
        Driver.WaitForTransaction();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(2);
        Driver.WaitForTransaction();
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        Driver.WaitForTransaction();
        GridSteps.CheckGridRowCount(0);
        Driver.WaitForTransaction();
        CommandSteps.WhenIgoBack();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(1);
        Driver.WaitForTransaction();
        RelatedGridSteps.WhenIOpenTheRelatedTab("Charges");
        Driver.WaitForTransaction();
        GridSteps.WhenIOpenTheRecordAtPositionInTheGrid(0);
        Driver.WaitForTransaction();
    }

    [Given(@"the system has created charge exempt country")]
    public void GivenTheSystemHasCreatedChargeExemptCountry()
    {
        var country = JsonConvert.DeserializeObject<JObject>(TestDataRepository.GetTestData("charge exempt country"));

        // Using WebApi - create country reference data
        using (var service = new CdsWebApiService(TestConfig.GetTestUrl(), AccessToken))
        {
            bool TryPost()
            {
                try
                {
                    service.Post("trd_countries", country);
                }
                catch { return false; }
                return true;
            }

            var success = TryPost();
        }
    }
}
