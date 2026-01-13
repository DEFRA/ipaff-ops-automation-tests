namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAddressBookPage
    {
        bool IsPageLoaded();
        void ClickAddAnAddress();
        bool IsAddressDisplayedInAddressBook(string addressName);
        void ClickDashboard();
        void ClickReturnToAddressBook();
    }
}