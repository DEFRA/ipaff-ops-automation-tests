using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterWhoAreYouSendingThisConsignmentToPage : IExporterWhoAreYouSendingThisConsignmentToPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Who are you sending this consignment to?']"), true);
        private IWebElement txtConsigneeName => _driver.WaitForElement(By.Id("consigneeName"), true);
        private IWebElement txtAddressLine1 => _driver.WaitForElement(By.Id("addressLine1"), true);
        private IWebElement txtAddressLine2 => _driver.WaitForElement(By.Id("addressLine2"), true);
        private IWebElement txtTownOrCity => _driver.WaitForElement(By.Id("townOrCity"), true);
        private IWebElement txtCounty => _driver.WaitForElement(By.Id("county"), true);
        private IWebElement txtPostCode => _driver.WaitForElement(By.Id("postCode"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));
        #endregion

        public ExporterWhoAreYouSendingThisConsignmentToPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Who are you sending this consignment to?");

        public void EnterConsigneeDetails(string name, string addressLine1, string addressLine2, string townOrCity, string county, string postCode)
        {
            EnterText(txtConsigneeName, name);
            EnterText(txtAddressLine1, addressLine1);
            EnterText(txtAddressLine2, addressLine2);
            EnterText(txtTownOrCity, townOrCity);
            EnterText(txtCounty, county);
            EnterText(txtPostCode, postCode);
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();

        private static void EnterText(IWebElement element, string value)
        {
            element.Clear();
            element.SendKeys(value);
        }
    }
}
