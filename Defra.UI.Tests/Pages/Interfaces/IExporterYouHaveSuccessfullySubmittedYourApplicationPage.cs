namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterYouHaveSuccessfullySubmittedYourApplicationPage
    {
        bool IsPageLoaded();
        bool IsPhytosanitaryConfirmationPageLoaded();
        bool IsSeedSamplingConfirmationPageLoaded();
        string GetAphaReferenceNumber();
        bool IsGuidanceDisplayed(string guidance);
        void ClickViewYourApplicationsButton();
    }
}