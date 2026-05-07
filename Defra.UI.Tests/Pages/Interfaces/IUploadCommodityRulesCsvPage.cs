namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IUploadCommodityRulesCsvPage
    {
        bool IsPageLoaded();
        void ClickChooseFileButton();
        void SelectBulkUploadFile(string fileName);
        bool IsSelectedFileNameDisplayed(string fileName);
        void ClickUploadButton();
        void UpdateCsvIdForCommodityCode(string fileName, string commodityCode, string newId);
        string GetCsvIdForCommodityCode(string fileName, string commodityCode);
    }
}