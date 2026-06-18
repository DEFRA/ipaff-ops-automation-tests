using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class RiskEngineHomePage : IRiskEngineHomePage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Home']"), true);
        private IWebElement headerLink(string linkText) =>
            _driver.WaitForElement(By.XPath($"//ul[@id='navigation']//a[normalize-space()='{linkText}']"));
        #endregion

        public RiskEngineHomePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Home");

        public void ClickHeaderLink(string linkText) => headerLink(linkText).Click();
    }
}