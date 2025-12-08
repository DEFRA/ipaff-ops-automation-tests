using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class DecisionPage : IDecisionPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement rdoAcceptabilityInternalMarket => _driver.FindElement(By.Id("acceptability-import"));
        private IWebElement rdoAcceptImportApproved => _driver.FindElement(By.Id("a_accimpapproved"));
        private IWebElement rdoAcceptImportQuarantine => _driver.FindElement(By.Id("a_accimpquarantine"));
        private IWebElement rdoAcceptImportSlaughter => _driver.FindElement(By.Id("a_accimpslaughter"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public DecisionPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Decision");
        }

        public void SelectAcceptableFor(string acceptableFor, string subOption)
        {
            if (acceptableFor.Equals("Internal market", StringComparison.OrdinalIgnoreCase))
            {
                rdoAcceptabilityInternalMarket.Click();

                switch (subOption)
                {
                    case "Approved bodies":
                        rdoAcceptImportApproved.Click();
                        break;
                    case "Quarantine":
                        rdoAcceptImportQuarantine.Click();
                        break;
                    case "Slaughter":
                        rdoAcceptImportSlaughter.Click();
                        break;
                }
            }
        }
    }
}