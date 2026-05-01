using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ReportsSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IReportsPage? reportsPage =>
            _objectContainer.IsRegistered<IReportsPage>()
                ? _objectContainer.Resolve<IReportsPage>()
                : null;

        public ReportsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Risk Engine Reports page should be displayed")]
        public void ThenTheRiskEngineReportsPageShouldBeDisplayed()
        {
            Assert.True(reportsPage?.IsPageLoaded(), "Reports page is not displayed");
        }

        [When("the user clicks the CHED-PP reports link")]
        public void WhenTheUserClicksTheCHEDPPReportsLink()
        {
            reportsPage?.ClickChedPPReportsLink();
        }
    }
}