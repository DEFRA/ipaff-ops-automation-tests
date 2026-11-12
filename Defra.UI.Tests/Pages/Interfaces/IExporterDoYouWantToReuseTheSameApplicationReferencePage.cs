namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterDoYouWantToReuseTheSameApplicationReferencePage
    {
        bool IsPageLoaded();
        void SelectOption(string option);
        void EnterNewReference(string reference);
        void ClickSaveAndContinueButton();
    }
}
