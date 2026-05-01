using System.Collections.Generic;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IViewAllPHSIImportCommodityRulesPage
    {
        bool IsPageLoaded();
        void ScrollToBottom();
        int GetTotalRuleCount();
        void EnterSearchText(string text);
        void SortByIdDescending();
        IDictionary<string, string> GetTopRowDetails();
        string GetTopRowId();
        bool SwitchToNewlyOpenedTab();
    }
}