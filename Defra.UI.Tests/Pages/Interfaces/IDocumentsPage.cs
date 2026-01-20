using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IDocumentsPage
    {
        bool IsPageLoaded();
        void ClickSaveAndContinue();
        bool IsAddAnotherDocumentLinkDisplayed();
        bool VerifyNoDocumentsInInspectorSection();
        (string? documentType, string? documentReference, string? dateOfIssue) GetInspectorDocumentDetails(int index = 0);
    }
}