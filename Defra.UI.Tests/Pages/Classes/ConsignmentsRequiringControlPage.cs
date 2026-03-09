using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using System.Globalization;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ConsignmentsRequiringControlPage : IConsignmentsRequiringControlPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//*[@class='govuk-heading-xl govuk-!-margin-bottom-6 govuk-!-font-size-48 ']"), true);
        private IWebElement lnkChedRefNumSearcResult => _driver.FindElement(By.XPath("//*[normalize-space()='Reference Number' or normalize-space()='Reference number']//following-sibling::dd"));
        private IWebElement lnkChedStatusSearcResult => _driver.FindElement(By.XPath("//*[normalize-space()='CHED status']//following-sibling::dd/strong[1]"));
        private IWebElement lnkChedRefNum => _driver.FindElement(By.XPath("//*[normalize-space()='Reference Number']/following-sibling::dd"));
        private IWebElement lblControlStatus => _driver.FindElement(By.XPath("//*[contains(@id,'control-status')]"));
        private IWebElement lnkFirstNotification => _driver.FindElement(By.XPath("(//*[contains(@id,'view-details')])[1]"));
        private IReadOnlyCollection<IWebElement> lblControlStatuses => _driver.FindElements(By.XPath("//*[contains(@id,'control-status')]"));
        private IReadOnlyCollection<IWebElement> lblNotificationDate => _driver.FindElements(By.XPath("//*[contains(@id,'decision-date')]"));
        private IWebElement lnkConsignmentControlPage(string link) => _driver.FindElement(By.XPath($"//a[normalize-space()='{link}']"));
        private IWebElement btnSearch => _driver.FindElement(By.CssSelector("#search-notifications"));
        private IWebElement txtStartDay => _driver.FindElement(By.Id("start-date-day"));
        private IWebElement txtEndDay => _driver.FindElement(By.Id("end-date-day"));
        private IWebElement txtStartMonth => _driver.FindElement(By.Id("start-date-month"));
        private IWebElement txtEndMonth => _driver.FindElement(By.Id("end-date-month"));
        private IWebElement txtStartYear => _driver.FindElement(By.Id("start-date-year"));
        private IWebElement txtEndYear => _driver.FindElement(By.Id("end-date-year"));
        private IWebElement drpdownSortBy => _driver.FindElement(By.Id("orderBy"));
        private IWebElement drpdownValue(string field) => _driver.FindElement(By.XPath($"//*[normalize-space(text())='{field}']/following-sibling::select"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ConsignmentsRequiringControlPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Consignments requiring control");
        }

        public bool VerifyNotificationStatus(string chedRef, string status)
        {
            return lnkChedRefNumSearcResult.Text.Trim().Contains(chedRef)
                && lnkChedStatusSearcResult.Text.ToUpper().Trim().Equals(status.ToUpper());
        }

        public void ClickCHEDReferencNum()
        {
            lnkChedRefNum.Click();
        }
        
        public void ClickSearchButton()
        {
            btnSearch.Click();
        }
        
        public void EnterStartDate(string day, string month, string year)
        {
            txtStartDay.SendKeys(day);
            txtStartMonth.SendKeys(month);
            txtStartYear.SendKeys(year);
        }
        
        public void EnterEndDate(string day, string month, string year)
        {
            txtEndDay.SendKeys(day);
            txtEndMonth.SendKeys(month);
            txtEndYear.SendKeys(year);
        }
        
        public bool VerifyControlStatus(string controlStatus)
        {
            return lblControlStatus.Text.Equals(controlStatus);
        }
        
        public bool VerifyLink(string link)
        {
            return lnkConsignmentControlPage(link).Text.Equals(link);
        }

        public void ClickLink(string link)
        {
            lnkConsignmentControlPage(link).Click();
        }
        
        public void ClickFirstNotification()
        {
            lnkFirstNotification.Click();
        }
        
        public bool VerifyDropdownFieldValue(string field, string value)
        {
            return new SelectElement(drpdownValue(field)).SelectedOption.Text.Equals(value);
        }
        
        public void SelectControlStatus(string value, string field)
        {
            var select = new SelectElement(drpdownValue(field));
            select.SelectByText(value);
        }

        public bool VerifyTheControlStatus(string status)
        {
            return lblControlStatuses.Count > 0 && lblControlStatuses.All(e => e.Text.Trim() == status);       
        }
        
        public bool VerifySortByDropdown(string sortBy)
        {
            return new SelectElement(drpdownSortBy).SelectedOption.Text.Equals(sortBy);
        }
        
        public bool VerifyTheResultsInTheDateRange(string startDate, string endDate)
        {
            string inputFormat = "dd/MM/yyyy";
            string uiFormat = "d MMMM yyyy";

            if (!DateTime.TryParseExact(startDate, inputFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out var start))
                throw new ArgumentException($"Invalid start date: {startDate}");

            if (!DateTime.TryParseExact(endDate, inputFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out var end))
                throw new ArgumentException($"Invalid end date: {endDate}");

            foreach (var date in lblNotificationDate)
            {
                var uiDate = (date.Text ?? string.Empty).Trim();

                if (!DateTime.TryParseExact(uiDate, uiFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out var dt))
                    return false; 
                if (dt < start || dt > end)
                    return false; 
            }
            return true;
        }

        public void SwitchToPart1Tab()
        {
            _driver.SwitchTo().Window(_driver.WindowHandles.FirstOrDefault());
        }

        public void SwitchToPart2Tab()
        {
            _driver.SwitchTo().Window(_driver.WindowHandles.LastOrDefault());
        }

        public bool WaitForStatusWithSearch(string expectedStatus)
        {
            int timeoutSeconds = 300;
            int pollIntervalSeconds = 5;

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds)) 
            { 
                PollingInterval = TimeSpan.FromSeconds(pollIntervalSeconds) 
            }; 

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException), 
                typeof(StaleElementReferenceException)); 
            
            return wait.Until(d => 
            {
                d.FindElement(By.CssSelector("#search-notifications")).Click();

                // Always re-find the status element
                var status = d.FindElement(By.XPath("//*[normalize-space()='CHED status']//following-sibling::dd/strong[1]")).Text.Trim();

                // Check match
                if (string.Equals(status, expectedStatus, StringComparison.OrdinalIgnoreCase)) 
                    return true; 
                else
                    return false;  // keep waiting
            });
        }
    }
}