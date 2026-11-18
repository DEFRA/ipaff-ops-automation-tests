namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterUploadAFilePage
    {
        bool IsPageLoaded();
        void DownloadCsvTemplate();
        bool IsCsvTemplateDownloaded();
        List<string> GetUnitsOfMeasurementFromTemplate();
        void ClickBackLink();
    }
}
