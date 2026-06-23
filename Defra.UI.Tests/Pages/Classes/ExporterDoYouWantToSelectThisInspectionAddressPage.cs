using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterDoYouWantToSelectThisInspectionAddressPage : IExporterDoYouWantToSelectThisInspectionAddressPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Do you want to select this inspection address?']"), true);
        private IWebElement optionLabel(string option) => _driver.WaitForElement(By.XPath($"//label[contains(normalize-space(),'{option}')]"), true);
        private IWebElement btnContinue => _driver.WaitForElement(By.XPath("//button[normalize-space()='Continue'] | //input[@value='Continue']"));
        #endregion

        public ExporterDoYouWantToSelectThisInspectionAddressPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Do you want to select this inspection address?");

        public void SelectInspectionAddressOptionAndContinue(string option)
        {
            optionLabel(option).Click();
            btnContinue.Click();
        }
    }
}