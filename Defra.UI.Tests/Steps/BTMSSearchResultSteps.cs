using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using AventStack.ExtentReports.Gherkin.Model;


namespace Defra.UI.Tests.Steps.IPAFF
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
            var CHEDPREFNum = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(btmsSearchResultPage?.IsPageLoaded(CHEDPREFNum), "Showing result for CHED page not loaded");
        }


        [Then("the user checks commodity code {string}, description {string}, quantity {string}, authority {string} and decision {string}")]
        public void ThenTheUserChecksCommodityCodeDescriptionQuantityAuthorityAndDecision(string commodityCode, string commodityDescription, string commodityQuantity, string authority, string decision)
        {
            Assert.True(btmsSearchResultPage?.ValidateBTMSSearchResult(commodityCode, commodityDescription, commodityQuantity, authority, decision));
        }
       
        
        [Then("the user checks commodity code {string}, description {string}, quantity {string}, authority {string} and decision {string} after the decision given")]
        public void ThenTheUserChecksCommodityCodeDescriptionQuantityAuthorityAndDecisionAfterTheDecisionGiven(string commodityCode, string commodityDescription, string commodityQuantity, string authority, string decision)
        {
            Assert.True(btmsSearchResultPage?.ValidateBTMSSearchResult(commodityCode, commodityDescription, commodityQuantity, authority, decision));
        }

        [When("the user logs out of BTMS")]
        public void WhenTheUserLogsOutOfBTMS()
        {
            signOutPage?.BTMSSignOut();
        }
    }
}