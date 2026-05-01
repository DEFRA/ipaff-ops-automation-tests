using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CHEDPPReportsSteps
    {
        private readonly IObjectContainer _objectContainer;

        private ICHEDPPReportsPage? chedPPReportsPage =>
            _objectContainer.IsRegistered<ICHEDPPReportsPage>()
                ? _objectContainer.Resolve<ICHEDPPReportsPage>()
                : null;

        public CHEDPPReportsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the CHED-PP reports page should be displayed")]
        public void ThenTheCHEDPPReportsPageShouldBeDisplayed()
        {
            Assert.True(chedPPReportsPage?.IsPageLoaded(), "CHED-PP reports page is not displayed");
        }

        [When("the user clicks the PHSI imports commodity rules report link")]
        public void WhenTheUserClicksThePHSIImportsCommodityRulesReportLink()
        {
            chedPPReportsPage?.ClickPHSIImportsCommodityRulesReportLink();
        }

        [When("the user clicks the Risk decision report link")]
        public void WhenTheUserClicksTheRiskDecisionReportLink()
        {
            chedPPReportsPage?.ClickRiskDecisionReportLink();
        }
    }
}