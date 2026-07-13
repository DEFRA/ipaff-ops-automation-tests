using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class SetTheInspectionRatePage : ISetTheInspectionRatePage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Set the inspection rate']"), true);
        private IWebElement inspectionRateInput => _driver.WaitForElement(By.Id("inspectionrate"));
        private IWebElement btnContinue => _driver.WaitForElement(By.XPath("//button[@type='submit' and normalize-space()='Continue']"));
        #endregion

        public SetTheInspectionRatePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Set the inspection rate");

        public void EnterInspectionRate(string rate)
        {
            inspectionRateInput.Clear();
            inspectionRateInput.SendKeys(rate);
        }

        public void ClickContinueButton() => btnContinue.Click();
    }
}