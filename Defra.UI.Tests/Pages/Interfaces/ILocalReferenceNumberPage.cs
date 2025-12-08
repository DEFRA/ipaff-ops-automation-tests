using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ILocalReferenceNumberPage
    { 
        bool IsPageLoaded();
        void EnterLocalReferenceNumber(string customDeclarionRef);
        void ClickSaveAndContinue();
    }
}
