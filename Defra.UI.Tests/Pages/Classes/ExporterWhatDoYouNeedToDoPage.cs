using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterWhatDoYouNeedToDoPage : IExporterWhatDoYouNeedToDoPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='What do you need to do?']"), true);
        private IWebElement optionLabel(string option) => _driver.WaitForElement(By.XPath($"//label[contains(normalize-space(),'{option}')]"), true);
        private IWebElement btnContinue => _driver.WaitForElement(By.XPath("//button[normalize-space()='Continue'] | //input[@value='Continue']"));
        #endregion

        public ExporterWhatDoYouNeedToDoPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("What do you need to do?");

        public void SelectWhatDoYouNeedToDoOption(string option) => optionLabel(option).Click();

        public void ClickContinueButton() => btnContinue.Click();
    }
}