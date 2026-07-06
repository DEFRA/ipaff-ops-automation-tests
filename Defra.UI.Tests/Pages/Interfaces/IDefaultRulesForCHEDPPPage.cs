namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IDefaultRulesForCHEDPPPage
    {
        bool IsPageLoaded();
        int GetHmiImportRate();
        void EnsureHmiImportRateIsZero();
        void SetHmiImportRate(int rate);
        void ClickConfirmAndSendButton();
    }
}