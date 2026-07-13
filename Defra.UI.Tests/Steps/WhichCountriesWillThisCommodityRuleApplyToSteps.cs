using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class WhichCountriesWillThisCommodityRuleApplyToSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWhichCountriesWillThisCommodityRuleApplyToPage? whichCountriesWillThisCommodityRuleApplyToPage =>
            _objectContainer.IsRegistered<IWhichCountriesWillThisCommodityRuleApplyToPage>()
                ? _objectContainer.Resolve<IWhichCountriesWillThisCommodityRuleApplyToPage>()
                : null;

        public WhichCountriesWillThisCommodityRuleApplyToSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Which countries will this commodity rule apply to? page should be displayed")]
        public void ThenTheWhichCountriesWillThisCommodityRuleApplyToPageShouldBeDisplayed()
        {
            Assert.True(whichCountriesWillThisCommodityRuleApplyToPage?.IsPageLoaded(), "Which countries will this commodity rule apply to? page is not displayed");
        }

        [When("the user selects {string} in the Countries field on the Which countries will this commodity rule apply to? page")]
        public void WhenTheUserSelectsInTheCountriesFieldOnTheWhichCountriesWillThisCommodityRuleApplyToPage(string country)
        {
            whichCountriesWillThisCommodityRuleApplyToPage?.SelectCountry(country);
        }

        [When("the user clicks the Continue button on the Which countries will this commodity rule apply to? page")]
        public void WhenTheUserClicksTheContinueButtonOnTheWhichCountriesWillThisCommodityRuleApplyToPage()
        {
            whichCountriesWillThisCommodityRuleApplyToPage?.ClickContinueButton();
        }
    }
}