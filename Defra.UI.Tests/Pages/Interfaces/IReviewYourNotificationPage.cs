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
        string? GetExitDate();
        string? GetExitBCP();
        string? GetDestinationCountry();

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
        string? GetHorseName(int index = 0);
        string? GetMicrochipNumber(int index = 0);
        string? GetPassportNumber(int index = 0);
        string? GetEarTag(int index = 0);

        // Additional details
        string? GetCommodityIntendedFor();
        string? GetTemperature();
        string? GetUnweanedAnimalsOption();

        // Documents
        string? GetHealthCertificateReference();
        string? GetHealthCertificateDateOfIssue();
        string? GetAdditionalDocumentType();
        string? GetAdditionalDocumentReference();
        string? GetAdditionalDocumentDateOfIssue();
        string? GetHealthCertificateFileName();
        string? GetAdditionalDocumentFileName();

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
        string? GetContainerNumber(int index = 0);
        string? GetSealNumber(int index = 0);
        string? GetOfficialSeal(int index = 0);
        string? GetMeansOfTransportAfterBCP();
        string? GetTransportIdentificationAfterBCP();
        string? GetTransportDocumentReferenceAfterBCP();
        string? GetDepartureDateFromBCP();
        string? GetDepartureTimeFromBCP();

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
        bool IsError(string errorMessage);
        void ClickChangeLink(string heading);
        (bool hasError, string errorMessages) VerifyErrorMsgDisplayed(string errorMessage);
    }
}