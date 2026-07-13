namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IYouHaveSelectedACommodityPage
    {
        bool IsPageLoaded();
        bool IsCommodityDisplayed(string commodityCode, string description);
        void ClickContinueButton();
    }
}