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

        [When("the user clicks the HMI import commodity rules link")]
        public void WhenTheUserClicksTheHMIImportCommodityRulesLink()
        {
            chedPPImportsAndExportsPage?.ClickHMIImportCommodityRulesLink();
        }

        [When("the user clicks the HMI export commodity rules link")]
        public void WhenTheUserClicksTheHMIExportCommodityRulesLink()
        {
            chedPPImportsAndExportsPage?.ClickHMIExportCommodityRulesLink();
        }

        [When("the user clicks the PHSI individual commodity rules link")]
        public void WhenTheUserClicksThePHSIIndividualCommodityRulesLink()
        {
            chedPPImportsAndExportsPage?.ClickPHSIIndividualCommodityRulesLink();
        }

        [When("the user clicks the System settings for HMI rules link")]
        public void WhenTheUserClicksTheSystemSettingsForHMIRulesLink()
        {
            chedPPImportsAndExportsPage?.ClickSystemSettingsForHMIRulesLink();
        }

        [When("the user clicks the Country rules link under the HMI imports and exports rules header")]
        public void WhenTheUserClicksTheCountryRulesLinkUnderTheHMIImportsAndExportsRulesHeader()
        {
            chedPPImportsAndExportsPage?.ClickCountryRulesLink();
        }
    }
}