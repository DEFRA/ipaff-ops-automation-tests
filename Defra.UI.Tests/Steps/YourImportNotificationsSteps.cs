using Reqnroll.BoDi;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class YourImportNotificationsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;


        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private IYourImportNotificationsPage? importNotificationsPage => _objectContainer.IsRegistered<IYourImportNotificationsPage>() ? _objectContainer.Resolve<IYourImportNotificationsPage>() : null;

        public YourImportNotificationsSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("the user should be logged into Notification page")]        
        [Then("the dashboard page should be displayed")]
        public void ThenTheDashboardShouldBeDisplayed()
        {
            Assert.True(importNotificationsPage?.IsPageLoaded(), "Dashboard not displayed");
        }

        [When("the user clicks Create a new notification")]
        public void WhenTheUserClicksCreateANewNotification()
        {
            importNotificationsPage?.ClickCreateNotification();
        }

        [When("user searches for the import notification")]
        public void WhenUserSearchesForTheImportNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            importNotificationsPage?.SearchForNotification(chedReference);
        }

        [Then("the notification should be present in the list")]
        public void ThenTheNotificationShouldBePresentInTheList()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(importNotificationsPage?.VerifyNotificationInList(chedReference), "Notification not found in list");
        }

        [When("the user clicks Show notification")]
        public void WhenTheUserClicksShowNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            importNotificationsPage?.ClickShowNotification(chedReference);
        }

        [Then("the certificate should be displayed in a new browser tab")]
        public void ThenTheCertificateShouldBeDisplayedInANewBrowserTab()
        {
            Assert.True(importNotificationsPage?.VerifyCertificateInNewTab(), "Certificate not displayed in new browser tab");
        }

        [When("the user checks that the data in the certificate matches the data entered into the notification")]
        public void WhenTheUserChecksThatTheDataInTheCertificateMatchesTheDataEnteredIntoTheNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(importNotificationsPage?.VerifyDataInCertificate(chedReference), "Certificate data verification failed");
        }

        [When("the user closes the PDF browser tab")]
        public void WhenTheUserClosesThePDFBrowserTab()
        {
            importNotificationsPage?.ClosePDFBrowserTab();
        }

        [Then("the browser tab is closed")]
        public void ThenTheBrowserTabIsClosed()
        {
            Assert.True(importNotificationsPage?.VerifyBrowserTabClosed(), "PDF browser tab not closed properly");
        }
    }
}