using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;


namespace Defra.UI.Tests.Pages.Classes
{
    public class ContactAddressPage : IContactAddressPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement firstContactAddressLabel => _driver.FindElement(By.XPath("//label[contains(@for, 'branch-address')]"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ContactAddressPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Complete notification")
                && primaryTitle.Text.Contains("Contact address for consignment");
        }

        public bool IsPageLoadedWithoutSecondaryTitle()
        {
            return primaryTitle.Text.Contains("Contact address for consignment");
        }

        public string GetSelectedContactAddress()
        {
            var fullAddress = firstContactAddressLabel.Text.Trim();

            // Split by newlines to get individual address components
            var lines = fullAddress.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            // Return only the first 4 lines (street, region, city, postcode)
            // This excludes the country which is not displayed on the review page
            if (lines.Length >= 4)
            {
                return string.Join("\n", lines.Take(4).Select(l => l.Trim()));
            }

            return fullAddress;
        }
    }
}