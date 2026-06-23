namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterWhenAndWhereWillTheConsignmentBeReadyPage
    {
        bool IsPageLoaded();
        void EnterReadyDateTimeAndPlace(DateTime date, string hhmm, string place);
        void ClickSaveAndContinueButton();
    }
}