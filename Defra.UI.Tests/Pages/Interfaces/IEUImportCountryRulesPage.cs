namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IEUImportCountryRulesPage
    {
        bool IsPageLoaded();
        void SelectCountry(string country);
        void SetInspectionRate(int rate);
        void EnsurePermanentCheckboxIsChecked();
        void ClickConfirmAndSendButton();
    }
}