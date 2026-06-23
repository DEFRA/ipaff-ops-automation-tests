using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterDeclarationPage : IExporterDeclarationPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Exporter declaration']"), true);
        private IWebElement chkDeclaration => _driver.WaitForElement(By.XPath("//input[@type='checkbox']/following-sibling::label[1]"), true);
        private IWebElement btnSubmitApplication => _driver.WaitForElement(By.XPath("//button[normalize-space()='Submit application'] | //input[@value='Submit application']"));
        #endregion

        public ExporterDeclarationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Exporter declaration");
        public void TickDeclarationCheckbox() => chkDeclaration.Click();
        public void ClickSubmitApplicationButton() => btnSubmitApplication.Click();
    }
}