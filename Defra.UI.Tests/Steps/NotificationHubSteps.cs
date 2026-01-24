using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class NotificationHubSteps
    {
        private readonly IObjectContainer _objectContainer;

        private INotificationHubPage? notificationHubPage => _objectContainer.IsRegistered<INotificationHubPage>() ? _objectContainer.Resolve<INotificationHubPage>() : null;


        public NotificationHubSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }


        [Then("the Notification Hub page should be displayed")]
        public void ThenTheNotificationHubPageShouldBeDisplayed()
        {
            Assert.True(notificationHubPage?.IsPageLoaded(), "Notification Hub page not loaded");
        }

        [Then("the Notification Hub page of a new draft notification should be displayed")]
        public void ThenTheNotificationHubPageOfANewDraftNotificationShouldBeDisplayed()
        {
            Assert.True(notificationHubPage?.IsPageLoaded(), "Notification Hub page not loaded");
            Assert.True(notificationHubPage?.GetRefNumber.Contains("DRAFT.GB"), "The Notification page does not display the newly created draft application");
        }

        [When("the user clicks the Commodity hyperlink")]
        public void WhenTheUserClicksTheCommodityHyperlink()
        {
            notificationHubPage?.ClickCommodityLink();
        }

        [When("the user clicks Contact address for consignment link")]
        public void WhenTheUserClicksContactAddressForConsignmentLink()
        {
            notificationHubPage?.ClickContactAddressForConsignmentLink();
        }

        [When("the user clicks the Countries the consignment will travel through hyperlink")]
        public void WhenTheUserClicksTheCountriesTheConsignmentWillTravelThroughHyperlink()
        {
            notificationHubPage?.ClickCountriesTheConsignmentWillTravelThroughLink();
        }

        [When(@"the user clicks on '(.*)' link")]
        public void WhenTheUserClicksOn(string link) 
        {
            notificationHubPage?.ClickLink(link);
        }

        [When("the user clicks the Transport contacts hyperlink")]
        public void WhenTheUserClicksTheTransportContactsHyperlink()
        {
            notificationHubPage?.ClickLink("Transport contacts");
        }
    }
}