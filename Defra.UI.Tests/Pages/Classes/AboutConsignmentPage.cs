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
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.XPath("//button[text()='Save and continue'] | //button[@value='Save and continue']"));
        private IWebElement rdoAgent => _driver.FindElement(By.XPath("//*[@id='for-own-organisation']/following-sibling::label"));
        private IWebElement rdoDiffOrg => _driver.FindElement(By.XPath("//*[@id='for-own-organisation-2']/following-sibling::label"));
        private IWebElement rdoCompany(string organisation) => _driver.FindElement(By.XPath($"//strong[normalize-space()='{organisation}']"));
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

        public bool IsWhoAreYouCreatingThisNotificationForPageLoaded()
        {
            return secondaryTitle.Text.Contains("About the consignment") 
                && primaryTitle.Text.Contains("Who are you creating this notification for?");
        }

        public void SelectToWhomNotificationCreatedFor(string option)
        {
            if (option.Equals(rdoAgent.Text.Trim()))
                rdoAgent.Click();
            else if (option.Equals(rdoDiffOrg.Text.Trim()))
                rdoDiffOrg.Click();
        }

        public bool IsWhichCompanyIsThisNotificationForPageLoaded()
        {
            return secondaryTitle.Text.Contains("About the consignment")
                && primaryTitle.Text.Contains("Which company is this notification for");
        }

        public void SelectCompany(string option)
        {
            rdoCompany(option).Click();
        }
    }
}