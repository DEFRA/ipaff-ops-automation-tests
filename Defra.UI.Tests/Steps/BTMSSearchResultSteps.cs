using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;


namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class BTMSSearchResultSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IBTMSSearchResultPage? btmsSearchResultPage => _objectContainer.IsRegistered<IBTMSSearchResultPage>() ? _objectContainer.Resolve<IBTMSSearchResultPage>() : null;
        private ISignOutPage? signOutPage => _objectContainer.IsRegistered<ISignOutPage>() ? _objectContainer.Resolve<ISignOutPage>() : null;

        public BTMSSearchResultSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the BTMS search result screen should be displayed")]
        public void ThenTheBTMSSearchResultScreenShouldBeDisplayed()
        {
            Assert.True(btmsSearchResultPage?.IsPageLoaded(), "Showing result for CHED page not loaded");
        }


        [Then("the user checks commodity code, description, quantity, authority and decision")]
        public void ThenTheUserChecksCommodityCodeDescriptionQuantityAuthorityAndDecision()
        {
            var commodityCode = "41015050";
            var commodityDescription = "Bison bison";
            var commodityQuantity = "1000";
            var authority = "POAO";
            var decision = "Decision not given";
            Assert.True(btmsSearchResultPage?.ValidateBTMSSearchResult(commodityCode, commodityDescription, commodityQuantity, authority, decision));
        }

        [When("the user logs out of BTMS")]
        public void WhenTheUserLogsOutOfBTMS()
        {
            signOutPage?.BTMSSignOut();
        }
    }
}