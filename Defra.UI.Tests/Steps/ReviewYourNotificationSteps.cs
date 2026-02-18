using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using YamlDotNet.Core;

namespace Defra.UI.Tests.Steps.IPAFF
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
            ValidateIfExists("ExitDate", reviewPage?.GetExitDate(), ref allDataMatches, mismatches);
            ValidateIfExists("ExitBCP", reviewPage?.GetExitBCP(), ref allDataMatches, mismatches);
            ValidateIfExists("DestinationCountry", reviewPage?.GetDestinationCountry(), ref allDataMatches, mismatches);

            // Commodity details  
            ValidateIfExists("CommodityCode", reviewPage?.GetCommodityCode(), ref allDataMatches, mismatches);
            ValidateIfExists("Species", reviewPage?.GetSpecies(), ref allDataMatches, mismatches);
            ValidateIfExists("NumberOfAnimals", reviewPage?.GetNumberOfAnimals(), ref allDataMatches, mismatches);
            ValidateIfExists("NumberOfPackages", reviewPage?.GetNumberOfPackages(), ref allDataMatches, mismatches);

            // Animal details - Use custom validation for CertificationOption
            ValidateCertificationOption(reviewPage?.GetCertificationOption(), ref allDataMatches, mismatches);
            ValidateIfExists("EarTag", reviewPage?.GetEarTag(), ref allDataMatches, mismatches);
            ValidateIfExists("UnweanedAnimalsOption", reviewPage?.GetUnweanedAnimalsOption(), ref allDataMatches, mismatches);
            ValidateIfExists("HorseName", reviewPage?.GetHorseName(), ref allDataMatches, mismatches);
            ValidateIfExists("MicrochipNumber", reviewPage?.GetMicrochipNumber(), ref allDataMatches, mismatches);
            ValidateIfExists("PassportNumber", reviewPage?.GetPassportNumber(), ref allDataMatches, mismatches);

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
            ValidateIfExists("PlaceOfDestinationName", reviewPage?.GetDestinationName(), ref allDataMatches, mismatches);
            ValidateIfExists("PlaceOfDestinationAddress", reviewPage?.GetDestinationAddress(), ref allDataMatches, mismatches);

            // Transport details
            ValidateIfExists("PortOfEntry", reviewPage?.GetPortOfEntry(), ref allDataMatches, mismatches);
            ValidateIfExists("MeansOfTransport", reviewPage?.GetMeansOfTransport(), ref allDataMatches, mismatches);
            ValidateIfExists("TransportId", reviewPage?.GetTransportId(), ref allDataMatches, mismatches);
            ValidateIfExists("AreContainers", reviewPage?.GetContainerUsage(), ref allDataMatches, mismatches);
            ValidateIfExists("ContainerNumber", reviewPage?.GetContainerNumber(), ref allDataMatches, mismatches);
            ValidateIfExists("SealNumber", reviewPage?.GetSealNumber(), ref allDataMatches, mismatches);
            ValidateIfExists("OfficialSealAffixed", reviewPage?.GetOfficialSeal(), ref allDataMatches, mismatches);
            ValidateIfExists("EnterTransportDocRef", reviewPage?.GetTransportDocumentReference(), ref allDataMatches, mismatches);
            ValidateIfExists("EstimatedArrivalDate", reviewPage?.GetEstimatedArrivalDate(), ref allDataMatches, mismatches);
            ValidateIfExists("EstimatedArrivalTime", reviewPage?.GetEstimatedArrivalTime(), ref allDataMatches, mismatches);
            ValidateIfExists("EstimatedJourneyTime", reviewPage?.GetEstimatedJourneyTime(), ref allDataMatches, mismatches);
            ValidateIfExists("IsGVMS", reviewPage?.GetGVMSUsage(), ref allDataMatches, mismatches);
            ValidateIfExists("MeansOfTransportAfterBCP", reviewPage?.GetMeansOfTransportAfterBCP(), ref allDataMatches, mismatches);
            ValidateIfExists("TransportIdentificationAfterBCP", reviewPage?.GetTransportIdentificationAfterBCP(), ref allDataMatches, mismatches);
            ValidateIfExists("TransportDocumentReferenceAfterBCP", reviewPage?.GetTransportDocumentReferenceAfterBCP(), ref allDataMatches, mismatches);
            ValidateIfExists("DepartureDateFromBCP", reviewPage?.GetDepartureDateFromBCP(), ref allDataMatches, mismatches);
            ValidateIfExists("DepartureTimeFromBCP", reviewPage?.GetDepartureTimeFromBCP(), ref allDataMatches, mismatches);

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

            if (_scenarioContext.ContainsKey("PlaceOfExit"))
            {
                var dateTime = reviewPage?.GetConsignmentDepartureDateTime();

                ValidateIfExists("PlaceOfExit", reviewPage?.GetPointOfExit, ref allDataMatches, mismatches);
                ValidateIfExists("ConsignmentLeavingFromGBDate", dateTime.Value.departureDate, ref allDataMatches, mismatches);
                ValidateIfExists("ConsignmentLeavingFromGBTime", dateTime.Value.departureTime, ref allDataMatches, mismatches);
            }

            if (_scenarioContext.ContainsKey("CommodityCodeFirstCommodity"))
            {
                ValidateIfExists("CommodityCodeFirstCommodity", reviewPage?.GetCommodityCodeList(0), ref allDataMatches, mismatches);
                ValidateIfExists("NetWeightFirstCommodity", reviewPage?.GetNetWeightList(0), ref allDataMatches, mismatches);
                ValidateIfExists("NumberOfPackagesFirstCommodity", reviewPage?.GetNumPackagesList(0), ref allDataMatches, mismatches);
                ValidateIfExists("TypeOfPackageFirstCommodity", reviewPage?.GetTypeOfPackagesList(0), ref allDataMatches, mismatches);

                ValidateIfExists("CommodityCodeSecondCommodity", reviewPage?.GetCommodityCodeList(1), ref allDataMatches, mismatches);
                ValidateIfExists("NetWeightSecondCommodity", reviewPage?.GetNetWeightList(1), ref allDataMatches, mismatches);
                ValidateIfExists("NumOfPackagesSecondCommodity", reviewPage?.GetNumPackagesList(1), ref allDataMatches, mismatches);
                ValidateIfExists("TypeOfPackageSecondCommodity", reviewPage?.GetTypeOfPackagesList(1), ref allDataMatches, mismatches);
            }
            else
            {
                ValidateIfExists("CommodityCode", reviewPage?.GetCommodityCodeList(0), ref allDataMatches, mismatches);
                ValidateIfExists("NetWeight", reviewPage?.GetNetWeightList(0), ref allDataMatches, mismatches);
                ValidateIfExists("NumberOfPackages", reviewPage?.GetNumPackagesList(0), ref allDataMatches, mismatches);
                ValidateIfExists("PackageType", reviewPage?.GetTypeOfPackagesList(0), ref allDataMatches, mismatches);
            }

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
            ValidateIfExists("PlaceOfDestinationName", reviewPage?.GetDestinationName(), ref allDataMatches, mismatches);
            ValidateIfExists("PlaceOfDestinationAddress", reviewPage?.GetDestinationAddress(), ref allDataMatches, mismatches);

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


        [Then("the data presented for review matches the data entered into the notification for CHED PP")]
        public void ThenTheDataPresentedForReviewMatchesTheDataEnteredIntoTheNotificationForCHEDPP()
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
            if (_scenarioContext.ContainsKey("CommodityCodeFirstCommodity")
                && !_scenarioContext.ContainsKey("AllCommodityDetails"))
            {
                ValidateIfExists("CommodityCodeFirstCommodity", reviewPage?.GetCommodityCodeList(0), ref allDataMatches, mismatches);
                ValidateIfExists("CommodityDescFirstCommodity", reviewPage?.GetDescriptionList(0), ref allDataMatches, mismatches);
                ValidateIfExists("GenusFirstCommodity", reviewPage?.GetGenusListCHEDPP(0), ref allDataMatches, mismatches);
                ValidateIfExists("EPPOCodeFirstCommodity", reviewPage?.GetEPPOCodeListCHEDPP(0), ref allDataMatches, mismatches);
                ValidateIfExists("NetWeight", reviewPage?.GetNetWeightListCHEDPP(0), ref allDataMatches, mismatches);
                ValidateIfExists("NumberOfPackages", reviewPage?.GetNumPackagesListCHEDPP(0), ref allDataMatches, mismatches);
                ValidateIfExists("PackageType", reviewPage?.GetTypeOfPackagesListCHEDPP(0), ref allDataMatches, mismatches);
                ValidateIfExists("Quantity", reviewPage?.GetQuantityListCHEDPP(0), ref allDataMatches, mismatches);
                ValidateIfExists("QuantityType", reviewPage?.GetQuantityTypeListCHEDPP(0), ref allDataMatches, mismatches);

                ValidateIfExists("CommodityCodeSecondCommodity", reviewPage?.GetCommodityCodeList(1), ref allDataMatches, mismatches);
                ValidateIfExists("CommodityDescSecondCommodity", reviewPage?.GetDescriptionList(1), ref allDataMatches, mismatches);
                ValidateIfExists("GenusSecondCommodity", reviewPage?.GetGenusListCHEDPP(1), ref allDataMatches, mismatches);
                ValidateIfExists("EPPOCodeSecondCommodity", reviewPage?.GetEPPOCodeListCHEDPP(1), ref allDataMatches, mismatches);
                ValidateIfExists("NetWeight", reviewPage?.GetNetWeightListCHEDPP(1), ref allDataMatches, mismatches);
                ValidateIfExists("NumberOfPackages", reviewPage?.GetNumPackagesListCHEDPP(1), ref allDataMatches, mismatches);
                ValidateIfExists("PackageType", reviewPage?.GetTypeOfPackagesListCHEDPP(1), ref allDataMatches, mismatches);
                ValidateIfExists("CommodityVariety", reviewPage?.GetCommodityVariety(1), ref allDataMatches, mismatches);
                ValidateIfExists("CommodityClass", reviewPage?.GetCommodityClass(1), ref allDataMatches, mismatches);
                ValidateIfExists("Quantity", reviewPage?.GetQuantityListCHEDPP(1), ref allDataMatches, mismatches);
                ValidateIfExists("QuantityType", reviewPage?.GetQuantityTypeListCHEDPP(1), ref allDataMatches, mismatches);
                ValidateIfExists("TotalNetWeight", reviewPage?.GetTotalNetWeight(), ref allDataMatches, mismatches);
                ValidateIfExists("TotalPackages", reviewPage?.GetTotalPackages(), ref allDataMatches, mismatches);
            }
            else if (!_scenarioContext.ContainsKey("CommodityCodeFirstCommodity")
                && !_scenarioContext.ContainsKey("AllCommodityDetails"))
            {
                ValidateIfExists("CommodityCode", reviewPage?.GetCommodityCodeList(0), ref allDataMatches, mismatches);
                ValidateIfExists("CommodityDescription", reviewPage?.GetDescriptionList(0), ref allDataMatches, mismatches);
                ValidateIfExists("GenusFirstCommodity", reviewPage?.GetGenusListCHEDPP(0), ref allDataMatches, mismatches);
                ValidateIfExists("EPPOCodeFirstCommodity", reviewPage?.GetEPPOCodeListCHEDPP(0), ref allDataMatches, mismatches);
                ValidateIfExists("NetWeight", reviewPage?.GetNetWeightListCHEDPP(0), ref allDataMatches, mismatches);
                ValidateIfExists("NumberOfPackages", reviewPage?.GetNumPackagesListCHEDPP(0), ref allDataMatches, mismatches);
                ValidateIfExists("PackageType", reviewPage?.GetTypeOfPackagesListCHEDPP(0), ref allDataMatches, mismatches);
                ValidateIfExists("Quantity", reviewPage?.GetQuantityListCHEDPP(0), ref allDataMatches, mismatches);
                ValidateIfExists("QuantityType", reviewPage?.GetQuantityTypeListCHEDPP(0), ref allDataMatches, mismatches);
                ValidateIfExists("TotalNetWeight", reviewPage?.GetTotalNetWeight(), ref allDataMatches, mismatches);
                ValidateIfExists("TotalPackages", reviewPage?.GetTotalPackages(), ref allDataMatches, mismatches);
            }
            ValidateIfExists("TotalGrossWeight", reviewPage?.GetTotalGrossWeight(), ref allDataMatches, mismatches);
            //ConfirmationToDeclareGMS exist for some GMS commodity codes only. 
            ValidateIfExists("ConfirmationToDeclareGMS", reviewPage?.GetConfirmationToDeclareGMS(), ref allDataMatches, mismatches);

            // Documents
            ValidateIfExists("DocumentType", reviewPage?.GetAdditionalDocumentType(), ref allDataMatches, mismatches);
            ValidateIfExists("DocumentReference", reviewPage?.GetAdditionalDocumentReference(), ref allDataMatches, mismatches);
            ValidateIfExists("DocumentDateOfIssue", reviewPage?.GetAdditionalDocumentDateOfIssue(), ref allDataMatches, mismatches);
            ValidateFileNameWithTruncation("DocumentName", reviewPage?.GetAdditionalDocumentFileName(), ref allDataMatches, mismatches);

            // Addresses
            ValidateIfExists("ConsignorName", reviewPage?.GetConsignorName(), ref allDataMatches, mismatches);
            ValidateIfExists("ConsignorAddress", reviewPage?.GetCHEDPPConsignorAddress(), ref allDataMatches, mismatches);
            ValidateIfExists("CompanyName", reviewPage?.GetImporterName(), ref allDataMatches, mismatches);
            ValidateIfExists("ImporterAddress", reviewPage?.GetCHEDPPImporterAddress(), ref allDataMatches, mismatches);
            ValidateIfExists("DeliveryAddressName", reviewPage?.GetDestinationName(), ref allDataMatches, mismatches);
            ValidateIfExists("DeliveryAddress", reviewPage?.GetDeliveryAddress(), ref allDataMatches, mismatches);

            // Transport details
            ValidateIfExists("BorderControlPost", reviewPage?.GetPortOfEntry(), ref allDataMatches, mismatches);
            ValidateIfExists("InspectionPremises", reviewPage?.GetInspectionPremises(), ref allDataMatches, mismatches);
            ValidateIfExists("MeansOfTransport", reviewPage?.GetMeansOfTransport(), ref allDataMatches, mismatches);
            ValidateIfExists("TransportId", reviewPage?.GetTransportId(), ref allDataMatches, mismatches);
            ValidateIfExists("AreContainers", reviewPage?.GetContainerUsage(), ref allDataMatches, mismatches);
            ValidateIfExists("EnterTransportDocRef", reviewPage?.GetTransportDocumentReference(), ref allDataMatches, mismatches);
            ValidateIfExists("EstimatedArrivalDate", reviewPage?.GetEstimatedArrivalDate(), ref allDataMatches, mismatches);
            ValidateIfExists("EstimatedArrivalTime", reviewPage?.GetEstimatedArrivalTime(), ref allDataMatches, mismatches);
            ValidateIfExists("IsCTC", reviewPage?.GetCTCUsage().Replace("-", "–"), ref allDataMatches, mismatches);
            ValidateIfExists("IsGVMS", reviewPage?.GetGVMSUsage(), ref allDataMatches, mismatches);


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


        /// <summary>
        /// Custom validation for Certification Option with special handling for abbreviated values
        /// on the review page (e.g., "Breeding and/or production" displays as "Breeding")
        /// </summary>
        private void ValidateCertificationOption(string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            const string contextKey = "CertificationOption";

            if (!_scenarioContext.ContainsKey(contextKey))
            {
                Console.WriteLine($"[REVIEW VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
                return;
            }

            var expectedValue = _scenarioContext.Get<string>(contextKey);
            if (string.IsNullOrEmpty(expectedValue))
            {
                Console.WriteLine($"[REVIEW VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                return;
            }

            var actualValue = reviewValue?.Trim();

            // Define mapping for certification options that display differently on review page
            var certificationMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Approved bodies", "Approved bodies" },
                { "Breeding and/or production", "Breeding" },
                { "Pets", "Pets" },
                { "Registered equidae", "Registered" },
                { "Slaughter", "Slaughter" },
                { "Other", "Other" }
            };

            // Get the expected display value (handles the mapping)
            var expectedDisplayValue = certificationMappings.ContainsKey(expectedValue)
                ? certificationMappings[expectedValue]
                : expectedValue;

            var isMatch = expectedDisplayValue.Equals(actualValue, StringComparison.OrdinalIgnoreCase);

            if (!isMatch)
            {
                allDataMatches = false;
                mismatches.Add($"{contextKey}: Expected '{expectedValue}' (displays as '{expectedDisplayValue}'), Found '{actualValue}'");
            }
            else
            {
                if (expectedValue != expectedDisplayValue)
                {
                    Console.WriteLine($"[REVIEW VALIDATION] ✓ {contextKey}: '{expectedValue}' matches (displays as '{expectedDisplayValue}')");
                }
                else
                {
                    Console.WriteLine($"[REVIEW VALIDATION] ✓ {contextKey}: '{expectedValue}' matches");
                }
            }
        }

        private void ValidateIfExists(string contextKey, string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            if (_scenarioContext.ContainsKey(contextKey))
            {
                object contextValue = _scenarioContext[contextKey];

                // Handle List<string> (for multiple countries)
                if (contextValue is List<string> countryList)
                {
                    // Join the list with line breaks to match the HTML format
                    var expectedValue = string.Join("\n", countryList).Trim();

                    // The review page displays countries with <br> which Selenium converts to \n
                    var actualValue = reviewValue?.Trim().Replace("\r\n", "\n").Replace("\r", "\n");

                    var isMatch = expectedValue.Equals(actualValue, StringComparison.OrdinalIgnoreCase);
                    if (!isMatch)
                    {
                        allDataMatches = false;
                        mismatches.Add($"{contextKey}: Expected '{expectedValue}', Found '{actualValue}'");
                    }
                    else
                    {
                        Console.WriteLine($"[REVIEW VALIDATION] ✓ {contextKey}: '{expectedValue}' matches");
                    }
                }
                // Handle string[] (for documents)
                else if (contextValue is string[] stringArray)
                {
                    var expectedValue = stringArray.FirstOrDefault();
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
                        Console.WriteLine($"[REVIEW VALIDATION] ⊘ {contextKey}: Skipped (empty value in array)");
                    }
                }
                // Handle single string value
                else if (contextValue is string expectedValue)
                {
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
                    Console.WriteLine($"[REVIEW VALIDATION] ⊘ {contextKey}: Skipped (unsupported type: {contextValue.GetType().Name})");
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

        [Then("the user verifies all the data displayed in review page for commodity code {string}")]
        public void ThenTheUserVerifiesAllTheDataDisplayedInReviewPageForCommodityCode(string code)
        {
            VerifyAboutTheConsignment();
            VerifyDescriptionOfTheGoods(code);
            VerifyDocuments();
            VerifyTraders();
            VerifyTransportAndConsignmentContactDetails();
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
            var mainReasonForImport = "For " + _scenarioContext.Get<string>("MainReasonForImport");
            var riskCategory = _scenarioContext.Get<string>("RiskCategory");
            if (_scenarioContext.TryGetValue("Purpose", out string purpose) &&
                !string.IsNullOrWhiteSpace(purpose))
            {
                Assert.AreEqual(purpose.Replace(" ", "").ToUpper(), summary?.Purpose.Replace(" ", "").ToUpper(), $"Purpose is not matching in {pageName} page!");
            }
            Assert.AreEqual(importType, summary?.ImportType, $"Import type is not matching in {pageName} page!");
            Assert.AreEqual(countryOfOrigin, summary?.CountryOfOrigin, $"Country of origin is not matching in {pageName} page!");
            Assert.AreEqual(contryFromWhereConsigned, summary?.ContryFromWhereConsigned, $"Country from where consigned is not matching in {pageName} page!");
            Assert.AreEqual(consignmentConformToRegulatoryRequirements, summary?.ConsignmentConformToRegulatoryRequirements, $"Consignment confirmation to regulatory requirements is not matching in {pageName} page!");
            Assert.AreEqual(consignmentRefNum, summary?.ConsignmentReferenceNumber, $"Consignment Reference Number is not matching in {pageName} page!");
            Assert.IsTrue(mainReasonForImport.ToUpper().Contains(summary?.MainReasonForImport.ToUpper()), $"Main reason for import is not matching in {pageName} page!");
            Assert.AreEqual(riskCategory.ToUpper(), summary?.RiskCategory.ToUpper(), $"Risk category is not matching in {pageName} page!");
        }

        private void VerifyDescriptionOfTheGoods(string code)
        {
            var summary = summaryPage?.GetSummaryDetails();
            var pageName = "Review your notification";
            var expectedList = new List<string>();
            
            var subtotalNetWeight = _scenarioContext.Get<string[]>("SubtotalNetWeight");
            var subtotalPackages = _scenarioContext.Get<string[]>("SubtotalPackages");
            var commodityCode = _scenarioContext["CommodityCode"] as List<string>;
            var typeOfCommodity = _scenarioContext.Get<List<string>>("TypeOfCommodity");
            var species = _scenarioContext["Species"] as List<string>;
            
            var totalNetWeight = _scenarioContext.Get<string>("TotalNetWeight");
            var totalPackages = _scenarioContext.Get<string>("TotalPackages");
            var totalGrossWeight = _scenarioContext.Get<string>("TotalGrossWeight");
            var temperature = _scenarioContext.Get<string>("Temperature");
            if (_scenarioContext.ContainsKey("CommodityCodeFirstCommodity"))
            {
                var commodityCodeFirstCommodity = _scenarioContext.Get<string>("CommodityCodeFirstCommodity");
                var netWeightFirstCommodity = _scenarioContext.Get<string>("NetWeightFirstCommodity");
                var numberOfPackagesFirstCommodity = _scenarioContext.Get<string>("NumberOfPackagesFirstCommodity");
                var typeOfPackageFirstCommodity = _scenarioContext.Get<string>("TypeOfPackageFirstCommodity");
                var commodityCodeSecondCommodity = _scenarioContext.Get<string>("CommodityCodeSecondCommodity");
                var netWeightSecondCommodity = _scenarioContext.Get<string>("NetWeightSecondCommodity");
                var numberOfPackagesSecondCommodity = _scenarioContext.Get<string>("NumOfPackagesSecondCommodity");
                var typeOfPackageSecondCommodity = _scenarioContext.Get<string>("TypeOfPackageSecondCommodity");

                Assert.AreEqual(commodityCodeFirstCommodity, summary?.CommodityCodeFirstCommodity, $"CommodityCode for First Commodity is not matching in {pageName} page!");
                Assert.AreEqual(netWeightFirstCommodity, summary?.NetWeightFirstCommodity, $"NetWeight for First Commodity is not matching in {pageName} page!");
                Assert.AreEqual(numberOfPackagesFirstCommodity, summary?.NumberOfPackagesFirstCommodity, $"Number Of Packages for First Commodity is not matching in {pageName} page!");
                Assert.AreEqual(typeOfPackageFirstCommodity, summary?.TypeOfPackageFirstCommodity, $"Type Of Package for First Commodity is not matching in {pageName} page!");
                Assert.AreEqual(commodityCodeSecondCommodity, summary?.CommodityCodeSecondCommodity, $"CommodityCode for Second Commodity is not matching in {pageName} page!");
                Assert.AreEqual(netWeightSecondCommodity, summary?.NetWeightSecondCommodity, $"NetWeight for Second Commodity is not matching in {pageName} page!");
                Assert.AreEqual(numberOfPackagesSecondCommodity, summary?.NumberOfPackagesSecondCommodity, $"Number Of Packages for Second Commodity is not matching in {pageName} page!");
                Assert.AreEqual(typeOfPackageSecondCommodity, summary?.TypeOfPackageSecondCommodity, $"Type Of Package for Second Commodity is not matching in {pageName} page!");
                CollectionAssert.AreEqual(typeOfCommodity, summary?.CommodityTypes, $"Type Of Package for Second Commodity is not matching in {pageName} page!");
            }
            else
            {
                var netWeight = _scenarioContext["NetWeight"] as List<string>;
                var packages = _scenarioContext["NumberOfPackages"] as List<string>;
                var packageType = _scenarioContext["PackageType"] as List<string>;
                Assert.AreEqual(commodityCode, summary?.CommodityCode, $"Commodity Code is not matching in {pageName} page!");
                for (int i = 0; i < netWeight.Count; i++)
                {
                    Assert.AreEqual(netWeight[i], summary?.NetWeight[i], $"NetWeight is not matching in {pageName} page!");
                }
                for (int i = 0; i < packages.Count; i++)
                {
                    Assert.AreEqual(packages[i], summary?.NumberOfPackages[i], $"Number Of Packages is not matching in {pageName} page!");
                }
                for (int i = 0; i < packageType.Count; i++)
                {
                    Assert.AreEqual(packageType[i], summary?.PackageType[i], $"Package Type is not matching in {pageName} page!");
                }

                foreach (var item in commodityCode.Where(c => c.StartsWith("160")))
                {
                    expectedList.AddRange(summary?.TypeOfCommodity1);
                }
                if (commodityCode.Any(c => !c.StartsWith("160")))
                {
                    expectedList.AddRange(summary?.TypeOfCommodity);
                }
                Assert.AreEqual(typeOfCommodity, expectedList);
            }
            
            Assert.AreEqual(species, summary?.Species, $"Species is not matching in {pageName} page!");
            CollectionAssert.AreEqual(subtotalNetWeight, summary?.SubtotalNetWeight, $"Subtotal NetWeight is not matching in {pageName} page!");
            CollectionAssert.AreEqual(subtotalPackages, summary?.SubtotalPackages, $"Subtotal Packages is not matching in {pageName} page!"); 
            Assert.AreEqual(totalNetWeight, summary?.TotalNetWeight, $"Total NetWeight is not matching in {pageName} page!");
            Assert.AreEqual(totalPackages, summary?.TotalPackages, $"Total Packages is not matching in {pageName} page!");
            Assert.AreEqual(totalGrossWeight, summary?.TotalGrossWeight, $"Total Gross Weight is not matching in {pageName} page!");
            Assert.AreEqual(temperature, summary?.Temperature, $"Temperature is not matching in {pageName} page!");
        }

        private void VerifyDocuments()
        {
            var summary = summaryPage?.GetSummaryDetails();
            var pageName = "Review your notification";

            var documentType = _scenarioContext.Get<string[]>("DocumentType");
            var documentReference = _scenarioContext.Get<string[]>("DocumentReference");
            var dateOfIssue = _scenarioContext.Get<string[]>("DateOfIssue");
            var approvedEstablishmentName = _scenarioContext.Get<string>("ApprovedEstablishmentName");
            var approvedEstablishmentCountry = _scenarioContext.Get<string>("ApprovedEstablishmentCountry");
            var approvedEstablishmentType = _scenarioContext.Get<string>("ApprovedEstablishmentType");
            var approvedEstablishmentApprovalNum = _scenarioContext.Get<string>("ApprovedEstablishmentApprovalNum");
            if (_scenarioContext.ContainsKey("CatchCertificateReference"))
            {
                var catchCertificateReference = _scenarioContext.Get<string[]>("CatchCertificateReference");
                var flagStateOfCatchingVessel = _scenarioContext.Get<string[]>("FlagStateOfCatchingVessel");
                var dateOfIssueCatchCertificate = _scenarioContext.Get<string[]>("DateOfIssueCatchCertificate");
                CollectionAssert.AreEqual(catchCertificateReference, summary?.CatchCertificateReference, $"Catch Certificate Reference is not matching in {pageName} page!");
                CollectionAssert.AreEqual(flagStateOfCatchingVessel, summary?.FlagStateOfCatchingVessel, $"Flag Stat eOf Catching Vessel is not matching in {pageName} page!");
                CollectionAssert.AreEqual(dateOfIssueCatchCertificate, summary?.DateOfIssueCatchCertificate, $"Date of Issue is not matching in {pageName} page!");
            }

            CollectionAssert.AreEqual(documentType, summary?.DocumentType, $"Document Type is not matching in {pageName} page!");
            CollectionAssert.AreEqual(documentReference, summary?.DocumentReference, $"Document Reference is not matching in {pageName} page!");
            CollectionAssert.AreEqual(dateOfIssue, summary?.DateOfIssue, $"Date Of Issue is not matching in {pageName} page!");
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
            var placeOfDestination = _scenarioContext.Get<string>("PlaceOfDestinationDetails");

            var actualConsignorDetails = summary?.ConsignorDetails.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var expectedConsignorDetails = consignorDetails.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var actualConsigneeDetails = summary?.ConsigneeDetails.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var expectedConsigneeDetails = consigneeDetails.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var actualImporterDetails = summary?.ImporterDetails.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var expectedImporterDetails = importerDetails.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var actualPlaceOfDestination = summary?.PlaceOfDestination.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var expectedPlaceOfDestination = placeOfDestination.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");

            Assert.IsTrue(actualConsignorDetails?.Contains(expectedConsignorDetails), $"Consignor Details is not matching in {pageName} page!");
            Assert.IsTrue(actualConsigneeDetails?.Contains(expectedConsigneeDetails), $"Consignee Details is not matching in {pageName} page!");
            Assert.IsTrue(actualImporterDetails?.Contains(expectedImporterDetails), $"Importer Details is not matching in {pageName} page!");
            Assert.IsTrue(actualPlaceOfDestination?.Contains(expectedPlaceOfDestination), $"Place Of Destination is not matching in {pageName} page!");
        }

        public void VerifyTransportAndConsignmentContactDetails()
        {
            var pageName = "Review your notification";

            var portOfEntry = _scenarioContext.Get<string>("PortOfEntry");
            var meansOfTransport = _scenarioContext.Get<string>("MeansOfTransport");
            var transportId = _scenarioContext.Get<string>("TransportId");
            var areContainers = _scenarioContext.Get<string>("AreContainers");
            var transportDocRef = _scenarioContext.Get<string>("EnterTransportDocRef");
            var estimatedArrivalDate = _scenarioContext.Get<string>("EstimatedArrivalDate");
            var estimatedArrivalTime = _scenarioContext.Get<string>("EstimatedArrivalTime");
            var isCTC = _scenarioContext.Get<string>("IsCTC");
            var isGVMS = _scenarioContext.Get<string>("IsGVMS");
            var consignmentContactAddress = _scenarioContext.Get<string>("ConsignmentContactAddress");

            Assert.AreEqual(portOfEntry, reviewPage?.GetPortOfEntry(), $"Port of Entry is not matching in {pageName} page!");
            Assert.AreEqual(meansOfTransport, reviewPage?.GetMeansOfTransport(), $"Means of Transport is not matching in {pageName} page!");
            Assert.AreEqual(transportId, reviewPage?.GetTransportId(), $"Transport Id is not matching in {pageName} page!");
            Assert.AreEqual(areContainers, reviewPage?.GetContainerUsage(), $"Containers is not matching in {pageName} page!");
            Assert.AreEqual(transportDocRef, reviewPage?.GetTransportDocumentReference(), $"Transport Document Reference is not matching in {pageName} page!");
            Assert.AreEqual(estimatedArrivalDate, reviewPage?.GetEstimatedArrivalDate(), $"Estimated Arrival Date is not matching in {pageName} page!");
            Assert.AreEqual(estimatedArrivalTime, reviewPage?.GetEstimatedArrivalTime(), $"Estimated Arrival Time is not matching in {pageName} page!");
            Assert.AreEqual(isCTC, reviewPage?.GetCTCUsage(), $"CTC Usage is not matching in {pageName} page!");
            Assert.AreEqual(isGVMS, reviewPage?.GetGVMSUsage(), $"GVMS Usage is not matching in {pageName} page!");
            Assert.AreEqual(consignmentContactAddress, reviewPage?.GetConsignmentContactAddress(), $"Consignment Contact Address is not matching in {pageName} page!");
        }

        [Then(@"the user should see an error message '(.*)' in review page")]
        public void ThenIShouldSeeAnErrorMessageInReviewPage(string errorMessage)
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                Assert.True(reviewPage?.IsError(errorMessage), $"There is no error message found with - {errorMessage}");
            }
        }

        [When(@"the user Clicks the change link under '(.*)'")]
        public void WhenIClickTheChangeLinkUnderHeading(string heading)
        {
            reviewPage?.ClickChangeLink(heading);
        }

        [Then("the user should not see an error message {string} in review page")]
        public void ThenTheUserShouldNotSeeAnErrorMessageInReviewPage(string errorMessage)
        {
            var (hasError, errorMessages) = reviewPage?.VerifyErrorMsgDisplayed(errorMessage)
                ?? (false, string.Empty);

            if (hasError)
            {
                Assert.Fail($"Error message '{errorMessage}' is still present on the review page. " +
                           $"All error messages found: {errorMessages}");
            }

            // If hasError is false, the assertion passes (no error banner or specific error not found)
            Assert.False(hasError, "Error validation passed - no error message found");
        }

        [Then("the user should not see any error messages in review page")]
        public void ThenTheUserShouldNotSeeAnyErrorMessagesInReviewPage()
        {
            var (hasError, errorMessages) = reviewPage?.VerifyErrorMsgDisplayed(string.Empty)
                ?? (false, string.Empty);

            if (hasError)
            {
                Assert.Fail($"Unexpected error messages found on the review page: {errorMessages}");
            }

            Assert.False(hasError, "Error validation passed - no error messages found");
        }

        [Then("the Copy as new button should be available")]
        public void ThenTheCopyAsNewButtonShouldBeAvailable()
        {
            Assert.True(reviewPage?.IsCopyAsNewButtonDisplayed(), "Copy as new button is not displayed");
        }

        [Then("the View CHED button should be available")]
        public void ThenTheViewCHEDButtonShouldBeAvailable()
        {
            Assert.True(reviewPage?.IsViewCHEDButtonDisplayed(), "View CHED button is not displayed");
        }

        [Then("the Change links should not be available")]
        public void ThenTheChangeLinksShouldNotBeAvailable()
        {
            Assert.True(reviewPage?.AreChangeLinksNotDisplayed(), "Change links should not be displayed but were found");
        }

        [When("the user clicks on the Dashboard link")]
        public void WhenTheUserClicksOnTheDashboardLink()
        {
            reviewPage?.ClickDashboardLink();
        }

        [Given("the user gets the CHED reference from the Review your notification page")]
        [When("the user gets the CHED reference from the Review your notification page")]
        [Then("the user gets the CHED reference from the Review your notification page")]
        public void WhenTheUserGetsTheCHEDReferenceFromTheReviewYourNotificationPage()
        {
            var chedReference = reviewPage?.GetCHEDReference();

            Assert.That(chedReference, Is.Not.Null.And.Not.Empty,
                "CHED reference not found on Review your notification page");

            _scenarioContext.Set(chedReference, "CHEDReference");

            Console.WriteLine($"[CHED REFERENCE] Retrieved and stored: {chedReference}");
        }

        [Then("the Consignor or exporter shows the new trader {string} on the review page")]
        public void ThenTheConsignorOrExporterShowsTheNewTraderOnTheReviewPage(string operatorType)
        {
            // Get the original operator details from address book (source of truth)
            var expectedName = _scenarioContext[$"{operatorType}Name"]?.ToString();
            var expectedAddress = _scenarioContext[$"{operatorType}Address"]?.ToString();
            var expectedCountry = _scenarioContext[$"{operatorType}Country"]?.ToString();

            // Get what's displayed on the review page
            var displayedName = reviewPage?.GetConsignorName();
            var displayedAddress = reviewPage?.GetConsignorAddress();
            var displayedCountry = reviewPage?.GetConsignorCountry();

            // Validate they match
            Assert.AreEqual(expectedName, displayedName,
                $"Consignor name mismatch. Expected: {expectedName}, Actual: {displayedName}");
            Assert.AreEqual(expectedAddress, displayedAddress,
                $"Consignor address mismatch. Expected: {expectedAddress}, Actual: {displayedAddress}");
            Assert.AreEqual(expectedCountry, displayedCountry,
                $"Consignor country mismatch. Expected: {expectedCountry}, Actual: {displayedCountry}");

            Console.WriteLine($"[REVIEW VALIDATION] ✓ Consignor from address book ({operatorType}) matches: {expectedName}, {expectedAddress}, {expectedCountry}");
        }

        [Then("the Consignee shows the new {string} on the review page")]
        public void ThenTheConsigneeShowsTheNewOnTheReviewPage(string operatorType)
        {
            // Get the original operator details from address book (source of truth)
            var expectedName = _scenarioContext[$"{operatorType}Name"]?.ToString();
            var expectedAddress = _scenarioContext[$"{operatorType}Address"]?.ToString();
            var expectedCountry = _scenarioContext[$"{operatorType}Country"]?.ToString();

            // Get what's displayed on the review page
            var displayedName = reviewPage?.GetConsigneeName();
            var displayedAddress = reviewPage?.GetConsigneeAddress();
            var displayedCountry = reviewPage?.GetConsigneeCountry();

            // Validate they match
            Assert.AreEqual(expectedName, displayedName,
                $"Consignee name mismatch. Expected: {expectedName}, Actual: {displayedName}");
            Assert.AreEqual(expectedAddress, displayedAddress,
                $"Consignee address mismatch. Expected: {expectedAddress}, Actual: {displayedAddress}");
            Assert.AreEqual(expectedCountry, displayedCountry,
                $"Consignee country mismatch. Expected: {expectedCountry}, Actual: {displayedCountry}");

            Console.WriteLine($"[REVIEW VALIDATION] ✓ Consignee from address book ({operatorType}) matches: {expectedName}, {expectedAddress}, {expectedCountry}");
        }

        [Then("the Importer shows the new {string} on the review page")]
        public void ThenTheImporterShowsTheNewOnTheReviewPage(string operatorType)
        {
            // Get the original operator details from address book (source of truth)
            var expectedName = _scenarioContext[$"{operatorType}Name"]?.ToString();
            var expectedAddress = _scenarioContext[$"{operatorType}Address"]?.ToString();
            var expectedCountry = _scenarioContext[$"{operatorType}Country"]?.ToString();

            // Get what's displayed on the review page
            var displayedName = reviewPage?.GetImporterName();
            var displayedAddress = reviewPage?.GetImporterAddress();
            var displayedCountry = reviewPage?.GetImporterCountry();

            // Validate they match
            Assert.AreEqual(expectedName, displayedName,
                $"Importer name mismatch. Expected: {expectedName}, Actual: {displayedName}");
            Assert.AreEqual(expectedAddress, displayedAddress,
                $"Importer address mismatch. Expected: {expectedAddress}, Actual: {displayedAddress}");
            Assert.AreEqual(expectedCountry, displayedCountry,
                $"Importer country mismatch. Expected: {expectedCountry}, Actual: {displayedCountry}");

            Console.WriteLine($"[REVIEW VALIDATION] ✓ Importer from address book ({operatorType}) matches: {expectedName}, {expectedAddress}, {expectedCountry}");
        }

        [Then("the Place of destination shows the new {string} on the review page")]
        public void ThenThePlaceOfDestinationShowsTheNewOnTheReviewPage(string operatorType)
        {
            // Get the original operator details from address book (source of truth)
            var expectedName = _scenarioContext[$"{operatorType}Name"]?.ToString();
            var expectedAddress = _scenarioContext[$"{operatorType}Address"]?.ToString();
            var expectedCountry = _scenarioContext[$"{operatorType}Country"]?.ToString();

            // Get what's displayed on the review page
            var displayedName = reviewPage?.GetDestinationName();
            var displayedAddress = reviewPage?.GetDestinationAddress();
            var displayedCountry = reviewPage?.GetPlaceOfDestinationCountry();

            // Validate they match
            Assert.AreEqual(expectedName, displayedName,
                $"Place of destination name mismatch. Expected: {expectedName}, Actual: {displayedName}");
            Assert.AreEqual(expectedAddress, displayedAddress,
                $"Place of destination address mismatch. Expected: {expectedAddress}, Actual: {displayedAddress}");
            Assert.AreEqual(expectedCountry, displayedCountry,
                $"Place of destination country mismatch. Expected: {expectedCountry}, Actual: {displayedCountry}");

            Console.WriteLine($"[REVIEW VALIDATION] ✓ Place of destination from address book ({operatorType}) matches: {expectedName}, {expectedAddress}, {expectedCountry}");
        }

        [Then("the Transporter shows the new {string} on the review page")]
        public void ThenTheTransporterShowsTheNewOnTheReviewPage(string operatorType)
        {
            // Get the original operator details from address book (source of truth)
            var expectedName = _scenarioContext[$"{operatorType}Name"]?.ToString();
            var expectedAddress = _scenarioContext[$"{operatorType}Address"]?.ToString();
            var expectedCountry = _scenarioContext[$"{operatorType}Country"]?.ToString();
            var expectedApprovalNumber = _scenarioContext[$"{operatorType}ApprovalNumber"]?.ToString();
            var expectedType = _scenarioContext[$"{operatorType}Type"]?.ToString();

            // Get what's displayed on the review page
            var displayedName = reviewPage?.GetTransporterName();
            var displayedAddress = reviewPage?.GetTransporterAddressWithoutContact(); // ← Use the NEW method here
            var displayedCountry = reviewPage?.GetTransporterCountry();
            var displayedApprovalNumber = reviewPage?.GetTransporterApprovalNumber();
            var displayedType = reviewPage?.GetTransporterType();

            // Validate they match
            Assert.AreEqual(expectedName, displayedName,
                $"Transporter name mismatch. Expected: {expectedName}, Actual: {displayedName}");
            Assert.AreEqual(expectedAddress, displayedAddress,
                $"Transporter address mismatch. Expected: {expectedAddress}, Actual: {displayedAddress}");
            Assert.AreEqual(expectedCountry, displayedCountry,
                $"Transporter country mismatch. Expected: {expectedCountry}, Actual: {displayedCountry}");

            // Approval number might be empty, so handle that
            if (!string.IsNullOrEmpty(expectedApprovalNumber))
            {
                Assert.AreEqual(expectedApprovalNumber, displayedApprovalNumber,
                    $"Transporter approval number mismatch. Expected: {expectedApprovalNumber}, Actual: {displayedApprovalNumber}");
            }

            Assert.AreEqual(expectedType, displayedType,
                $"Transporter type mismatch. Expected: {expectedType}, Actual: {displayedType}");

            Console.WriteLine($"[REVIEW VALIDATION] ✓ Transporter from address book ({operatorType}) matches: {expectedName}, {expectedAddress}, {expectedCountry}");
        }
    }
}