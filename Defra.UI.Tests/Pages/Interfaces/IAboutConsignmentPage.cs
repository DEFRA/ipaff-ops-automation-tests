using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAboutConsignmentPage
    {
        bool IsPageLoaded();
        bool AreImportOptionsPresent();
        void ClickImportingProduct(string option);
        void ClickSaveAndContinue();
        bool IsElementPresent(IWebElement element);
    }
}
