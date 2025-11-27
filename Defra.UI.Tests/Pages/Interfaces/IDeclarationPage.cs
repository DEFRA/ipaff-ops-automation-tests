using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IDeclarationPage
    { 
        bool IsPageLoaded();
        void ClickSubmitNotification();
        bool VerifyInitialAssessmentPage();
        string GetCHEDReference();
        string GetCustomsDeclarationReference();
        string GetCustomsDocumentCode();
        void SignedOut();
        bool VerifySignedOutPage();
    }
}
