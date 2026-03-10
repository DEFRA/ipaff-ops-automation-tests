using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class BTMSSearchPage : IBTMSSearchPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//label[contains(@class,'govuk-label')]"), true);
        private IWebElement txtCHEDRefInput => _driver.WaitForElement(By.Id("search-term"));
        private IWebElement btnSearch => _driver.WaitForElement(By.XPath("//button[@class='btms-submit-search']"));     
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public BTMSSearchPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Trim().Contains("Enter an MRN, CHED, GMR, VRN or TRN");
        }

        public void SearchForChed(string chedRef)
        {
            txtCHEDRefInput.Clear();
            txtCHEDRefInput.SendKeys(chedRef);
            btnSearch.Click();
        }
    }
}