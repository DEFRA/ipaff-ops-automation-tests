using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IOCDetailsPage : IIOCDetailsPage
    {
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1"), true);
        private IWebElement btnStopControl => _driver.FindElement(By.Id("stop-control-button"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public IOCDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return pageTitle.Displayed;
        }

        public void ClickStopControl()
        {
            btnStopControl.Click();
        }
    }
}