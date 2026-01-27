using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ManageCatchCertificatesPage : IManageCatchCertificatesPage
    {
        private IObjectContainer _objectContainer;
        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement rdoMoreCertificateOption(string option) => _driver.FindElement(By.XPath($"//input[@type='radio' and @value='{option}']"));
        private IReadOnlyCollection<IWebElement> RemoveCertificates => _driver.WaitForElements(By.XPath("//a[normalize-space()='Remove']"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ManageCatchCertificatesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded(string pageTitle)
        {
            return primaryTitle.Text.Trim().Equals(pageTitle, StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifyNumberOfCertificates(string numberOfCertificates) => RemoveCertificates.Count.ToString() == numberOfCertificates;

        public void SelectOption(string option)
        {
            rdoMoreCertificateOption(option).Click();
        }
    }
}
