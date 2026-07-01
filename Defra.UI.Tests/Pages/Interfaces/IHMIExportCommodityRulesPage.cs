namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IHMIExportCommodityRulesPage
    {
        bool IsPageLoaded();
        void SelectCommodity(string commodity);
        void SelectVariety(string variety);
        void SetInspectionRate(int rate);
        void EnsurePermanentIsChecked();
        void ClickConfirmAndSendButton();
    }
}