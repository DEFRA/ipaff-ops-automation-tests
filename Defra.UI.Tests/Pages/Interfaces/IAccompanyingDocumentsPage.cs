using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAccompanyingDocumentsPage
    {
        bool IsPageLoaded();  
        void SelectDocumentType(string type);
        void EnterDocumentReference(string reference);
        void EnterDateOfIssue(string day, string month, string year);
        // void ClickSaveAndContinue();
    }
}
