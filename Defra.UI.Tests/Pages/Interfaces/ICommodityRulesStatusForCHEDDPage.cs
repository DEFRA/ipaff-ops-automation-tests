namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICommodityRulesStatusForCHEDDPage
    {
        bool IsPageLoaded();
        IDictionary<string, string> GetSummaryDetails();
        int GetSummaryFieldAsInt(string field);
        void ClickViewAllCHEDDImportsCommodityRulesLink();
    }
}