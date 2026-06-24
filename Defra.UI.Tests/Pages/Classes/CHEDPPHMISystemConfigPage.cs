using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CHEDPPHMISystemConfigPage : ICHEDPPHMISystemConfigPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='HMI System Configuration']"), true);
        private IWebElement aisRate => _driver.WaitForElement(By.Id("aisrate"));
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.XPath("//button[@type='submit' and normalize-space()='Send and continue']"));

        #endregion

        public CHEDPPHMISystemConfigPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("HMI System Configuration");

        /*public bool IsPageLoaded()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(50));
            wait.Until(ExpectedConditions.TextToBePresentInElement(pageTitle, "HMI System Configuration"));
            return pageTitle.Text.Trim().Contains("HMI System Configuration");
        }*/

        public int? GetCurrentAISRate() => int.TryParse(aisRate.GetAttribute("value"), out int value)? value: null;

        public void SetNewAISRate(int newAISRate)
        {
            aisRate.Clear();
            aisRate.SendKeys(newAISRate.ToString());
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();

    }
}