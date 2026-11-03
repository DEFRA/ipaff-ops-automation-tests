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
        private IIdmCheckYourAnswersPage idmCheckYourAnswersPage => _objectContainer.Resolve<IIdmCheckYourAnswersPage>();
        public IdmCheckYourAnswersSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the Check your answers page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(idmCheckYourAnswersPage.IsPageLoaded(), Is.True);

        [Then("the details on the Check your answers page match the entered details")]
        public void ThenReviewMatchesEnteredDetails()
        {
            var allDataMatches = true;
            var mismatches = new List<string>();
            var address = _scenarioContext.Get<string>("RegistrationAddress");
            var fullName = $"{_scenarioContext.Get<string>("RegistrationFirstName")} {_scenarioContext.Get<string>("RegistrationLastName")}";
            var credential = _objectContainer.Resolve<IUserObject>().GetUser("IPAFF", "Trader 1").Credential;
            Assert.That(address, Is.Not.Empty, "No selected business address recorded");

            var expected = new List<(string Section, string Field, string Value)>
            {
                ("Defra account", "Account type", _scenarioContext.Get<string>("RegistrationAccountType")),
                ("Business details", "Type", _scenarioContext.Get<string>("RegistrationOrganisationType")),
                ("Business details", "Trading name", _scenarioContext.Get<string>("RegistrationBusinessName")),
                ("Business details", "Trading address", address),
                ("Business details", "Phone number", _scenarioContext.Get<string>("RegistrationBusinessPhone")),
                ("Business details", "Email", _scenarioContext.Get<string>("RegistrationEmail")),
                ("Your details", "Name", fullName),
                ("Your details", "Phone number", _scenarioContext.Get<string>("RegistrationPhone")),
                ("Your details", "Memorable word", credential),
                ("Your details", "Hint question", credential)
            };

            foreach (var (section, field, value) in expected)
            {
                var actual = idmCheckYourAnswersPage.GetSummaryValue(section, field);
                var matches = field == "Trading address"
                    ? AddressContains(actual, value)
                    : Regex.Replace(actual, @"\s+", " ").Equals(value, StringComparison.Ordinal);
                if (!matches)
                {
                    allDataMatches = false;
                    var expectedText = field is "Memorable word" or "Hint question" ? "<configured credential>" : value;
                    var actualText = field is "Memorable word" or "Hint question" ? "<hidden>" : actual;
                    mismatches.Add($"{section}: {field} - Expected: '{expectedText}', Actual: '{actualText}'");
                }
            }

            if (!allDataMatches)
            {
                Console.WriteLine("[REVIEW VALIDATION] Data mismatches found:");
                foreach (var mismatch in mismatches)
                {
                    Console.WriteLine($"[REVIEW VALIDATION] {mismatch}");
                }
            }

            Assert.True(allDataMatches, $"Registration review data validation failed. Mismatches: {string.Join(", ", mismatches)}");
        }

        [Then(@"^the Check your answers page offers change links for (.+)$")]
        public void ThenReviewOffersChangeLinks(string fieldList)
        {
            var pageRows = new List<(string Section, string Field)>
            {
                ("Defra account", "Account type"),
                ("Business details", "Trading name"),
                ("Business details", "Trading address"),
                ("Business details", "Phone number"),
                ("Business details", "Email"),
                ("Your details", "Name"),
                ("Your details", "Phone number"),
                ("Your details", "Memorable word"),
                ("Your details", "Hint question")
            };

            var missing = new List<string>();
            foreach (var field in Regex.Matches(fieldList, "'([^']+)'").Select(m => m.Groups[1].Value))
            {
                var index = pageRows.FindIndex(r => r.Field.Equals(field, StringComparison.OrdinalIgnoreCase));
                if (index < 0)
                {
                    missing.Add($"Unknown field: {field}");
                    continue;
                }
                var (section, rowField) = pageRows[index];
                pageRows.RemoveAt(index);
                if (!idmCheckYourAnswersPage.HasChangeLink(section, rowField))
                    missing.Add($"{section}: {rowField}");
            }

            if (missing.Count > 0)
            {
                Console.WriteLine("[REVIEW VALIDATION] Missing Change links:");
                foreach (var field in missing)
                {
                    Console.WriteLine($"[REVIEW VALIDATION] {field}");
                }
            }

            Assert.True(missing.Count == 0, $"Registration review change link validation failed. Missing: {string.Join(", ", missing)}");
        }

        [Then("the registration review has no reCAPTCHA checkbox")]
        public void ThenRecaptchaIsAbsent() => Assert.That(idmCheckYourAnswersPage.IsRecaptchaAbsent(), Is.True, "reCAPTCHA must not be present");

        [When("the user clicks Complete on the Check your answers page")]
        public void WhenRegistrationIsCompleted() => idmCheckYourAnswersPage.CompleteRegistration();

        private static string NormalizeAddress(string address) => Regex.Replace(address, @"[\s,]+", " ").Trim().ToUpperInvariant();

        private static bool AddressContains(string actual, string expected)
        {
            var actualParts = NormalizeAddress(actual).Split(' ');
            var position = 0;
            foreach (var part in NormalizeAddress(expected).Split(' '))
            {
                position = Array.IndexOf(actualParts, part, position);
                if (position < 0)
                    return false;
                position++;
            }
            return true;
        }
    }
}
