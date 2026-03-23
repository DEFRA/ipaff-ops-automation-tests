using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ChooseApprovedEstablishmentPage : IChooseApprovedEstablishmentPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[contains(@class,'govuk-heading-xl')]"), true);
        private IWebElement ddlCountry => _driver.FindElement(By.Id("establishment-country"));
        private IWebElement txtEstablishmentName => _driver.FindElement(By.Id("establishment-name"));
        private IWebElement btnSearch => _driver.FindElement(By.Id("search"));
        private IReadOnlyCollection<IWebElement> lstResultCountries => _driver.FindElements(By.XPath("//*[@id='approved-establishments-table']//td[@headers='establishment-search-result-country']"));
        private IReadOnlyCollection<IWebElement> lstResultRows => _driver.FindElements(By.XPath("//*[@id='approved-establishments-table']//tbody/tr"));
        private IWebElement NameCellInRow(IWebElement row) => row.FindElement(By.XPath(".//td[@headers='establishment-search-result-name']"));
        private IWebElement SelectButtonInRow(IWebElement row) => row.FindElement(By.XPath(".//td[@headers='establishment-search-result-id']/button"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ChooseApprovedEstablishmentPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        #region Methods

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Contains("Choose approved establishment");
        }

        public void SelectCountryAndSearch(string country)
        {
            new SelectElement(ddlCountry).SelectByText(country);
            btnSearch.Click();
        }

        public void EnterEstablishmentName(string name)
        {
            txtEstablishmentName.Clear();
            txtEstablishmentName.SendKeys(name);
        }

        public bool AreAllResultsForCountry(string country)
        {
            return lstResultCountries.All(x => x.Text.Trim().Equals(country, StringComparison.OrdinalIgnoreCase));
        }

        public void SelectEstablishmentByName(string name)
        {
            var row = lstResultRows.FirstOrDefault(r =>
                NameCellInRow(r).Text.Contains(name, StringComparison.OrdinalIgnoreCase));

            SelectButtonInRow(row!).Click();
        }

        #endregion
    }
}