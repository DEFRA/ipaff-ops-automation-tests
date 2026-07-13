using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class DoesThisCommodityRuleHaveAStartOrEndDateSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IDoesThisCommodityRuleHaveAStartOrEndDatePage? doesThisCommodityRuleHaveAStartOrEndDatePage =>
            _objectContainer.IsRegistered<IDoesThisCommodityRuleHaveAStartOrEndDatePage>()
                ? _objectContainer.Resolve<IDoesThisCommodityRuleHaveAStartOrEndDatePage>()
                : null;

        public DoesThisCommodityRuleHaveAStartOrEndDateSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Does this commodity rule have a start or end date? page should be displayed")]
        public void ThenTheDoesThisCommodityRuleHaveAStartOrEndDatePageShouldBeDisplayed()
        {
            Assert.True(doesThisCommodityRuleHaveAStartOrEndDatePage?.IsPageLoaded(), "Does this commodity rule have a start or end date? page is not displayed");
        }

        [When("the user selects the {string} radio button on the Does this commodity rule have a start or end date? page")]
        public void WhenTheUserSelectsTheRadioButtonOnTheDoesThisCommodityRuleHaveAStartOrEndDatePage(string option)
        {
            doesThisCommodityRuleHaveAStartOrEndDatePage?.SelectRadioButton(option);
        }

        [When("the user clicks the Continue button on the Does this commodity rule have a start or end date? page")]
        public void WhenTheUserClicksTheContinueButtonOnTheDoesThisCommodityRuleHaveAStartOrEndDatePage()
        {
            doesThisCommodityRuleHaveAStartOrEndDatePage?.ClickContinueButton();
        }
    }
}