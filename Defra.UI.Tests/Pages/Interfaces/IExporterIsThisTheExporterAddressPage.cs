namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterIsThisTheExporterAddressPage
    {
        bool IsPageLoaded();
        void SelectOption(string option);
        void ClickSaveAndContinueButton();
        bool IsGuidanceDisplayed(string guidance);
        bool IsRadioButtonPresentAndUnselected(string option);
    }
}
