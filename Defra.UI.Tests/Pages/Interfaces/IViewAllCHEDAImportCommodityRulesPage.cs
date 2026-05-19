namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IViewAllCHEDAImportCommodityRulesPage
    {
        bool IsPageLoaded();
        void ScrollToBottom();
        int GetTotalRuleCount();
        void EnterSearchText(string text);
        IDictionary<string, string> GetTopRowDetails();
        string GetTopRowId();
        bool SwitchToNewlyOpenedTab();
    }
}