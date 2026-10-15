namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterManageApplicationPage
    {
        bool IsPageLoaded();
        void ClickSamplingReportLink();
        bool IsSamplingReportDownloaded();
        string GetRemainingApprovedStockQuantity();
    }
}
