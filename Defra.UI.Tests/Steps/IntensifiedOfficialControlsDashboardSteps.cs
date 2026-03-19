using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class IntensifiedOfficialControlsDashboardSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IIntensifiedOfficialControlsDashboardPage? intensifiedOfficialControlsDashboardPage =>
            _objectContainer.IsRegistered<IIntensifiedOfficialControlsDashboardPage>()
                ? _objectContainer.Resolve<IIntensifiedOfficialControlsDashboardPage>()
                : null;

        private IIOCDetailsPage? iocDetailsPage =>
            _objectContainer.IsRegistered<IIOCDetailsPage>()
                ? _objectContainer.Resolve<IIOCDetailsPage>()
                : null;

        private IAreYouSureYouWantToStopTheIOCPage? areYouSureYouWantToStopTheIOCPage =>
            _objectContainer.IsRegistered<IAreYouSureYouWantToStopTheIOCPage>()
                ? _objectContainer.Resolve<IAreYouSureYouWantToStopTheIOCPage>()
                : null;

        public IntensifiedOfficialControlsDashboardSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Intensified Official Controls dashboard should be displayed")]
        public void ThenTheIntensifiedOfficialControlsDashboardShouldBeDisplayed()
        {
            Assert.True(
                intensifiedOfficialControlsDashboardPage?.IsPageLoaded(),
                "Intensified Official Controls dashboard page is not loaded");
        }

        [When("the user clicks Create new intensified official control button")]
        public void WhenTheUserClicksCreateNewIntensifiedOfficialControlButton()
        {
            intensifiedOfficialControlsDashboardPage?.ClickCreateNewIntensifiedControlCheck();
        }

        [When("the user locates the intensified official control just created")]
        public void WhenTheUserLocatesTheIntensifiedOfficialControlJustCreated()
        {
            var iocNumber = _scenarioContext.Get<string>("IntensifiedOfficialControlNumber");
            var status = intensifiedOfficialControlsDashboardPage?.GetStatusForIOCNumber(iocNumber);
            _scenarioContext["LocatedIOCStatus"] = status;
        }

        [Then("the status of the intensified official control should be {string}")]
        public void ThenTheStatusOfTheIntensifiedOfficialControlShouldBe(string expectedStatus)
        {
            var actualStatus = _scenarioContext.Get<string>("LocatedIOCStatus");
            Assert.True(
                actualStatus.Equals(expectedStatus, StringComparison.OrdinalIgnoreCase),
                $"Expected IOC status '{expectedStatus}' but found '{actualStatus}'");
        }

        [When("the user logs out of IPAFFS IOC")]
        public void WhenTheUserLogsOutOfIPAFFSIOC()
        {
            intensifiedOfficialControlsDashboardPage?.ClickSignOut();
        }

        [When("I filter Intensified offical controls by Status {string} and Commodity {string}")]
        public void WhenIFilterIntensifiedOfficalControlsByStatusAndCommodity(string status, string commodity)
        {
            intensifiedOfficialControlsDashboardPage?.FilterByStatusAndCommodity(status, commodity);
        }

        [Then("I stop any existing IOC")]
        public void ThenIStopAnyExistingIOC()
        {
            if (intensifiedOfficialControlsDashboardPage?.HasSearchResults() != true)
            {
                return;
            }

            intensifiedOfficialControlsDashboardPage.ClickFirstResult();
            iocDetailsPage?.ClickStopControl();
            areYouSureYouWantToStopTheIOCPage?.ClickYesStopTheIntensifiedOfficialControl();
        }
    }
}