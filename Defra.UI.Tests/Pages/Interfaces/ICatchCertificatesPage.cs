using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICatchCertificatesPage
    {
        bool IsPageLoaded();
        void SelectAddCatchCertificate(string option);
    }
}
