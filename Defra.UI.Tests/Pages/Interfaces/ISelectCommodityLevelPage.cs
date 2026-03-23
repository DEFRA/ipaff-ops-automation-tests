namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISelectCommodityLevelPage
    {
        bool IsPageLoaded();
        void SelectCommodityByDescription(string description);
        void ClickSelect();
    }
}