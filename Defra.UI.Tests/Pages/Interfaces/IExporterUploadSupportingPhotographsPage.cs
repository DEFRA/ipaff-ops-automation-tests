namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterUploadSupportingPhotographsPage
    {
        bool IsPageLoaded();
        string UploadPhotograph(string sourceFileName, string description);
        bool IsPhotographListedWithActions(string fileName, string description);
        void ClickSaveAndContinueButton();
    }
}
