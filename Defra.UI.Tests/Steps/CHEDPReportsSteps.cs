using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CHEDPReportsSteps
    {
        private readonly IObjectContainer _objectContainer;

        private ICHEDPReportsPage? chedPReportsPage =>
            _objectContainer.IsRegistered<ICHEDPReportsPage>()
                ? _objectContainer.Resolve<ICHEDPReportsPage>()
                : null;

        public CHEDPReportsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the CHED-P reports page should be displayed")]
        public void ThenTheCHEDPReportsPageShouldBeDisplayed()
        {
            Assert.True(chedPReportsPage?.IsPageLoaded(), "CHED-P reports page is not displayed");
        }

        [When("the user clicks the CHED-P imports commodity rules report link")]
        public void WhenTheUserClicksTheCHEDPImportsCommodityRulesReportLink()
        {
            chedPReportsPage?.ClickImportsCommodityRulesReportLink();
        }

        [When("the user clicks the CHED-P Risk decision report link")]
        public void WhenTheUserClicksTheCHEDPRiskDecisionReportLink()
        {
            chedPReportsPage?.ClickRiskDecisionReportLink();
        }

        [When("the user clicks the Country rules report link under the CHED-P reports header")]
        public void WhenTheUserClicksTheCountryRulesReportLinkUnderTheCHED_PReportsHeader()
        {
            chedPReportsPage?.ClickCountryRulesReportLink();
        }
    }
}