using Defra.UI.Tests.Tools.PDFProcessor;

namespace Defra.UI.Tests.Pages.Interfaces
{
    /// <summary>
    /// Interface for the Notification Certificate (PDF) page
    /// Handles PDF download, extraction, and validation operations
    /// </summary>
    public interface INotificationCertificatePage
    {
        /// <summary>
        /// Checks if the PDF certificate page is loaded (URL contains /certificate/pdf)
        /// </summary>
        bool IsPageLoaded();

        /// <summary>
        /// Verifies that the current URL contains the expected CHED reference
        /// </summary>
        /// <param name="chedReference">The CHED reference to verify</param>
        bool VerifyChedReferenceInUrl(string chedReference);

        /// <summary>
        /// Downloads the PDF from the current browser tab and extracts its content
        /// </summary>
        /// <returns>True if PDF was successfully downloaded and extracted</returns>
        bool ExtractPdfContent();

        /// <summary>
        /// Validates the extracted PDF content against the notification data stored in ScenarioContext
        /// </summary>
        /// <returns>PdfValidationResult containing validation status and details</returns>
        PdfValidationResult ValidatePdfContent();

        /// <summary>
        /// Gets a specific field value from the extracted PDF content
        /// </summary>
        /// <param name="sectionKey">The section key (e.g., "I11CountryOfOrigin")</param>
        /// <param name="fieldKey">The field key within the section (e.g., "value", "IsoCode")</param>
        /// <returns>The field value or null if not found</returns>
        string? GetPdfFieldValue(string sectionKey, string fieldKey);

        /// <summary>
        /// Validates that the PDF certificate data matches the notification data entered during test
        /// </summary>
        /// <returns>True if all critical fields match, false otherwise</returns>
        bool VerifyPdfDataMatchesNotification();

        /// <summary>
        /// Gets the CHED Reference from the PDF
        /// </summary>
        string? GetChedReference();

        /// <summary>
        /// Gets the Country of Origin from the PDF
        /// </summary>
        string? GetCountryOfOrigin();

        /// <summary>
        /// Gets the Commodity Code from the PDF
        /// </summary>
        string? GetCommodityCode();

        /// <summary>
        /// Gets the BCP/Port of Entry from the PDF
        /// </summary>
        string? GetBcpOrPortOfEntry();

        /// <summary>
        /// Gets the Means of Transport from the PDF
        /// </summary>
        string? GetMeansOfTransport();

        /// <summary>
        /// Gets the Transport Identification from the PDF
        /// </summary>
        string? GetTransportIdentification();

        /// <summary>
        /// Gets the Consignor/Exporter name from the PDF
        /// </summary>
        string? GetConsignorName();

        /// <summary>
        /// Gets the Consignee/Importer name from the PDF
        /// </summary>
        string? GetConsigneeName();

        /// <summary>
        /// Gets all extracted PDF sections as a dictionary
        /// </summary>
        Dictionary<string, object>? GetAllPdfSections();
    }
}