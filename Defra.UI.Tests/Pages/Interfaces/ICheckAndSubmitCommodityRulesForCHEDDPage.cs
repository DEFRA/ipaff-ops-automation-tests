namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICheckAndSubmitCommodityRulesForCHEDDPage
    {
        bool IsPageLoaded();
        IDictionary<string, string> GetSummaryDetails();
        int GetSummaryFieldAsInt(string field);
        void ClickConfirmAndSubmitRulesButton();
    }
}