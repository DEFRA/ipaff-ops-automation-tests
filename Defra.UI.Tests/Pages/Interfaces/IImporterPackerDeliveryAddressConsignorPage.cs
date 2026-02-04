using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IImporterPackerDeliveryAddressConsignorPage
    {
        void AddADeliveryAddress();
        bool IsPageLoaded();
        bool VerifyImporterName(string importerName);
        bool VerifySelectedDeliveryAddress(string deliveryAddressName, string deliveryAddress, string deliveryCountry);
    }
}
