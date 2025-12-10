using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIdentityAndPhysicalChecksPage
    { 
        bool IsPageLoaded();
        void ClickIdentityCheckOption(string decision, string checkType);
        void ClickPhysicalCheckDecision(string decision);
        void ClickSaveAndContinue();
        void ClickSaveAndReturn();
    }
}
