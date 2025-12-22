using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAddressesPage
    { 
        bool IsPageLoaded();
        bool IsConsignorExporterConsigneeImporterandPlaceOfDestinationPageLoaded();
        void ClickAddConsignor();
        bool VerifySelectedConsignor();
        void ClickAddConsignee();
        bool VerifySelectedConsignee();
        void ClickImporterSameAsConsignee();
        void ClickAddDestination();
        bool VerifySelectedDestination();
        string GetSelectedImporter();
        bool VerifySelectedConsignor(string name, string address, string country);
        bool VerifySelectedConsignee(string name, string address, string country);
        bool VerifySelectedDestination(string name, string address, string country);
        void ClickPlaceOfDestinationSameAsConsignee();
        string GetSelectedPlaceOfDestination();
        void ClickAddImporter();
        bool VerifySelectedImporter(string importerName, string importerAddress, string importerCountry);
        void ClickChangeInAddressPage(string link);
    }
}
