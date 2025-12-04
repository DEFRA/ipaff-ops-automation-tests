using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class AboutConsignmentPage : IAboutConsignmentPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement optLiveAnimals => _driver.WaitForElement(By.XPath("//*[@id='cert-type']/following-sibling::label"));
        private IWebElement optProductsAnimalOrigin => _driver.WaitForElement(By.XPath("//*[@id='cert-type-2']/following-sibling::label"));
        private IWebElement optHighRiskFoodFeed => _driver.WaitForElement(By.XPath("//*[@id='cert-type-3']/following-sibling::label"));
        private IWebElement optPlantsProducts => _driver.WaitForElement(By.XPath("//*[@id='cert-type-4']/following-sibling::label"));
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.XPath("//button[text()='Save and continue']"));     
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public AboutConsignmentPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("About the consignment")
                && primaryTitle.Text.Contains("What are you importing?");
        }

        public bool IsElementPresent(IWebElement element)
        {
            try
            {
                return !string.IsNullOrEmpty(element.Text);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool AreImportOptionsPresent()
        {
            return IsElementPresent(optLiveAnimals)
                && IsElementPresent(optProductsAnimalOrigin)
                && IsElementPresent(optHighRiskFoodFeed)
                && IsElementPresent(optPlantsProducts);
        }

        public void ClickImportingProduct(string option)
        {
            if(option.Equals(optLiveAnimals.Text))
                optLiveAnimals.Click();
            else if (option.Equals(optProductsAnimalOrigin.Text))
                optProductsAnimalOrigin.Click();
            else if (option.Equals(optHighRiskFoodFeed.Text))
                optHighRiskFoodFeed.Click();
            else if (option.Equals(optPlantsProducts.Text))
                optPlantsProducts.Click();
        }

        public void ClickSaveAndContinue()
        {
            btnSaveAndContinue.Click();
        }
    }
}