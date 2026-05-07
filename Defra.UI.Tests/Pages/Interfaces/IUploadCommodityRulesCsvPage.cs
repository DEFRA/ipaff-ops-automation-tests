namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IUploadCommodityRulesCsvPage
    {
        bool IsPageLoaded();
        void ClickChooseFileButton();
        void SelectBulkUploadFile(string fileName);
        bool IsSelectedFileNameDisplayed(string fileName);
        void ClickUploadButton();
    }
}