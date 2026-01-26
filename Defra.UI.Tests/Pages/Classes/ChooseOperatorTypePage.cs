using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ChooseOperatorTypePage : IChooseOperatorTypePage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//h1[@class='govuk-fieldset__heading']"), true);
        private By GetLabelLocator(string labelText) => By.XPath($"//label[normalize-space()='{labelText}']");
        private By GetRadioButtonLocator(string labelText) => By.XPath($"//label[normalize-space()='{labelText}']/preceding-sibling::input[@type='radio']");
        private IWebElement btnContinue => _driver.FindElement(By.Id("continue"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ChooseOperatorTypePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Choose operator type");
        }

        public void SelectOperatorType(string operatorType)
        {
            var radioButton = _driver.FindElement(GetRadioButtonLocator(operatorType));
            radioButton.Click();
        }    

        public bool AreRadioButtonsDisplayed(string radioButton1, string radioButton2, string radioButton3, string radioButton4)
        {
            try
            {
                var label1 = _driver.FindElement(GetLabelLocator(radioButton1));
                var label2 = _driver.FindElement(GetLabelLocator(radioButton2));
                var label3 = _driver.FindElement(GetLabelLocator(radioButton3));
                var label4 = _driver.FindElement(GetLabelLocator(radioButton4));

                return label1.Displayed && label2.Displayed && label3.Displayed && label4.Displayed;
            }
            catch
            {
                return false;
            }
        }

        public void ClickContinue()
        {
            btnContinue.Click();
        }
    }
}