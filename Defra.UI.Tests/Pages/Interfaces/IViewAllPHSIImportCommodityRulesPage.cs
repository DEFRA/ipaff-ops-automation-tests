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
        void TickSelectToDeleteCheckboxForRuleId(string ruleId);
        string GetSelectedRulesInfoText();
        void ClickDeleteRulesButton();
        bool IsConfirmDeletionDialogDisplayed();
        int GetConfirmDeletionDialogRuleCount();
        void ClickConfirmDeleteButton();
        bool IsConfirmDeletionDialogClosed();
        bool IsRuleIdPresent(string ruleId);
        string GetSearchInputText();
        bool IsIdColumnSorted();
    }
}