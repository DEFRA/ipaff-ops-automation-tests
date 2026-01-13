namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAddressBookPage
    {
        bool IsPageLoaded();
        void SelectType(string type);
        void ClickSearchButton();
        bool ValidateTypeInSearchResults(string type);
    }
}
