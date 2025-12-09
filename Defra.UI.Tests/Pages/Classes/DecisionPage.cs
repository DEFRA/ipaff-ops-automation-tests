using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class DecisionPage : IDecisionPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement btnSaveAndContinue => _driver.FindElement(By.Id("button-save-and-continue"));
        // Dynamic helper to get radio button by label text
        private IWebElement GetRadioButtonByLabel(string labelText) =>
            _driver.FindElement(By.XPath($"//label[normalize-space(text())='{labelText}']/preceding-sibling::input[@type='radio']"));
        // Dynamic helper to get sub-option radio button within conditional radios
        private IWebElement GetConditionalRadioButtonByLabel(string labelText) =>
            _driver.FindElement(By.XPath($"//div[contains(@class, 'govuk-radios--conditional')]//label[normalize-space(text())='{labelText}']/preceding-sibling::input[@type='radio']"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public DecisionPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Decision");
        }

        public void SelectAcceptableFor(string acceptableFor, string subOption)
        {
            // Click the main acceptableFor radio button based on label text
            GetRadioButtonByLabel(acceptableFor).Click();

            // If acceptableFor is "Internal market", click the sub-option
            if (acceptableFor.Equals("Internal market", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(subOption))
                {
                    GetConditionalRadioButtonByLabel(subOption).Click();
                }
            }
        }

        public void ClickSaveAndContinue()
        {
            btnSaveAndContinue.Click();
        }
    }
}