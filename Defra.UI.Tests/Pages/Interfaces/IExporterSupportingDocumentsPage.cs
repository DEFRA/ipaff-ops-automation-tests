namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterSupportingDocumentsPage
    {
        bool IsPageLoaded();
        void UploadDocument(string fileName, string description);
        void ClickSaveAndContinueButton();
    }
}
