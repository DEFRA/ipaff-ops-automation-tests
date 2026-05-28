using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CHEDDImportsSteps
    {
        private readonly IObjectContainer _objectContainer;

        private ICHEDDImportsPage? chedDImportsPage =>
            _objectContainer.IsRegistered<ICHEDDImportsPage>()
                ? _objectContainer.Resolve<ICHEDDImportsPage>()
                : null;

        public CHEDDImportsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the CHED-D imports page should be displayed")]
        public void ThenTheCHEDDImportsPageShouldBeDisplayed()
        {
            Assert.True(chedDImportsPage?.IsPageLoaded(), "CHED-D imports page is not displayed");
        }

        [When("the user clicks the Bulk upload commodity rules link on the CHED-D imports page")]
        public void WhenTheUserClicksTheBulkUploadCommodityRulesLinkOnTheCHEDDImportsPage()
        {
            chedDImportsPage?.ClickBulkUploadCommodityRulesLink();
        }
    }
}