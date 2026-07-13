using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class WillThisCommodityRuleApplyToAllBorderControlPostsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWillThisCommodityRuleApplyToAllBorderControlPostsPage? willThisCommodityRuleApplyToAllBorderControlPostsPage =>
            _objectContainer.IsRegistered<IWillThisCommodityRuleApplyToAllBorderControlPostsPage>()
                ? _objectContainer.Resolve<IWillThisCommodityRuleApplyToAllBorderControlPostsPage>()
                : null;

        public WillThisCommodityRuleApplyToAllBorderControlPostsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Will this commodity rule apply to all border control posts? page should be displayed")]
        public void ThenTheWillThisCommodityRuleApplyToAllBorderControlPostsPageShouldBeDisplayed()
        {
            Assert.True(willThisCommodityRuleApplyToAllBorderControlPostsPage?.IsPageLoaded(), "Will this commodity rule apply to all border control posts? page is not displayed");
        }

        [When("the user selects the {string} radio button on the Will this commodity rule apply to all border control posts? page")]
        public void WhenTheUserSelectsTheRadioButtonOnTheWillThisCommodityRuleApplyToAllBorderControlPostsPage(string option)
        {
            willThisCommodityRuleApplyToAllBorderControlPostsPage?.SelectRadioButton(option);
        }

        [When("the user clicks the Continue button on the Will this commodity rule apply to all border control posts? page")]
        public void WhenTheUserClicksTheContinueButtonOnTheWillThisCommodityRuleApplyToAllBorderControlPostsPage()
        {
            willThisCommodityRuleApplyToAllBorderControlPostsPage?.ClickContinueButton();
        }
    }
}