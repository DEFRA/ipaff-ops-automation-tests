using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class SelectCommodityLevelSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ISelectCommodityLevelPage? selectCommodityLevelPage =>
            _objectContainer.IsRegistered<ISelectCommodityLevelPage>()
                ? _objectContainer.Resolve<ISelectCommodityLevelPage>()
                : null;

        public SelectCommodityLevelSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Select the commodity level screen should be displayed")]
        public void ThenTheSelectTheCommodityLevelScreenShouldBeDisplayed()
        {
            Assert.True(
                selectCommodityLevelPage?.IsPageLoaded(),
                "Select the commodity level page is not loaded");
        }

        [When("the user clicks Select for the commodity code from the notification")]
        public void WhenTheUserClicksSelectForTheCommodityCodeFromTheNotification()
        {
            var descriptions = _scenarioContext.Get<List<string>>("CommodityDescription");
            selectCommodityLevelPage?.SelectCommodityByDescription(descriptions.First());
            selectCommodityLevelPage?.ClickSelect();
        }
    }
}