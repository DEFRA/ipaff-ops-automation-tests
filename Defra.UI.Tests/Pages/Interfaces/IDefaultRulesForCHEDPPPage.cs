namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IDefaultRulesForCHEDPPPage
    {
        bool IsPageLoaded();
        int GetHmiImportRate();
        void EnsureHmiImportRateIs(int targetRate);
        void SetHmiImportRate(int rate);
        int GetGmsExportRate();
        void EnsureGmsExportRateIs(int targetRate);
        void SetGmsExportRate(int rate);
        void ClickConfirmAndSendButton();
    }
}