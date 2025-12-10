namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IReviewYourNotificationPage
    {
        bool IsPageLoaded();

        // About the consignment
        string? GetImportType();
        string? GetPartOfImportType();
        string? GetCountryOfOrigin();
        string? GetCountryFromWhereConsigned();
        string? GetMainReasonForImport();
        string? GetPurpose();
        string? GetConsignmentReferenceNumber();

        // Commodity details
        string? GetCommodityCode();
        string? GetSpecies();
        string? GetNumberOfAnimals();
        string? GetNumberOfPackages();
        string? GetCommodityCodeList(int index);
        string? GetNetWeightList(int index);
        string? GetNumPackagesList(int index);
        string? GetTypeOfPackagesList(int index);
        string? GetTotalNetWeight();
        string? GetTotalPackages();
        string? GetTotalGrossWeight();
        // Animal details
        string? GetCertificationOption();

        // Additional details
        string? GetCommodityIntendedFor();
        string? GetTemperature();

        // Documents
        string? GetHealthCertificateReference();
        string? GetHealthCertificateDateOfIssue();
        string? GetAdditionalDocumentType();
        string? GetAdditionalDocumentReference();
        string? GetAdditionalDocumentDateOfIssue();
        string? GetAdditionalDocName();
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
        string? GetCTCUsage();
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