using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class CreateIntensifiedOfficialControlSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICreateIntensifiedOfficialControlPage? createIntensifiedOfficialControlPage =>
            _objectContainer.IsRegistered<ICreateIntensifiedOfficialControlPage>()
                ? _objectContainer.Resolve<ICreateIntensifiedOfficialControlPage>()
                : null;

        public CreateIntensifiedOfficialControlSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Create intensified official control screen should be displayed")]
        public void ThenTheCreateIntensifiedOfficialControlScreenShouldBeDisplayed()
        {
            Assert.True(
                createIntensifiedOfficialControlPage?.IsPageLoaded(),
                "Create intensified official control page is not loaded");
        }

        [When("the user enters the Certificate number from the notification")]
        public void WhenTheUserEntersTheCertificateNumberFromTheNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            createIntensifiedOfficialControlPage?.EnterCertificateNumber(chedReference);
        }

        [When("the user clicks the Search for establishment link")]
        public void WhenTheUserClicksTheSearchForEstablishmentLink()
        {
            createIntensifiedOfficialControlPage?.ClickSearchForEstablishment();
        }

        [Then("the Establishment is populated with the details of the approved establishment from the notification")]
        public void ThenTheEstablishmentIsPopulatedWithTheDetailsOfTheApprovedEstablishmentFromTheNotification()
        {
            var name = _scenarioContext.Get<string>("ApprovedEstablishmentName");
            var country = _scenarioContext.Get<string>("ApprovedEstablishmentCountry");
            var approvalNumber = _scenarioContext.Get<string>("ApprovedEstablishmentApprovalNum");

            Assert.True(
                createIntensifiedOfficialControlPage?.IsEstablishmentPopulated(name, country, approvalNumber),
                $"Establishment table is not populated with the expected name '{name}', country '{country}' and approval number '{approvalNumber}'");
        }

        [When("the user enters the commodity code from the notification")]
        public void WhenTheUserEntersTheCommodityCodeFromTheNotification()
        {
            var commodityCodes = _scenarioContext.Get<List<string>>("CommodityCode");
            createIntensifiedOfficialControlPage?.EnterCommodityCode(commodityCodes.First());
        }

        [When("the user clicks the Search commodities link")]
        public void WhenTheUserClicksTheSearchCommoditiesLink()
        {
            createIntensifiedOfficialControlPage?.ClickSearchCommodities();
        }
    }
}