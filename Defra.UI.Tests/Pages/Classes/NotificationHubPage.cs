using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class NotificationHubPage : INotificationHubPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement lblRefNumber => _driver.FindElement(By.Id("reference-number"));
        private IWebElement lnkCountriesConsignmentTravel => _driver.WaitForElement(By.XPath("//a[contains(text(), 'Countries the consignment will travel through')]"));
        private IWebElement lnkCommodity => _driver.WaitForElement(By.Id("commodity-details-link"), true);
        private IWebElement lnkContactAddressForConsignment => _driver.FindElement(By.Id("organisation-branch-address"));
        private IWebElement lnkNotificationHub(string link) => _driver.WaitForElement(By.XPath($"//a[normalize-space(text())='{link}']"), true);
        private IWebElement spanVersionNumber => _driver.WaitForElement(By.Id("cved-version-number"), true);

        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public NotificationHubPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Trim().Contains("Notification Hub");
        }

        public string GetRefNumber => lblRefNumber?.Text?.Trim() ?? string.Empty;

        public void ClickCommodityLink()
        {
            lnkCommodity.Click();
        }

        public void ClickContactAddressForConsignmentLink() => lnkContactAddressForConsignment.Click();

        public void ClickCountriesTheConsignmentWillTravelThroughLink()
        {
            lnkCountriesConsignmentTravel.Click();
        }

        public void ClickLink(string link)
        {
            lnkNotificationHub(link).Click();
        }

        public string GetNotificationVersion()
        {
            // The text contains "- V2" so we need to trim and extract just the version
            string versionText = spanVersionNumber.Text.Trim();
            // Remove the leading "- " if present (note: dash followed by space)
            return versionText.TrimStart('-', ' ');
        }
    }
}