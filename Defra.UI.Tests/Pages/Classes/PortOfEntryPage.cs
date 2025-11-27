using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Collections.ObjectModel;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;

namespace Defra.UI.Tests.Pages.Classes
{
    public class PortOfEntryPage : IPortOfEntryPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-primary-title']"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-secondary-title']"), true);
        private IWebElement btnSelect => _driver.WaitForElement(By.XPath("//*[@id='Table-SearchResults']//button[normalize-space()='Select']"));
        private IWebElement txtPortOfEntry => _driver.WaitForElement(By.Id("bcp"));
        private IWebElement selectPortOfEntry => _driver.WaitForElement(By.Id("bcp-select"));
        private IWebElement optTransportMode => _driver.WaitForElement(By.XPath("//*[@class='govuk-form-group  ']/select"));
        private IWebElement txtTransportId => _driver.WaitForElement(By.Id("identification"));
        private IWebElement txtTransportDocRef => _driver.WaitForElement(By.Id("document"));
        private IWebElement txtDay => _driver.WaitForElement(By.Id("arrival-date-day"));
        private IWebElement txtMonth => _driver.WaitForElement(By.Id("arrival-date-month"));
        private IWebElement txtYear => _driver.WaitForElement(By.Id("arrival-date-year"));
        private IWebElement txtHour => _driver.WaitForElement(By.Id("arrival-time-hour"));
        private IWebElement txtMinutes => _driver.WaitForElement(By.Id("arrival-time-minutes"));
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

        public void EnterPortOfEntry(string port)
        {
            txtPortOfEntry.Click();
            txtPortOfEntry.SendKeys(port);
        }

        public void SelectMeansOfTransport(string mode)
        {
            new SelectElement(optTransportMode).SelectByText(mode);
        }

        public void EnterTransportId(string transId) 
        { 
            txtTransportId.SendKeys(transId); 
        }
        
        public void EnterTransportDocRef(string DocumentRef) 
        { 
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
            txtHour.SendKeys(hour);
            txtMinutes.SendKeys(minutes);
        }
    }
}