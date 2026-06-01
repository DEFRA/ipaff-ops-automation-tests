using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;
using System.Globalization;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CsvFileDetailsAndStatusPage : ICsvFileDetailsAndStatusPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageHeading => _driver.WaitForElement(By.XPath("//h1[normalize-space()='CSV file details and status']"), true);
        private IWebElement summaryList => _driver.WaitForElement(By.Id("commodity-information"));
        private IWebElement phsiReportingLink => _driver.WaitForElement(By.XPath("//a[normalize-space()='PHSI reporting']"));
        private IWebElement euChedPReportingLink => _driver.WaitForElement(By.XPath("//a[normalize-space()='EU CHED-P reporting']"));
        private By summaryRowsBy => By.XPath(".//div[contains(@class,'govuk-summary-list__row')]");
        private By summaryKeyBy => By.XPath("./dt");
        private By summaryValueBy => By.XPath("./dd");
        #endregion

        public CsvFileDetailsAndStatusPage(IObjectContainer container) => _objectContainer = container;

        public bool IsPageLoaded() => pageHeading.Displayed;

        public IDictionary<string, string> GetSummaryDetails()
        {
            var rows = summaryList.FindElements(summaryRowsBy);
            return rows.ToDictionary(
                r => r.FindElement(summaryKeyBy).Text.Trim(),
                r => r.FindElement(summaryValueBy).Text.Trim());
        }

        public int GetSummaryFieldAsInt(string field)
        {
            var details = GetSummaryDetails();
            if (!details.TryGetValue(field, out var raw))
                throw new NoSuchElementException($"Summary field '{field}' not found on CSV file details and status page.");

            return int.Parse(raw.Replace(",", string.Empty), CultureInfo.InvariantCulture);
        }

        public void ClickPhsiReportingLink() => phsiReportingLink.Click();

        public void ClickEuChedPReportingLink() => euChedPReportingLink.Click();
    }
}