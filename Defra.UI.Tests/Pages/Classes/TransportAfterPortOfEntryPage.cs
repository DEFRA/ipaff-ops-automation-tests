using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class TransportAfterPortOfEntryPage : ITransportAfterPortOfEntryPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement ddlMeansOfTransportAfter => _driver.WaitForElement(By.Id("transport-means-after"));
        private IWebElement txtIdentification => _driver.WaitForElement(By.Id("identification"));
        private IWebElement txtDocument => _driver.WaitForElement(By.Id("document"));
        private IWebElement txtDepartureDateDay => _driver.WaitForElement(By.Id("departure-date-day"));
        private IWebElement txtDepartureDateMonth => _driver.WaitForElement(By.Id("departure-date-month"));
        private IWebElement txtDepartureDateYear => _driver.WaitForElement(By.Id("departure-date-year"));
        private IWebElement txtDepartureTimeHour => _driver.WaitForElement(By.Id("departure-time-hour"));
        private IWebElement txtDepartureTimeMinutes => _driver.WaitForElement(By.Id("departure-time-minutes"));

        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public TransportAfterPortOfEntryPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Transport")
                && primaryTitle.Text.Contains("Transport after the BCP or Port of entry");
        }

        public void SelectMeansOfTransportAfterBCP(string mode)
        {
            new SelectElement(ddlMeansOfTransportAfter).SelectByText(mode);
        }

        public void EnterTransportIdentificationAfterBCP(string transportId)
        {
            txtIdentification.Click();
            txtIdentification.Clear();
            txtIdentification.SendKeys(transportId);
        }

        public void EnterTransportDocumentReferenceAfterBCP(string documentRef)
        {
            txtDocument.Click();
            txtDocument.Clear();
            txtDocument.SendKeys(documentRef);
        }

        public void EnterDepartureDateFromBCP(DateTime departureDate)
        {
            txtDepartureDateDay.Click();
            txtDepartureDateDay.SendKeys(departureDate.Day.ToString());
            txtDepartureDateMonth.Click();
            txtDepartureDateMonth.SendKeys(departureDate.Month.ToString());
            txtDepartureDateYear.Click();
            txtDepartureDateYear.SendKeys(departureDate.Year.ToString());
        }

        public void EnterDepartureTimeFromBCP(string time)
        {
            var timeParts = time.Split(':');
            var hour = timeParts[0];
            var minutes = timeParts[1];

            txtDepartureTimeHour.Clear();
            txtDepartureTimeHour.SendKeys(hour);
            txtDepartureTimeMinutes.Clear();
            txtDepartureTimeMinutes.SendKeys(minutes);
        }

        public string GetMeansOfTransportAfterBCP => new SelectElement(ddlMeansOfTransportAfter).AllSelectedOptions.FirstOrDefault()?.Text?.Trim() ?? string.Empty;

        public string GetTransportIdentificationAfterBCP => txtIdentification.GetAttribute("value")?.Trim() ?? string.Empty;

        public string GetTransportDocumentReferenceAfterBCP => txtDocument.GetAttribute("value")?.Trim() ?? string.Empty;

        public string GetDepartureDateFromBCP => $"{txtDepartureDateDay.GetAttribute("value")} {txtDepartureDateMonth.GetAttribute("value")} {txtDepartureDateYear.GetAttribute("value")}".Trim();

        public string GetDepartureTimeFromBCP => $"{txtDepartureTimeHour.GetAttribute("value")}:{txtDepartureTimeMinutes.GetAttribute("value")}".Trim();
    }
}