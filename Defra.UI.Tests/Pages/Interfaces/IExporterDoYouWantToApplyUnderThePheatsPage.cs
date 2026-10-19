namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterDoYouWantToApplyUnderThePheatsPage
    {
        bool IsPageLoaded();
        void SelectOption(string option);
        void ClickSaveAndContinueButton();
    }
}
