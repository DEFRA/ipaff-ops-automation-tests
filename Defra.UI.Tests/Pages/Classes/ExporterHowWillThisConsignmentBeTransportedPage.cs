using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterHowWillThisConsignmentBeTransportedPage : IExporterHowWillThisConsignmentBeTransportedPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='How will this consignment be transported?']"), true);
        private IWebElement rdoAir => _driver.WaitForElementExists(By.Id("air"), true);
        private IWebElement rdoMaritime => _driver.WaitForElementExists(By.Id("maritime"), true);
        private IWebElement rdoRail => _driver.WaitForElementExists(By.Id("rail"), true);
        private IWebElement rdoRoad => _driver.WaitForElementExists(By.Id("road"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));
        #endregion

        public ExporterHowWillThisConsignmentBeTransportedPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("How will this consignment be transported?");

        public void SelectAnyTransportMethod() => rdoAir.Click();

        public void SelectTransportMethod(string method)
        {
            switch (method.ToLower())
            {
                case "air":
                    rdoAir.Click();
                    break;
                case "maritime":
                    rdoMaritime.Click();
                    break;
                case "rail":
                    rdoRail.Click();
                    break;
                case "road":
                    rdoRoad.Click();
                    break;
                default:
                    rdoAir.Click();
                    break;
            }
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();
    }
}