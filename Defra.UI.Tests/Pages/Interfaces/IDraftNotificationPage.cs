namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IDraftNotificationPage
    {
        bool IsPageLoaded();
        void ClickSaveAndContinue();
        bool AreTitleHelpTextDisplayed();
        bool VerifyAllMissingOrErrorLinksExists(IList<string> missingOrErrorLinks);
        void ClickEachLinkAndEnterMissingInformation();
        string GetDraftNotificationNumber();
    }
}
