namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IViewAllCHEDDImportCommodityRulesPage
    {
        bool IsPageLoaded();
        void ScrollToBottom();
        int GetTotalRuleCount();
        void EnterSearchText(string text);
        IDictionary<string, string> GetTopRowDetails();
        string GetTopRowId();
        bool SwitchToNewlyOpenedTab();
        void ClickRemoveRuleLinkForRuleId(string ruleId);
        bool IsRuleIdPresent(string ruleId);
    }
}