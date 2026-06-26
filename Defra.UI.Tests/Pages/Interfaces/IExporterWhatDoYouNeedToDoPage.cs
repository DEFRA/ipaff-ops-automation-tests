namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterWhatDoYouNeedToDoPage
    {
        bool IsPageLoaded();
        void SelectWhatDoYouNeedToDoOption(string option);
        void ClickContinueButton();
    }
}