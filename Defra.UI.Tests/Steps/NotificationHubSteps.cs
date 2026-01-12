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

        [When("the user clicks the Commodity hyperlink")]
        public void WhenTheUserClicksTheCommodityHyperlink()
        {
            notificationHubPage?.ClickCommodityLink();
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