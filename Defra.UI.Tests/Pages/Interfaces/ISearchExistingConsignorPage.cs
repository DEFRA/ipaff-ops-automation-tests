namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISearchExistingConsignorPage
    {
        bool IsPageLoaded();
        void ClickSelect(string consignorName);
        string GetSelectedConsignor(string consignorName);
        string GetSelectedConsignorName(string consignorName);
        string GetSelectedConsignorAddress(string consignorName);
        string GetSelectedConsignorCountry(string consignorName);
    }
}