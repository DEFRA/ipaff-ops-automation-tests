using Defra.UI.Tests.Tools;
using Defra.UI.Tests.HelperMethods;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmEnterYourEmailAddressSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IIdmEnterYourEmailAddressPage idmEnterYourEmailAddressPage => _objectContainer.Resolve<IIdmEnterYourEmailAddressPage>();

        public IdmEnterYourEmailAddressSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the Enter your email address page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(idmEnterYourEmailAddressPage.IsPageLoaded(), Is.True);

        [When("the user enters a unique registration email address for {string} and continues")]
        public void WhenEmailIsEntered(string country)
        {
            if (!country.Equals("UK", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("This registration scenario requires a UK postcode lookup", nameof(country));

            var inbox = DateTime.Now.ToString("ddMMyyHHmmss");
            var email = $"{inbox}@{_objectContainer.Resolve<IFetchCodeFromEmail>().DomainName}";

            _scenarioContext.Set(email, "RegistrationEmail");
            _scenarioContext.Set(Utils.GenerateRandomUKPhonenumber(), "RegistrationPhone");
            idmEnterYourEmailAddressPage.EnterEmailAddress(email);
            idmEnterYourEmailAddressPage.ClickContinueButton();
        }
    }
}
