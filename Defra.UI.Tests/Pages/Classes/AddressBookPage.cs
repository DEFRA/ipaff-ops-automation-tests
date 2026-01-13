using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private IWebElement PageHeading => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement ddlType => _driver.FindElement(By.Id("type"));
        private IWebElement btnSearch => _driver.FindElement(By.Id("search"));
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
        #endregion
    }
}
