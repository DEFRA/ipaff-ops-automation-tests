using System.Collections.Generic;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICheckAndSubmitCommodityRulesForCHEDAPage
    {
        bool IsPageLoaded();
        IDictionary<string, string> GetSummaryDetails();
        int GetSummaryFieldAsInt(string field);
        void ClickConfirmAndSubmitRulesButton();
    }
}