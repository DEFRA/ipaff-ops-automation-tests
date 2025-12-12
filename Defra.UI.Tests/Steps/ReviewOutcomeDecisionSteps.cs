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

        [When("the user selects the radio button to declare that the checks have been carried out in accordance with EU law")]
        public void WhenTheUserSelectsTheRadioButtonToDeclareChecksCarriedOutInAccordanceWithEULaw()
        {
            reviewOutcomeDecisionPage?.SelectCertifyingOfficerRadioButton();
        }

        [Then("the details reflect the information added")]
        public void ThenTheDetailsReflectTheInformationAdded()
        {
            var allDataMatches = true;
            var mismatches = new List<string>();

            // Border Control Post
            ValidateIfExists("BorderControlPostReference", reviewOutcomeDecisionPage?.GetBorderControlPostReference(), ref allDataMatches, mismatches);

            // Checks
            ValidateIfExists("DocumentaryCheckDecision", reviewOutcomeDecisionPage?.GetDocumentaryCheckDecision(), ref allDataMatches, mismatches);
            ValidateIfExists("IdentityCheckType", reviewOutcomeDecisionPage?.GetIdentityCheckType(), ref allDataMatches, mismatches);
            ValidateIfExists("IdentityCheckDecision", reviewOutcomeDecisionPage?.GetIdentityCheckDecision(), ref allDataMatches, mismatches);
            ValidateIfExists("PhysicalCheckDecision", reviewOutcomeDecisionPage?.GetPhysicalCheckDecision(), ref allDataMatches, mismatches);
            ValidateIfExists("NumberOfAnimalsChecked", reviewOutcomeDecisionPage?.GetNumberOfAnimalsChecked(), ref allDataMatches, mismatches);
            ValidateIfExists("WelfareCheckDecision", reviewOutcomeDecisionPage?.GetWelfareCheckDecision(), ref allDataMatches, mismatches);

            // Impact of transport on animals
            ValidateIfExists("NumberOfDeadAnimals", reviewOutcomeDecisionPage?.GetNumberOfDeadAnimals(), ref allDataMatches, mismatches);
            ValidateIfExists("NumberOfDeadAnimalsUnit", reviewOutcomeDecisionPage?.GetNumberOfDeadAnimalsUnit(), ref allDataMatches, mismatches);
            ValidateIfExists("NumberOfUnfitAnimals", reviewOutcomeDecisionPage?.GetNumberOfUnfitAnimals(), ref allDataMatches, mismatches);
            ValidateIfExists("NumberOfUnfitAnimalsUnit", reviewOutcomeDecisionPage?.GetNumberOfUnfitAnimalsUnit(), ref allDataMatches, mismatches);
            ValidateIfExists("NumberOfBirthsOrAbortions", reviewOutcomeDecisionPage?.GetNumberOfBirthsOrAbortions(), ref allDataMatches, mismatches);

            // Seal Numbers
            ValidateIfExists("AreNewSealNumbersRequired", reviewOutcomeDecisionPage?.GetSealNumbersStatus(), ref allDataMatches, mismatches);

            // Laboratory Tests
            ValidateIfExists("AreLaboratoryTestsRequired", reviewOutcomeDecisionPage?.GetLaboratoryTestsRequired(), ref allDataMatches, mismatches);

            // Documents
            ValidateIfExists("HealthCertificateReference", reviewOutcomeDecisionPage?.GetHealthCertificateReference(), ref allDataMatches, mismatches);
            ValidateIfExists("HealthCertificateDateOfIssue", reviewOutcomeDecisionPage?.GetHealthCertificateDateOfIssue(), ref allDataMatches, mismatches);
            ValidateFileNameWithTruncation("HealthCertificateFileName", reviewOutcomeDecisionPage?.GetHealthCertificateFileName(), ref allDataMatches, mismatches);
            ValidateIfExists("DocumentType", reviewOutcomeDecisionPage?.GetAdditionalDocumentType(), ref allDataMatches, mismatches);
            ValidateIfExists("DocumentReference", reviewOutcomeDecisionPage?.GetAdditionalDocumentReference(), ref allDataMatches, mismatches);
            ValidateIfExists("DocumentDateOfIssue", reviewOutcomeDecisionPage?.GetAdditionalDocumentDateOfIssue(), ref allDataMatches, mismatches);
            ValidateFileNameWithTruncation("DocumentName", reviewOutcomeDecisionPage?.GetAdditionalDocumentFileName(), ref allDataMatches, mismatches);

            // Decision
            ValidateIfExists("AcceptableFor", reviewOutcomeDecisionPage?.GetAcceptanceDecision(), ref allDataMatches, mismatches);
            ValidateIfExists("AcceptableForSubOption", GetAcceptableForSubOptionValue(), ref allDataMatches, mismatches);

            // Controlled Destination
            ValidateIfExists("ControlledDestinationName", reviewOutcomeDecisionPage?.GetControlledDestinationName(), ref allDataMatches, mismatches);
            ValidateIfExists("ControlledDestinationAddress", reviewOutcomeDecisionPage?.GetControlledDestinationAddress(), ref allDataMatches, mismatches);

            if (!allDataMatches)
            {
                Console.WriteLine("[REVIEW OUTCOME VALIDATION] Data mismatches found:");
                foreach (var mismatch in mismatches)
                {
                    Console.WriteLine($"[REVIEW OUTCOME VALIDATION] {mismatch}");
                }
            }

            Assert.True(allDataMatches, $"Review outcome decision page data validation failed. Mismatches: {string.Join(", ", mismatches)}");
        }        

        private void ValidateIfExists(string contextKey, string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            if (_scenarioContext.ContainsKey(contextKey))
            {
                var expectedValue = _scenarioContext.Get<string>(contextKey);
                if (!string.IsNullOrEmpty(expectedValue))
                {
                    // Handle special comparisons
                    var isMatch = CompareValues(contextKey, expectedValue, reviewValue);

                    if (!isMatch)
                    {
                        allDataMatches = false;
                        mismatches.Add($"{contextKey}: Expected '{expectedValue}', Found '{reviewValue?.Trim()}'");
                    }
                    else
                    {
                        Console.WriteLine($"[REVIEW OUTCOME VALIDATION] ✓ {contextKey}: '{expectedValue}' matches");
                    }
                }
                else
                {
                    Console.WriteLine($"[REVIEW OUTCOME VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                }
            }
            else
            {
                Console.WriteLine($"[REVIEW OUTCOME VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
            }
        }

        /// <summary>
        /// Gets the appropriate value for AcceptableForSubOption based on CHED type.
        /// Tries CHED-A's CertifiedFor first, then falls back to CHED-P's ConsignmentUse.
        /// </summary>
        private string? GetAcceptableForSubOptionValue()
        {
            // Try CHED-A first
            var certifiedFor = reviewOutcomeDecisionPage?.GetCertifiedFor();
            if (!string.IsNullOrEmpty(certifiedFor))
            {
                return certifiedFor;
            }

            // Fallback to CHED-P
            var consignmentUse = reviewOutcomeDecisionPage?.GetConsignmentUse();
            return consignmentUse;
        }

        private bool CompareValues(string contextKey, string expected, string? actual)
        {
            if (string.IsNullOrEmpty(actual))
                return false;

            // Special handling for specific fields
            switch (contextKey)
            {
                case "NumberOfDeadAnimalsUnit":
                case "NumberOfUnfitAnimalsUnit":
                    return CompareUnit(expected, actual);

                case "AreNewSealNumbersRequired":
                    return actual.Contains("No new seal numbers have been entered", StringComparison.OrdinalIgnoreCase);

                case "AcceptableFor":
                    return actual.Equals($"Acceptable for {expected.ToLower()}", StringComparison.OrdinalIgnoreCase);

                case "AcceptableForSubOption":
                    // Ignore spaces for AcceptableForSubOption (handles both CHED-A CertifiedFor and CHED-P ConsignmentUse)
                    return expected.Replace(" ", "").Equals(actual.Replace(" ", ""), StringComparison.OrdinalIgnoreCase);

                default:
                    return expected.Equals(actual.Trim(), StringComparison.OrdinalIgnoreCase);
            }
        }

        private bool CompareUnit(string expected, string actual)
        {
            string displayValue = expected.Equals("%", StringComparison.OrdinalIgnoreCase) ||
                                expected.Equals("percent", StringComparison.OrdinalIgnoreCase)
                                ? "Percent"
                                : "Units";

            return displayValue.Equals(actual.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        private void ValidateFileNameWithTruncation(string contextKey, string? displayedFileName, ref bool allDataMatches, List<string> mismatches)
        {
            if (_scenarioContext.ContainsKey(contextKey))
            {
                var expectedFileName = _scenarioContext.Get<string>(contextKey);
                if (!string.IsNullOrEmpty(expectedFileName))
                {
                    // Handle filename truncation - the UI truncates long filenames but keeps the extension
                    var isMatch = false;

                    if (!string.IsNullOrEmpty(displayedFileName))
                    {
                        var displayedExtension = Path.GetExtension(displayedFileName);
                        var expectedExtension = Path.GetExtension(expectedFileName);

                        var displayedNameWithoutExt = Path.GetFileNameWithoutExtension(displayedFileName);
                        var expectedNameWithoutExt = Path.GetFileNameWithoutExtension(expectedFileName);

                        // Check if extensions match and displayed name is the start of expected name (handles truncation)
                        isMatch = displayedExtension.Equals(expectedExtension, StringComparison.OrdinalIgnoreCase) &&
                                  expectedNameWithoutExt.StartsWith(displayedNameWithoutExt, StringComparison.OrdinalIgnoreCase);
                    }

                    if (!isMatch)
                    {
                        allDataMatches = false;
                        mismatches.Add($"{contextKey}: Expected '{expectedFileName}', Found '{displayedFileName}' (with truncation handling)");
                    }
                    else
                    {
                        Console.WriteLine($"[REVIEW OUTCOME VALIDATION] ✓ {contextKey}: '{expectedFileName}' matches (truncated to '{displayedFileName}')");
                    }
                }
                else
                {
                    Console.WriteLine($"[REVIEW OUTCOME VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                }
            }
            else
            {
                Console.WriteLine($"[REVIEW OUTCOME VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
            }
        }
    }
}