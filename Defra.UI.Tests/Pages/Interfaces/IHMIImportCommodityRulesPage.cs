namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IHMIImportCommodityRulesPage
    {
        bool IsPageLoaded();
        void SelectCountry(string country);
        void SearchCommodity(string commodityCode, string commodityName);
        void SetInspectionRate(int rate);
        void EnsurePermanentIsChecked();
        void ClickConfirmAndSendButton();
    }
}