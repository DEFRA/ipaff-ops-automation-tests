using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class ChooseApprovedEstablishmentSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IChooseApprovedEstablishmentPage? chooseApprovedEstablishmentPage =>
            _objectContainer.IsRegistered<IChooseApprovedEstablishmentPage>()
                ? _objectContainer.Resolve<IChooseApprovedEstablishmentPage>()
                : null;

        public ChooseApprovedEstablishmentSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Choose approved establishment screen should be displayed")]
        public void ThenTheChooseApprovedEstablishmentScreenShouldBeDisplayed()
        {
            Assert.True(
                chooseApprovedEstablishmentPage?.IsPageLoaded(),
                "Choose approved establishment page is not loaded");
        }

        [When("the user selects the country of origin from the notification and clicks Search")]
        public void WhenTheUserSelectsTheCountryOfOriginFromTheNotificationAndClicksSearch()
        {
            var country = _scenarioContext.Get<string>("ApprovedEstablishmentCountry");
            chooseApprovedEstablishmentPage?.SelectCountryAndSearch(country);
        }

        [When("the user types in the name of the approved establishment from the notification")]
        public void WhenTheUserTypesInTheNameOfTheApprovedEstablishment()
        {
            var name = _scenarioContext.Get<string>("ApprovedEstablishmentName");
            chooseApprovedEstablishmentPage?.EnterEstablishmentName(name);
        }

        [Then("the list of approved establishments is displayed for the selected country")]
        public void ThenTheListOfApprovedEstablishmentsIsDisplayedForTheSelectedCountry()
        {
            var country = _scenarioContext.Get<string>("ApprovedEstablishmentCountry");
            Assert.True(
                chooseApprovedEstablishmentPage?.AreAllResultsForCountry(country),
                $"Not all search results have the country '{country}'");
        }

        [When("the user clicks Select for the approved establishment from the notification")]
        public void WhenTheUserClicksSelectForTheApprovedEstablishmentFromTheNotification()
        {
            var name = _scenarioContext.Get<string>("ApprovedEstablishmentName");
            chooseApprovedEstablishmentPage?.SelectEstablishmentByName(name);
        }
    }
}