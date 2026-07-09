namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISelectTheCHEDDCommodityYouWantToAddARuleForPage
    {
        bool IsPageLoaded();
        void SearchCommodity(string commodityCode, string commodityName);
    }
}