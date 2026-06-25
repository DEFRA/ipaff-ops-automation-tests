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
        private IWebElement rdoPackerCode => _driver.WaitForElementExists(By.Id("packer-or-defra-code"), true);
        private IWebElement rdoExporter => _driver.WaitForElementExists(By.Id("packer-conditional-exporter"), true);
        private IWebElement rdoOther => _driver.WaitForElementExists(By.Id("packer-conditional-user"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));
        #endregion

        public ExporterWhatAreThePackingDetailsAsTheyAppearOnThePackagingPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("What are the packing details as they appear on the packaging?");

        public void SelectMiddleOption() => rdoExporter.Click();

        public void SelectPackingOption(string option)
        {
            switch (option.ToLower())
            {
                case "packer code":
                case "defra code":
                    rdoPackerCode.Click();
                    break;
                case "exporter":
                    rdoExporter.Click();
                    break;
                case "other":
                    rdoOther.Click();
                    break;
                default:
                    rdoExporter.Click();
                    break;
            }
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();
    }
}