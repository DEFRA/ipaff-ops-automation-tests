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
        private IWebElement rdoFirstTransportOption => _driver.WaitForElement(By.XPath("//input[@type='radio']/following-sibling::label[1]"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.XPath("//button[normalize-space()='Save and continue'] | //input[@value='Save and continue']"));
        #endregion

        public ExporterHowWillThisConsignmentBeTransportedPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("How will this consignment be transported?");
        public void SelectAnyTransportMethod() => rdoFirstTransportOption.Click();
        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();
    }
}