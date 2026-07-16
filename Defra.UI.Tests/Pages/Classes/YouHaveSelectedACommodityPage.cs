using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class YouHaveSelectedACommodityPage : IYouHaveSelectedACommodityPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='You have selected a commodity']"), true);
        private IWebElement commodityCodeCell => _driver.WaitForElement(By.XPath("//table//tbody//tr//td[1]"));
        private IWebElement descriptionCell => _driver.WaitForElement(By.XPath("//table//tbody//tr//td[2]"));
        private IWebElement btnContinue => _driver.WaitForElement(By.XPath("//button[@type='submit' and normalize-space()='Continue']"));
        #endregion

        public YouHaveSelectedACommodityPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("You have selected a commodity");

        public bool IsCommodityDisplayed(string commodityCode, string description)
        {
            var normalisedDescription = descriptionCell.Text
                .Trim()
                .Replace("\u00C2\u00A0", " ")  // remove UTF-8 mojibake: Â + non-breaking space
                .Replace("\u00A0", " ")         // remove any remaining non-breaking spaces
                .Replace("\u00C2", "");          // remove any remaining Â characters

            return commodityCodeCell.Text.Trim().Contains(commodityCode)
                && normalisedDescription.Contains(description);
        }

        public void ClickContinueButton() => btnContinue.Click();
    }
}