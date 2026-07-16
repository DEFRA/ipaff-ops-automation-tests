namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISelectTheCHEDACommodityYouWantToAddARuleForPage
    {
        bool IsPageLoaded();
        void SearchCommodity(string commodityCode, string commodityName);
    }
}