using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IDecisionHubPage
    { 
        bool IsPageLoaded();
        void ClickSaveAndSetAsInProgress();
        bool VerifyStatusUpdate(string stausNew, string statusInProgress);
        void ClickLocalReferenceNumberLink();
    }
}
