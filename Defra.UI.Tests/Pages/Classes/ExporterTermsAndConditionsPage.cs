using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterTermsAndConditionsPage : IExporterTermsAndConditionsPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private IWebElement pageTitle => _driver.WaitForElement(By.CssSelector("main[data-qa-page='Page-ExportApplicationTermsAndConditions'] h1"));
        private IWebElement chkConfirm => _driver.WaitForElement(By.Id("confirm"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("Button-AcceptTermsAndConditions"));

        public ExporterTermsAndConditionsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Text.Trim() == "Terms and conditions";
        public void AcceptAndContinue()
        {
            if (!chkConfirm.Selected)
                chkConfirm.Click();
            btnContinue.Click();
        }
    }
}
