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
    public class NotificationHubPage : INotificationHubPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement lnkCountriesConsignmentTravel => _driver.WaitForElement(By.XPath("//a[contains(text(), 'Countries the consignment will travel through')]"));
        private IWebElement lnkCommodity => _driver.WaitForElement(By.Id("commodity-details-link"), true);
        private IWebElement lnkNotificationHub(string link) => _driver.WaitForElement(By.XPath($"//a[normalize-space(text())='{link}']"), true);

        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public NotificationHubPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Trim().Contains("Notification Hub");
        }

        public void ClickCommodityLink()
        {
            lnkCommodity.Click();
        }

        public void ClickCountriesTheConsignmentWillTravelThroughLink()
        {
            lnkCountriesConsignmentTravel.Click();
        }

        public void ClickLink(string link)
        {
            lnkNotificationHub(link).Click();
        }
    }
}