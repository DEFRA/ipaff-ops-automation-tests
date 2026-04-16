using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISignOutPage
    {
        void SignedOut();
        void BTMSSignOut();
        bool VerifySignedOutPage();
        void CloseBrowser();
        bool VerifyInspectorSignedOutPage();
        void CloseCurrentTab();
    }
}
