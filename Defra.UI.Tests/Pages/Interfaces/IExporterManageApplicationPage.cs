namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterManageApplicationPage
    {
        bool IsPageLoaded();
        void ClickSamplingReportLink();
        bool IsSamplingReportDownloaded();
        string GetRemainingApprovedStockQuantity();
        bool IsMessageDisplayed(string message);
        void ClickLink(string linkText);
        void ClickCopyAsNewApplicationLink();
    }
}
