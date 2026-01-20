using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class BorderNotificationSubmittedSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IBorderNotificationSubmittedPage? borderNotificationSubmittedPage => _objectContainer.IsRegistered<IBorderNotificationSubmittedPage>() ? _objectContainer.Resolve<IBorderNotificationSubmittedPage>() : null;

        public BorderNotificationSubmittedSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("your border notification has been submitted page should be displayed")]
        public void ThenYourBorderNotificationHasBeenSubmittedPageShouldBeDisplayed()
        {
            Assert.True(borderNotificationSubmittedPage?.IsPageLoaded(), "Your border notification has been submitted page not loaded");
        }

        [When("the user records the BN number")]
        public void WhenTheUserRecordsTheBNNumber()
        {
            _scenarioContext["BNNumber"] = borderNotificationSubmittedPage.GetBNNumber();
        }

        [When("the user clicks Return to dashboard button")]
        public void WhenTheUserClicksReturnToDashboardButton()
        {
            borderNotificationSubmittedPage?.ClickReturnToDashboard();
        }
    }
}