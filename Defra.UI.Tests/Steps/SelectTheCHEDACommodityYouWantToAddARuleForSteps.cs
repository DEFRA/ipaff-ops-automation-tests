using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class SelectTheCHEDACommodityYouWantToAddARuleForSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ISelectTheCHEDACommodityYouWantToAddARuleForPage? selectTheCHEDACommodityYouWantToAddARuleForPage =>
            _objectContainer.IsRegistered<ISelectTheCHEDACommodityYouWantToAddARuleForPage>()
                ? _objectContainer.Resolve<ISelectTheCHEDACommodityYouWantToAddARuleForPage>()
                : null;

        public SelectTheCHEDACommodityYouWantToAddARuleForSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Select the CHED-A commodity you want to add a rule for page should be displayed")]
        public void ThenTheSelectTheCHED_ACommodityYouWantToAddARuleForPageShouldBeDisplayed()
        {
            Assert.True(selectTheCHEDACommodityYouWantToAddARuleForPage?.IsPageLoaded(), "Select the CHED-A commodity you want to add a rule for page is not displayed");
        }

        [When("the user searches for commodity {string} with name {string} on the Select the CHED-A commodity you want to add a rule for page")]
        public void WhenTheUserSearchesForCommodityWithNameOnTheSelectTheCHED_ACommodityYouWantToAddARuleForPage(string commodityCode, string commodityName)
        {
            selectTheCHEDACommodityYouWantToAddARuleForPage?.SearchCommodity(commodityCode, commodityName);
        }
    }
}