using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAddressesPage
    { 
        bool IsPageLoaded();
        void ClickAddConsignor();
        void ClickAddConsignee();
        bool VerifySelectedConsignor();
        bool VerifySelectedConsignee();
        void ClickImporterSameAsConsignee();
        void ClickAddDestination();
        bool VerifySelectedDestination();
        string GetSelectedImporter();
    }
}
