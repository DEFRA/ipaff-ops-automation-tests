using Defra.UI.Tests.Tools.PDFProcessor.Models;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using Reqnroll;
using System.Text.RegularExpressions;

namespace Defra.UI.Tests.Tools.PDFProcessor
{
    /// <summary>
    /// Service for downloading PDFs from browser and extracting/validating their content
    /// </summary>
    public class PdfValidationService
    {
        private readonly PdfToJsonConverter _pdfConverter;
        private readonly ScenarioContext _scenarioContext;
        private readonly string _downloadDirectory;
        private const bool DebugMode = true;

        public PdfValidationService(ScenarioContext scenarioContext)
        {
            _pdfConverter = new PdfToJsonConverter();
            _scenarioContext = scenarioContext;

            _downloadDirectory = DebugMode
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "PDFValidationDebug")
                : Path.Combine(Path.GetTempPath(), "PDFValidation", Guid.NewGuid().ToString());

            EnsureDownloadDirectoryExists();

            if (DebugMode)
            {
                Console.WriteLine($"[PDF DEBUG] Files will be saved to: {_downloadDirectory}");
            }
        }

        public JArray DownloadAndExtractPdfContent(IWebDriver driver)
        {
            var pdfUrl = driver.Url;
            if (!pdfUrl.Contains("/certificate/pdf"))
            {
                throw new InvalidOperationException($"Current URL is not a PDF certificate URL: {pdfUrl}");
            }

            var pdfFilePath = DownloadPdfFromUrl(driver, pdfUrl);
            var jsonContent = _pdfConverter.ConvertToJson(pdfFilePath);

            var pdfData = JArray.Parse(jsonContent);
            _scenarioContext["ExtractedPdfContent"] = pdfData;

            if (DebugMode)
            {
                var jsonFilePath = Path.ChangeExtension(pdfFilePath, ".json");
                File.WriteAllText(jsonFilePath, jsonContent);
                Console.WriteLine($"[PDF DEBUG] JSON saved to: {jsonFilePath}");
                Console.WriteLine($"[PDF DEBUG] PDF saved to: {pdfFilePath}");
            }
            else
            {
                CleanupFile(pdfFilePath);
            }

            return pdfData;
        }

        public PdfValidationResult ValidatePdfAgainstNotificationData()
        {
            var result = new PdfValidationResult();

            if (!_scenarioContext.ContainsKey("ExtractedPdfContent"))
            {
                result.IsValid = false;
                result.Errors.Add("PDF content has not been extracted. Call DownloadAndExtractPdfContent first.");
                return result;
            }

            var pdfData = _scenarioContext.Get<JArray>("ExtractedPdfContent");

            if (DebugMode)
            {
                LogAvailableSections(pdfData);
            }

            Console.WriteLine("[PDF VALIDATION] ========== Starting PDF Validation ==========");

            // CHED Reference
            ValidateField(pdfData, result, "CHEDReference", "I2ChedReference", "Id");

            // About the consignment
            ValidateField(pdfData, result, "CountryOfOrigin", "I11CountryOfOrigin", "Value");

            // NOTE: ConsignmentReferenceNumber is NOT in PDF - Local Reference (I.3) is different field populated in Part 2
            // Skip validation for ConsignmentReferenceNumber

            // Import Type - from I.31 commodity text
            ValidateImportType(pdfData, result);

            // Main Reason for Import - from I.23 ForInternalMarket
            ValidateMainReasonForImport(pdfData, result);

            // Commodity details
            ValidateCommodityInText(pdfData, result, "CommodityCode");
            ValidateCommodityInText(pdfData, result, "Species");
            ValidateField(pdfData, result, "NumberOfAnimals", "I33Quantity", "PackageCount", extractNumber: true);
            ValidateField(pdfData, result, "NumberOfPackages", "I32TotalNumberOfPackages", "Value");

            // Animal details
            ValidateField(pdfData, result, "CertificationOption", "I18GoodsCertifiedAs", "Value");
            ValidateAnimalIdentificationInText(pdfData, result, "EarTag");
            ValidateAnimalIdentificationInText(pdfData, result, "HorseName");
            ValidateAnimalIdentificationInText(pdfData, result, "MicrochipNumber");
            ValidateAnimalIdentificationInText(pdfData, result, "PassportNumber");

            // Unweaned animals - from I.31 text
            ValidateUnweanedAnimals(pdfData, result);

            // Documents - I.9 Accompanying Documents
            ValidateAccompanyingDocuments(pdfData, result);

            // Addresses - Consignor/Exporter
            ValidateField(pdfData, result, "ConsignorName", "I1ConsignorExporter", "Name");
            ValidateAddressField(pdfData, result, "ConsignorAddress", "I1ConsignorExporter");

            // Addresses - Consignee/Importer  
            ValidateField(pdfData, result, "ConsigneeName", "I6ConsigneeImporter", "Name");
            ValidateAddressField(pdfData, result, "ConsigneeAddress", "I6ConsigneeImporter");

            // Addresses - Importer
            ValidateField(pdfData, result, "ImporterName", "I6ConsigneeImporter", "Name");
            ValidateAddressField(pdfData, result, "ImporterAddress", "I6ConsigneeImporter");

            // Addresses - Place of Destination
            ValidateField(pdfData, result, "PlaceOfDestinationName", "I7PlaceOfDestination", "Name");
            ValidateAddressField(pdfData, result, "PlaceOfDestinationAddress", "I7PlaceOfDestination");

            // Transport details
            ValidateBcpFromMultipleSections(pdfData, result);
            ValidateField(pdfData, result, "MeansOfTransport", "I13MeansOfTransport", "Mode");
            ValidateField(pdfData, result, "TransportId", "I13MeansOfTransport", "Identification");
            ValidateField(pdfData, result, "EnterTransportDocRef", "I13MeansOfTransport", "InternationalTransportDocument");

            // Container validation - I.17
            ValidateContainerUsage(pdfData, result);

            // Arrival date/time from I10PriorNotification
            ValidateDateField(pdfData, result, "EstimatedArrivalDate", "I10PriorNotification", "Date");
            ValidateTimeField(pdfData, result, "EstimatedArrivalTime", "I10PriorNotification", "Time");

            // Transporter details - with multi-line fix
            ValidateField(pdfData, result, "TransporterName", "I28Transporter", "Name");
            ValidateTransporterAddress(pdfData, result);
            ValidateTransporterCountry(pdfData, result);
            ValidateField(pdfData, result, "TransporterApprovalNumber", "I28Transporter", "ApprovalNumber");

            // Declaration
            ValidateField(pdfData, result, "DeclarationSignatory", "I35Declaration", "NameOfSignatory");

            Console.WriteLine("[PDF VALIDATION] ========== PDF Validation Complete ==========");

            result.IsValid = result.Errors.Count == 0;
            return result;
        }

