namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IHMIImportCountryRulesPage
    {
        bool IsPageLoaded();
        void SelectCountry(string country);
        void SetInspectionRate(int rate);
        void EnsureApprovedInspectionServiceIsChecked();
        void EnsureApprovedInspectionServiceIsNotChecked();
        void EnsurePermanentIsChecked();
        void ClickConfirmAndSendButton();
    }
}