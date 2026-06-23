using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterYourCommoditiesPage : IExporterYourCommoditiesPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Your commodities']"), true);
        private IWebElement tblCommodities => _driver.WaitForElement(By.Id("commodity-table"), true);
        private IWebElement finishedAddingOption(string option) => _driver.WaitForElement(By.XPath($"//label[contains(normalize-space(),'{option}')]"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.XPath("//button[normalize-space()='Save and continue'] | //input[@value='Save and continue']"));
        #endregion

        public ExporterYourCommoditiesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Your commodities");

        public bool IsCommodityLineDisplayed(string commodityName) => tblCommodities.Text.Contains(commodityName);

        public void SelectFinishedAddingCommoditiesOption(string option) => finishedAddingOption(option).Click();

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();
    }
}