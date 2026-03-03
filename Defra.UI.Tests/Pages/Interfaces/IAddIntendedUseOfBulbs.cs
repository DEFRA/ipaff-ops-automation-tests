namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAddIntendedUseOfBulbs
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
