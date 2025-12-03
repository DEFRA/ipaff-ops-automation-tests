using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class AdditionalDetailsPage : IAdditionalDetailsPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"));
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"));
        private IWebElement rdoAmbient => _driver.WaitForElement(By.XPath("//*[@id='productTemperature']/following-sibling::label"));
        private IWebElement rdoChilled => _driver.WaitForElement(By.XPath("//*[@id='productTemperature-2']/following-sibling::label"));
        private IWebElement rdoFrozen => _driver.WaitForElement(By.XPath("//*[@id='productTemperature-3']/following-sibling::label"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public AdditionalDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Description of the goods")
                && primaryTitle.Text.Contains("Additional details");
        }

        public void ClickImportingProduct(string option)
        {
            if(option.Equals(rdoAmbient.Text))
                rdoAmbient.Click();
            else if (option.Equals(rdoChilled.Text))
                rdoChilled.Click();
            else if (option.Equals(rdoFrozen.Text))
                rdoFrozen.Click();
        }
    }
}