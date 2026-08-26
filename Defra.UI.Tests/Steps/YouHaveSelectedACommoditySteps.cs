using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class YouHaveSelectedACommoditySteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IYouHaveSelectedACommodityPage? youHaveSelectedACommodityPage =>
            _objectContainer.IsRegistered<IYouHaveSelectedACommodityPage>()
                ? _objectContainer.Resolve<IYouHaveSelectedACommodityPage>()
                : null;

        public YouHaveSelectedACommoditySteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the You have selected a commodity page should be displayed showing commodity {string} with description {string}")]
        public void ThenTheYouHaveSelectedACommodityPageShouldBeDisplayedShowingCommodityWithDescription(string commodityCode, string description)
        {
            Assert.True(youHaveSelectedACommodityPage?.IsPageLoaded(), "You have selected a commodity page is not displayed");
            Assert.True(youHaveSelectedACommodityPage?.IsCommodityDisplayed(commodityCode, description), $"Commodity {commodityCode} with description {description} is not displayed");
        }

        [When("the user clicks the Continue button on the You have selected a commodity page")]
        public void WhenTheUserClicksTheContinueButtonOnTheYouHaveSelectedACommodityPage()
        {
            youHaveSelectedACommodityPage?.ClickContinueButton();
        }
    }
}