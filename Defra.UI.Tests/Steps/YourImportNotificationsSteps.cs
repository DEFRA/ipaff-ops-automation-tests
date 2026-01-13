using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class YourImportNotificationsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;


        private IYourImportNotificationsPage? importNotificationsPage => _objectContainer.IsRegistered<IYourImportNotificationsPage>() ? _objectContainer.Resolve<IYourImportNotificationsPage>() : null;

        public YourImportNotificationsSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("the user should be logged into Notification page")]        
        [Then("the dashboard page should be displayed")]
        [Then("the user is taken to the Your import notifications page")]
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

        [When("the user closes the newly opened tab")]
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

        [When("the user clicks Amend")]
        public void WhenTheUserClicksAmend()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            importNotificationsPage?.ClickAmend(chedReference);
        }

        [When("user searches for the '(.*)' import notification")]
        public void WhenUserSearchesForTheImportNotification(string reference)
        {
            _scenarioContext.Add("CHEDReference", reference);
            importNotificationsPage?.SearchForNotification(reference);
        }


        [When("the user clicks Cookies link from the footer of the page")]
        public void WhenTheUserClicksCookiesLinkFromTheFooterOfThePage()
        {
            importNotificationsPage?.ClickCookiesLink();
        }

        [When("the user clicks Contact link on the footer")]
        public void WhenTheUserClicksContactLinkOnTheFooter()
        {
            importNotificationsPage?.ClickContactLink();
        }

        [When("the user clicks the Address book link on the Your import notifications page")]
        public void WhenTheUserClicksTheAddressBookLinkOnTheYourImportNotificationsPage()
        {
            importNotificationsPage?.ClickAddressBookLink();
        }

        [Then("the Search notifications by section displays all the fields on the Your import notifications page")]
        public void ThenTheSearchNotificationsBySectionDisplaysAllTheFieldsOnTheYourImportNotificationsPage()
        {
            Assert.Multiple(() =>
            {
                Assert.True(importNotificationsPage?.IsSearchNotiByPanelDisplayed, "Search notifications by panel is not displayed on the Your import notifications page");
                Assert.True(importNotificationsPage?.AreAllSearchFieldsDisplayed(), "Not all search fields are displayed under Search notifications by panel");
            });
        }
    }
}