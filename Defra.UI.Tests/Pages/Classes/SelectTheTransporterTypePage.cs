using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class SelectTheTransporterTypePage : ISelectTheTransporterTypePage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement GetRadioButton(string transporterType) => _driver.FindElement(By.XPath($"//label[normalize-space()='{transporterType}']/preceding-sibling::input[@type='radio']"));
        private IWebElement btnSaveAndContinue => _driver.FindElement(By.Id("button-save-and-continue"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public SelectTheTransporterTypePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Select the transporter type");
        }

        public void SelectTransporterType(string transporterType)
        {
            var radioButton = GetRadioButton(transporterType);
            radioButton.Click();
        }

        public void ClickSaveAndContinue()
        {
            btnSaveAndContinue.Click();
        }
    }
}