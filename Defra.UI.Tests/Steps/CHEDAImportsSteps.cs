using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CHEDAImportsSteps
    {
        private readonly IObjectContainer _objectContainer;

        private ICHEDAImportsPage? chedAImportsPage =>
            _objectContainer.IsRegistered<ICHEDAImportsPage>()
                ? _objectContainer.Resolve<ICHEDAImportsPage>()
                : null;

        public CHEDAImportsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the CHED-A imports page should be displayed")]
        public void ThenTheCHEDAImportsPageShouldBeDisplayed()
        {
            Assert.True(chedAImportsPage?.IsPageLoaded(), "CHED-A imports page is not displayed");
        }

        [When("the user clicks the Bulk upload commodity rules link on the CHED-A imports page")]
        public void WhenTheUserClicksTheBulkUploadCommodityRulesLinkOnTheCHEDAImportsPage()
        {
            chedAImportsPage?.ClickBulkUploadCommodityRulesLink();
        }

        [When("the user clicks the Individual commodity rules link under the CHED-A rules header")]
        public void WhenTheUserClicksTheIndividualCommodityRulesLinkUnderTheCHED_ARulesHeader()
        {
            chedAImportsPage?.ClickIndividualCommodityRulesLink();
        }

        [When("the user clicks the Country rules link under the CHED-A rules header")]
        public void WhenTheUserClicksTheCountryRulesLinkUnderTheCHED_ARulesHeader()
        {
            chedAImportsPage?.ClickCountryRulesLink();
        }
    }
}