using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Collections.ObjectModel;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Faker;

namespace Defra.UI.Tests.Pages.Classes
{
    public class TransportContactsPage : ITransportContactsPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement rdoTransportContactYes => _driver.FindElement(By.XPath("//*[@id='transport-contact-yesno-question-yes']/following-sibling::label"));
        private IWebElement rdoTransportContactNo => _driver.FindElement(By.XPath("//*[@id='transport-contact-yesno-question-no']/following-sibling::label"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public TransportContactsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Transport")
                && primaryTitle.Text.Contains("Should we notify any transport contacts about inspections?");
        }

        public void SelectTransportContactNotification(string option)
        {
            if (option.Equals("Yes") || rdoTransportContactYes.Text.Contains(option))
                rdoTransportContactYes.Click();
            else if (option.Equals("No") || rdoTransportContactNo.Text.Contains(option))
                rdoTransportContactNo.Click();
        }

        public bool IsTransportContactNotificationNotSelected()
        {
            // Get the radio button input elements
            var rdoYesInput = rdoTransportContactYes;
            var rdoNoInput = rdoTransportContactNo;

            // Check if neither radio button is selected
            var isYesSelected = rdoYesInput.Selected;
            var isNoSelected = rdoNoInput.Selected;

            // Return true if neither is selected (i.e., not copied from original)
            return !isYesSelected && !isNoSelected;
        }
    }
}