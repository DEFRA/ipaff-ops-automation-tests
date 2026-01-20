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

        [Then("the BTMS search result screen should be displayed for the replacement CHED reference")]
        public void ThenTheBTMSSearchResultScreenShouldBeDisplayedForTheReplacementCHEDReference()
        {
            var ReplacementCHEDPREFNum = _scenarioContext.Get<string>("ReplacementCHEDReference");
            Assert.True(btmsSearchResultPage?.IsPageLoadedForReplacementCHED(ReplacementCHEDPREFNum), "Showing result for CHED page not loaded");
        }

        [Then("the user checks commodity code {string}, description {string}, quantity {string}, authority {string} and decision {string} after the decision given")]
        [Then("the user checks commodity code {string}, description {string}, quantity {string}, authority {string} and decision {string}")]
        public void ThenTheUserChecksCommodityCodeDescriptionQuantityAuthorityAndDecision(string commodityCode, string commodityDescription, string commodityQuantity, string authority, string decision)
        {
            Assert.True(btmsSearchResultPage?.ValidateBTMSSearchResult(commodityCode, commodityDescription, commodityQuantity, authority, decision));

        }  

        [Then("the user validates the commodity code {string}, description {string}, quantity {string}, authority {string} and decision {string} for commodity {string} after the decision is given")]
        public void ThenTheUserValidatesTheCommodityCodeDescriptionQuantityAuthorityAndDecisionForCommodityAfterTheDecisionIsGiven(string commCode, string desc, string quantity, string authority, string decision, string commodityNum)
        {
            Assert.Multiple(() =>
            {
                Assert.True(commCode.Equals(btmsSearchResultPage?.GetCommodityCode(commodityNum)), $"Commodity code of commodity number {commodityNum} did not match");
                Assert.True(desc.Equals(btmsSearchResultPage?.GetCommodityDesc(commodityNum)), $"Commodity description of commodity number {commodityNum} did not match");
                Assert.True(quantity.Equals(btmsSearchResultPage?.GetCommodityQuantity(commodityNum)), $"Commodity quantity of commodity number {commodityNum} did not match");
                Assert.True(authority.Equals(btmsSearchResultPage?.GetCommodityAuthority(commodityNum)), $"Commodity authority of commodity number {commodityNum} did not match");
                Assert.True(decision.Equals(btmsSearchResultPage?.GetCommodityDecision(commodityNum)), $"Commodity decision of commodity number {commodityNum} did not match");
            });
        }

        [When("the user logs out of BTMS")]
        public void WhenTheUserLogsOutOfBTMS()
        {
            signOutPage?.BTMSSignOut();
        }

        [Then("the CHED status should be {string} in BTMS search result page")]
        public void ThenTheCHEDStatusShouldBeInBTMSSearchResultPage(string status)
        {
            Assert.True(btmsSearchResultPage?.VerifyStatus(status));
        }
    }
}