using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class WhichImportPurposesWillThisCommodityRuleApplyToSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWhichImportPurposesWillThisCommodityRuleApplyToPage? whichImportPurposesWillThisCommodityRuleApplyToPage =>
            _objectContainer.IsRegistered<IWhichImportPurposesWillThisCommodityRuleApplyToPage>()
                ? _objectContainer.Resolve<IWhichImportPurposesWillThisCommodityRuleApplyToPage>()
                : null;

        public WhichImportPurposesWillThisCommodityRuleApplyToSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Which import purposes will this commodity rule apply to? page should be displayed")]
        public void ThenTheWhichImportPurposesWillThisCommodityRuleApplyToPageShouldBeDisplayed()
        {
            Assert.True(whichImportPurposesWillThisCommodityRuleApplyToPage?.IsPageLoaded(), "Which import purposes will this commodity rule apply to? page is not displayed");
        }

        [When("the user ticks both import purpose checkboxes on the Which import purposes will this commodity rule apply to? page")]
        public void WhenTheUserTicksBothImportPurposeCheckboxesOnTheWhichImportPurposesWillThisCommodityRuleApplyToPage()
        {
            whichImportPurposesWillThisCommodityRuleApplyToPage?.TickBothImportPurposeCheckboxes();
        }

        [When("the user clicks the Continue button on the Which import purposes will this commodity rule apply to? page")]
        public void WhenTheUserClicksTheContinueButtonOnTheWhichImportPurposesWillThisCommodityRuleApplyToPage()
        {
            whichImportPurposesWillThisCommodityRuleApplyToPage?.ClickContinueButton();
        }

        [When("the user ticks the {string} checkbox on the Which import purposes will this commodity rule apply to? page")]
        public void WhenTheUserTicksTheCheckboxOnTheWhichImportPurposesWillThisCommodityRuleApplyToPage(string checkboxLabel)
        {
            whichImportPurposesWillThisCommodityRuleApplyToPage?.TickCheckbox(checkboxLabel);
        }
    }
}