namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAddIntendedUseOfBulbsPage
    {
        void ClickApplyButton();
        void ClickSaveAndContinueButton();
        bool IsPageLoaded();
        void SelctCommodityCode();
        void SelectOptionForIntentedFinalUsers(string option);
        bool VerifyMessageOnThePage(string message);
        string[] GetCommodityDetails();
    }
}
