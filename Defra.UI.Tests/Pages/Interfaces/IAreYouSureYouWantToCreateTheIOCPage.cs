namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAreYouSureYouWantToCreateTheIOCPage
    {
        bool IsPageLoaded();
        bool IsYesCreateButtonDisplayed();
        bool IsNoDontCreateLinkDisplayed();
        void ClickYesCreateIntensifiedOfficialControl();
    }
}