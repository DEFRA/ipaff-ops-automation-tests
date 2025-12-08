namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IReviewYourNotificationPage
    {
        bool IsPageLoaded();

        // About the consignment
        string? GetImportType();
        string? GetCountryOfOrigin();
        string? GetMainReasonForImport();
        string? GetPurpose();
        string? GetConsignmentReferenceNumber();

        // Commodity details
        string? GetCommodityCode();
        string? GetSpecies();
        string? GetNumberOfAnimals();
        string? GetNumberOfPackages();

        // Animal details
        string? GetCertificationOption();

        // Documents
        string? GetHealthCertificateReference();
        string? GetHealthCertificateDateOfIssue();
        string? GetAdditionalDocumentType();
        string? GetAdditionalDocumentReference();
        string? GetAdditionalDocumentDateOfIssue();

        // Addresses
        string? GetConsignorName();
        string? GetConsignorAddress();
        string? GetConsigneeName();
        string? GetConsigneeAddress();
        string? GetImporterName();
        string? GetImporterAddress();
        string? GetDestinationName();
        string? GetDestinationAddress();

        // Transport details
        string? GetPortOfEntry();
        string? GetMeansOfTransport();
        string? GetTransportId();
        string? GetContainerUsage();
        string? GetTransportDocumentReference();
        string? GetEstimatedArrivalDate();
        string? GetEstimatedArrivalTime();
        string? GetEstimatedJourneyTime();
        string? GetGVMSUsage();

        // Transporter details
        string? GetTransporterName();
        string? GetTransporterAddress();
        string? GetTransporterCountry();
        string? GetTransporterApprovalNumber();
        string? GetTransporterType();

        // Route and contacts
        string? GetRouteCountries();
        string? GetNotifyTransportContacts();
        string? GetConsignmentContactAddress();
    }
}