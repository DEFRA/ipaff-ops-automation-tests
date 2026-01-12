using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Collections.ObjectModel;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;

namespace Defra.UI.Tests.Pages.Classes
{
    public class AnimalIdentificationDetailsPage : IAnimalIdentificationDetailsPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-primary-title']"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-secondary-title']"), true);
        private IWebElement txtIdentificationDetails => _driver.FindElement(By.XPath("//input[contains(@id, 'identification_details')]"));
        private IWebElement txtDescription => _driver.FindElement(By.XPath("//input[contains(@id, 'description')]"));
        private IWebElement txtHorseName => _driver.FindElement(By.XPath("//input[contains(@id, 'horse_name')]"));
        private IWebElement txtMicrochipNumber => _driver.FindElement(By.XPath("//input[contains(@id, 'microchip')]"));
        private IWebElement txtPassportNumber => _driver.FindElement(By.XPath("//input[contains(@id, 'passport')]"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public AnimalIdentificationDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Description of the goods")
                && primaryTitle.Text.Contains("Enter animal identification details");
        }

        public void EnterIdentificationDetails(string identificationDetails)
        {
            txtIdentificationDetails.Clear();
            txtIdentificationDetails.SendKeys(identificationDetails);
        }

        public void EnterDescription(string description)
        {
            txtDescription.Clear();
            txtDescription.SendKeys(description);
        }

        public void EnterHorseName(string horseName)
        {
            txtHorseName.Clear();
            txtHorseName.SendKeys(horseName);
        }

        public void EnterMicrochipNumber(string microchipNumber)
        {
            txtMicrochipNumber.Clear();
            txtMicrochipNumber.SendKeys(microchipNumber);
        }

        public void EnterPassportNumber(string passportNumber)
        {
            txtPassportNumber.Clear();
            txtPassportNumber.SendKeys(passportNumber);
        }
    }
}