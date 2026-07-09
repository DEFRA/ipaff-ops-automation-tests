using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CommodityRuleSubmittedSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICommodityRuleSubmittedPage? commodityRuleSubmittedPage =>
            _objectContainer.IsRegistered<ICommodityRuleSubmittedPage>()
                ? _objectContainer.Resolve<ICommodityRuleSubmittedPage>()
                : null;

        public CommodityRuleSubmittedSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Commodity rule submitted page should be displayed")]
        public void ThenTheCommodityRuleSubmittedPageShouldBeDisplayed()
        {
            Assert.True(commodityRuleSubmittedPage?.IsPageLoaded(), "Commodity rule submitted page is not displayed");
        }
    }
}