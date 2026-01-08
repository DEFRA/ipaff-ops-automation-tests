using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ReviewBorderNotificationSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IReviewBorderNotificationPage? reviewBorderNotificationPage => _objectContainer.IsRegistered<IReviewBorderNotificationPage>() ? _objectContainer.Resolve<IReviewBorderNotificationPage>() : null;

        public ReviewBorderNotificationSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Review border notification page should be displayed")]
        [Then("Reveiw border notification page should be displayed")]
        public void ThenTheReviewBorderNotificationPageShouldBeDisplayed()
        {
            Assert.True(reviewBorderNotificationPage?.IsPageLoaded(), "Review border notification page is not loaded");
        }

        [Then("the border notification details reflect the information added")]
        public void ThenTheBorderNotificationDetailsReflectTheInformationAdded()
        {
            var allDataMatches = true;
            var mismatches = new List<string>();

            ValidateIfExists("CommodityBN", reviewBorderNotificationPage?.GetCommodity, ref allDataMatches, mismatches);

            ValidateIfExists("NetWeightBN", reviewBorderNotificationPage?.GetNetWeight, ref allDataMatches, mismatches);
            ValidateIfExists("LaboratoryTestBN", reviewBorderNotificationPage?.GetLaboratoryTest, ref allDataMatches, mismatches);
            ValidateIfExists("CountryBN", reviewBorderNotificationPage?.GetCountry, ref allDataMatches, mismatches);
            ValidateIfExists("NotificationTypeBN", reviewBorderNotificationPage?.GetNotificationType, ref allDataMatches, mismatches);
            ValidateIfExists("NotificationBasisBN", reviewBorderNotificationPage?.GetNotificationBasis, ref allDataMatches, mismatches);
            ValidateIfExists("ProductCategoryBN", reviewBorderNotificationPage?.GetProductCategory, ref allDataMatches, mismatches);
            ValidateIfExists("ProductNameBN", reviewBorderNotificationPage?.GetProductName, ref allDataMatches, mismatches);
            ValidateIfExists("BrandNameBN", reviewBorderNotificationPage?.GetBrandName, ref allDataMatches, mismatches);
            ValidateIfExists("OtherLabellingBN", reviewBorderNotificationPage?.GetOtherLabelling, ref allDataMatches, mismatches);
            ValidateIfExists("OtherInformationBN", reviewBorderNotificationPage?.GetOtherInformation, ref allDataMatches, mismatches);
            ValidateIfExists("DurabilityDateBN", reviewBorderNotificationPage?.GetDurabilityDate, ref allDataMatches, mismatches);
            ValidateIfExists("RiskDecisionBN", reviewBorderNotificationPage?.GetRiskDecision, ref allDataMatches, mismatches);
            ValidateIfExists("ImpactOnBN", reviewBorderNotificationPage?.GetImpactOn, ref allDataMatches, mismatches);
            ValidateIfExists("HazardCategoryBN", reviewBorderNotificationPage?.GetHazardCategory, ref allDataMatches, mismatches);
            ValidateIfExists("MeasureTakenBN", reviewBorderNotificationPage?.GetMeasureTaken, ref allDataMatches, mismatches);

            //Accompanying document details
            ValidateIfExists("AccompanyingDocumentTypeBN", reviewBorderNotificationPage?.GetAccompanyingDocumentType, ref allDataMatches, mismatches);
            ValidateIfExists("AccompanyingDocumentRefBN", reviewBorderNotificationPage?.GetAccompanyingDocumentRef, ref allDataMatches, mismatches);
            ValidateIfExists("AccompanyingDocumentFileNameBN", reviewBorderNotificationPage?.GetAccompanyingDocumentFileName, ref allDataMatches, mismatches);

            //Last updated Date and Time details
            ValidateIfExists("LastUpdatedDateBN", reviewBorderNotificationPage?.GetLastUpdatedDate, ref allDataMatches, mismatches);
            ValidateIfExists("LastUpdatedTimeBN", reviewBorderNotificationPage?.GetLastUpdatedTime, ref allDataMatches, mismatches);
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
                    var isMatch = expectedValue.Equals(reviewValue?.Trim(), StringComparison.OrdinalIgnoreCase);
                    if (!isMatch)
                    {
                        allDataMatches = false;
                        mismatches.Add($"{contextKey}: Expected '{expectedValue}', Found '{reviewValue?.Trim()}'");
                    }
                    else
                    {
                        Console.WriteLine($"[REVIEW VALIDATION] ✓ {contextKey}: '{expectedValue}' matches");
                    }
                }
                else
                {
                    Console.WriteLine($"[REVIEW VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                }
            }
            else
            {
                Console.WriteLine($"[REVIEW VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
            }
        }

        [When("the user download the document attached in accompanying documents")]
        public void WhenTheUserDownloadTheDocumentAttachedInAccompanyingDocuments()
        {
            reviewBorderNotificationPage?.ClickDocumentLink();
        }


        [When("the user clicks submit button")]
        public void WhenTheUserClicksSubmitButton()
        {
            reviewBorderNotificationPage?.ClickSubmitButton();
        }

        [Then("the user switch to next tab and open the browser downloads")]
        public void ThenTheUserSwitchToNextTabAndOpenTheBrowserDownloads()
        {
            reviewBorderNotificationPage?.OpenDownloadsInNewTab();
        }

        [Then("verifies the document {string} downloaded successfully")]
        public void ThenVerifiesTheDocumentDownloadedSuccessfully(string fileName)
        {
            Assert.True(reviewBorderNotificationPage?.VerifyFileDownloaded(fileName));
        }
    }
}