using Reqnroll.BoDi;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class ReviewYourNotificationSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private IReviewYourNotificationPage? reviewPage => _objectContainer.IsRegistered<IReviewYourNotificationPage>() ? _objectContainer.Resolve<IReviewYourNotificationPage>() : null;


        public ReviewYourNotificationSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Review your notification page should be displayed")]
        public void ThenTheReviewYourNotificationPageShouldBeDisplayed()
        {
            Assert.True(reviewPage?.IsPageLoaded(), "Review your notification page not loaded");
        }

        [Then("the data presented for review matches the data entered into the notification")]
        public void ThenTheDataPresentedForReviewMatchesTheDataEnteredIntoTheNotification()
        {
            var allDataMatches = true;
            var mismatches = new List<string>();

            // About the consignment
            ValidateIfExists("ImportType", reviewPage?.GetImportType(), ref allDataMatches, mismatches);
            ValidateIfExists("CountryOfOrigin", reviewPage?.GetCountryOfOrigin(), ref allDataMatches, mismatches);
            ValidateIfExists("MainReasonForImport", reviewPage?.GetMainReasonForImport(), ref allDataMatches, mismatches);
            ValidateIfExists("Purpose", reviewPage?.GetPurpose(), ref allDataMatches, mismatches);
            ValidateIfExists("ConsignmentReferenceNumber", reviewPage?.GetConsignmentReferenceNumber(), ref allDataMatches, mismatches);

            // Commodity details  
            ValidateIfExists("CommodityCode", reviewPage?.GetCommodityCode(), ref allDataMatches, mismatches);
            ValidateIfExists("Species", reviewPage?.GetSpecies(), ref allDataMatches, mismatches);
            ValidateIfExists("NumberOfAnimals", reviewPage?.GetNumberOfAnimals(), ref allDataMatches, mismatches);
            ValidateIfExists("NumberOfPackages", reviewPage?.GetNumberOfPackages(), ref allDataMatches, mismatches);

            // Animal details
            ValidateIfExists("CertificationOption", reviewPage?.GetCertificationOption(), ref allDataMatches, mismatches);

            // Documents
            ValidateIfExists("HealthCertificateReference", reviewPage?.GetHealthCertificateReference(), ref allDataMatches, mismatches);
            ValidateIfExists("HealthCertificateDateOfIssue", reviewPage?.GetHealthCertificateDateOfIssue(), ref allDataMatches, mismatches);
            ValidateIfExists("DocumentType", reviewPage?.GetAdditionalDocumentType(), ref allDataMatches, mismatches);
            ValidateIfExists("DocumentReference", reviewPage?.GetAdditionalDocumentReference(), ref allDataMatches, mismatches);
            ValidateIfExists("DocumentDateOfIssue", reviewPage?.GetAdditionalDocumentDateOfIssue(), ref allDataMatches, mismatches);

            // Addresses
            ValidateIfExists("ConsignorName", reviewPage?.GetConsignorName(), ref allDataMatches, mismatches);
            ValidateIfExists("ConsignorAddress", reviewPage?.GetConsignorAddress(), ref allDataMatches, mismatches);
            ValidateIfExists("ConsigneeName", reviewPage?.GetConsigneeName(), ref allDataMatches, mismatches);
            ValidateIfExists("ConsigneeAddress", reviewPage?.GetConsigneeAddress(), ref allDataMatches, mismatches);
            ValidateIfExists("ImporterName", reviewPage?.GetImporterName(), ref allDataMatches, mismatches);
            ValidateIfExists("ImporterAddress", reviewPage?.GetImporterAddress(), ref allDataMatches, mismatches);
            ValidateIfExists("DestinationName", reviewPage?.GetDestinationName(), ref allDataMatches, mismatches);
            ValidateIfExists("DestinationAddress", reviewPage?.GetDestinationAddress(), ref allDataMatches, mismatches);

            // Transport details
            ValidateIfExists("PortOfEntry", reviewPage?.GetPortOfEntry(), ref allDataMatches, mismatches);
            ValidateIfExists("MeansOfTransport", reviewPage?.GetMeansOfTransport(), ref allDataMatches, mismatches);
            ValidateIfExists("TransportId", reviewPage?.GetTransportId(), ref allDataMatches, mismatches);
            ValidateIfExists("AreContainers", reviewPage?.GetContainerUsage(), ref allDataMatches, mismatches);
            ValidateIfExists("EnterTransportDocRef", reviewPage?.GetTransportDocumentReference(), ref allDataMatches, mismatches);
            ValidateIfExists("EstimatedArrivalDate", reviewPage?.GetEstimatedArrivalDate(), ref allDataMatches, mismatches);
            ValidateIfExists("EstimatedArrivalTime", reviewPage?.GetEstimatedArrivalTime(), ref allDataMatches, mismatches);
            ValidateIfExists("EstimatedJourneyTime", reviewPage?.GetEstimatedJourneyTime(), ref allDataMatches, mismatches);
            ValidateIfExists("IsGVMS", reviewPage?.GetGVMSUsage(), ref allDataMatches, mismatches);

            // Transporter details
            ValidateIfExists("TransporterName", reviewPage?.GetTransporterName(), ref allDataMatches, mismatches);
            ValidateIfExists("TransporterAddress", reviewPage?.GetTransporterAddress(), ref allDataMatches, mismatches);
            ValidateIfExists("TransporterCountry", reviewPage?.GetTransporterCountry(), ref allDataMatches, mismatches);
            ValidateIfExists("TransporterApprovalNumber", reviewPage?.GetTransporterApprovalNumber(), ref allDataMatches, mismatches);
            ValidateIfExists("TransporterType", reviewPage?.GetTransporterType(), ref allDataMatches, mismatches);

            // Route and contacts
            ValidateIfExists("CountriesConsignmentWillTravelThrough", reviewPage?.GetRouteCountries(), ref allDataMatches, mismatches);
            ValidateIfExists("ShouldNotifyTransportContacts", reviewPage?.GetNotifyTransportContacts(), ref allDataMatches, mismatches);
            ValidateIfExists("ConsignmentContactAddress", reviewPage?.GetConsignmentContactAddress(), ref allDataMatches, mismatches);

            if (!allDataMatches)
            {
                Console.WriteLine("[REVIEW VALIDATION] Data mismatches found:");
                foreach (var mismatch in mismatches)
                {
                    Console.WriteLine($"[REVIEW VALIDATION] {mismatch}");
                }
            }

            Assert.True(allDataMatches, $"Review page data validation failed. Mismatches: {string.Join(", ", mismatches)}");
        }

        private void ValidateIfExists(string contextKey, string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            if (_scenarioContext.ContainsKey(contextKey))
            {
                var expectedValue = _scenarioContext.Get<string>(contextKey);
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
            }
        }
    }
}