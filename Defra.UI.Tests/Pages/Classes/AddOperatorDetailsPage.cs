using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using Defra.UI.Tests.HelperMethods;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class AddOperatorDetailsPage : IAddOperatorDetailsPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement txtOperatorName => _driver.WaitForElement(By.Id("company-name"));
        private IWebElement txtAddressLine1 => _driver.WaitForElement(By.Id("address-line-1"));
        private IWebElement txtCityOrTown => _driver.WaitForElement(By.Id("city-or-town"));
        private IWebElement txtPostcode => _driver.WaitForElement(By.Id("postcode"));
        private IWebElement ddlCountry => _driver.WaitForElement(By.Id("country"));
        private IWebElement txtTelephoneNumber => _driver.WaitForElement(By.Id("telephone"));
        private IWebElement txtEmail => _driver.WaitForElement(By.Id("email"));
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("button-save-and-continue"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public AddOperatorDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Add operator details");
        }

        public void EnterOperatorName(string name)
        {
            txtOperatorName.Clear();
            txtOperatorName.SendKeys(name);
        }

        public void EnterAddressLine1(string addressLine1)
        {
            txtAddressLine1.Clear();
            txtAddressLine1.SendKeys(addressLine1);
        }

        public void EnterCityOrTown(string cityOrTown)
        {
            txtCityOrTown.Clear();
            txtCityOrTown.SendKeys(cityOrTown);
        }

        public void EnterPostcode(string postcode)
        {
            txtPostcode.Clear();
            txtPostcode.SendKeys(postcode);
        }

        public void SelectCountry(string country)
        {
            _driver.SelectFromDropdown(ddlCountry, country);
        }

        public void EnterTelephoneNumber(string telephoneNumber)
        {
            txtTelephoneNumber.Clear();
            txtTelephoneNumber.SendKeys(telephoneNumber);
        }

        public void EnterEmail(string email)
        {
            txtEmail.Clear();
            txtEmail.SendKeys(email);
        }

        public void EnterOperatorDetails(OperatorDetails operatorDetails)
        {
            EnterOperatorName(operatorDetails.OperatorName);
            EnterAddressLine1(operatorDetails.AddressLine1);
            EnterCityOrTown(operatorDetails.CityOrTown);
            EnterPostcode(operatorDetails.Postcode);
            SelectCountry(operatorDetails.Country);
            EnterTelephoneNumber(operatorDetails.TelephoneNumber);
            EnterEmail(operatorDetails.Email);
        }

        public void ClickSaveAndContinue()
        {
            btnSaveAndContinue.Click();
        }
    }
}