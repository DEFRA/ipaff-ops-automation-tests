using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class UploadCatchCertificatesPage : IUploadCatchCertificatesPage
    {
        private IObjectContainer _objectContainer;
        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);

        private IWebElement uploadInput => _driver.WaitForElement(By.CssSelector("input[type='file']"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public UploadCatchCertificatesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded(string pageTitle)
        {
            return primaryTitle.Text.Trim().Equals(pageTitle, StringComparison.OrdinalIgnoreCase);
        }
    }
}
