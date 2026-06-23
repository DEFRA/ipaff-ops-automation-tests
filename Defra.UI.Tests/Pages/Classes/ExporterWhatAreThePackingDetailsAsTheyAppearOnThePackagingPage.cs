using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterWhatAreThePackingDetailsAsTheyAppearOnThePackagingPage : IExporterWhatAreThePackingDetailsAsTheyAppearOnThePackagingPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='What are the packing details as they appear on the packaging?']"), true);
        private IReadOnlyCollection<IWebElement> rdoPackingOptions => _driver.WaitForElements(By.XPath("//input[@type='radio']/following-sibling::label"));
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.XPath("//button[normalize-space()='Save and continue'] | //input[@value='Save and continue']"));
        #endregion

        public ExporterWhatAreThePackingDetailsAsTheyAppearOnThePackagingPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("What are the packing details as they appear on the packaging?");

        public void SelectMiddleOption()
        {
            if (rdoPackingOptions.Count >= 2)
            {
                rdoPackingOptions.ElementAt(1).Click();
            }
            else
            {
                rdoPackingOptions.First().Click();
            }
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();
    }
}