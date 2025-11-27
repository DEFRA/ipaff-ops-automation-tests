using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Contracts;
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
    public class ReviewYourNotificationPage : IReviewYourNotificationPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//*[@id='page-primary-title']"), true);
        private IReadOnlyCollection<IWebElement> divAboutTheConsignmentDetails => _driver.WaitForElements(By.XPath("//div[@id='document-pet-card']//dl/div"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ReviewYourNotificationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Contains("Review your notification");
        }
    }
}