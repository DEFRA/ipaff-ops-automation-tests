using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CsvFileDetailsAndStatusPage : ICsvFileDetailsAndStatusPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        private IWebElement pageHeading => _driver.WaitForElement(By.XPath("//h1[normalize-space()='CSV file details and status']"), true);
        private IWebElement summaryList => _driver.WaitForElement(By.Id("commodity-information"));
        private IWebElement phsiReportingLink => _driver.WaitForElement(By.XPath("//a[normalize-space()='PHSI reporting']"));

        public CsvFileDetailsAndStatusPage(IObjectContainer container) => _objectContainer = container;

        public bool IsPageLoaded() => pageHeading.Displayed;

        public IDictionary<string, string> GetSummaryDetails()
        {
            var rows = summaryList.FindElements(By.XPath(".//div[contains(@class,'govuk-summary-list__row')]"));
            return rows.ToDictionary(
                r => r.FindElement(By.XPath("./dt")).Text.Trim(),
                r => r.FindElement(By.XPath("./dd")).Text.Trim());
        }

        public int GetSummaryFieldAsInt(string field)
        {
            var details = GetSummaryDetails();
            if (!details.TryGetValue(field, out var raw))
                throw new NoSuchElementException($"Summary field '{field}' not found on CSV file details and status page.");

            return int.Parse(raw.Replace(",", string.Empty), CultureInfo.InvariantCulture);
        }

        public void ClickPhsiReportingLink() => phsiReportingLink.Click();
    }
}