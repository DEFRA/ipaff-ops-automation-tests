namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterCreateAReferencePage
    {
        bool IsPageLoaded();
        void EnterReference(string reference);
        void ClickSaveAndContinueButton();
    }
}