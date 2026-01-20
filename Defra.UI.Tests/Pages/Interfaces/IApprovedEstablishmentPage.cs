namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IApprovedEstablishmentPage
    {
        bool IsPageLoaded();
        void ClickSearchForApproved();
        bool VerifySelectedCountryOfOrigin(string country);
        void ClickSelectEstablishment();
        bool VerifySelectedEstablismentName(string establishmentListFirstName);
        string GetSelectedEstablishmentName();
        string GetSelectedEstablishmentCountry();
        string GetSelectedEstablishmentType();
        string GetSelectedEstablishmentApprovalNumber();
        string GetEstablishmentListFirstName();
        void ClickRemoveEstablishment();
        bool VerifySelectedCountryOnlyDisplayed(string country);
        bool VerifySelectedTypeOnlyDisplayed(string type);
        bool VerifySelectedStatusOnlyDisplayed(string status);
        void SelectTypeFromDropdown(string type);
        void SelectStatusFromDropdown(string status);
        void ClickSearchButton();
    }
}
