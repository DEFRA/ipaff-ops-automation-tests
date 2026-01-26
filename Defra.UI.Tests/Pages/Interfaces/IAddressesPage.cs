namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAddressesPage
    { 
        bool IsPageLoaded();
        bool IsConsignorExporterConsigneeImporterandPlaceOfDestinationPageLoaded();
        void ClickAddConsignor();
        bool VerifySelectedConsignor(string consignor);
        void ClickAddConsignee();
        bool VerifySelectedConsignee(string consigneeName);
        void ClickImporterSameAsConsignee();
        void ClickAddDestination();
        bool VerifySelectedDestination(string destination);
        string GetSelectedImporter();
        bool VerifySelectedConsignor(string name, string address, string country);
        bool VerifySelectedConsignee(string name, string address, string country);
        bool VerifySelectedDestination(string name, string address, string country);
        void ClickPlaceOfDestinationSameAsConsignee();
        string GetSelectedPlaceOfDestination();
        void ClickAddImporter();
        bool VerifySelectedImporter(string importerName, string importerAddress, string importerCountry);
        void ClickChangeInAddressPage(string link);
        int GetConsignorRowsCount();
        int GetConsigneeRowsCount();
        int GetImporterRowsCount();
        int GetDestinationRowsCount();
        string GetSelectedConsignor();
    }
}