using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class ReviewOutcomeDecisionSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IReviewOutcomeDecisionPage? reviewOutcomeDecisionPage => _objectContainer.IsRegistered<IReviewOutcomeDecisionPage>() ? _objectContainer.Resolve<IReviewOutcomeDecisionPage>() : null;
        private ISummaryPage? summaryPage => _objectContainer.IsRegistered<ISummaryPage>() ? _objectContainer.Resolve<ISummaryPage>() : null;

        public ReviewOutcomeDecisionSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Review outcome decision page should be displayed")]
        public void ThenTheReviewOutcomeDecisionPageShouldBeDisplayed()
        {
            Assert.True(reviewOutcomeDecisionPage?.IsPageLoaded(), "Review outcome decision page not loaded");
        }

        [When("the user populates the Date and time of checks")]
        public void WhenTheUserPopulatesTheDateAndTimeOfChecks()
        {
            DateTime today = DateTime.Now;
            string day = today.Day.ToString();
            string month = today.Month.ToString();
            string year = today.Year.ToString();
            string hours = today.Hour.ToString();
            string minutes = today.Minute.ToString();
            reviewOutcomeDecisionPage?.EnterCurrentDateAndTime(day, month, year, hours, minutes);
        }

        [When("user clicks Submit decision")]
        public void WhenUserClicksSubmitDecision()
        {
            reviewOutcomeDecisionPage?.ClickSubmitDecision();
        }

/*        [Then("the user verifies all the data displayed in review outcome decision page")]
        public void ThenTheUserVerifiesAllTheDataDisplayedInReviewOutcomeDecisionPage()
        {
            var summary = summaryPage?.GetSummaryDetails();
            var pageName = "Review outcome decision";

            var BCPReferenceNumber = _scenarioContext.Get<string>("CustomsDeclarationReference");
            var DocumentaryCheck = _scenarioContext.Get<string>("DocumentaryCheckDecision");
            var IdentityCheckType = _scenarioContext.Get<string>("IdentityCheckType");
            var IdentityCheckDecision = _scenarioContext.Get<string>("IdentityCheckDecision");
            var PhysicalCheckDecision = _scenarioContext.Get<string>("PhysicalCheckDecision");

            Assert.AreEqual(BCPReferenceNumber, summary?.BCPRefNum, $"BCP Reference Number is not matching in {pageName} page!");
            Assert.AreEqual(DocumentaryCheck, summary?.DocumentaryChk, $"Documentary Check is not matching in {pageName} page!");
            Assert.AreEqual(IdentityCheckType, summary?.IdentityChkType, $"Identity Check Type is not matching in {pageName} page!");
            Assert.AreEqual(IdentityCheckDecision, summary?.IdentityChkDecision, $"Identity Check Decision is not matching in {pageName} page!");
            Assert.AreEqual(PhysicalCheckDecision, summary?.PhysicalChkDecision, $"Physical Check Decision is not matching in {pageName} page!");
        }   */
    }
}