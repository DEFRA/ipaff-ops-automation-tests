using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;
using System.Linq;
using System.Text.RegularExpressions;

namespace Defra.UI.Tests.Pages.Classes
{
    public class RiskDecisionReportPage : IRiskDecisionReportPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Risk decision report']"), true);
        private IWebElement queryInput => _driver.WaitForElement(By.Id("query"));
        private IWebElement btnSearch => _driver.WaitForElement(By.Id("search"));
        private IWebElement caption => _driver.FindElement(By.XPath("//table//caption"));
        private IWebElement btnExpandForCHED(string chedReference) =>
            _driver.WaitForElement(By.XPath($"//button[starts-with(normalize-space(),'Expand') and contains(.,'{chedReference}')]"));
        private By btnExpandDecisionsBy => By.XPath("//button[@data-action='expand-decision']");
        private IWebElement summaryRequests =>
            _driver.WaitForElement(By.XPath("//details//span[normalize-space()='Requests']/ancestor::summary"));
        private IWebElement summaryDecision =>
            _driver.WaitForElement(By.XPath("//details//span[normalize-space()='Decision']/ancestor::summary"));
        private IWebElement preDecisionJson => _driver.WaitForElement(By.XPath("//pre[contains(@class,'decision')]"));
        #endregion

        public RiskDecisionReportPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Risk decision report");

        public void Search(string chedReference)
        {
            queryInput.Clear();
            queryInput.SendKeys(chedReference);
            btnSearch.Click();
        }

        public int GetRecordCount()
        {
            var match = Regex.Match(caption.Text, @"Records found\s+(\d+)");
            return match.Success ? int.Parse(match.Groups[1].Value) : 0;
        }

        public void ClickExpandForCHED(string chedReference)
        {
            btnExpandForCHED(chedReference).Click();

            var expandDecisions = _driver.FindElements(btnExpandDecisionsBy).FirstOrDefault();
            expandDecisions?.Click();
        }

        public void ClickRequestsDetails() => summaryRequests.Click();

        public void ClickDecisionDetails() => summaryDecision.Click();

        public string GetDecisionJson() => preDecisionJson.Text.Trim();
    }
}