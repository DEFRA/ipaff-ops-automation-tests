using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CHEDPPImportsAndExportsSteps
    {
        private readonly IObjectContainer _objectContainer;

        private ICHEDPPImportsAndExportsPage? chedPPImportsAndExportsPage =>
            _objectContainer.IsRegistered<ICHEDPPImportsAndExportsPage>()
                ? _objectContainer.Resolve<ICHEDPPImportsAndExportsPage>()
                : null;

        public CHEDPPImportsAndExportsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the CHED-PP imports and exports page should be displayed")]
        public void ThenTheCHEDPPImportsAndExportsPageShouldBeDisplayed()
        {
            Assert.True(chedPPImportsAndExportsPage?.IsPageLoaded(), "CHED-PP imports and exports page is not displayed");
        }

        [When("the user clicks the Bulk upload commodity rules link")]
        public void WhenTheUserClicksTheBulkUploadCommodityRulesLink()
        {
            chedPPImportsAndExportsPage?.ClickBulkUploadCommodityRulesLink();
        }
    }
}