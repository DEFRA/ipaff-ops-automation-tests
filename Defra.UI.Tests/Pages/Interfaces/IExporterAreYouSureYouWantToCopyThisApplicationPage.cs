namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterAreYouSureYouWantToCopyThisApplicationPage
    {
        bool IsPageLoaded();
        void SelectOption(string option);
        void ClickContinueButton();
    }
}
