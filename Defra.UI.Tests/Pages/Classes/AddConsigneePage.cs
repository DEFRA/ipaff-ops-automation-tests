using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class AddConsigneePage : IAddConsigneePage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[contains(@class,'govuk-heading-xl')]"), true);
        private IWebElement txtConsigneName => _driver.FindElement(By.Id("company-name"));
        private IWebElement txtConsigneeAddress => _driver.FindElement(By.Id("address-line-1"));
        private IWebElement txtCityOrTown => _driver.FindElement(By.Id("city-or-town"));
        private IWebElement txtPostCode => _driver.FindElement(By.Id("postcode"));
        private IWebElement txtTelephoneNumber => _driver.FindElement(By.Id("telephone"));
        private IWebElement txtEmailAddress => _driver.FindElement(By.Id("email"));
        private IWebElement drpdownCountry => _driver.FindElement(By.Id("country"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public AddConsigneePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        #region Methods

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Contains("Add consignee");
        }

        public void EnterConsigneeName(string consigneeName)
        {
            txtConsigneName.SendKeys(consigneeName);
        }
        
        public void EnterConsigneeAddress(string consigneeAddress)
        {
            txtConsigneeAddress.SendKeys(consigneeAddress);
        }
        
        public void EnterConsigneeCityOrTown(string cityOrTown)
        {
            txtCityOrTown.SendKeys(cityOrTown);
        }
        
        public void EnterConsigneePostCode(string postCode)
        {
            txtPostCode.SendKeys(postCode);
        }
        
        public void EnterConsigneeTelephone(string telephoneNumber)
        {
            txtTelephoneNumber.SendKeys(telephoneNumber);
        }
        
        public void EnterConsigneeCountry(string country)
        {
            new SelectElement(drpdownCountry).SelectByText(country);
        }
        
        public void EnterConsigneeEmail(string email)
        {
            txtEmailAddress.SendKeys(email);
        }
        #endregion
    }
}
