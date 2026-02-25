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
        
        [When("the user clicks Clear all in consinments requiring control page")]
        public void WhenTheUserClicksClearAll()
        {
            chedOverviewPage?.ClickClearAll();
        }

        [Then("Show CHED button is displayed")]
        public void TheShowChedButtonIsDisplayed()
        {
            Assert.IsTrue(chedOverviewPage?.VerifyShowChedButton() , "The Show CHED button is not present");
        }

        [Then("the user verifies {string} tab in CHED Overview page")]
        public void ThenTheUserVerifiesTabInChedOverviewPage(string tabName)
        {
            Assert.IsTrue(chedOverviewPage?.VerifyTab(tabName), "The " + tabName + " is not present");
        }
        
        [Then("the user verifies the value is present for {string}")]
        public void ThenTheUserVerifiesValueIsPresent(string fieldName)
        {
            Assert.IsTrue(chedOverviewPage?.IsFieldValuePresent(fieldName), "The field value for " + fieldName + " is not present");
        }
        
        [Then("the user verifies the value is present for {string} under {string}")]
        public void ThenTheUserVerifiesValueIsPresent(string fieldName, string sectionName)
        {
            Assert.IsTrue(chedOverviewPage?.IsFieldValuePresent(fieldName,sectionName), "The field value for " + fieldName + " is not present");
        }
        
        [Then("the user verifies the value is present for {string} in {string} column")]
        public void ThenTheUserVerifiesValueIsPresentInColumn(string fieldName, string column)
        {
            Assert.IsTrue(chedOverviewPage?.IsFieldValuePresentInTable(fieldName, column), "The field value for " + fieldName + " is not present in "+column);
        }

        [When("the user switches to {string} tab in CHED Overview page")]
        public void WhenTheUserSwitchesToTabInChedOverviewPage(string tabName)
        {
            chedOverviewPage?.SwitchTab(tabName);
        }

        [Then("verifies Risk decision PHSI is set to {string}")]
        public void ThenVerifiesRiskDecisionPHSIIsSetTo(string decision)
        {
            Assert.True(chedOverviewPage?.VerifyRiskDecisionPHSI(decision));
        }

        [Then("verifies Document check is set to {string}")]
        public void ThenVerifiesDocumentCheckIsSetTo(string status)
        {
            Assert.True(chedOverviewPage?.VerifyDocumentCheck(status));
        }

        [Then("verifies Risk decision HMI is set to {string}")]
        public void ThenVerifiesRiskDecisionHMIIsSetTo(string decision)
        {
            Assert.True(chedOverviewPage?.VerifyRiskDecisionHMI(decision));
        }

        [Then("verifies {string} field is set to {string}")]
        public void ThenVerifiesFieldIsSetTo(string fieldName, string status)
        {
            Assert.True(chedOverviewPage?.VerifyDecisionRecordedBy(fieldName, status));
        }
    }
}