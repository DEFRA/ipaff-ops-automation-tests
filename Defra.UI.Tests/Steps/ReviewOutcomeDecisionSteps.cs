using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
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
    }
}