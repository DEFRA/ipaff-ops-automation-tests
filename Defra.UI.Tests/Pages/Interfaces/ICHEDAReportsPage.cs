namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICHEDAReportsPage
    {
        bool IsPageLoaded();
        void ClickImportsCommodityRulesReportLink();
        void ClickRiskDecisionReportLink();
        void ClickCountryRulesReportLink();
    }
}