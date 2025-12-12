using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ILatestHealthCertificatePage
    {
        bool IsPageLoaded();  
        void EnterDocumentReference(string reference);
        void EnterDateOfIssue(string day, string month, string year);
        void ClickAddAttachmentLink();
        string GetFileName { get; }
        string GetDocumentIssueDate();
    }
}
