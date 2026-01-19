namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAddressBookPage
    {
        bool IsPageLoaded();
        void SelectType(string type);
        void ClickSearchButton();
        bool ValidateTypeInSearchResults(string type);
        void ClickDashboardLink();
        void ClickAddAnAddress();
        bool IsOperatorDisplayedInAddressBook(string operatorName, string operatorType, string operatorAddress, string operatorCountry);
        string GetOperatorName(string operatorName);
        string GetOperatorType(string operatorName);
        string GetOperatorAddress(string operatorName);
        string GetOperatorCountry(string operatorName);
        void ClickViewOperator(string operatorName);
    }
}
