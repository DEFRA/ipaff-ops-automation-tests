using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Tools;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;
using System.Text.RegularExpressions;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmCheckYourAnswersSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IIdmCheckYourAnswersPage page => _objectContainer.Resolve<IIdmCheckYourAnswersPage>();
        public IdmCheckYourAnswersSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the Check your answers page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(page.IsPageLoaded(), Is.True);

        [Then("the registration review matches the entered details and offers change links")]
        public void ThenReviewMatchesEnteredDetails()
        {
            var business = _scenarioContext.Get<OperatorDetails>("RegistrationBusiness");
            var address = _scenarioContext.Get<string>("RegistrationAddress");
            var fullName = $"{_scenarioContext.Get<string>("RegistrationFirstName")} {_scenarioContext.Get<string>("RegistrationLastName")}";
            var credential = _objectContainer.Resolve<IUserObject>().GetUser("IPAFF", "Trader 1").Credential;
            Assert.That(address, Is.Not.Empty, "No selected business address recorded");
            var expected = new[]
            {
                ("Defra account", "Account type", _scenarioContext.Get<string>("RegistrationAccountType")),
                ("Business details", "Type", _scenarioContext.Get<string>("RegistrationOrganisationType")),
                ("Business details", "Trading name", business.OperatorName),
                ("Business details", "Trading address", address),
                ("Business details", "Phone number", business.TelephoneNumber),
                ("Business details", "Email", business.Email),
                ("Your details", "Name", fullName),
                ("Your details", "Phone number", _scenarioContext.Get<string>("RegistrationPhone"))
            };
            Assert.Multiple(() =>
            {
                foreach (var (section, field, value) in expected)
                {
                    var actual = page.GetSummaryValue(section, field);
                    if (field == "Trading address")
                        Assert.That(NormalizeAddress(actual), Is.EqualTo(NormalizeAddress(value)), $"{section}: {field}");
                    else
                        Assert.That(Regex.Replace(actual, @"\s+", " "), Is.EqualTo(value), $"{section}: {field}");
                    if (field != "Type")
                        Assert.That(page.HasChangeLink(section, field), Is.True, $"Missing Change link for {section}: {field}");
                }
                foreach (var field in new[] { "Memorable word", "Hint question" })
                {
                    Assert.That(page.GetSummaryValue("Your details", field).Equals(credential, StringComparison.Ordinal), Is.True, $"{field} does not match the configured Trader 1 credential");
                    Assert.That(page.HasChangeLink("Your details", field), Is.True, $"Missing Change link for {field}");
                }
            });
        }

        [Then("the registration review has no reCAPTCHA checkbox")]
        public void ThenRecaptchaIsAbsent() => Assert.That(page.IsRecaptchaAbsent(), Is.True, "reCAPTCHA must not be present");

        [When("the user completes registration")]
        public void WhenRegistrationIsCompleted() => page.CompleteRegistration();

        private static string NormalizeAddress(string address) => Regex.Replace(address, @"[\s,]+", " ").Trim().ToUpperInvariant();
    }
}
