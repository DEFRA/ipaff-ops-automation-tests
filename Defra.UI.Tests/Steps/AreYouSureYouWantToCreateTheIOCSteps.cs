using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class AreYouSureYouWantToCreateTheIOCSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IAreYouSureYouWantToCreateTheIOCPage? areYouSureYouWantToCreateTheIOCPage =>
            _objectContainer.IsRegistered<IAreYouSureYouWantToCreateTheIOCPage>()
                ? _objectContainer.Resolve<IAreYouSureYouWantToCreateTheIOCPage>()
                : null;

        public AreYouSureYouWantToCreateTheIOCSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Are you sure you want to create the intensified official control? page is displayed")]
        public void ThenTheAreYouSureYouWantToCreateTheIntensifiedOfficialControlPageIsDisplayed()
        {
            Assert.True(
                areYouSureYouWantToCreateTheIOCPage?.IsPageLoaded(),
                "Are you sure you want to create the intensified official control? page is not loaded");
        }

        [Then("the 'Yes, create the intensified official control' button is displayed")]
        public void ThenTheYesCreateTheIntensifiedOfficialControlButtonIsDisplayed()
        {
            Assert.True(
                areYouSureYouWantToCreateTheIOCPage?.IsYesCreateButtonDisplayed(),
                "Yes, create the intensified official control button is not displayed");
        }

        [Then("the 'No, don't create the intensified official control' link is displayed")]
        public void ThenTheNoDontCreateTheIntensifiedOfficialControlLinkIsDisplayed()
        {
            Assert.True(
                areYouSureYouWantToCreateTheIOCPage?.IsNoDontCreateLinkDisplayed(),
                "No, don't create the intensified official control link is not displayed");
        }

        [When("the user clicks the 'Yes, create the intensified official control' button")]
        public void WhenTheUserClicksTheYesCreateTheIntensifiedOfficialControlButton()
        {
            areYouSureYouWantToCreateTheIOCPage?.ClickYesCreateIntensifiedOfficialControl();
        }
    }
}