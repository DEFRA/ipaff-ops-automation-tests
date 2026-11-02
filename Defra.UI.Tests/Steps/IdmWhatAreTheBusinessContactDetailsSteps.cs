using Defra.UI.Tests.Tools;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmWhatAreTheBusinessContactDetailsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IIdmWhatAreTheBusinessContactDetailsPage page => _objectContainer.Resolve<IIdmWhatAreTheBusinessContactDetailsPage>();
        public IdmWhatAreTheBusinessContactDetailsSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the What are the business contact details? page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(page.IsPageLoaded(), Is.True);

        [When("the user enters the business contact details and continues")]
        public void WhenContactDetailsAreEntered()
        {
            var business = _scenarioContext.Get<OperatorDetails>("RegistrationBusiness");
            page.EnterContactDetailsAndContinue(business.TelephoneNumber, business.Email);
        }
    }
}
