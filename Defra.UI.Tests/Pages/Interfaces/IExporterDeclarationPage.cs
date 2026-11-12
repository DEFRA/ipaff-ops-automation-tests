namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterDeclarationPage
    {
        bool IsPageLoaded();
        bool IsDeclarationPageLoaded();
        void TickDeclarationCheckbox();
        void ClickSubmitApplicationButton();
    }
}