using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ReviewBorderNotificationPage : IReviewBorderNotificationPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement commodity => _driver.FindElement(By.Id("commodity"));
        private IWebElement netWeight => _driver.FindElement(By.Id("net-weight"));
        private IWebElement laboratoryTest => _driver.FindElement(By.Id("laboratory-test-"));
        private IWebElement country => _driver.FindElement(By.Id("country"));
        private IWebElement notificationType => _driver.FindElement(By.Id("notification-type"));
        private IWebElement notificationBasis => _driver.FindElement(By.Id("notification-basis"));
        private IWebElement productCategory => _driver.FindElement(By.Id("product-category"));
        private IWebElement productName => _driver.FindElement(By.Id("product-name"));
        private IWebElement brandName => _driver.FindElement(By.Id("brand-name"));
        private IWebElement otherLabelling => _driver.FindElement(By.Id("other-labelling"));
        private IWebElement otherInformation => _driver.FindElement(By.Id("other-information"));
        private IWebElement durabilityDate => _driver.FindElement(By.Id("durability-date"));
        private IWebElement riskDecision => _driver.FindElement(By.Id("risk-decision"));
        private IWebElement impactOn => _driver.FindElement(By.Id("impact-on"));
        private IWebElement hazardCategory => _driver.FindElement(By.Id("hazard-category"));
        private IWebElement measureTaken => _driver.FindElement(By.Id("measure-taken"));

        //Accompanying documents
        private IWebElement accompanyingDocumentType => _driver.FindElement(By.Id("document-type-0"));
        private IWebElement accompanyingDocumentRef => _driver.FindElement(By.Id("document-reference-0"));
        private IWebElement accompanyingDocumentFileName => _driver.FindElement(By.Id("attachment-name-0"));

        //Last updated Date and Time
        private IWebElement lastUpdatedDate => _driver.FindElement(By.Id("last-updated-date"));
        private IWebElement lastUpdatedTime => _driver.FindElement(By.Id("last-updated-time"));
        
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
            return pageTitle.Text.Contains("Review border notification");
        }

        public string GetCommodity => commodity?.Text?.Trim() ?? string.Empty;
        public string GetNetWeight => netWeight?.Text?.Trim() ?? string.Empty;
        public string GetLaboratoryTest => laboratoryTest?.Text?.Trim() ?? string.Empty;
        public string GetCountry => country?.Text?.Trim() ?? string.Empty;
        public string GetNotificationType => notificationType?.Text?.Trim() ?? string.Empty;
        public string GetNotificationBasis => notificationBasis?.Text?.Trim() ?? string.Empty;
        public string GetProductCategory => productCategory?.Text?.Trim() ?? string.Empty;
        public string GetProductName => productName?.Text?.Trim() ?? string.Empty;
        public string GetBrandName => brandName?.Text?.Trim() ?? string.Empty;
        public string GetOtherLabelling => otherLabelling?.Text?.Trim() ?? string.Empty;
        public string GetOtherInformation => otherInformation?.Text?.Trim() ?? string.Empty;
        public string GetDurabilityDate => durabilityDate?.Text?.Trim() ?? string.Empty;
        public string GetRiskDecision => riskDecision?.Text?.Trim() ?? string.Empty;
        public string GetImpactOn => impactOn?.Text?.Trim() ?? string.Empty;
        public string GetHazardCategory => hazardCategory?.Text?.Trim() ?? string.Empty;
        public string GetMeasureTaken => measureTaken?.Text?.Trim() ?? string.Empty;

        //Accompanying document details
        public string GetAccompanyingDocumentType => accompanyingDocumentType?.Text?.Trim() ?? string.Empty;
        public string GetAccompanyingDocumentRef => accompanyingDocumentRef?.Text?.Trim() ?? string.Empty;
        public string GetAccompanyingDocumentFileName => accompanyingDocumentFileName?.Text?.Trim() ?? string.Empty;
        
        //Last updated Date and Time details
        public string GetLastUpdatedDate => lastUpdatedDate?.Text?.Trim() ?? string.Empty;
        public string GetLastUpdatedTime => lastUpdatedTime?.Text?.Trim() ?? string.Empty;
        
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