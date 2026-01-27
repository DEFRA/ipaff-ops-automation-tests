using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class RequestAmendmentSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IRequestAmendmentPage? requestAmendmentPage => _objectContainer.IsRegistered<IRequestAmendmentPage>() 
            ? _objectContainer.Resolve<IRequestAmendmentPage>() 
            : null;

        public RequestAmendmentSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Request that the responsible person amends this CHED page should be displayed")]
        public void ThenTheRequestThatTheResponsiblePersonAmendsThisCHEDPageShouldBeDisplayed()
        {
            Assert.True(requestAmendmentPage?.IsPageLoaded(), 
                "Request that the responsible person amends this CHED page not loaded");
        }

        [When("the user enters amendment reason {string}")]
        public void WhenTheUserEntersAmendmentReason(string reason)
        {
            requestAmendmentPage?.EnterAmendmentReason(reason);
        }

        [When("the user clicks Request amendment button")]
        public void WhenTheUserClicksRequestAmendmentButton()
        {
            requestAmendmentPage?.ClickRequestAmendmentButton();
        }

        [When("the user clicks Do not request amendment")]
        public void WhenTheUserClicksDoNotRequestAmendment()
        {
            requestAmendmentPage?.ClickDoNotRequestAmendment();
        }

        [Then("the CHED reference should be {string}")]
        public void ThenTheCHEDReferenceShouldBe(string expectedReference)
        {
            var actualReference = requestAmendmentPage?.GetCHEDReference();
            Assert.AreEqual(expectedReference, actualReference, 
                $"CHED reference mismatch. Expected: {expectedReference}, Actual: {actualReference}");
        }

        [Then("the status on Request Amendment page should be {string}")]
        public void ThenTheStatusOnRequestAmendmentPageShouldBe(string expectedStatus)
        {
            var actualStatus = requestAmendmentPage?.GetStatus();
            Assert.AreEqual(expectedStatus, actualStatus, 
                $"Status mismatch. Expected: {expectedStatus}, Actual: {actualStatus}");
        }
    }
}