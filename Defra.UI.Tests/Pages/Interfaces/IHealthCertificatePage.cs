using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IHealthCertificatePage
    {
        bool IsPageLoaded();
        void ClickContinue();
    }
}
