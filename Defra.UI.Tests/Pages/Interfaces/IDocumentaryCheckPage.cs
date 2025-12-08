using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IDocumentaryCheckPage
    { 
        bool IsPageLoaded();
        void SelectDocumentaryCheckDecision(string decision);
        void ClickSaveAndContinue();
    }
}
