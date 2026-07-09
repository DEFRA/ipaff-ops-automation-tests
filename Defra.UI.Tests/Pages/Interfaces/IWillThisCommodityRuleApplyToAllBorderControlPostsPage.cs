namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IWillThisCommodityRuleApplyToAllBorderControlPostsPage
    {
        bool IsPageLoaded();
        void SelectRadioButton(string option);
        void ClickContinueButton();
    }
}