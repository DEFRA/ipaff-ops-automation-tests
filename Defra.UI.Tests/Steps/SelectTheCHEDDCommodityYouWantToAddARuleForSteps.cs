using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class SelectTheCHEDDCommodityYouWantToAddARuleForSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ISelectTheCHEDDCommodityYouWantToAddARuleForPage? selectTheCHEDDCommodityYouWantToAddARuleForPage =>
            _objectContainer.IsRegistered<ISelectTheCHEDDCommodityYouWantToAddARuleForPage>()
                ? _objectContainer.Resolve<ISelectTheCHEDDCommodityYouWantToAddARuleForPage>()
                : null;

        public SelectTheCHEDDCommodityYouWantToAddARuleForSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Select the CHED-D commodity you want to add a rule for page should be displayed")]
        public void ThenTheSelectTheCHED_DCommodityYouWantToAddARuleForPageShouldBeDisplayed()
        {
            Assert.True(selectTheCHEDDCommodityYouWantToAddARuleForPage?.IsPageLoaded(), "Select the CHED-D commodity you want to add a rule for page is not displayed");
        }

        [When("the user searches for commodity {string} with name {string} on the Select the CHED-D commodity you want to add a rule for page")]
        public void WhenTheUserSearchesForCommodityWithNameOnTheSelectTheCHED_DCommodityYouWantToAddARuleForPage(string commodityCode, string commodityName)
        {
            selectTheCHEDDCommodityYouWantToAddARuleForPage?.SearchCommodity(commodityCode, commodityName);
        }
    }
}