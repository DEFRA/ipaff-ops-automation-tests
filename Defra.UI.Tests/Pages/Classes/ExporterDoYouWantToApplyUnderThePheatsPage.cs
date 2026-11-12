using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterDoYouWantToApplyUnderThePheatsPage : IExporterDoYouWantToApplyUnderThePheatsPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.Id("pheats-title"), true);
        private IWebElement rdoYes => _driver.WaitForElementExists(By.Id("apply_pheats-yes"), true);
        private IWebElement rdoNo => _driver.WaitForElementExists(By.Id("apply_pheats-no"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));
        #endregion

        public ExporterDoYouWantToApplyUnderThePheatsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() =>
            pageTitle.Text.Trim().Equals("Do you want to apply under the plant health exports audited trader scheme (PHEATS)?");

        public void SelectOption(string option)
        {
            if (option.Trim().StartsWith("Yes", StringComparison.OrdinalIgnoreCase))
                rdoYes.Click();
            else
                rdoNo.Click();
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();
    }
}
