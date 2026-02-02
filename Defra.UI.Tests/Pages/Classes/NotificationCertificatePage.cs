using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools.PDFProcessor;
using OpenQA.Selenium;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    /// <summary>
    /// Page class for the Notification Certificate (PDF) page
    /// Handles PDF download, extraction, and validation operations
    /// </summary>
    public class NotificationCertificatePage : INotificationCertificatePage
    {
        private readonly IObjectContainer _objectContainer;
        private PdfValidationService? _pdfValidationService;

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private ScenarioContext _scenarioContext => _objectContainer.Resolve<ScenarioContext>();

        /// <summary>
        /// Gets or initializes the PDF validation service
        /// </summary>
        private PdfValidationService PdfValidationService
        {
            get
            {
                _pdfValidationService ??= new PdfValidationService(_scenarioContext);
                return _pdfValidationService;
            }
        }

        public NotificationCertificatePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        /// <inheritdoc />
        public bool IsPageLoaded()
        {
            try
            {
                // Wait for the page to load
                Thread.Sleep(2000);
                return _driver.Url.Contains("/certificate/pdf");
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc />
        public bool VerifyChedReferenceInUrl(string chedReference)
        {
            return _driver.Url.Contains($"/{chedReference}/certificate/pdf");
        }

        /// <inheritdoc />
        public bool ExtractPdfContent()
        {
            try
            {
                // Ensure we're on the PDF tab
                if (!_driver.Url.Contains("/certificate/pdf"))
                {
                    Console.WriteLine("[PDF EXTRACTION] Error: Current URL is not a PDF certificate URL");
                    return false;
                }

                // Download and extract PDF content
                PdfValidationService.DownloadAndExtractPdfContent(_driver);
                Console.WriteLine("[PDF EXTRACTION] Successfully extracted PDF content");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PDF EXTRACTION] Failed to extract PDF content: {ex.Message}");
                return false;
            }
        }

        /// <inheritdoc />
        public PdfValidationResult ValidatePdfContent()
        {
            return PdfValidationService.ValidatePdfAgainstNotificationData();
        }

        /// <inheritdoc />
        public string? GetPdfFieldValue(string sectionKey, string fieldKey)
        {
            return PdfValidationService.GetPdfSectionValue(sectionKey, fieldKey);
        }

        /// <inheritdoc />
        public bool VerifyPdfDataMatchesNotification()
        {
            try
            {
                // First extract the PDF content if not already done
                if (!_scenarioContext.ContainsKey("ExtractedPdfContent"))
                {
                    if (!ExtractPdfContent())
                    {
                        Console.WriteLine("[PDF VALIDATION] Failed to extract PDF content for validation");
                        return false;
                    }
                }

                // Validate the PDF content
                var validationResult = ValidatePdfContent();

                // Log the validation result
                Console.WriteLine(validationResult.ToString());

                // Store the validation result in ScenarioContext for later reference
                _scenarioContext["PdfValidationResult"] = validationResult;

                return validationResult.IsValid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PDF VALIDATION] Validation failed with exception: {ex.Message}");
                return false;
            }
        }

        /// <inheritdoc />
        public string? GetChedReference()
        {
            return GetPdfFieldValue("I1ChedReference", "id") ??
                   GetPdfFieldValue("I1ChedReference", "value");
        }

        /// <inheritdoc />
        public string? GetCountryOfOrigin()
        {
            return GetPdfFieldValue("I11CountryOfOrigin", "value") ??
                   GetPdfFieldValue("I11CountryOfOrigin", "IsoCode");
        }

        /// <inheritdoc />
        public string? GetCommodityCode()
        {
            return GetPdfFieldValue("I31DescriptionOfTheGoods", "Commodity");
        }

        /// <inheritdoc />
        public string? GetBcpOrPortOfEntry()
        {
            return GetPdfFieldValue("I4BorderControlPostControlPointControlUnit", "value");
        }

        /// <inheritdoc />
        public string? GetMeansOfTransport()
        {
            return GetPdfFieldValue("I13MeansOfTransport", "mode") ??
                   GetPdfFieldValue("I13MeansOfTransport", "Mode");
        }

        /// <inheritdoc />
        public string? GetTransportIdentification()
        {
            return GetPdfFieldValue("I13MeansOfTransport", "identification") ??
                   GetPdfFieldValue("I13MeansOfTransport", "Identification");
        }

        /// <inheritdoc />
        public string? GetConsignorName()
        {
            return GetPdfFieldValue("I7ConsignorExporter", "Name") ??
                   GetPdfFieldValue("I7ConsignorExporter", "value");
        }

        /// <inheritdoc />
        public string? GetConsigneeName()
        {
            return GetPdfFieldValue("I5ConsigneeImporter", "Name") ??
                   GetPdfFieldValue("I5ConsigneeImporter", "value");
        }

        /// <inheritdoc />
        public Dictionary<string, object>? GetAllPdfSections()
        {
            return PdfValidationService.GetAllPdfSections();
        }
    }
}