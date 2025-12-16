using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class PortOfEntryPage : IPortOfEntryPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement txtPortOfEntry => _driver.WaitForElement(By.Id("bcp"));
        private IWebElement optTransportMode => _driver.WaitForElement(By.XPath("//*[@class='govuk-form-group  ']/select"));
        private IWebElement txtTransportId => _driver.WaitForElement(By.Id("identification"));
        private IWebElement txtTransportDocRef => _driver.WaitForElement(By.Id("document"));
        private IWebElement txtDay => _driver.WaitForElement(By.Id("arrival-date-day"));
        private IWebElement txtMonth => _driver.WaitForElement(By.Id("arrival-date-month"));
        private IWebElement txtYear => _driver.WaitForElement(By.Id("arrival-date-year"));
        private IWebElement txtHour => _driver.WaitForElement(By.Id("arrival-time-hour"));
        private IWebElement txtMinutes => _driver.WaitForElement(By.Id("arrival-time-minutes"));
        private IWebElement txtEstimatedJourneyTimeHour => _driver.FindElement(By.Id("estimated-journey-time-hour"));
        private IWebElement txtContainerNumber => _driver.FindElement(By.Id("container-number-1"));
        private IWebElement txtSealNumber => _driver.FindElement(By.Id("seal-number-1"));
        private IWebElement chkOfficalSeal => _driver.FindElement(By.Id("official-seal-1"));
        private IWebElement rdoAreConsignmentInContainer => _driver.FindElement(By.Id("are-consignments-in-containers-yes"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public PortOfEntryPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Transport")
                && primaryTitle.Text.Contains("Transport to the port of entry");
        }

        public bool IsBCPOrPortOfEntryPageLoaded()
        {
            return secondaryTitle.Text.Contains("Transport")
                && primaryTitle.Text.Contains("Transport to the BCP or Port of entry");
        }

        public void EnterPortOfEntry(string port)
        {
            txtPortOfEntry.Click();
            txtPortOfEntry.Clear();
            txtPortOfEntry.Click();
            txtPortOfEntry.SendKeys(port);
        }

        public void SelectMeansOfTransport(string mode)
        {
            new SelectElement(optTransportMode).SelectByText(mode);
        }

        public void EnterTransportId(string transId) 
        { 
            txtTransportId.Clear(); 
            txtTransportId.SendKeys(transId); 
        }
        
        public void EnterTransportDocRef(string DocumentRef) 
        {
            txtTransportDocRef.Clear();
            txtTransportDocRef.SendKeys(DocumentRef); 
        }

        public void EnterEstimatedArrivalDate(string day, string month, string year)
        {
            txtDay.SendKeys(day);
            txtMonth.SendKeys(month);
            txtYear.SendKeys(year);
        }

        public void EnterEstimatedArrivalTime(string hour, string minutes)
        {
            txtHour.Clear();
            txtHour.SendKeys(hour);
            txtMinutes.Clear();
            txtMinutes.SendKeys(minutes);
        }

        public void EnterEstimatedJourneyTime(string hours)
        {
            try
            {
                txtEstimatedJourneyTimeHour.SendKeys(hours);
            }
            catch (NoSuchElementException)
            {
                // Field not present (CHED-P), continue without error
            }
        }

        public void EnterContainerDetails(string containerNumber, string sealNumber)
        {
            txtContainerNumber.SendKeys(containerNumber);
            txtSealNumber.SendKeys(sealNumber);
            chkOfficalSeal.Click();
        }

        public void SelectAreConsignmentsInContainer(string option)
        {
            rdoAreConsignmentInContainer.Click();
        }
    }
}