using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class AddressBookPage : IAddressBookPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement lnkAddAnAddress => _driver.WaitForElement(By.LinkText("Add an address"));
        private IWebElement lnkReturnToAddressBook => _driver.WaitForElement(By.LinkText("Return to Address Book"));
        private IWebElement lnkDashboard => _driver.WaitForElement(By.LinkText("Dashboard"));
        private IReadOnlyCollection<IWebElement> addressBookEntries => _driver.FindElements(By.CssSelector("[data-test='address-book-entry']"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public AddressBookPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Address book");
        }

        public void ClickAddAnAddress()
        {
            lnkAddAnAddress.Click();
        }

        public bool IsAddressDisplayedInAddressBook(string addressName)
        {
            try
            {
                var addressElement = _driver.WaitForElement(By.XPath($"//*[contains(text(), '{addressName}')]"), true);
                return addressElement.Displayed;
            }
            catch
            {
                return false;
            }
        }

        public void ClickReturnToAddressBook()
        {
            lnkReturnToAddressBook.Click();
        }

        public void ClickDashboard()
        {
            lnkDashboard.Click();
        }
    }
}