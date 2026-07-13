namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IViewRulesForAllCountriesEUImportPage
    {
        bool IsPageLoaded();
        int GetTotalRuleCount();
        bool IsTableEmpty();
        void EnterSearchText(string text);
        void SortByIdDescending();
        IDictionary<string, string> GetTopRowDetails();
        string GetTopRowId();
        bool IsRuleIdPresent(string ruleId);
        void ClickRemoveRuleLinkForRuleId(string ruleId);
    }
}