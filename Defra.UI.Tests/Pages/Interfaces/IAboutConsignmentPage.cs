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
        bool IsWhoAreYouCreatingThisNotificationForPageLoaded();
        void SelectToWhomNotificationCreatedFor(string option);
        bool IsWhichCompanyIsThisNotificationForPageLoaded();
        void SelectCompany(string option);
        void WaitAndSelectCompanyRadioButton(string businessName, TimeSpan maxWait, TimeSpan retryInterval);
    }
}
