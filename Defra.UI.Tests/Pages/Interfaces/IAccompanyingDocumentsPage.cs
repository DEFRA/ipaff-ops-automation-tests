namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAccompanyingDocumentsPage
    {
        bool IsPageLoaded();
        bool IsAccompanyingDocPageLoadedOnInspectorApp();
        void SelectDocumentType(string type);
        void EnterDocumentReference(string reference);
        void EnterDateOfIssue(string day, string month, string year);
        bool IsDatePickerIconDisplayed();
        void SelectDateFromDatePicker();
        string GetDocumentIssueDate();
        void ClickAddAttachmentLink();
        void AddAccompanyingDocument(string fileName);
        void ClickCancelLink();
        void ClickAddADocument();
        bool IsRowPresent();
        string GetFileName { get; }
        public int GetFileLength { get; }
        bool ValidateDocUploadErrors();
        bool IsDownloadAttachmentLinkPresent();
        bool IsRemoveAttachmentLinkPresent();
        void SelectPreviousDateFromDatePicker(string previousDay);
    }
}