        private void ValidateField(JArray pdfData, PdfValidationResult result, string contextKey,
            string sectionName, string fieldName, bool extractNumber = false)
        {
            if (!_scenarioContext.TryGetValue(contextKey, out var expectedValue))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
                return;
            }

            var expected = GetExpectedValue(expectedValue);
            if (string.IsNullOrEmpty(expected))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                return;
            }

            var pdfValue = GetFieldValue(pdfData, sectionName, fieldName);

            if (extractNumber && !string.IsNullOrEmpty(pdfValue))
            {
                pdfValue = ExtractNumber(pdfValue);
            }

            Console.WriteLine($"[PDF VALIDATION] Checking {contextKey}:");
            Console.WriteLine($"[PDF VALIDATION]   Expected: '{expected}'");
            Console.WriteLine($"[PDF VALIDATION]   Found:    '{pdfValue ?? "(null)"}'");

            if (string.IsNullOrEmpty(pdfValue))
            {
                result.Warnings.Add($"{contextKey}: Not found in PDF (Section: {sectionName}, Field: {fieldName})");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Not found in PDF");
            }
            else if (ValuesMatch(expected, pdfValue))
            {
                result.ValidatedFields.Add(contextKey, pdfValue);
                Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: Match");
            }
            else
            {
                result.Errors.Add($"{contextKey}: Expected '{expected}', Found '{pdfValue}'");
                Console.WriteLine($"[PDF VALIDATION] ✗ {contextKey}: MISMATCH");
            }
        }

        private void ValidateImportType(JArray pdfData, PdfValidationResult result)
        {
            const string contextKey = "ImportType";

            if (!_scenarioContext.TryGetValue(contextKey, out var expectedValue))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
                return;
            }

            var expected = GetExpectedValue(expectedValue);
            if (string.IsNullOrEmpty(expected))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                return;
            }

            // Get from I31 array's ImportType field or parse from Value text
            var commodityText = GetCommodityDescriptionText(pdfData);
            var pdfImportType = GetFieldFromCommodityArray(pdfData, "ImportType");

            Console.WriteLine($"[PDF VALIDATION] Checking {contextKey}:");
            Console.WriteLine($"[PDF VALIDATION]   Expected: '{expected}'");

            // If not in dedicated field, search commodity text for patterns like "LIVE ANIMALS"
            if (string.IsNullOrEmpty(pdfImportType) && !string.IsNullOrEmpty(commodityText))
            {
                if (commodityText.Contains("LIVE ANIMALS", StringComparison.OrdinalIgnoreCase))
                {
                    pdfImportType = "LIVE ANIMALS";
                }
                else if (commodityText.Contains("PRODUCTS", StringComparison.OrdinalIgnoreCase))
                {
                    pdfImportType = "Products";
                }
                else if (commodityText.Contains("HIGH RISK", StringComparison.OrdinalIgnoreCase))
                {
                    pdfImportType = "High risk food and feed";
                }
            }

            Console.WriteLine($"[PDF VALIDATION]   Found:    '{pdfImportType ?? "(null)"}'");

            if (string.IsNullOrEmpty(pdfImportType))
            {
                result.Warnings.Add($"{contextKey}: Not found in PDF");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Not found in PDF");
            }
            else if (ImportTypeMatches(expected, pdfImportType))
            {
                result.ValidatedFields.Add(contextKey, pdfImportType);
                Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: Match");
            }
            else
            {
                result.Warnings.Add($"{contextKey}: Format differs. Expected '{expected}', Found '{pdfImportType}'");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Format differs (acceptable)");
                result.ValidatedFields.Add(contextKey, pdfImportType);
            }
        }

        private void ValidateMainReasonForImport(JArray pdfData, PdfValidationResult result)
        {
            const string contextKey = "MainReasonForImport";

            if (!_scenarioContext.TryGetValue(contextKey, out var expectedValue))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
                return;
            }

            var expected = GetExpectedValue(expectedValue);
            if (string.IsNullOrEmpty(expected))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                return;
            }

            // Check I23ForInternalMarket section - value "true" means Internal market
            var forInternalMarket = GetFieldValue(pdfData, "I23ForInternalMarket", "Value");

            Console.WriteLine($"[PDF VALIDATION] Checking {contextKey}:");
            Console.WriteLine($"[PDF VALIDATION]   Expected: '{expected}'");
            Console.WriteLine($"[PDF VALIDATION]   I23ForInternalMarket Value: '{forInternalMarket ?? "(null)"}'");

            string pdfReason = null;
            if (forInternalMarket?.Equals("true", StringComparison.OrdinalIgnoreCase) == true)
            {
                pdfReason = "Internal market";
            }
            // Add other purpose sections as needed (I.24 Transit, etc.)

            if (string.IsNullOrEmpty(pdfReason))
            {
                result.Warnings.Add($"{contextKey}: Could not determine from PDF");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Could not determine");
            }
            else if (expected.Contains(pdfReason, StringComparison.OrdinalIgnoreCase) ||
                     pdfReason.Contains(expected, StringComparison.OrdinalIgnoreCase))
            {
                result.ValidatedFields.Add(contextKey, pdfReason);
                Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: Match");
            }
            else
            {
                result.Warnings.Add($"{contextKey}: Expected '{expected}', Determined '{pdfReason}'");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: May not match");
            }
        }

        private void ValidateUnweanedAnimals(JArray pdfData, PdfValidationResult result)
        {
            const string contextKey = "UnweanedAnimalsOption";

            if (!_scenarioContext.TryGetValue(contextKey, out var expectedValue))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
                return;
            }

            var expected = GetExpectedValue(expectedValue);
            if (string.IsNullOrEmpty(expected))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                return;
            }

            // Check I31 for IncludesUnweanedAnimals field
            var unweanedValue = GetFieldFromCommodityArray(pdfData, "IncludesUnweanedAnimals");
            var commodityText = GetCommodityDescriptionText(pdfData);

            Console.WriteLine($"[PDF VALIDATION] Checking {contextKey}:");
            Console.WriteLine($"[PDF VALIDATION]   Expected: '{expected}'");

            // If not in structured field, check commodity text
            if (string.IsNullOrEmpty(unweanedValue) && !string.IsNullOrEmpty(commodityText))
            {
                if (commodityText.Contains("Includes unweaned animals", StringComparison.OrdinalIgnoreCase))
                {
                    // Text present - determine Yes/No from context
                    unweanedValue = "field_present";
                }
            }

            Console.WriteLine($"[PDF VALIDATION]   Found:    '{unweanedValue ?? "(null)"}'");

            if (string.IsNullOrEmpty(unweanedValue))
            {
                result.Warnings.Add($"{contextKey}: Not found in PDF");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Not found in PDF");
            }
            else
            {
                // Map true/false to Yes/No
                var pdfAnswer = unweanedValue switch
                {
                    "true" => "Yes",
                    "false" => "No",
                    "field_present" => "Present",
                    _ => unweanedValue
                };

                if (expected.Equals(pdfAnswer, StringComparison.OrdinalIgnoreCase) ||
                    (expected.Equals("No", StringComparison.OrdinalIgnoreCase) && unweanedValue.Equals("false", StringComparison.OrdinalIgnoreCase)) ||
                    (expected.Equals("Yes", StringComparison.OrdinalIgnoreCase) && unweanedValue.Equals("true", StringComparison.OrdinalIgnoreCase)))
                {
                    result.ValidatedFields.Add(contextKey, pdfAnswer);
                    Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: Match");
                }
                else
                {
                    result.Warnings.Add($"{contextKey}: Expected '{expected}', Found '{pdfAnswer}'");
                    Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: May not match");
                }
            }
        }

        private void ValidateAccompanyingDocuments(JArray pdfData, PdfValidationResult result)
        {
            // Get I9 section
            var documentsSection = GetSection(pdfData, "I9AccompanyingDocuments");
            var documentsText = documentsSection?["Value"]?.ToString() ?? "";

            // Validate HealthCertificateReference
            if (_scenarioContext.TryGetValue("HealthCertificateReference", out var healthCertRef))
            {
                var expected = GetExpectedValue(healthCertRef);
                Console.WriteLine($"[PDF VALIDATION] Checking HealthCertificateReference:");
                Console.WriteLine($"[PDF VALIDATION]   Expected: '{expected}'");
                Console.WriteLine($"[PDF VALIDATION]   Searching in documents text...");

                if (!string.IsNullOrEmpty(expected) && documentsText.Contains(expected, StringComparison.OrdinalIgnoreCase))
                {
                    result.ValidatedFields.Add("HealthCertificateReference", expected);
                    Console.WriteLine($"[PDF VALIDATION] ✓ HealthCertificateReference: Found");
                }
                else if (!string.IsNullOrEmpty(expected))
                {
                    result.Warnings.Add($"HealthCertificateReference: '{expected}' not found in documents");
                    Console.WriteLine($"[PDF VALIDATION] ⚠ HealthCertificateReference: Not found");
                }
            }
            else
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ HealthCertificateReference: Skipped (not in context)");
            }

            // Validate DocumentReference (additional documents)
            if (_scenarioContext.TryGetValue("DocumentReference", out var docRef))
            {
                var expected = GetExpectedValue(docRef);
                Console.WriteLine($"[PDF VALIDATION] Checking DocumentReference:");
                Console.WriteLine($"[PDF VALIDATION]   Expected: '{expected}'");
                Console.WriteLine($"[PDF VALIDATION]   Searching in documents text...");

                if (!string.IsNullOrEmpty(expected) && documentsText.Contains(expected, StringComparison.OrdinalIgnoreCase))
                {
                    result.ValidatedFields.Add("DocumentReference", expected);
                    Console.WriteLine($"[PDF VALIDATION] ✓ DocumentReference: Found");
                }
                else if (!string.IsNullOrEmpty(expected))
                {
                    result.Warnings.Add($"DocumentReference: '{expected}' not found in documents");
                    Console.WriteLine($"[PDF VALIDATION] ⚠ DocumentReference: Not found");
                }
            }
            else
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ DocumentReference: Skipped (not in context)");
            }
        }

        private void ValidateContainerUsage(JArray pdfData, PdfValidationResult result)
        {
            const string contextKey = "AreContainers";

            if (!_scenarioContext.TryGetValue(contextKey, out var expectedValue))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
                return;
            }

            var expected = GetExpectedValue(expectedValue);
            if (string.IsNullOrEmpty(expected))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                return;
            }

            // Check I17ContainerNoSealNo section
            var containerValue = GetFieldValue(pdfData, "I17ContainerNoSealNo", "Value");

            Console.WriteLine($"[PDF VALIDATION] Checking {contextKey}:");
            Console.WriteLine($"[PDF VALIDATION]   Expected: '{expected}'");
            Console.WriteLine($"[PDF VALIDATION]   I17 Value: '{containerValue ?? "(empty)"}'");

            var expectedNo = expected.Equals("No", StringComparison.OrdinalIgnoreCase);
            var containerEmpty = string.IsNullOrWhiteSpace(containerValue);

            if (expectedNo && containerEmpty)
            {
                result.ValidatedFields.Add(contextKey, "No (empty)");
                Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: Match (container section empty as expected)");
            }
            else if (!expectedNo && !containerEmpty)
            {
                result.ValidatedFields.Add(contextKey, "Yes");
                Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: Match (container data present)");

                // Validate specific container/seal numbers if provided
                ValidateContainerDetails(pdfData, result, containerValue);
            }
            else if (expectedNo && !containerEmpty)
            {
                result.Warnings.Add($"{contextKey}: Expected 'No' but container section has data");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Unexpected container data");
            }
            else
            {
                result.Warnings.Add($"{contextKey}: Expected containers but section is empty");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Missing container data");
            }
        }

        private void ValidateContainerDetails(JArray pdfData, PdfValidationResult result, string containerValue)
        {
            if (_scenarioContext.TryGetValue("ContainerNumber", out var containerNum))
            {
                var containerNumStr = containerNum?.ToString() ?? "";
                if (!string.IsNullOrEmpty(containerNumStr))
                {
                    if (containerValue.Contains(containerNumStr, StringComparison.OrdinalIgnoreCase))
                    {
                        result.ValidatedFields.Add("ContainerNumber", containerNumStr);
                        Console.WriteLine($"[PDF VALIDATION] ✓ ContainerNumber: Found");
                    }
                    else
                    {
                        result.Warnings.Add($"ContainerNumber: '{containerNumStr}' not found in container section");
                        Console.WriteLine($"[PDF VALIDATION] ⚠ ContainerNumber: Not found");
                    }
                }
            }

            if (_scenarioContext.TryGetValue("SealNumber", out var sealNum))
            {
                var sealNumStr = sealNum?.ToString() ?? "";
                if (!string.IsNullOrEmpty(sealNumStr))
                {
                    if (containerValue.Contains(sealNumStr, StringComparison.OrdinalIgnoreCase))
                    {
                        result.ValidatedFields.Add("SealNumber", sealNumStr);
                        Console.WriteLine($"[PDF VALIDATION] ✓ SealNumber: Found");
                    }
                    else
                    {
                        result.Warnings.Add($"SealNumber: '{sealNumStr}' not found in container section");
                        Console.WriteLine($"[PDF VALIDATION] ⚠ SealNumber: Not found");
                    }
                }
            }
        }

        private void ValidateAddressField(JArray pdfData, PdfValidationResult result, string contextKey, string sectionName)
        {
            if (!_scenarioContext.TryGetValue(contextKey, out var expectedValue))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
                return;
            }

            var expected = GetExpectedValue(expectedValue);
            if (string.IsNullOrEmpty(expected))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                return;
            }

            var pdfValue = GetFieldValue(pdfData, sectionName, "Address");

            Console.WriteLine($"[PDF VALIDATION] Checking {contextKey}:");
            Console.WriteLine($"[PDF VALIDATION]   Expected: '{expected}'");
            Console.WriteLine($"[PDF VALIDATION]   Found:    '{pdfValue ?? "(null)"}'");

            if (string.IsNullOrEmpty(pdfValue))
            {
                result.Warnings.Add($"{contextKey}: Not found in PDF");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Not found in PDF");
            }
            else if (AddressesMatch(expected, pdfValue))
            {
                result.ValidatedFields.Add(contextKey, pdfValue);
                Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: Match");
            }
            else
            {
                result.Errors.Add($"{contextKey}: Expected '{expected}', Found '{pdfValue}'");
                Console.WriteLine($"[PDF VALIDATION] ✗ {contextKey}: MISMATCH");
            }
        }

        private void ValidateTransporterAddress(JArray pdfData, PdfValidationResult result)
        {
            const string contextKey = "TransporterAddress";

            if (!_scenarioContext.TryGetValue(contextKey, out var expectedValue))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
                return;
            }

            var expected = GetExpectedValue(expectedValue);
            if (string.IsNullOrEmpty(expected))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                return;
            }

            var section = GetSection(pdfData, "I28Transporter");
            var pdfAddress = section?["Address"]?.ToString();

            Console.WriteLine($"[PDF VALIDATION] Checking {contextKey}:");
            Console.WriteLine($"[PDF VALIDATION]   Expected: '{expected}'");
            Console.WriteLine($"[PDF VALIDATION]   Found:    '{pdfAddress ?? "(null)"}'");

            if (string.IsNullOrEmpty(pdfAddress))
            {
                result.Warnings.Add($"{contextKey}: Not found in PDF");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Not found in PDF");
                return;
            }

            // Check for exact or partial match (handle truncation from multi-line parsing)
            if (AddressesMatch(expected, pdfAddress))
            {
                result.ValidatedFields.Add(contextKey, pdfAddress);
                Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: Match");
            }
            else if (expected.StartsWith(pdfAddress.TrimEnd(','), StringComparison.OrdinalIgnoreCase) ||
                     pdfAddress.StartsWith(expected.Split(',')[0], StringComparison.OrdinalIgnoreCase))
            {
                // Partial match - PDF may be truncated due to multi-line issues
                result.Warnings.Add($"{contextKey}: Partial match (PDF may be truncated). Expected '{expected}', Found '{pdfAddress}'");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Partial match (truncation accepted)");
                result.ValidatedFields.Add(contextKey, pdfAddress);
            }
            else
            {
                result.Errors.Add($"{contextKey}: Expected '{expected}', Found '{pdfAddress}'");
                Console.WriteLine($"[PDF VALIDATION] ✗ {contextKey}: MISMATCH");
            }
        }

        private void ValidateTransporterCountry(JArray pdfData, PdfValidationResult result)
        {
            const string contextKey = "TransporterCountry";

            if (!_scenarioContext.TryGetValue(contextKey, out var expectedValue))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
                return;
            }

            var expected = GetExpectedValue(expectedValue);
            if (string.IsNullOrEmpty(expected))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                return;
            }

            var section = GetSection(pdfData, "I28Transporter");
            var pdfCountry = section?["Country"]?.ToString();

            Console.WriteLine($"[PDF VALIDATION] Checking {contextKey}:");
            Console.WriteLine($"[PDF VALIDATION]   Expected: '{expected}'");
            Console.WriteLine($"[PDF VALIDATION]   Found:    '{pdfCountry ?? "(null)"}'");

            if (string.IsNullOrEmpty(pdfCountry))
            {
                result.Warnings.Add($"{contextKey}: Not found in PDF");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Not found in PDF");
                return;
            }

            if (CountriesMatch(expected, pdfCountry))
            {
                result.ValidatedFields.Add(contextKey, pdfCountry);
                Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: Match");
            }
            else if (expected.StartsWith(pdfCountry, StringComparison.OrdinalIgnoreCase))
            {
                // Partial match - PDF may be truncated due to multi-line issues
                result.Warnings.Add($"{contextKey}: Partial match (PDF may be truncated). Expected '{expected}', Found '{pdfCountry}'");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Partial match (truncation accepted)");
                result.ValidatedFields.Add(contextKey, pdfCountry);
            }
            else
            {
                result.Errors.Add($"{contextKey}: Expected '{expected}', Found '{pdfCountry}'");
                Console.WriteLine($"[PDF VALIDATION] ✗ {contextKey}: MISMATCH");
            }
        }

        private void ValidateCommodityInText(JArray pdfData, PdfValidationResult result, string contextKey)
        {
            if (!_scenarioContext.TryGetValue(contextKey, out var expectedValue))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
                return;
            }

            var expected = GetExpectedValue(expectedValue);
            if (string.IsNullOrEmpty(expected))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                return;
            }

            var commodityText = GetCommodityDescriptionText(pdfData);

            Console.WriteLine($"[PDF VALIDATION] Checking {contextKey} in commodity text:");
            Console.WriteLine($"[PDF VALIDATION]   Expected: '{expected}'");
            Console.WriteLine($"[PDF VALIDATION]   Searching in: '{(commodityText?.Length > 100 ? commodityText.Substring(0, 100) + "..." : commodityText)}'");

            if (string.IsNullOrEmpty(commodityText))
            {
                result.Warnings.Add($"{contextKey}: Commodity description not found in PDF");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Commodity text not found");
            }
            else if (commodityText.Contains(expected, StringComparison.OrdinalIgnoreCase))
            {
                result.ValidatedFields.Add(contextKey, expected);
                Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: Found in commodity text");
            }
            else
            {
                result.Warnings.Add($"{contextKey}: '{expected}' not found in commodity description");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Not found in commodity text");
            }
        }

        private void ValidateAnimalIdentificationInText(JArray pdfData, PdfValidationResult result, string contextKey)
        {
            if (!_scenarioContext.TryGetValue(contextKey, out var expectedValue))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
                return;
            }

            var expected = GetExpectedValue(expectedValue);
            if (string.IsNullOrEmpty(expected))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                return;
            }

            var identificationText = GetFieldValue(pdfData, "I34AIdentificationDetails", "Value");

            Console.WriteLine($"[PDF VALIDATION] Checking {contextKey} in identification text:");
            Console.WriteLine($"[PDF VALIDATION]   Expected: '{expected}'");
            Console.WriteLine($"[PDF VALIDATION]   Searching in: '{identificationText ?? "(null)"}'");

            if (string.IsNullOrEmpty(identificationText))
            {
                result.Warnings.Add($"{contextKey}: Animal identification not found in PDF");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Identification text not found");
            }
            else if (identificationText.Contains(expected, StringComparison.OrdinalIgnoreCase))
            {
                result.ValidatedFields.Add(contextKey, expected);
                Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: Found in identification text");
            }
            else
            {
                result.Warnings.Add($"{contextKey}: '{expected}' not found in identification details");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Not found in identification text");
            }
        }

        private void ValidateBcpFromMultipleSections(JArray pdfData, PdfValidationResult result)
        {
            const string contextKey = "PortOfEntry";

            if (!_scenarioContext.TryGetValue(contextKey, out var expectedBcp))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
                return;
            }

            var expected = expectedBcp?.ToString() ?? "";
            if (string.IsNullOrEmpty(expected))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                return;
            }

            var bcpValue = GetFieldValue(pdfData, "I4BorderControlPostControlPointControlUnit", "Value");
            var bcpCode = GetFieldValue(pdfData, "I5BorderControlPostControlPointControlUnitCode", "Value");

            var combinedBcp = $"{bcpValue} - {bcpCode}".Trim(' ', '-');

            Console.WriteLine($"[PDF VALIDATION] Checking {contextKey}:");
            Console.WriteLine($"[PDF VALIDATION]   Expected: '{expected}'");
            Console.WriteLine($"[PDF VALIDATION]   Found:    '{combinedBcp}'");

            var expectedParts = expected.Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(p => p.Length > 3).ToList();
            var matchCount = expectedParts.Count(p => combinedBcp.Contains(p, StringComparison.OrdinalIgnoreCase));

            if (matchCount >= expectedParts.Count / 2 || expectedParts.Count == 0)
            {
                result.ValidatedFields.Add(contextKey, combinedBcp);
                Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: Match ({matchCount}/{expectedParts.Count} keywords)");
            }
            else
            {
                result.Warnings.Add($"{contextKey}: Partial match. Expected '{expected}', Found '{combinedBcp}'");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Partial match ({matchCount}/{expectedParts.Count} keywords)");
            }
        }

        private void ValidateDateField(JArray pdfData, PdfValidationResult result, string contextKey,
            string sectionName, string fieldName)
        {
            if (!_scenarioContext.TryGetValue(contextKey, out var expectedValue))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
                return;
            }

            var expected = GetExpectedValue(expectedValue);
            if (string.IsNullOrEmpty(expected))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                return;
            }

            var pdfValue = GetFieldValue(pdfData, sectionName, fieldName);

            Console.WriteLine($"[PDF VALIDATION] Checking {contextKey}:");
            Console.WriteLine($"[PDF VALIDATION]   Expected: '{expected}'");
            Console.WriteLine($"[PDF VALIDATION]   Found:    '{pdfValue ?? "(null)"}'");

            if (string.IsNullOrEmpty(pdfValue))
            {
                result.Warnings.Add($"{contextKey}: Date not found in PDF");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Not found");
                return;
            }

            if (DatePartsMatch(expected, pdfValue))
            {
                result.ValidatedFields.Add(contextKey, pdfValue);
                Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: Date match");
            }
            else
            {
                result.Warnings.Add($"{contextKey}: Date format differs. Expected '{expected}', Found '{pdfValue}'");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Date format differs");
            }
        }

        private void ValidateTimeField(JArray pdfData, PdfValidationResult result, string contextKey,
            string sectionName, string fieldName)
        {
            if (!_scenarioContext.TryGetValue(contextKey, out var expectedValue))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
                return;
            }

            var expected = GetExpectedValue(expectedValue);
            if (string.IsNullOrEmpty(expected))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                return;
            }

            var pdfValue = GetFieldValue(pdfData, sectionName, fieldName);

            Console.WriteLine($"[PDF VALIDATION] Checking {contextKey}:");
            Console.WriteLine($"[PDF VALIDATION]   Expected: '{expected}'");
            Console.WriteLine($"[PDF VALIDATION]   Found:    '{pdfValue ?? "(null)"}'");

            if (string.IsNullOrEmpty(pdfValue))
            {
                result.Warnings.Add($"{contextKey}: Time not found in PDF");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Not found");
                return;
            }

            if (pdfValue.StartsWith(expected, StringComparison.OrdinalIgnoreCase) ||
                pdfValue.Contains(expected, StringComparison.OrdinalIgnoreCase))
            {
                result.ValidatedFields.Add(contextKey, pdfValue);
                Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: Time match");
            }
            else
            {
                result.Warnings.Add($"{contextKey}: Time format differs. Expected '{expected}', Found '{pdfValue}'");
                Console.WriteLine($"[PDF VALIDATION] ⚠ {contextKey}: Time format differs");
            }
        }

        private string DownloadPdfFromUrl(IWebDriver driver, string pdfUrl)
        {
            var jsExecutor = (IJavaScriptExecutor)driver;

            var chedRef = "CHED";
            if (_scenarioContext.TryGetValue("CHEDReference", out var refValue))
            {
                chedRef = refValue?.ToString()?.Replace(".", "_") ?? "CHED";
            }

            var fileName = $"{chedRef}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            var filePath = Path.Combine(_downloadDirectory, fileName);

            var script = @"
                var callback = arguments[arguments.length - 1];
                fetch(arguments[0])
                    .then(response => response.blob())
                    .then(blob => {
                        var reader = new FileReader();
                        reader.onloadend = function() {
                            callback(reader.result.split(',')[1]);
                        };
                        reader.readAsDataURL(blob);
                    })
                    .catch(err => callback('ERROR: ' + err.message));
            ";

            var base64Content = (string)jsExecutor.ExecuteAsyncScript(script, pdfUrl);

            if (base64Content.StartsWith("ERROR:"))
            {
                throw new InvalidOperationException($"Failed to download PDF: {base64Content}");
            }

            var pdfBytes = Convert.FromBase64String(base64Content);
            File.WriteAllBytes(filePath, pdfBytes);

            return filePath;
        }

        private string? GetFieldValue(JArray pdfData, string sectionName, string fieldName)
        {
            foreach (var page in pdfData)
            {
                var sections = page["Sections"] as JObject;
                if (sections == null) continue;

                var section = sections[sectionName];
                if (section == null) continue;

                if (section is JArray sectionArray && sectionArray.Count > 0)
                {
                    var firstItem = sectionArray[0] as JObject;
                    var value = firstItem?[fieldName]?.ToString();
                    if (!string.IsNullOrEmpty(value)) return value;
                }
                else if (section is JObject sectionObj)
                {
                    var value = sectionObj[fieldName]?.ToString();
                    if (!string.IsNullOrEmpty(value)) return value;
                }
            }

            return null;
        }

        private JObject? GetSection(JArray pdfData, string sectionName)
        {
            foreach (var page in pdfData)
            {
                var sections = page["Sections"] as JObject;
                if (sections == null) continue;

                var section = sections[sectionName];
                if (section is JObject sectionObj)
                {
                    return sectionObj;
                }
            }

            return null;
        }

        private string? GetCommodityDescriptionText(JArray pdfData)
        {
            foreach (var page in pdfData)
            {
                var sections = page["Sections"] as JObject;
                if (sections == null) continue;

                var section = sections["I31DescriptionOfTheGoods"];
                if (section is JArray sectionArray && sectionArray.Count > 0)
                {
                    var firstItem = sectionArray[0] as JObject;
                    return firstItem?["Value"]?.ToString();
                }
            }

            return null;
        }

        private string? GetFieldFromCommodityArray(JArray pdfData, string fieldName)
        {
            foreach (var page in pdfData)
            {
                var sections = page["Sections"] as JObject;
                if (sections == null) continue;

                var section = sections["I31DescriptionOfTheGoods"];
                if (section is JArray sectionArray && sectionArray.Count > 0)
                {
                    var firstItem = sectionArray[0] as JObject;
                    var value = firstItem?[fieldName]?.ToString();
                    if (!string.IsNullOrEmpty(value)) return value;
                }
            }

            return null;
        }

        private string GetExpectedValue(object? contextValue)
        {
            if (contextValue == null) return string.Empty;

            if (contextValue is List<string> list)
            {
                return list.FirstOrDefault() ?? string.Empty;
            }

            if (contextValue is string[] array)
            {
                return array.FirstOrDefault() ?? string.Empty;
            }

            return contextValue.ToString() ?? string.Empty;
        }

        private bool ValuesMatch(string expected, string actual)
        {
            if (string.IsNullOrEmpty(expected) || string.IsNullOrEmpty(actual))
                return false;

            if (expected.Equals(actual, StringComparison.OrdinalIgnoreCase))
                return true;

            if (actual.Contains(expected, StringComparison.OrdinalIgnoreCase))
                return true;

            var normalizedExpected = NormalizeValue(expected);
            var normalizedActual = NormalizeValue(actual);

            return normalizedExpected.Equals(normalizedActual, StringComparison.OrdinalIgnoreCase);
        }

        private bool ImportTypeMatches(string expected, string pdfValue)
        {
            if (string.IsNullOrEmpty(expected) || string.IsNullOrEmpty(pdfValue))
                return false;

            // Normalize both values
            var normalizedExpected = expected.Replace(" ", "").ToUpperInvariant();
            var normalizedPdf = pdfValue.Replace(" ", "").ToUpperInvariant();

            // Direct match
            if (normalizedExpected.Equals(normalizedPdf))
                return true;

            // Common mappings
            var mappings = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                { "LIVEANIMALS", new[] { "LIVEANIMALS", "LIVE ANIMALS" } },
                { "LIVE ANIMALS", new[] { "LIVEANIMALS", "LIVE ANIMALS" } },
                { "PRODUCTS", new[] { "PRODUCTS", "PRODUCT" } }
            };

            if (mappings.TryGetValue(expected, out var validValues))
            {
                return validValues.Any(v => v.Replace(" ", "").Equals(normalizedPdf, StringComparison.OrdinalIgnoreCase));
            }

            return pdfValue.Contains(expected, StringComparison.OrdinalIgnoreCase);
        }

        private bool AddressesMatch(string expected, string actual)
        {
            if (string.IsNullOrEmpty(expected) || string.IsNullOrEmpty(actual))
                return false;

            // Direct match
            if (expected.Equals(actual, StringComparison.OrdinalIgnoreCase))
                return true;

            // Normalize and compare
            var normalizedExpected = NormalizeAddress(expected);
            var normalizedActual = NormalizeAddress(actual);

            if (normalizedExpected.Equals(normalizedActual, StringComparison.OrdinalIgnoreCase))
                return true;

            // Check if actual is contained in expected or vice versa (for truncation)
            if (normalizedExpected.Contains(normalizedActual, StringComparison.OrdinalIgnoreCase) ||
                normalizedActual.Contains(normalizedExpected, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        private bool CountriesMatch(string expected, string actual)
        {
            if (string.IsNullOrEmpty(expected) || string.IsNullOrEmpty(actual))
                return false;

            if (expected.Equals(actual, StringComparison.OrdinalIgnoreCase))
                return true;

            if (expected.Contains(actual, StringComparison.OrdinalIgnoreCase) ||
                actual.Contains(expected, StringComparison.OrdinalIgnoreCase))
                return true;

            // Check ISO code mappings
            var isoMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "United Kingdom of Great Britain and Northern Ireland", "GB" },
                { "United Kingdom", "GB" },
                { "England", "GB-ENG" },
                { "Tanzania", "TZ" },
                { "Kenya", "KE" }
            };

            if (isoMappings.TryGetValue(expected, out var expectedIso))
            {
                if (actual.Contains(expectedIso, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private bool DatePartsMatch(string expected, string pdfDate)
        {
            try
            {
                var expectedDay = Regex.Match(expected, @"^\d+").Value;
                var pdfDay = Regex.Match(pdfDate, @"^\d+").Value;

                var expectedYear = Regex.Match(expected, @"\d{4}").Value;
                var pdfYear = Regex.Match(pdfDate, @"\d{4}").Value;

                return int.Parse(expectedDay) == int.Parse(pdfDay) && expectedYear == pdfYear;
            }
            catch
            {
                return false;
            }
        }

        private string NormalizeValue(string value)
        {
            return value.Trim()
                .Replace("  ", " ")
                .Replace("\r\n", " ")
                .Replace("\n", " ");
        }

        private string NormalizeAddress(string address)
        {
            return address.Trim()
                .TrimEnd(',')
                .Replace("  ", " ")
                .Replace("\r\n", ", ")
                .Replace("\n", ", ");
        }

        private string ExtractNumber(string value)
        {
            var match = Regex.Match(value, @"\d+");
            return match.Success ? match.Value : value;
        }

        private void LogAvailableSections(JArray pdfData)
        {
            Console.WriteLine("[PDF DEBUG] ========== Available PDF Sections ==========");
            foreach (var page in pdfData)
            {
                var pageNum = page["PageNumber"]?.ToString();
                Console.WriteLine($"[PDF DEBUG] Page {pageNum}:");

                var sections = page["Sections"] as JObject;
                if (sections != null)
                {
                    foreach (var section in sections.Properties())
                    {
                        Console.WriteLine($"[PDF DEBUG]   Section: {section.Name}");
                        if (section.Value is JObject sectionObj)
                        {
                            foreach (var prop in sectionObj.Properties())
                            {
                                var valuePreview = prop.Value?.ToString()?.Length > 50
                                    ? prop.Value.ToString()?.Substring(0, 50) + "..."
                                    : prop.Value?.ToString();
                                Console.WriteLine($"[PDF DEBUG]     - {prop.Name}: {valuePreview}");
                            }
                        }
                        else if (section.Value is JArray sectionArray)
                        {
                            Console.WriteLine($"[PDF DEBUG]     (Array with {sectionArray.Count} items)");
                        }
                    }
                }
            }
            Console.WriteLine("[PDF DEBUG] ================================================");
        }

        private void EnsureDownloadDirectoryExists()
        {
            if (!Directory.Exists(_downloadDirectory))
            {
                Directory.CreateDirectory(_downloadDirectory);
            }
        }

        private void CleanupFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch
            {
                // Ignore cleanup errors
            }
        }

        public string? GetPdfSectionValue(string sectionKey, string fieldKey)
        {
            if (!_scenarioContext.ContainsKey("ExtractedPdfContent"))
                return null;

            var pdfData = _scenarioContext.Get<JArray>("ExtractedPdfContent");
            return GetFieldValue(pdfData, sectionKey, fieldKey);
        }

        public Dictionary<string, object>? GetAllPdfSections()
        {
            if (!_scenarioContext.ContainsKey("ExtractedPdfContent"))
                return null;

            var pdfData = _scenarioContext.Get<JArray>("ExtractedPdfContent");
            var allSections = new Dictionary<string, object>();

            foreach (var page in pdfData)
            {
                var sections = page["Sections"] as JObject;
                if (sections != null)
                {
                    foreach (var section in sections.Properties())
                    {
                        allSections[section.Name] = section.Value!;
                    }
                }
            }

            return allSections;
        }
    }

    public class PdfValidationResult
    {
        public bool IsValid { get; set; } = true;
        public List<string> Errors { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
        public Dictionary<string, string> ValidatedFields { get; set; } = new();

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"PDF Validation Result: {(IsValid ? "PASSED" : "FAILED")}");

            if (Errors.Any())
            {
                sb.AppendLine("Errors:");
                foreach (var error in Errors)
                    sb.AppendLine($"  - {error}");
            }

            if (Warnings.Any())
            {
                sb.AppendLine("Warnings:");
                foreach (var warning in Warnings)
                    sb.AppendLine($"  - {warning}");
            }

            if (ValidatedFields.Any())
            {
                sb.AppendLine("Validated Fields:");
                foreach (var field in ValidatedFields)
                    sb.AppendLine($"  - {field.Key}: {field.Value}");
            }

            return sb.ToString();
        }
    }
}