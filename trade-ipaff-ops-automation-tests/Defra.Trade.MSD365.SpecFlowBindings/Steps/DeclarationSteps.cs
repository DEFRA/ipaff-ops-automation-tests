// <copyright file="AlertSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Capgemini.PowerApps.SpecFlowBindings.Steps;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Polly;
using System;
using System.Linq;
using Reqnroll;

[Binding]
public class DeclarationSteps : PowerAppsStepDefiner
{
    private readonly SessionContext ctx;

    public DeclarationSteps(SessionContext context)
    {
        this.ctx = context;
    }

    [Given("the '(.*)' declaration for '(.*)' has been selected on the '(.*)'")]
    public void GivenDeclarationSelected(string declarationShortName, string applicationAlias, string taskName)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);

        using (var svc = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svc))
        {
            var workOrderId = WorkOrderSteps.GetWorkOrder(svc, application);
            var workOrder = context.msdyn_workorderSet.Where(x => x.Id == workOrderId.Id).FirstOrDefault();

            var task = Policy
            .HandleResult<msdyn_workorderservicetask>(r => r == null)
            .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(10))
            .Execute(() =>
            {
                return context.msdyn_workorderservicetaskSet.Where(x => x.msdyn_WorkOrder.Id == workOrder.Id && x.msdyn_name == taskName).FirstOrDefault();
            });

            var declaration = context.trd_declarationSet.Where(x => x.trd_shortname == declarationShortName && x.trd_CountryId.Id == workOrder.trd_destinationcountry.Id).FirstOrDefault();

            context.ClearChanges();

            var selectedDeclaration = new trd_selecteddeclaration
            {
                Id = Guid.NewGuid(),
                trd_Country = workOrder.trd_destinationcountry,
                trd_Reason = declaration.trd_Reason,
                trd_servicetask = task.ToEntityReference(),
                trd_workorder = workOrder.ToEntityReference(),
                trd_declarationid = declaration.ToEntityReference(),
                trd_declaration = declaration.trd_Declaration,
                trd_bigapproved = declaration.trd_BIGApproved,
                trd_UseInPhyto = true,
                trd_DeclarationSource = declaration.trd_DeclarationSource,
                trd_name = declaration.trd_shortname,
                OwnerId = workOrder.OwnerId
            };

            context.AddObject(selectedDeclaration);
            context.SaveChanges();
        }
    }

    [When(@"I search a value '(.*)' in the declaration subgrid")]
    public void WhenISearchAValueInTheDeclarationSubgrid(string shortName)
    {
        this.SearchDeclationSubGrid(shortName);
    }

    [When(@"I search a value from variable '(.*)' in the declaration subgrid")]
    public void WhenISearchAVariableInTheDeclarationSubgrid(string variableName)
    {
        this.SearchDeclationSubGrid(this.ctx.GetVariable(variableName)?.ToString() ?? string.Empty);
    }

    [When(@"I select the record from declaration subgrid")]
    public void WhenISelectTheRecordFromDeclarationSubgrid()
    {
        this.SelectRecordsDeclarationSubGrid();
    }

    [When(@"I select the add declaration")]
    public void WhenISelectTheAddDeclaration()
    {
        this.SelectAddDeclarationSubGrid();
    }

    [When(@"I select the Edit selected declaration subgrid")]
    public void WhenISelectTheEditSelectedDeclarationSubgrid()
    {
        this.SelectEditSelectedDeclarationSubGrid();
    }

    [When(@"I select the Exclude From Phyto from the selected declaration subgrid")]
    public void WhenISelectTheExcludeFromPhytoFromTheSelectedDeclarationSubgrid()
    {
        this.SelectExcludeFromPhytoFromSelectedDeclarationSubGrid();
    }

    [When(@"I select the Edit selected phyto commodity subgrid")]
    public void WhenISelectTheEditSelectedPhytoCommoditySubgrid()
    {
        this.SelectEditSelectedPhytoCommoditySubGrid();
    }

    [When(@"I select the Exclude From Phyto from the commodity subgrid")]
    public void WhenISelectTheExcludeFromPhytoFromTheCommoditySubgrid()
    {
        this.SelectExcludeFromPhytoFromPhytoSubgrid();
    }

    [When(@"I select the record from selected declaration subgrid")]
    public void WhenISelectTheRecordFromSelectedDeclarationSubgrid()
    {
        this.SelectRecordsSelectedDeclarationSubGrid();
    }

    [When(@"edit the declaration description in selected declaration subgrid")]
    public void WhenEditTheDeclarationDescriptionInSelectedDeclarationSubgrid()
    {
        this.EditDeclarationDescription();
    }

    [When(@"I maximise the selected declaration popup")]
    public void WhenIMaximiseTheSelectedDeclarationPopup()
    {
        this.MaximizeSelectedDeclarationPopupSubGrid();
    }

    [When(@"clear the declaration description in selected declaration subgrid")]
    public void WhenClearTheDeclarationDescriptionInSelectedDeclarationSubgrid()
    {
        this.ClearDeclarationDescription();
    }

    [When(@"When I save the record in the selected declaration popup box")]
    public void WhenWhenISaveTheRecordInTheSelectedDeclarationPopupBox()
    {
        this.SaveSelectedDeclarationPopupSubGrid();
    }

    /// <summary>
    /// Handles error when you try to attach the same declaration multiple times.
    /// </summary>
    [When(@"I add a declaration that is already selected")]
    public void WhenIAddADuplicatedDeclaration()
    {
        EntitySteps.ISelectTab("Declarations");
        Driver.WaitForTransaction();

        for (int i = 0; i < 2; i++)
        {
            GridSteps.WhenISelectTheRecordAtPositionInTheSubgrid(0, "declarations_subgrid");
            Driver.WaitForTransaction();
            CommandSteps.WhenISelectTheCommand("Add Declarations", "declarations_subgrid");
            Driver.WaitForTransaction();
        }
    }

    public void WhenISelectSomeDeclaration(int noOfDeclaration)
    {
        EntitySteps.ISelectTab("Declarations");
        Driver.WaitForTransaction();

        for (int i = 0; i < noOfDeclaration; i++)
        {
            GridHelper.SelectRow(Driver, "dataSetRoot_SelectedDeclarations_grid", i);
            Driver.WaitForTransaction();
        }
    }

    public IWebElement SearchTextForDeclarationSubgrid => Driver.WaitUntilAvailable(By.XPath("(.//div[@data-id='dataSetRoot_declarations_subgrid'])//div[@data-id='data-set-quickFind-container']//input"));

    public IWebElement SelectRecordsForDeclarationSubgrid => Driver.WaitUntilAvailable(By.XPath("(.//div[@data-id='dataSetRoot_declarations_subgrid'])//div[@data-id='btnheaderselectcolumn']//button"));

    public IWebElement SelectAddDeclarationSubgrid => Driver.WaitUntilAvailable(By.XPath("(.//div[@data-id='dataSetRoot_declarations_subgrid'])//ul[@data-lp-id='commandbar-SubGridStandard:trd_declaration']//button[@data-id='trd_declaration|NoRelationship|SubGridStandard|trd.trd_declaration.Button.AddDeclarations']"));

    public IWebElement SelectEditSelectedDeclarationSubgrid => Driver.WaitUntilAvailable(By.XPath("(.//div[@data-id='dataSetRoot_SelectedDeclarations_grid'])//ul[@data-lp-id='commandbar-SubGridStandard:trd_selecteddeclaration']//button[@data-id='trd_selecteddeclaration|NoRelationship|SubGridStandard|Mscrm.SubGrid.trd_selecteddeclaration.Edit']"));

    public IWebElement SelectEditSelectedPhytoCommoditySubgrid => Driver.WaitUntilAvailable(By.XPath("(.//div[@data-id='dataSetRoot_subgrid_selected_commodities'])//ul[@data-lp-id='commandbar-SubGridStandard:trd_selectedcommodityitem']//button[@data-id='trd_selectedcommodityitem|NoRelationship|SubGridStandard|Mscrm.SubGrid.trd_selectedcommodityitem.Edit']"));

    public IWebElement SelectExcludeFromPhytoFromSelectedDeclarationSubgrid => Driver.WaitUntilAvailable(By.XPath("(.//div[@data-id='dataSetRoot_SelectedDeclarations_grid'])//ul[@data-lp-id='commandbar-SubGridStandard:trd_selecteddeclaration']//button[@data-id='trd_selecteddeclaration|NoRelationship|SubGridStandard|trd.trd_selecteddeclaration.Button.ExcludeFromPhyto']"));

    public IWebElement SelectIncludeOnPhytoFromPhytoSubgrid => Driver.WaitUntilAvailable(By.XPath("(.//div[@data-id='dataSetRoot_subgrid_selected_commodities'])//ul[@data-lp-id='commandbar-SubGridStandard:trd_selectedcommodityitem']//button[@data-id='trd_selectedcommodityitem|NoRelationship|SubGridStandard|trd.trd_selectedcommodityitem.SubGrid.IncludeOnPhyto']"));

    public IWebElement DeclarationDescription => Driver.WaitUntilAvailable(By.XPath(".//div[@data-id='trd_declaration']//div[@data-lp-id='MscrmControls.FieldControls.TextBoxControl|trd_declaration.fieldControl|trd_selecteddeclaration']//textarea[@data-id='trd_declaration.fieldControl-text-box-text']"));

    public IWebElement SelectRecordsForSelectedDeclarationSubgrid => Driver.WaitUntilAvailable(By.XPath("(.//div[@data-id='dataSetRoot_SelectedDeclarations_grid'])//div[@data-id='btnheaderselectcolumn']//button"));

    public IWebElement MaximizeSelectedDeclarationPopupSubGridButton => Driver.WaitUntilAvailable(By.XPath("(//div[@id='defaultDialogChromeView']//button[@title='Enter full screen mode']//span[contains(@class,'MiniExpand-symbol')])[1]"));

    public IWebElement SaveSelectedDeclarationPopupSubGridButton => Driver.WaitUntilAvailable(By.XPath("(//li[contains(@id,'trd_selecteddeclaration|NoRelationship|Form|Mscrm.SavePrimary')])[1]"));

    public void SearchDeclationSubGrid(string shortName)
    {
        this.SearchTextForDeclarationSubgrid.InputText(shortName);
        this.SearchTextForDeclarationSubgrid.SendKeys(Keys.Enter);
    }

    public void SelectRecordsDeclarationSubGrid()
    {
        this.SelectRecordsForDeclarationSubgrid.Click();
    }

    public void SelectRecordsSelectedDeclarationSubGrid()
    {
        this.SelectRecordsForSelectedDeclarationSubgrid.Click();
    }

    public void SelectEditSelectedDeclarationSubGrid()
    {
        this.SelectEditSelectedDeclarationSubgrid.Click();
    }

    public void SelectEditSelectedPhytoCommoditySubGrid()
    {
        this.SelectEditSelectedPhytoCommoditySubgrid.Click();
    }

    public void SelectExcludeFromPhytoFromSelectedDeclarationSubGrid()
    {
        this.SelectExcludeFromPhytoFromSelectedDeclarationSubgrid.Click();
    }

    public void SelectExcludeFromPhytoFromPhytoSubgrid()
    {
        this.SelectIncludeOnPhytoFromPhytoSubgrid.Click();
    }

    public void ClearDeclarationDescription()
    {
        Actions act = new Actions(Driver);
        var selectInput = Driver.FindElement(By.XPath(".//div[@data-id='trd_declaration']//div[@data-lp-id='MscrmControls.FieldControls.TextBoxControl|trd_declaration.fieldControl|trd_selecteddeclaration']//textarea[@data-id='trd_declaration.fieldControl-text-box-text']"));
        act.Click(selectInput)
            .KeyDown(Keys.Control)
            .SendKeys("a")
            .KeyUp(Keys.Control)
            .SendKeys(Keys.Backspace)
            .Build()
            .Perform();
    }

    public void EditDeclarationDescription()
    {
        var act = new Actions(Driver);
        var selectInput = Driver.FindElement(By.XPath(".//div[@data-id='trd_declaration']//div[@data-lp-id='MscrmControls.FieldControls.TextBoxControl|trd_declaration.fieldControl|trd_selecteddeclaration']//textarea[@data-id='trd_declaration.fieldControl-text-box-text']"));
        act.MoveToElement(selectInput);
        this.DeclarationDescription.SendKeys("dddddsdsds");
    }

    public void MaximizeSelectedDeclarationPopupSubGrid()
    {
        Driver.ExecuteScript("arguments[0].click()", this.MaximizeSelectedDeclarationPopupSubGridButton);
    }

    public void SaveSelectedDeclarationPopupSubGrid()
    {
        Driver.SwitchTo().ActiveElement();
        this.SaveSelectedDeclarationPopupSubGridButton.Click();
    }

    public void SelectAddDeclarationSubGrid()
    {
        var subGridName = "declarations_subgrid";
        var commandName = "Add Declarations";

        var subGrid = GridHelper.GetGrid(Driver, subGridName);
        CommandHelper.ClickCommand(Driver, subGrid, commandName);
    }
}
