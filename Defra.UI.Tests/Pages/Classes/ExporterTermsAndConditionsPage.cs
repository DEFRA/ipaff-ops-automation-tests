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

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Terms and conditions']"));
        private IWebElement chkConfirm => _driver.WaitForElementExists(By.Id("confirm"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("Button-AcceptTermsAndConditions"));
        #endregion

        public ExporterTermsAndConditionsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Displayed;
        public void AcceptAndContinue()
        {
            var confirm = chkConfirm;
            if (!confirm.Selected)
                confirm.Click();
            btnContinue.Click();
        }
    }
}
