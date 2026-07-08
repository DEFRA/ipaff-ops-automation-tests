namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICHEDPPImportsAndExportsPage
    {
        bool IsPageLoaded();
        void ClickBulkUploadCommodityRulesLink();
        void ClickHMIImportCommodityRulesLink();
        void ClickHMIExportCommodityRulesLink();
        void ClickPHSIIndividualCommodityRulesLink();
        void ClickSystemSettingsForHMIRulesLink();
        void ClickCountryRulesLink();
        void ClickGlobalInspectionRulesLink();
    }
}