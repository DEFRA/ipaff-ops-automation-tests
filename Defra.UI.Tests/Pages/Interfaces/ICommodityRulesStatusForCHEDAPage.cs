using System.Collections.Generic;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICommodityRulesStatusForCHEDAPage
    {
        bool IsPageLoaded();
        IDictionary<string, string> GetSummaryDetails();
        int GetSummaryFieldAsInt(string field);
        void ClickViewAllCHEDAImportsCommodityRulesLink();
    }
}