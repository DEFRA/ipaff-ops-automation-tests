using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CHEDDReportsSteps
    {
        private readonly IObjectContainer _objectContainer;

        private ICHEDDReportsPage? chedDReportsPage =>
            _objectContainer.IsRegistered<ICHEDDReportsPage>()
                ? _objectContainer.Resolve<ICHEDDReportsPage>()
                : null;

        public CHEDDReportsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the CHED-D reports page should be displayed")]
        public void ThenTheCHEDDReportsPageShouldBeDisplayed()
        {
            Assert.True(chedDReportsPage?.IsPageLoaded(), "CHED-D reports page is not displayed");
        }

        [When("the user clicks the CHED-D Imports commodity rules report link")]
        public void WhenTheUserClicksTheCHEDDImportsCommodityRulesReportLink()
        {
            chedDReportsPage?.ClickImportsCommodityRulesReportLink();
        }

        [When("the user clicks the CHED-D Risk decision report link")]
        public void WhenTheUserClicksTheCHEDDRiskDecisionReportLink()
        {
            chedDReportsPage?.ClickRiskDecisionReportLink();
        }
    }
}