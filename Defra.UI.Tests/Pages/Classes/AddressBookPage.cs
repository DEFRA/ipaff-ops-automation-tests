using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class AddressBookPage : IAddressBookPage
    {
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement PageHeading => _driver.WaitForElement(By.XPath("//div[@id='address-book-page']//h1[@id='page-primary-title']"), true);
        private IWebElement ddlType => _driver.FindElement(By.Id("type"));
        private IWebElement btnSearch => _driver.FindElement(By.Id("search"));
        private IWebElement economicOperatorsTable => _driver.WaitForElement(By.Id("economic-operators-table"));
        private IWebElement lnkDashboard => _driver.FindElement(By.XPath("//a[text()='Dashboard']"));

        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        public AddressBookPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        #region Methods
        public bool IsPageLoaded()
        {
            return PageHeading.Text.Contains("Address book");
        }

        public void SelectType(string type)
        {
            new SelectElement(ddlType).SelectByText(type);
        }

        public void ClickSearchButton() => btnSearch.Click();

        public bool ValidateTypeInSearchResults(string type)
        {
            // Count rows
            var rows = economicOperatorsTable.FindElements(By.XPath("//tbody/tr")).Count;
            if (rows == 0) 
                return false;

            // Count rows where the 2nd td contains the text (Type column is the 2nd column)
            /*var matchedRows = economicOperatorsTable.FindElements(By.XPath(
                "//tbody/tr[td[2][contains(normalize-space(.), " +
                $"\"{type}\"" + ")]]")).Count;*/

            var matchedRows = economicOperatorsTable.FindElements(By.XPath("//tbody/tr[td[2][contains(normalize-space(.), " + $"{type}" + ")]]")).Count;

            return matchedRows == rows;
        }

        public void ClickDashboardLink() => lnkDashboard.Click();
        #endregion
    }
}
