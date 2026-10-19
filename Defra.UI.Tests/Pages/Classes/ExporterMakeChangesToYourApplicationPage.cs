using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterMakeChangesToYourApplicationPage : IExporterMakeChangesToYourApplicationPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.Id("review-heading"), true);
        private IWebElement lnkChange(string fieldName) =>
            _driver.WaitForElement(By.XPath($"//dt[normalize-space()='{fieldName}']/following-sibling::dd[contains(@class,'govuk-summary-list__actions')][1]//a[starts-with(normalize-space(),'Change')]"));
        #endregion

        public ExporterMakeChangesToYourApplicationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Make changes to your application");

        public void ClickChangeLink(string fieldName) => lnkChange(fieldName).Click();
    }
}
