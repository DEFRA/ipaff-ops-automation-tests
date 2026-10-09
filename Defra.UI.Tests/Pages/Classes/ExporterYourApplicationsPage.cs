using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterYourApplicationsPage : IExporterYourApplicationsPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Your applications']"), true);
        private IWebElement btnStartNewApplication => _driver.WaitForElement(By.Id("link-startNewApplicationLink"));
        private IWebElement txtSearch => _driver.WaitForElement(By.Id("filter"), true);
        private IWebElement btnSearch => _driver.WaitForElement(By.Id("Button-Search"));
        private IReadOnlyCollection<IWebElement> tblApplicationRows => _driver.WaitForElements(By.XPath("//table[@id='applications-table']//tbody/tr"));
        private IWebElement lnkAphaReference(string reference) => _driver.WaitForElement(By.Id($"{reference}-summary-link"), true);
        #endregion

        public ExporterYourApplicationsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Your applications");

        public void ClickStartNewApplicationButton() => btnStartNewApplication.Click();

        public void SearchForApplication(string reference)
        {
            txtSearch.Clear();
            txtSearch.SendKeys(reference);
            btnSearch.Click();
        }

        public int GetApplicationRowCount() => tblApplicationRows.Count;

        public void ClickAphaReferenceLink(string reference) => lnkAphaReference(reference).Click();
    }
}