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
        private IWebElement txtPortOfEntry => _driver.FindElement(By.Id("bcp"));
        private IWebElement optTransportMode => _driver.FindElement(By.XPath("//*[@class='govuk-form-group  ']/select"));
        private IWebElement txtTransportId => _driver.FindElement(By.Id("identification"));
        private IWebElement txtTransportDocRef => _driver.FindElement(By.Id("document"));
        private IWebElement txtDay => _driver.FindElement(By.Id("arrival-date-day"));
        private IWebElement txtMonth => _driver.FindElement(By.Id("arrival-date-month"));
        private IWebElement txtYear => _driver.FindElement(By.Id("arrival-date-year"));
        private IWebElement txtHour => _driver.FindElement(By.Id("arrival-time-hour"));
        private IWebElement txtMinutes => _driver.FindElement(By.Id("arrival-time-minutes"));
        private IWebElement txtEstimatedJourneyTimeHour => _driver.FindElement(By.Id("estimated-journey-time-hour"));
        private IWebElement txtContainerNumber => _driver.FindElement(By.Id("container-number-1"));
        private IWebElement txtSealNumber => _driver.FindElement(By.Id("seal-number-1"));
        private IWebElement chkOfficalSeal => _driver.FindElement(By.Id("official-seal-1"));
        private IWebElement rdoAreConsignmentsInContainerYes => _driver.FindElement(By.Id("are-consignments-in-containers-yes"));
        private IWebElement rdoAreConsignmentsInContainerNo => _driver.FindElement(By.Id("are-consignments-in-containers-no"));
        private List<IWebElement> verifyPortOfEntry => _driver.WaitForElements(By.XPath("//input[@aria-describedby='bcp__assistiveHint']")).ToList();
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
            txtDay.Clear();
            txtDay.SendKeys(day);
            txtMonth.Clear();
            txtMonth.SendKeys(month);
            txtYear.Clear();
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
                txtEstimatedJourneyTimeHour.Clear();
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

        public void SelectAreTrailersOrContainersUsed(string option)
        {
            if (option.Equals("Yes", StringComparison.OrdinalIgnoreCase))
            {
                rdoAreConsignmentsInContainerYes.Click();
            }
            else
            {
                rdoAreConsignmentsInContainerNo.Click();
            }
        }

        public void EnterContainerNumber(string containerNumber)
        {
            txtContainerNumber.Click();
            txtContainerNumber.Clear();
            txtContainerNumber.SendKeys(containerNumber);
        }

        public void EnterSealNumber(string sealNumber)
        {
            txtSealNumber.Click();
            txtSealNumber.Clear();
            txtSealNumber.SendKeys(sealNumber);
        }

        public void TickOfficialSealCheckbox()
        {
            if (!chkOfficalSeal.Selected)
            {
                chkOfficalSeal.Click();
            }
        }

        public bool VerifyPortOfEntryIfNotAlreadyPopulated()
        {
            return verifyPortOfEntry.Count > 0;
        }

        public string GetPortOfEntry => txtPortOfEntry.GetAttribute("value")?.Trim() ?? string.Empty;

        public string GetMeansOfTransport => new SelectElement(optTransportMode).AllSelectedOptions.FirstOrDefault()?.Text?.Trim() ?? string.Empty;
    }
}