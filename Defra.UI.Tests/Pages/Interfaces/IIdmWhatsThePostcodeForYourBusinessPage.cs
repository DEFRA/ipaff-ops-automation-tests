namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIdmWhatsThePostcodeForYourBusinessPage
    {
        bool IsPageLoaded();
        bool IsDisplayed();
        bool IsErrorSummaryDisplayed();
        string CurrentUrl { get; }
        bool ReloadPostcodePage(string url);
        void EnterPostcodeAndContinue(string postcode);
    }
}
