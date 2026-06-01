using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CHEDPImportsAndExportsSteps
    {
        private readonly IObjectContainer _objectContainer;

        private ICHEDPImportsAndExportsPage? chedPImportsAndExportsPage =>
            _objectContainer.IsRegistered<ICHEDPImportsAndExportsPage>()
                ? _objectContainer.Resolve<ICHEDPImportsAndExportsPage>()
                : null;

        public CHEDPImportsAndExportsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the CHED-P imports and exports page should be displayed")]
        public void ThenTheCHEDPImportsAndExportsPageShouldBeDisplayed()
        {
            Assert.True(chedPImportsAndExportsPage?.IsPageLoaded(), "CHED-P imports and exports page is not displayed");
        }

        [When("the user clicks the Bulk upload commodity rules link on the CHED-P imports page")]
        public void WhenTheUserClicksTheBulkUploadCommodityRulesLinkOnTheCHEDPImportsPage()
        {
            chedPImportsAndExportsPage?.ClickBulkUploadCommodityRulesLink();
        }
    }
}