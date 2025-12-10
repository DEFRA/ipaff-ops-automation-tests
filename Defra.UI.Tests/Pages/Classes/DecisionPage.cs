using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class DecisionPage : IDecisionPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement rdoTransit => _driver.FindElement(By.Id("acceptability-transit"));
        private IWebElement drpExitBCP => _driver.FindElement(By.Id("transitExitBipUk"));
        private IWebElement drpTransitedCountry => _driver.FindElement(By.Id("transitCountry"));
        private IWebElement drpDestinationCountry => _driver.FindElement(By.Id("transitDestinationCountry"));
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

        public bool VerifyTransitRadioButtonPrePopulated()
        {
            return rdoTransit.Selected;
        }

        public bool VerifyPrepopulatedTransitDetails(string exitBCP, string transitedCountry, string destinationCountry)
        {
            var parts = exitBCP.Split(' ');
            string updatedexitBCP = parts[0];

            SelectElement exitBCPDropdown = new SelectElement(drpExitBCP);
            var prePopulatedExitBCP = exitBCPDropdown.SelectedOption.Text.Trim();

            SelectElement transitedCountryDropdown = new SelectElement(drpTransitedCountry);
            var prePopulatedTransitedCountry = transitedCountryDropdown.SelectedOption.Text.Trim();

            SelectElement destinationCountryDropdown = new SelectElement(drpDestinationCountry);
            var prePopulatedDestinationCountry = destinationCountryDropdown.SelectedOption.Text.Trim();

            return prePopulatedTransitedCountry.Equals(transitedCountry)
                && prePopulatedDestinationCountry.Equals(destinationCountry)
                && prePopulatedExitBCP.ToUpper().Contains(updatedexitBCP);             
        }
    }
}