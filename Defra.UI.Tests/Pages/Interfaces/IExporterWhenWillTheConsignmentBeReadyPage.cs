namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterWhenWillTheConsignmentBeReadyPage
    {
        bool IsPageLoaded();
        void EnterReadyDate(DateTime date);
        void EnterReadyTime(string hour, string minute, string meridiem);
        void ClickSaveAndContinueButton();
    }
}
