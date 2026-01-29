using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using System.Globalization;

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

        [Then("the user checks the reason should be {string} in the review page")]
        public void ThenTheUserChecksTheReasonShouldBeInTheReviewPage(string reason)
        {
            reviewOutcomeDecisionPage?.VerifyReason(reason);
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

            // Laboratory Tests - Basic
            ValidateIfExists("AreLaboratoryTestsRequired", reviewOutcomeDecisionPage?.GetLaboratoryTestsRequired(), ref allDataMatches, mismatches);
            ValidateIfExists("LaboratoryTestsReason", reviewOutcomeDecisionPage?.GetLaboratoryTestsReason(), ref allDataMatches, mismatches);

            // Laboratory Tests - Detailed Fields
            ValidateIfExists("AnalysisType", reviewOutcomeDecisionPage?.GetLaboratoryTestAnalysisType(0), ref allDataMatches, mismatches);
            ValidateLabTestCommoditySampled("SelectedCommoditySampledCode", "SelectedCommoditySampledDescription", reviewOutcomeDecisionPage?.GetLaboratoryTestCommoditySampled(0), ref allDataMatches, mismatches);
            ValidateIfExists("LaboratoryTestName", reviewOutcomeDecisionPage?.GetLaboratoryTestName(0), ref allDataMatches, mismatches);
            ValidateIfExists("SampleDate", reviewOutcomeDecisionPage?.GetLaboratoryTestSampleDate(0), ref allDataMatches, mismatches);
            ValidateIfExists("SampleTime", reviewOutcomeDecisionPage?.GetLaboratoryTestSampleTime(0), ref allDataMatches, mismatches);
            ValidateIfExists("SampleUseByDate", reviewOutcomeDecisionPage?.GetLaboratoryTestSampleUseByDate(0), ref allDataMatches, mismatches);
            ValidateIfExists("ReleasedDate", reviewOutcomeDecisionPage?.GetLaboratoryTestReleasedDate(0), ref allDataMatches, mismatches);
            ValidateIfExists("Conclusion", reviewOutcomeDecisionPage?.GetLaboratoryTestConclusion(0), ref allDataMatches, mismatches);

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

            // Determine decision type from scenario context
            var acceptableFor = _scenarioContext.ContainsKey("AcceptableFor")
                ? _scenarioContext.Get<string>("AcceptableFor")
                : string.Empty;

            // Temporary Admission Horses - Decision Fields (only validate if decision is Temporary admission horses)
            if (acceptableFor.Equals("Temporary admission horses", StringComparison.OrdinalIgnoreCase))
            {
                ValidateIfExists("ExitDate", reviewOutcomeDecisionPage?.GetDeadline(), ref allDataMatches, mismatches);
                ValidateExitBCP("ExitBCP", reviewOutcomeDecisionPage?.GetExitBCP(), ref allDataMatches, mismatches);
            }

            // Transit - Decision Fields (only validate if decision is Transit)
            if (acceptableFor.Equals("Transit", StringComparison.OrdinalIgnoreCase))
            {
                ValidateExitBCP("ExitBCP", reviewOutcomeDecisionPage?.GetTransitExitBCP(), ref allDataMatches, mismatches);
                ValidateTransitDestinationCountry("DestinationCountry", reviewOutcomeDecisionPage?.GetTransitDestinationCountry(), ref allDataMatches, mismatches);
            }

            // Controlled Destination (only for Internal market decisions)
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

        [Then("the details reflect the information added for CHED D")]
        public void ThenTheDetailsReflectTheInformationAddedForCHEDD()
        {
            var allDataMatches = true;
            var mismatches = new List<string>();

            // Border Control Post
            ValidateIfExists("BorderControlPostReference", reviewOutcomeDecisionPage?.GetBorderControlPostReference(), ref allDataMatches, mismatches);

            // Checks
            ValidateIfExists("DocumentaryCheckDecision", reviewOutcomeDecisionPage?.GetDocumentaryCheckDecision(), ref allDataMatches, mismatches);
            ValidateIfExists("IdentityCheckDecision", reviewOutcomeDecisionPage?.GetIdentityCheckDecision(), ref allDataMatches, mismatches);
            ValidateIfExists("PhysicalCheckDecision", reviewOutcomeDecisionPage?.GetPhysicalCheckDecision(), ref allDataMatches, mismatches);

            // Seal Numbers
            ValidateIfExists("AreNewSealNumbersRequired", reviewOutcomeDecisionPage?.GetSealNumbersStatus(), ref allDataMatches, mismatches);

            // Laboratory Tests
            ValidateIfExists("AreLaboratoryTestsRequired", reviewOutcomeDecisionPage?.GetLaboratoryTestsRequired(), ref allDataMatches, mismatches);

            // Documents
            ValidateIfExists("DocumentType", reviewOutcomeDecisionPage?.GetAdditionalDocumentType(), ref allDataMatches, mismatches);
            ValidateIfExists("DocumentReference", reviewOutcomeDecisionPage?.GetAdditionalDocumentReference(), ref allDataMatches, mismatches);
            ValidateIfExists("DocumentDateOfIssue", reviewOutcomeDecisionPage?.GetAdditionalDocumentDateOfIssue(), ref allDataMatches, mismatches);
            ValidateFileNameWithTruncation("DocumentName", reviewOutcomeDecisionPage?.GetAdditionalDocumentFileName(), ref allDataMatches, mismatches);

            // Decision
            ValidateIfExists("AcceptableFor", reviewOutcomeDecisionPage?.GetAcceptanceDecision(), ref allDataMatches, mismatches);
            ValidateIfExists("AcceptableForSubOption", GetAcceptableForSubOptionValue(), ref allDataMatches, mismatches);
            ValidateIfExists("RefusalReason", GetAcceptableForSubOptionValue(), ref allDataMatches, mismatches);

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
                string expectedValue = null;
                try
                {
                    expectedValue = _scenarioContext.Get<string>(contextKey);
                }
                catch (InvalidCastException)
                {
                    expectedValue = _scenarioContext.Get<string[]>(contextKey)?.FirstOrDefault();
                }

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
                    return CompareAcceptableFor(expected, actual);

                case "AcceptableForSubOption":
                    // Ignore spaces for AcceptableForSubOption (handles both CHED-A CertifiedFor and CHED-P ConsignmentUse)
                    return expected.Replace(" ", "").Equals(actual.Replace(" ", ""), StringComparison.OrdinalIgnoreCase);

                case "LaboratoryTestsReason":
                    // Handle variation: "Suspicion" (task list) vs "Suspicious" (review page)
                    return CompareLaboratoryTestsReason(expected, actual);

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

        private bool CompareLaboratoryTestsReason(string expected, string actual)
        {
            // Direct match
            if (expected.Equals(actual.Trim(), StringComparison.OrdinalIgnoreCase))
                return true;

            // Handle "Suspicion" -> "Suspicious" variation
            if (expected.Equals("Suspicion", StringComparison.OrdinalIgnoreCase) &&
                actual.Trim().Equals("Suspicious", StringComparison.OrdinalIgnoreCase))
                return true;

            // Handle reverse case (if ever needed)
            if (expected.Equals("Suspicious", StringComparison.OrdinalIgnoreCase) &&
                actual.Trim().Equals("Suspicion", StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        private bool CompareAcceptableFor(string expected, string actual)
        {
            // Expected format from task list: "Temporary admission horses"
            // Actual format on review page: "Acceptable for temporary admission"

            // Handle "Temporary admission horses" -> "Acceptable for temporary admission"
            if (expected.Equals("Temporary admission horses", StringComparison.OrdinalIgnoreCase))
            {
                return actual.Equals("Acceptable for temporary admission", StringComparison.OrdinalIgnoreCase);
            }

            // Handle other variations (Internal market, Transit, etc.)
            return actual.Equals($"Acceptable for {expected.ToLower()}", StringComparison.OrdinalIgnoreCase);
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

        private void ValidateLabTestCommoditySampled(string codeContextKey, string descriptionContextKey, string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            if (_scenarioContext.ContainsKey(codeContextKey) && _scenarioContext.ContainsKey(descriptionContextKey))
            {
                var code = _scenarioContext.Get<string>(codeContextKey);
                var description = _scenarioContext.Get<string>(descriptionContextKey);
                var expectedValue = $"{code} - {description}";

                if (!string.IsNullOrEmpty(reviewValue))
                {
                    // The review page shows the Commodity sampled as Commodity Code - Description (e.g., "0103 - Live Swine") 
                    // Therefore we need scenario context values for  the commodity code and description

                    var isMatch = expectedValue.Equals(reviewValue.Trim(), StringComparison.OrdinalIgnoreCase);

                    if (!isMatch)
                    {
                        allDataMatches = false;
                        mismatches.Add($"CommoditySampled: Expected '{expectedValue}', Found '{reviewValue?.Trim()}'");
                    }
                    else
                    {
                        Console.WriteLine($"[REVIEW OUTCOME VALIDATION] ✓ CommoditySampled: '{expectedValue}' matches");
                    }
                }
                else
                {
                    allDataMatches = false;
                    mismatches.Add($"CommoditySampled: Expected '{expectedValue}', Found null/empty");
                }
            }
            else
            {
                Console.WriteLine($"[REVIEW OUTCOME VALIDATION] ⊘ CommoditySampled: Skipped (not in context)");
            }
        }

        private void ValidateExitBCP(string contextKey, string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            if (_scenarioContext.ContainsKey(contextKey))
            {
                var expectedExitBCP = _scenarioContext.Get<string>(contextKey);

                if (!string.IsNullOrEmpty(expectedExitBCP) && !string.IsNullOrEmpty(reviewValue))
                {
                    // The review page shows the code (e.g., "GBMNC4" or "GBLHR4A")
                    // The scenario context has the full name (e.g., "Manchester Airport (animals) - GBMNC4" or "Heathrow Airport - HARC (animals)")

                    // Extract the code by finding the last occurrence of " - " and taking everything after it
                    var lastDashIndex = expectedExitBCP.LastIndexOf(" - ");
                    var expectedCode = lastDashIndex >= 0
                        ? expectedExitBCP.Substring(lastDashIndex + 3).Trim()
                        : expectedExitBCP;

                    var isMatch = reviewValue.Trim().Equals(expectedCode, StringComparison.OrdinalIgnoreCase);

                    if (!isMatch)
                    {
                        allDataMatches = false;
                        mismatches.Add($"Transit {contextKey}: Expected code '{expectedCode}', Found '{reviewValue?.Trim()}'");
                    }
                    else
                    {
                        Console.WriteLine($"[REVIEW OUTCOME VALIDATION] ✓ Transit {contextKey}: '{expectedCode}' matches");
                    }
                }
                else
                {
                    Console.WriteLine($"[REVIEW OUTCOME VALIDATION] ⊘ Transit {contextKey}: Skipped (empty value)");
                }
            }
            else
            {
                Console.WriteLine($"[REVIEW OUTCOME VALIDATION] ⊘ Transit {contextKey}: Skipped (not in context)");
            }
        }

        private void ValidateTransitDestinationCountry(string contextKey, string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            if (_scenarioContext.ContainsKey(contextKey))
            {
                var expectedCountry = _scenarioContext.Get<string>(contextKey);

                if (!string.IsNullOrEmpty(expectedCountry) && !string.IsNullOrEmpty(reviewValue))
                {
                    // The review page shows the country code (e.g., "QA" for Qatar)
                    // The scenario context has the full country name (e.g., "Qatar")

                    var expectedCode = GetCountryISOCode(expectedCountry);

                    var isMatch = reviewValue.Trim().Equals(expectedCode, StringComparison.OrdinalIgnoreCase);

                    if (!isMatch)
                    {
                        allDataMatches = false;
                        mismatches.Add($"Transit {contextKey}: Expected code '{expectedCode}' for '{expectedCountry}', Found '{reviewValue?.Trim()}'");
                    }
                    else
                    {
                        Console.WriteLine($"[REVIEW OUTCOME VALIDATION] ✓ Transit {contextKey}: '{expectedCode}' ({expectedCountry}) matches");
                    }
                }
                else
                {
                    Console.WriteLine($"[REVIEW OUTCOME VALIDATION] ⊘ Transit {contextKey}: Skipped (empty value)");
                }
            }
            else
            {
                Console.WriteLine($"[REVIEW OUTCOME VALIDATION] ⊘ Transit {contextKey}: Skipped (not in context)");
            }
        }

        /// <summary>
        /// Gets the ISO 3166-1 alpha-2 country code for a given country name.
        /// Returns the input unchanged if it's already a 2-letter code or if no match is found.
        /// </summary>
        private string GetCountryISOCode(string countryName)
        {
            // If it's already a 2-letter code, return as-is
            if (countryName.Length == 2)
            {
                return countryName.ToUpper();
            }

            try
            {
                // Try to find the country by English name using CultureInfo
                var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

                foreach (var culture in cultures)
                {
                    try
                    {
                        var region = new RegionInfo(culture.Name);

                        // Check if the English name matches
                        if (region.EnglishName.Equals(countryName, StringComparison.OrdinalIgnoreCase) ||
                            region.DisplayName.Equals(countryName, StringComparison.OrdinalIgnoreCase))
                        {
                            return region.TwoLetterISORegionName;
                        }
                    }
                    catch
                    {
                        // Skip cultures that don't have region info
                        continue;
                    }
                }

                // If no match found, return the original value
                Console.WriteLine($"[WARNING] Could not find ISO code for country: '{countryName}', using as-is");
                return countryName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error getting ISO code for '{countryName}': {ex.Message}");
                return countryName;
            }
        }
    }
}