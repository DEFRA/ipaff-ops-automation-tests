namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICHEDPImportCommodityRulesPage
    {
        bool IsPageLoaded();
        void SearchCommodity(string commodityCode, string commodityName);
        void SelectCountry(string country);
        void SetInspectionRate(int rate);
        void EnsureCheckboxIsChecked(string checkboxLabel);
        void ClickConfirmAndSendButton();
    }
}