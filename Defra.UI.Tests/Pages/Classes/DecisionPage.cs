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
        private IWebElement rdoTranshipment => _driver.FindElement(By.Id("acceptability-transhipment"));
        private IWebElement rdoChannelled => _driver.FindElement(By.Id("acceptability-channelled"));
        private IWebElement rdoInternalMarket => _driver.FindElement(By.Id("acceptability-internalmarket"));
        private IWebElement rdoSpecificWarehouse => _driver.FindElement(By.Id("acceptability-nonconforming"));
        private IWebElement rdoNotAcceptable => _driver.FindElement(By.Id("acceptability-refused"));
        private IWebElement rdoDestruction => _driver.FindElement(By.Id("notAcceptAction-destruction"));
        private IWebElement rdoReDispatching => _driver.FindElement(By.Id("notAcceptAction-reexport"));
        private IWebElement rdoTransformation => _driver.FindElement(By.Id("notAcceptAction-transformation"));
        private IWebElement rdoOther => _driver.FindElement(By.Id("notAcceptAction-other"));
        private IWebElement txtNotAcceptableDay => _driver.FindElement(By.Id("not-acceptable-day"));
        private IWebElement txtNotAcceptableMonth => _driver.FindElement(By.Id("not-acceptable-month"));
        private IWebElement txtNotAcceptableYear => _driver.FindElement(By.Id("not-acceptable-year"));
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

        public void SelectDecision(string subOption, string decision)
        {
            if (decision.Equals("Transhipment / Onward travel"))
                rdoTranshipment.Click();
            else if (decision.Equals("Transit"))
                rdoTransit.Click();
            else if (decision.Equals("Channelled"))
                rdoChannelled.Click();
            else if (decision.Equals("Internal market"))
                rdoInternalMarket.Click();
            else if (decision.Equals("Specific warehouse procedure"))
                rdoSpecificWarehouse.Click();
            else if (decision.Equals("Not acceptable"))
            {
                rdoNotAcceptable.Click();
                if(subOption.Equals("Destruction"))
                    rdoDestruction.Click();
                else if (subOption.Equals("Re-dispatching"))
                    rdoReDispatching.Click();
                else if (subOption.Equals("Transformation"))
                    rdoTransformation.Click();
                else if (subOption.Equals("Other"))
                    rdoOther.Click();
            }
        }

        public void EnterCurrentDateInDecisionPage(string day, string month, string year)
        {
            txtNotAcceptableDay.Click();
            txtNotAcceptableDay.SendKeys(day);
            txtNotAcceptableMonth.Click();
            txtNotAcceptableMonth.SendKeys(month);
            txtNotAcceptableYear.Click();
            txtNotAcceptableYear.SendKeys(year);
        }
    }
}