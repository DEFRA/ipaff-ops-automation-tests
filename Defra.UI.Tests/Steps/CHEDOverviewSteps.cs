using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CHEDOverviewSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICHEDOverviewPage? chedOverviewPage => _objectContainer.IsRegistered<ICHEDOverviewPage>() ? _objectContainer.Resolve<ICHEDOverviewPage>() : null;

        public CHEDOverviewSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the CHED overview page should be displayed")]
        public void ThenTheCHEDOverviewPageShouldBeDisplayed()
        {
            Assert.True(chedOverviewPage?.IsPageLoaded(), "CHED overview page not loaded");
        }

        [Then("the CHED overview page should be displayed for the {string} notification")]
        public void ThenTheCHEDOverviewPageShouldBeDisplayedForTheNotification(string type)
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            var replacementChedReference = _scenarioContext.Get<string>("ReplacementCHEDReference");
            Assert.True(chedOverviewPage?.VerifyCHEDReference(type, chedReference, replacementChedReference));
        }

        [When("the user clicks Raise border notification button")]
        public void WhenTheUserClicksRaiseBorderNotificationButton()
        {
            chedOverviewPage?.ClickRaiseBorderNotification();
        }

        [When("the user clicks Copy as replacement button")]
        public void WhenTheUserClicksCopyAsReplacementButton()
        {
            chedOverviewPage?.ClickCopyAsReplacement();
        }

        [Then("link should be displayed as Replaced by along with {string} notification number")]
        public void ThenLinkShouldBeDisplayedAsReplacedByAlongWithNotificationNumber(string type)
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            var replacementChedReference = _scenarioContext.Get<string>("ReplacementCHEDReference");
            Assert.True(chedOverviewPage?.VerifyReplacedByLink(type, chedReference, replacementChedReference));
        }

        [When("the user clicks Replaced by link")]
        public void WhenTheUserClicksReplacedByLink()
        {
            chedOverviewPage?.ClickReplacedByLink();
        }
    }
}