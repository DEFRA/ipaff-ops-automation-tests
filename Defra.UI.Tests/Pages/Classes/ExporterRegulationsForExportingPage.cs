using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterRegulationsForExportingPage : IExporterRegulationsForExportingPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.Id("regulation-page-heading"), true);
        private IWebElement chkConfirm => _driver.WaitForElementExists(By.Id("confirm"), true);
        private IWebElement btnContinue => _driver.WaitForElement(By.Id("Button-Confirm_regulations"));
        #endregion

        public ExporterRegulationsForExportingPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded(string commodity, string country) =>
            pageTitle.Text.Trim().Equals($"Regulations for exporting {commodity} to {country}", StringComparison.OrdinalIgnoreCase);

        public void TickConfirmRegulationsCheckbox()
        {
            if (!chkConfirm.Selected)
                chkConfirm.Click();
        }

        public void ClickContinueButton() => btnContinue.Click();
    }
}
