using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ChooseHazardPage : IChooseHazardPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[contains(@class,'govuk-heading-xl')]"), true);
        private IWebElement ddlHazardCategory => _driver.FindElement(By.Id("hazard-category"));
        private IWebElement ddlHazardSubcategory => _driver.FindElement(By.Id("hazard-subcategory"));
        private IWebElement btnSearch => _driver.FindElement(By.Id("search"));
        private IReadOnlyCollection<IWebElement> lstResultRows => _driver.FindElements(By.XPath("//*[@id='hazards-table']//tbody/tr"));
        private IWebElement LabTestNameCellInRow(IWebElement row) => row.FindElement(By.XPath(".//td[@class='govuk-table__cell'][1]"));
        private IWebElement SubcategoryCellInRow(IWebElement row) => row.FindElement(By.XPath(".//td[@class='govuk-table__cell'][2]"));
        private IWebElement SelectButtonInRow(IWebElement row) => row.FindElement(By.XPath(".//td[@class='govuk-table__cell'][3]/button"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ChooseHazardPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        #region Methods

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Contains("Choose a hazard");
        }

        public void SelectHazardCategory(string category)
        {
            new SelectElement(ddlHazardCategory).SelectByText(category);
        }

        public void SelectHazardSubcategory(string subcategory)
        {
            new SelectElement(ddlHazardSubcategory).SelectByText(subcategory);
        }

        public void ClickSearch()
        {
            btnSearch.Click();
        }

        public bool AreAllResultsForSubcategory(string subcategory)
        {
            return lstResultRows.All(r =>
                SubcategoryCellInRow(r).Text.Trim().Equals(subcategory, StringComparison.OrdinalIgnoreCase));
        }

        public void SelectHazardByLabTestName(string labTestName)
        {
            var row = lstResultRows.FirstOrDefault(r =>
                LabTestNameCellInRow(r).Text.Trim().Equals(labTestName, StringComparison.OrdinalIgnoreCase));

            SelectButtonInRow(row!).Click();
        }

        #endregion
    }
}