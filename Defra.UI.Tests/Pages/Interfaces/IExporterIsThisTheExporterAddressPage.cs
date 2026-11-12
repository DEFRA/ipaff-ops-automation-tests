namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterIsThisTheExporterAddressPage
    {
        bool IsPageLoaded();
        void SelectOption(string option);
        void ClickSaveAndContinueButton();
        string GetExporterName();
        string GetExporterAddress();
        bool IsGuidanceDisplayed(string guidance);
        bool IsRadioButtonPresentAndUnselected(string option);
    }
}
