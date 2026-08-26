namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICheckAndSubmitCommodityRulePage
    {
        bool IsPageLoaded();
        void ClickConfirmAndSubmitRuleButton();
        IDictionary<string, string> GetSummaryDetails();
    }
}