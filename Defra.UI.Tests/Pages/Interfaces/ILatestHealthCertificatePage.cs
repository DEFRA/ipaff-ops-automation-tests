namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ILatestHealthCertificatePage
    {
        bool IsPageLoaded();
        void EnterDocumentReference(string reference);
        void EnterDateOfIssue(string day, string month, string year);
        void ClickAddAttachmentLink();
        void AddHealthCertificate(string fileName);
        string GetFileName { get; }
        string GetDocumentIssueDate();
    }
}