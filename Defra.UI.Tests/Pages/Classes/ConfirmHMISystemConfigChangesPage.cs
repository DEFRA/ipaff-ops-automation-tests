using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ConfirmHMISystemConfigChangesPage : IConfirmHMISystemConfigChangesPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Confirm HMI System Configuration Changes']"), true);
        private IWebElement aisRate => _driver.WaitForElement(By.XPath("//ul[contains(@class,'govuk-list--bullet')]/li[contains(text(), 'AIS: Country Rate to')]"));
        private IWebElement btnConfirmAndSend => _driver.WaitForElement(By.XPath("//button[@type='submit' and normalize-space()='Confirm and send']"));

        #endregion

        public ConfirmHMISystemConfigChangesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Confirm HMI System Configuration Changes");
        
        public string GetAISCountryRate() => aisRate.Text?.Trim() ?? string.Empty;

        public void ClickConfirmAndSendButton() => btnConfirmAndSend.Click();

    }
}