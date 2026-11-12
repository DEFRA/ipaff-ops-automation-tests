using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterHowDoYouWantToAddCommoditiesToThisApplicationPage : IExporterHowDoYouWantToAddCommoditiesToThisApplicationPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='How do you want to add commodities to this application?']"), true);
        private IWebElement rdoOneAtATime => _driver.WaitForElementExists(By.Id("one-at-a-time"));
        private IWebElement rdoUploadAFile => _driver.WaitForElementExists(By.Id("upload-a-file"));
        private IWebElement btnContinue => _driver.WaitForElement(By.Id("InputMethod-Continue"));
        #endregion

        public ExporterHowDoYouWantToAddCommoditiesToThisApplicationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("How do you want to add commodities to this application?");

        public void SelectCommodityAddMethod(string option)
        {
            var radio = option.Trim() switch
            {
                "Manually, by adding one at a time" => rdoOneAtATime,
                "All at once, by uploading a file" => rdoUploadAFile,
                _ => throw new ArgumentException($"Unknown commodity add method '{option}'")
            };
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", radio);
        }

        public void ClickContinueButton() => btnContinue.Click();
    }
}
