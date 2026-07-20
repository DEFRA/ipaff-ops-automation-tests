using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CHEDPImportsSteps
    {
        private readonly IObjectContainer _objectContainer;

        private ICHEDPImportsPage? chedPImportsPage =>
            _objectContainer.IsRegistered<ICHEDPImportsPage>()
                ? _objectContainer.Resolve<ICHEDPImportsPage>()
                : null;

        public CHEDPImportsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the CHED-P imports page should be displayed")]
        public void ThenTheCHEDPImportsPageShouldBeDisplayed()
        {
            Assert.True(chedPImportsPage?.IsPageLoaded(), "CHED-P imports page is not displayed");
        }

        [When("the user clicks the Bulk upload commodity rules link on the CHED-P imports page")]
        public void WhenTheUserClicksTheBulkUploadCommodityRulesLinkOnTheCHEDPImportsPage()
        {
            chedPImportsPage?.ClickBulkUploadCommodityRulesLink();
        }

        [When("the user clicks the Individual commodity rules link under the CHED-P rules header")]
        public void WhenTheUserClicksTheIndividualCommodityRulesLinkUnderTheCHED_PRulesHeader()
        {
            chedPImportsPage?.ClickIndividualCommodityRulesLink();
        }

        [When("the user clicks the Country rules link under the CHED-P rules header")]
        public void WhenTheUserClicksTheCountryRulesLinkUnderTheCHED_PRulesHeader()
        {
            chedPImportsPage?.ClickCountryRulesLink();
        }
    }
}