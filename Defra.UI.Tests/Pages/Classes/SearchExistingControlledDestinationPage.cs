using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class SearchExistingControlledDestinationPage : ISearchExistingControlledDestinationPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.ClassName("govuk-heading-xl"), true);
        private IWebElement btnSelect => _driver.FindElement(By.XPath("//button[contains(text(),'Select')]"));
        private IWebElement selectedControlledDestinationName => _driver.FindElement(By.XPath("//table//tbody//tr[1]//td[1]"));
        private IWebElement selectedControlledDestinationAddress => _driver.FindElement(By.XPath("//table//tbody//tr[1]//td[2]"));
        private IWebElement selectedControlledDestinationType => _driver.FindElement(By.XPath("//table//tbody//tr[1]//td[3]"));
        private IWebElement selectedControlledDestinationApprovalNumber => _driver.FindElement(By.XPath("//table//tbody//tr[1]//td[4]"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public SearchExistingControlledDestinationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Search for a controlled destination");
        }

        public void ClickSelect()
        {
            btnSelect.Click();
        }

        public string GetSelectedControlledDestinationName()
        {
            return selectedControlledDestinationName.Text.Trim();
        }

        public string GetSelectedControlledDestinationAddress()
        {
            return selectedControlledDestinationAddress.Text.Trim();
        }

        public string GetSelectedControlledDestinationType()
        {
            return selectedControlledDestinationType.Text.Trim();
        }

        public string GetSelectedControlledDestinationApprovalNumber()
        {
            return selectedControlledDestinationApprovalNumber.Text.Trim();
        }
    }
}