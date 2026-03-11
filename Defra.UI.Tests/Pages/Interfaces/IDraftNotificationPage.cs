namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IDraftNotificationPage
    {
        bool IsPageLoaded(string chedType);
        void ClickSaveAndContinue();
        bool AreTitleHelpTextDisplayed();
        bool VerifyAllMissingOrErrorLinksExists(IList<string> missingOrErrorLinks);
        string GetDraftNotificationNumber();
        void ClickCheckOrUpdateCommodityDetailsLink();
    }
}
