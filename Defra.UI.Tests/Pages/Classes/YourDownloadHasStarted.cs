using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;
using System.Diagnostics;

namespace Defra.UI.Tests.Pages.Classes
{
    public class YourDownloadHasStarted : IYourDownloadHasStarted
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement btnReturnToDocument => _driver.FindElement(By.Id("return-to-documents-button"));
        #endregion

        public YourDownloadHasStarted(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Your download has started");
        }

        public bool IsDownloadedZipFile(string chedReference)
        {
            return Utils.IsDownloaded(chedReference, "zip");
        }

        public void ClickReturnToDocuments()
        {
            btnReturnToDocument.Click();
        }
    }
}
