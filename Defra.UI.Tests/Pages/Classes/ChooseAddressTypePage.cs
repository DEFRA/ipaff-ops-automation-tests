using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ChooseAddressTypePage : IChooseAddressTypePage
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

        public ChooseAddressTypePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Choose address type");
        }

        public void SelectAddressType(string addressType)
        {
            var radioButton = _driver.FindElement(GetRadioButtonLocator(addressType));
            radioButton.Click();
        }

        public bool AreRadioButtonsDisplayed(string radioButton1, string radioButton2)
        {
            try
            {
                var label1 = _driver.FindElement(GetLabelLocator(radioButton1));
                var label2 = _driver.FindElement(GetLabelLocator(radioButton2));

                return label1.Displayed && label2.Displayed;
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