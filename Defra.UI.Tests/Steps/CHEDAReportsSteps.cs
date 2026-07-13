using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CHEDAReportsSteps
    {
        private readonly IObjectContainer _objectContainer;

        private ICHEDAReportsPage? chedAReportsPage =>
            _objectContainer.IsRegistered<ICHEDAReportsPage>()
                ? _objectContainer.Resolve<ICHEDAReportsPage>()
                : null;

        public CHEDAReportsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the CHED-A reports page should be displayed")]
        public void ThenTheCHEDAReportsPageShouldBeDisplayed()
        {
            Assert.True(chedAReportsPage?.IsPageLoaded(), "CHED-A reports page is not displayed");
        }

        [When("the user clicks the CHED-A Imports commodity rules report link")]
        public void WhenTheUserClicksTheCHEDAImportsCommodityRulesReportLink()
        {
            chedAReportsPage?.ClickImportsCommodityRulesReportLink();
        }

        [When("the user clicks the CHED-A Risk decision report link")]
        public void WhenTheUserClicksTheCHEDARiskDecisionReportLink()
        {
            chedAReportsPage?.ClickRiskDecisionReportLink();
        }
    }
}