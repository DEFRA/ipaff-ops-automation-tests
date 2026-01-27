using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;

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
        [Then("the Your notifications page is displayed")]
        [Then("the user is taken back to the dashboard page")]
        public void ThenTheDashboardShouldBeDisplayed()
        {
            Assert.True(importNotificationsPage?.IsPageLoaded(), "Dashboard not displayed");
        }

        [When("the user clicks Create a new notification")]
        public void WhenTheUserClicksCreateANewNotification()
        {
            importNotificationsPage?.ClickCreateNotification();
        }

        [When("the user searches for the import notification")]
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
        [When("the user clicks the Show notification link")]
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
            _scenarioContext.AddOrUpdate("CHEDReference", reference);
            importNotificationsPage?.SearchForNotification(reference);
        }


        [When("the user clicks Cookies link from the footer of the page")]
        public void WhenTheUserClicksCookiesLinkFromTheFooterOfThePage()
        {
            importNotificationsPage?.ClickCookiesLink();
        }

        [Then("the notification returned in the search has the status {string}")]
        public void ThenTheNotificationReturnedInTheSearchHasTheStatus(string expectedStatus)
        {
            var actualStatus = importNotificationsPage?.GetNotificationStatus();
            Assert.AreEqual(expectedStatus, actualStatus, $"Expected status '{expectedStatus}' but found '{actualStatus}'");
        }

        [Then("the Amend link should be available for the notification")]
        public void ThenTheAmendLinkShouldBeAvailableForTheNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(importNotificationsPage?.IsAmendLinkPresent(chedReference), "Amend link is not present");
        }

        [Then("the Amend link should not be available for the notification")]
        public void ThenTheAmendLinkShouldNotBeAvailableForTheNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(importNotificationsPage?.IsAmendLinkNotPresent(chedReference), "Amend link should not be present but was found");
        }

        [Then("the Copy as new link should be available for the notification")]
        public void ThenTheCopyAsNewLinkShouldBeAvailableForTheNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(importNotificationsPage?.IsCopyAsNewLinkPresent(chedReference), "Copy as new link is not present");
        }

        [Then("the View details link should be available for the notification")]
        public void ThenTheViewDetailsLinkShouldBeAvailableForTheNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(importNotificationsPage?.IsViewDetailsLinkPresent(chedReference), "View details link is not present");
        }

        [Then("the Show notification link should be available for the notification")]
        public void ThenTheShowNotificationLinkShouldBeAvailableForTheNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(importNotificationsPage?.IsShowNotificationLinkPresent(chedReference), "Show notification link is not present");
        }

        [When("the user clicks View details for the notification")]
        public void WhenTheUserClicksViewDetailsForTheNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            importNotificationsPage?.ClickViewDetails(chedReference);
        }

        [When(@"the user clicks Address book link")]
        [When("the user clicks the Address book link on the Your import notifications page")]
        public void WhenTheUserClicksAddressBookLink()
        {
            importNotificationsPage?.ClickAddressBookLink();
        }

        [When("the user clicks Contact link on the footer")]
        public void WhenTheUserClicksContactLinkOnTheFooter()
        {
            importNotificationsPage?.ClickContactLink();
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

        [When("the user clicks the View details link")]
        public void WhenTheUserClicksTheViewDetailsLink()
        {
            importNotificationsPage?.ClickViewDetailsLink();
        }

        [Then("the user searches for the Draft CHED reference on the dashboard")]
        public void ThenTheUserSearchesForTheDraftCHEDReferenceOnTheDashboard()
        {
            var draftCHEDReference = _scenarioContext.Get<string>("DraftCHEDReference");
            importNotificationsPage?.SearchForNotification(draftCHEDReference);
        }

        [Then("the draft notification should be present in the list")]
        public void ThenTheDraftNotificationShouldBePresentInTheList()
        {
            var draftCHEDReference = _scenarioContext.Get<string>("DraftCHEDReference");
            Assert.True(importNotificationsPage?.VerifyNotificationInList(draftCHEDReference), "Draft notification not found in the list");
        }

        [When("the user clicks the Amend link")]
        public void WhenTheUserClicksTheAmendLink()
        {
            var draftCHEDReference = _scenarioContext.Get<string>("DraftCHEDReference");
            importNotificationsPage?.ClickAmend(draftCHEDReference);
        }

        [When("the user clicks the Copy as new link for the notification")]
        public void WhenTheUserClicksTheCopyAsNewLinkForTheNotification()
        {
            importNotificationsPage?.ClickCopyAsNewLink();
        }
    }
}