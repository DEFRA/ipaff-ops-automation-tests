using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterHowDoYouWantToAddCommoditiesToYourConsignmentPage : IExporterHowDoYouWantToAddCommoditiesToYourConsignmentPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='How do you want to add commodities to your consignment?']"), true);
        private IWebElement addMethodLabel(string method) => _driver.WaitForElement(By.XPath($"//label[contains(normalize-space(),'{method}')]"), true);
        private IWebElement btnContinue => _driver.WaitForElement(By.Id("InputMethod-Continue"));
        #endregion

        public ExporterHowDoYouWantToAddCommoditiesToYourConsignmentPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("How do you want to add commodities to your consignment?");

        public void SelectCommodityAddMethod(string method) => addMethodLabel(method).Click();

        public void ClickContinueButton() => btnContinue.Click();
    }
}