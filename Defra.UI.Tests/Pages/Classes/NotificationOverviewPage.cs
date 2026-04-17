using AventStack.ExtentReports.Gherkin.Model;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;

namespace Defra.UI.Tests.Pages.Classes
{
    public class NotificationOverviewPage : INotificationOverviewPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//h1[@class='govuk-heading-xl govuk-!-margin-bottom-0']"), true);
        private IWebElement txtStatus => _driver.FindElement(By.XPath("//*[@id='Status-Label' or @id='status-0']"));
        private IWebElement chedReference => _driver.FindElement(By.Id("notification-reference-number"));
        private IWebElement customsDeclarationReference => _driver.FindElement(By.Id("reference-number-customs"));
        private IWebElement customsDocumentCode => _driver.FindElement(By.Id("reference-number-document"));
        private IWebElement lnkCommodityChange => _driver.FindElement(By.Id("commodity-change-link-1"));
        private IWebElement txtTotalNetWeight => _driver.FindElement(By.XPath("//*[@id='review-table-commodities-total']//tr[1]/td[2]"));
        private IWebElement txtTotalGrossWeight => _driver.FindElement(By.XPath("//*[@id='review-table-commodities-total']//tr[3]/td[2]"));
        private IWebElement btnSetToInProgress => _driver.FindElement(By.Id("button-save-and-continue"));
        private IWebElement btnRecordChecks => _driver.FindElement(By.Id("enter-local-reference-number-button"));
        private IWebElement btnRequestAmendment => _driver.WaitForElement(By.Id("amend"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public NotificationOverviewPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Trim().Contains("Notification overview");
        }

        public bool VerifyStatus(string status)
        {
            return txtStatus.Text.Trim().Equals(status , StringComparison.OrdinalIgnoreCase);
        }

        public string GetCHEDReference()
        {
            Console.WriteLine("CHED Reference: " + chedReference.Text.Trim());
            return chedReference.Text.Trim();
        }

        public string GetCustomsDeclarationReference()
        {
            Console.WriteLine("customsDeclarationReference: " + customsDeclarationReference.Text.Trim());
            return customsDeclarationReference.Text.Trim();
        }

        public string GetCustomsDocumentCode()
        {
            Console.WriteLine("customsDocumentCode: " + customsDocumentCode.Text.Trim());
            return customsDocumentCode.Text.Trim();
        }

        public void ClickChangeInCommoditySection()
        {
            lnkCommodityChange.Click();
        }

        public bool VerifyTotalNetWeight(string netWeight)
        {
            return txtTotalNetWeight.Text.Trim().Equals(netWeight);
        }

        public bool VerifyTotalGrossWeight(string grossWeight)
        {
            return txtTotalGrossWeight.Text.Trim().Equals(grossWeight);
        }

        public void ClickSetToInProgressButton()
        {
            btnSetToInProgress.Click();
        }

        public void ClickRecordChecksButton()
        {
            btnRecordChecks.Click();
        }

        public void ClickRequestAmendment()
        {
            btnRequestAmendment.Click();
        }

        public bool StatusContains(string status)
        {
            return txtStatus.Text.Trim().Contains(status, StringComparison.OrdinalIgnoreCase);
        }
    }
}