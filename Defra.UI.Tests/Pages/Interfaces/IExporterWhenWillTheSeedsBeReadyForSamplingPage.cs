namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterWhenWillTheSeedsBeReadyForSamplingPage
    {
        bool IsPageLoaded();
        void EnterReadyDate(DateTime date);
        void ClickSaveAndContinueButton();
    }
}
