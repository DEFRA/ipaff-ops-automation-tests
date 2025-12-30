using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;
using System.Drawing;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ReviewBorderNotificationPage : IReviewBorderNotificationPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement btnSubmit => _driver.FindElement(By.Id("submit-button"));
        private IWebElement lnkDocument => _driver.FindElement(By.Id("download-attachment-0"));
        //private IReadOnlyCollection <IWebElement> lnkDownloadedFile => _driver.FindElements(By.Id("file-link"));
        #endregion
       
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ReviewBorderNotificationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Review border notification");
        }

        public void ClickSubmitButton()
        {
            btnSubmit.Click();
        }

        public void ClickDocumentLink()
        {
            lnkDocument.Click();
            Thread.Sleep(1000);
        }

        public void OpenDownloadsInNewTab()
        {
            _driver.SwitchTo().NewWindow(WindowType.Tab);
            _driver?.Navigate().GoToUrl("chrome://downloads/");
        }

        public bool VerifyFileDownloaded(string fileName)
        {
            string downloadedFile = (string)((IJavaScriptExecutor)_driver)
                .ExecuteScript("return document.querySelector('downloads-manager')" +
                ".shadowRoot.querySelector('downloads-item')" +
                ".shadowRoot.querySelector('#file-link').textContent;");

            return downloadedFile.Contains(fileName);
        }
    }
}