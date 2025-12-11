using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class ReviewYourNotificationSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IReviewYourNotificationPage? reviewPage => _objectContainer.IsRegistered<IReviewYourNotificationPage>() ? _objectContainer.Resolve<IReviewYourNotificationPage>() : null;
        private ISummaryPage? summaryPage => _objectContainer.IsRegistered<ISummaryPage>() ? _objectContainer.Resolve<ISummaryPage>() : null;

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
            ValidateFileNameWithTruncation("HealthCertificateFileName", reviewPage?.GetHealthCertificateFileName(), ref allDataMatches, mismatches);
            ValidateIfExists("DocumentType", reviewPage?.GetAdditionalDocumentType(), ref allDataMatches, mismatches);
            ValidateIfExists("DocumentReference", reviewPage?.GetAdditionalDocumentReference(), ref allDataMatches, mismatches);
            ValidateIfExists("DocumentDateOfIssue", reviewPage?.GetAdditionalDocumentDateOfIssue(), ref allDataMatches, mismatches);
            ValidateFileNameWithTruncation("DocumentName", reviewPage?.GetAdditionalDocumentFileName(), ref allDataMatches, mismatches);

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

        [Then("the data presented for review matches the data entered into the notification for CHED D")]
        public void ThenTheDataPresentedForReviewMatchesTheDataEnteredIntoTheNotificationForCHEDD()
        {
            var allDataMatches = true;
            var mismatches = new List<string>();

            // About the consignment
            ValidateIfContains("ImportType", reviewPage?.GetPartOfImportType(), ref allDataMatches, mismatches);
            ValidateIfExists("CountryOfOrigin", reviewPage?.GetCountryOfOrigin(), ref allDataMatches, mismatches);
            ValidateIfExists("ContryFromWhereConsigned", reviewPage?.GetCountryFromWhereConsigned(), ref allDataMatches, mismatches);
            ValidateIfExists("ConsignmentReferenceNumber", reviewPage?.GetConsignmentReferenceNumber(), ref allDataMatches, mismatches);
            ValidateIfExists("MainReasonForImport", reviewPage?.GetMainReasonForImport(), ref allDataMatches, mismatches);

            ValidateIfExists("CommodityCodeFirstCommodity", reviewPage?.GetCommodityCodeList(0), ref allDataMatches, mismatches);
            ValidateIfExists("NetWeightFirstCommodity", reviewPage?.GetNetWeightList(0), ref allDataMatches, mismatches);
            ValidateIfExists("NumberOfPackagesFirstCommodity", reviewPage?.GetNumPackagesList(0), ref allDataMatches, mismatches);
            ValidateIfExists("TypeOfPackageFirstCommodity", reviewPage?.GetTypeOfPackagesList(0), ref allDataMatches, mismatches);

            ValidateIfExists("CommodityCodeSecondCommodity", reviewPage?.GetCommodityCodeList(1), ref allDataMatches, mismatches);
            ValidateIfExists("NetWeightSecondCommodity", reviewPage?.GetNetWeightList(1), ref allDataMatches, mismatches);
            ValidateIfExists("NumOfPackagesSecondCommodity", reviewPage?.GetNumPackagesList(1), ref allDataMatches, mismatches);
            ValidateIfExists("TypeOfPackageSecondCommodity", reviewPage?.GetTypeOfPackagesList(1), ref allDataMatches, mismatches);

            ValidateIfExists("TotalNetWeight", reviewPage?.GetTotalNetWeight(), ref allDataMatches, mismatches);
            ValidateIfExists("TotalPackages", reviewPage?.GetTotalPackages(), ref allDataMatches, mismatches);
            ValidateIfExists("TotalGrossWeight", reviewPage?.GetTotalGrossWeight(), ref allDataMatches, mismatches);

            // Additional details
            ValidateIfExists("CommodityIntendedFor", reviewPage?.GetCommodityIntendedFor(), ref allDataMatches, mismatches);
            ValidateIfExists("Temperature", reviewPage?.GetTemperature(), ref allDataMatches, mismatches);

            // Documents
            ValidateIfExists("DocumentType", reviewPage?.GetAdditionalDocumentType(), ref allDataMatches, mismatches);
            ValidateIfExists("DocumentReference", reviewPage?.GetAdditionalDocumentReference(), ref allDataMatches, mismatches);
            ValidateIfExists("DocumentDateOfIssue", reviewPage?.GetAdditionalDocumentDateOfIssue(), ref allDataMatches, mismatches);
            ValidateFileNameWithTruncation("DocumentName", reviewPage?.GetAdditionalDocumentFileName(), ref allDataMatches, mismatches);

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
            ValidateIfExists("IsCTC", reviewPage?.GetCTCUsage(), ref allDataMatches, mismatches);
            ValidateIfExists("IsGVMS", reviewPage?.GetGVMSUsage(), ref allDataMatches, mismatches);

            // Contacts
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
                        Console.WriteLine($"[REVIEW VALIDATION] ✓ {contextKey}: '{expectedFileName}' matches (truncated to '{displayedFileName}')");
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

        private void ValidateIfContains(string contextKey, string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            if (_scenarioContext.ContainsKey(contextKey))
            {
                var expectedValue = _scenarioContext.Get<string>(contextKey);
                if (!string.IsNullOrEmpty(expectedValue))
                {
                    var isMatch = expectedValue.Contains(reviewValue?.Trim(), StringComparison.OrdinalIgnoreCase);
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

        [Then("the user verifies all the data displayed in review page")]
        public void ThenTheUserVerifiesAllTheDataDisplayedInReviewPage()
        {
            VerifyAboutTheConsignment();
            VerifyDescriptionOfTheGoods();
            VerifyDocuments();
            VerifyTraders();
        }

        private void VerifyAboutTheConsignment()
        {
            var summary = summaryPage?.GetSummaryDetails();
            var pageName = "Review your notification";

            var importType = _scenarioContext.Get<string>("ImportType");
            var countryOfOrigin = _scenarioContext.Get<string>("CountryOfOrigin");
            var contryFromWhereConsigned = _scenarioContext.Get<string>("ContryFromWhereConsigned");
            var consignmentConformToRegulatoryRequirements = _scenarioContext.Get<string>("ConsignmentConformToRegulatoryRequirements");
            var consignmentRefNum = _scenarioContext.Get<string>("ConsignmentReferenceNumber");
            var mainReasonForImport = _scenarioContext.Get<string>("MainReasonForImport");
            var purpose = _scenarioContext.Get<string>("Purpose");
            var riskCategory = _scenarioContext.Get<string>("RiskCategory");

            Assert.AreEqual(importType, summary?.ImportType, $"Import type is not matching in {pageName} page!");
            Assert.AreEqual(countryOfOrigin, summary?.CountryOfOrigin, $"Country of origin is not matching in {pageName} page!");
            Assert.AreEqual(contryFromWhereConsigned, summary?.ContryFromWhereConsigned, $"Country from where consigned is not matching in {pageName} page!");
            Assert.AreEqual(consignmentConformToRegulatoryRequirements, summary?.ConsignmentConformToRegulatoryRequirements, $"Consignment confirmation to regulatory requirements is not matching in {pageName} page!");
            Assert.AreEqual(consignmentRefNum, summary?.ConsignmentReferenceNumber, $"Consignment Reference Number is not matching in {pageName} page!");
            Assert.IsTrue(summary?.MainReasonForImport.ToUpper().Contains(mainReasonForImport.ToUpper()), $"Main reason for import is not matching in {pageName} page!");
            Assert.AreEqual(purpose.Replace(" ", "").ToUpper(), summary?.Purpose.Replace(" ", "").ToUpper(), $"Purpose is not matching in {pageName} page!");
            Assert.AreEqual(riskCategory.ToUpper(), summary?.RiskCategory.ToUpper(), $"Risk category is not matching in {pageName} page!");
        }

        private void VerifyDescriptionOfTheGoods()
        {
            var summary = summaryPage?.GetSummaryDetails();
            var pageName = "Review your notification";

            var commodityCode = _scenarioContext.Get<string>("CommodityCode");
            var typeOfCommodity = _scenarioContext.Get<string>("TypeOfCommodity");
            var species = _scenarioContext.Get<string>("Species");
            var netWeight = _scenarioContext.Get<string>("NetWeight");
            var packages = _scenarioContext.Get<string>("NumberOfPackages");
            var packageType = _scenarioContext.Get<string>("PackageType");
            var subtotalNetWeight = _scenarioContext.Get<string>("SubtotalNetWeight");
            var subtotalPackages = _scenarioContext.Get<string>("SubtotalPackages");
            var totalNetWeight = _scenarioContext.Get<string>("TotalNetWeight");
            var totalPackages = _scenarioContext.Get<string>("TotalPackages");
            var totalGrossWeight = _scenarioContext.Get<string>("TotalGrossWeight");
            var temperature = _scenarioContext.Get<string>("Temperature");

            Assert.AreEqual(commodityCode, summary?.CommodityCode, $"Commodity Code is not matching in {pageName} page!");
            Assert.AreEqual(typeOfCommodity, summary?.TypeOfCommodity, $"Type Of Commodity is not matching in {pageName} page!");
            Assert.AreEqual(species, summary?.Species, $"Species is not matching in {pageName} page!");
            Assert.AreEqual(netWeight, summary?.NetWeight, $"NetWeight is not matching in {pageName} page!");
            Assert.AreEqual(packages, summary?.NumberOfPackages, $"Number Of Packages is not matching in {pageName} page!");
            Assert.AreEqual(packageType, summary?.PackageType, $"Package Type is not matching in {pageName} page!");
            Assert.AreEqual(subtotalNetWeight, summary?.SubtotalNetWeight, $"Subtotal NetWeight is not matching in {pageName} page!");
            Assert.AreEqual(subtotalPackages, summary?.SubtotalPackages, $"Subtotal Packages is not matching in {pageName} page!");
            Assert.AreEqual(totalNetWeight, summary?.TotalNetWeight, $"Total NetWeight is not matching in {pageName} page!");
            Assert.AreEqual(totalPackages, summary?.TotalPackages, $"Total Packages is not matching in {pageName} page!");
            Assert.AreEqual(totalGrossWeight, summary?.TotalGrossWeight, $"Total Gross Weight is not matching in {pageName} page!");
            Assert.AreEqual(temperature, summary?.Temperature, $"Temperature is not matching in {pageName} page!");
        }

        private void VerifyDocuments()
        {
            var summary = summaryPage?.GetSummaryDetails();
            var pageName = "Review your notification";

            var documentType = _scenarioContext.Get<string>("DocumentType");
            var documentReference = _scenarioContext.Get<string>("DocumentReference");
            var dateOfIssue = _scenarioContext.Get<string>("DateOfIssue");
            var approvedEstablishmentName = _scenarioContext.Get<string>("ApprovedEstablishmentName");
            var approvedEstablishmentCountry = _scenarioContext.Get<string>("ApprovedEstablishmentCountry");
            var approvedEstablishmentType = _scenarioContext.Get<string>("ApprovedEstablishmentType");
            var approvedEstablishmentApprovalNum = _scenarioContext.Get<string>("ApprovedEstablishmentApprovalNum");

            Assert.AreEqual(documentType, summary?.DocumentType, $"Document Type is not matching in {pageName} page!");
            Assert.AreEqual(documentReference, summary?.DocumentReference, $"Document Reference is not matching in {pageName} page!");
            Assert.AreEqual(dateOfIssue, summary?.DateOfIssue, $"Date Of Issue is not matching in {pageName} page!");
            Assert.AreEqual(approvedEstablishmentName, summary?.ApprovedEstablishmentName, $"Approved Establishment Name is not matching in {pageName} page!");
            Assert.AreEqual(approvedEstablishmentCountry, summary?.ApprovedEstablishmentCountry, $"Approved Establishment Country is not matching in {pageName} page!");
            Assert.AreEqual(approvedEstablishmentType, summary?.ApprovedEstablishmentType, $"Approved Establishment Type is not matching in {pageName} page!");
            Assert.AreEqual(approvedEstablishmentApprovalNum, summary?.ApprovedEstablishmentApprovalNum, $"Approved Establishment Approval Number is not matching in {pageName} page!");
        }

        private void VerifyTraders()
        {
            var summary = summaryPage?.GetSummaryDetails();
            var pageName = "Review your notification";

            var consignorDetails = _scenarioContext.Get<string>("ConsignorDetails");
            var consigneeDetails = _scenarioContext.Get<string>("ConsigneeDetails");
            var importerDetails = _scenarioContext.Get<string>("ImporterDetails");
            var placeOfDestination = _scenarioContext.Get<string>("PlaceOfDestination");

            var actualConsignorDetails = summary?.ConsignorDetails.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var expectedConsignorDetails = consignorDetails.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var actualConsigneeDetails = summary?.ConsigneeDetails.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var expectedConsigneeDetails = consigneeDetails.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var actualImporterDetails = summary?.ImporterDetails.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var expectedImporterDetails = importerDetails.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var actualPlaceOfDestination = summary?.PlaceOfDestination.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var expectedPlaceOfDestination = placeOfDestination.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");

            Assert.IsTrue(actualConsignorDetails.Contains(expectedConsignorDetails), $"Consignor Details is not matching in {pageName} page!");
            Assert.IsTrue(actualConsigneeDetails.Contains(expectedConsigneeDetails), $"Consignee Details is not matching in {pageName} page!");
            Assert.IsTrue(actualImporterDetails.Contains(expectedImporterDetails), $"Importer Details is not matching in {pageName} page!");
            Assert.IsTrue(actualPlaceOfDestination.Contains(expectedPlaceOfDestination), $"Place Of Destination is not matching in {pageName} page!");
        }
    }
}