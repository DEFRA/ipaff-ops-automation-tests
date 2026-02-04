namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISearchExistingDeliveryAddressPage
    {
        bool IsPageLoaded();
        void ClickSelect(string deliveryAddress);
        string GetSelectedDeliveryAddressName(string deliveryAddress);
        string GetSelectedDeliveryAddress(string deliveryAddress);
        string GetSelectedDeliveryCountry(string deliveryAddress);
        string GetSelectedDeliveryAddressDetails(string deliveryAddress);
    }
}