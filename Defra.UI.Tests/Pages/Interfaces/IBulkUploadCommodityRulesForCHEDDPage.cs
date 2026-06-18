namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IBulkUploadCommodityRulesForCHEDDPage
    {
        bool IsPageLoaded();
        void ClickChooseFileButton();
        void SelectBulkUploadFile(string fileName);
        bool IsSelectedFileNameDisplayed(string fileName);
        void ClickContinueButton();
    }
}