namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterWhenDoYouNeedTheCertificatePage
    {
        bool IsPageLoaded();
        void EnterNeededDateAndTime(DateTime date, string hhmm);
        void ClickContinueButton();
    }
}