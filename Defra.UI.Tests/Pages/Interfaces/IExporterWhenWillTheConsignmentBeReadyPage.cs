namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterWhenWillTheConsignmentBeReadyPage
    {
        bool IsPageLoaded();
        void EnterReadyDate(DateTime date);
        void ClickSaveAndContinueButton();
    }
}
