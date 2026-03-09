
using Reqnroll;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IUploadCommodityDetailsUsingCSVFilePage
    {
        void ClickCommDetailsCSVTemplateLink(string linkName, string oldFile);
        void ClickDownloadCSVTemplateLink();
        void ClickDownloadedFile(string fileName);
        void ClickUploadButton();
        string CreateCSVFromExcelTemplate(string templateFilePath, DataTable dataTable);
        bool IsGuidancePageLoadedInNewTab();
        bool IsPageLoaded();       
        void SelectCSVFile(string csvFilePath);   
        void VerifyExcelFileHeaders(string filePath, List<string> expectedHeaders);      
    }
}
