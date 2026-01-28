using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IConfirmationPage
    {
        bool VerifyInitialAssessmentPage();
        string GetCHEDReference();
        string GetCustomsDeclarationReference();
        string GetCustomsDocumentCode();
        void SignedOut();
        bool VerifySignedOutPage();
        void ClickReturnToDashboard();
        bool VerifyBannerMessage(string message);
        void ClickReturnToDashboardLink();
    }
}
