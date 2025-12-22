using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IBillingDetailsPage
    {
        void ClickRatesAndEligibilityLink();
        void ClickSaveAndContinue();
        void ClickTermsAndConditionsLink();
        void CloseTheNewTab();
        bool IsPageLoaded();
        bool VerifyNewTabClosed();
        bool VerifyPageOpensInNewTab(string pageName);
    }
}
