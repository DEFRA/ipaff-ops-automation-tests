using Defra.UI.Tests.Tools;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;
using System.Text.RegularExpressions;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmSelectYourAddressSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IIdmSelectYourAddressPage idmSelectYourAddressPage => _objectContainer.Resolve<IIdmSelectYourAddressPage>();
        private IIdmWhatIsYourBusinessAddressPage idmWhatIsYourBusinessAddressPage => _objectContainer.Resolve<IIdmWhatIsYourBusinessAddressPage>();
        public IdmSelectYourAddressSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the address selection or manual business address page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(idmSelectYourAddressPage.IsDisplayed() || idmWhatIsYourBusinessAddressPage.IsDisplayed(), Is.True, "Expected address selection or manual address entry after postcode lookup");

        [When("the user selects a random business address or enters the generated address manually and continues")]
        public void WhenAddressIsSelected()
        {
            var postcode = _scenarioContext.Get<string>("RegistrationPostcode");
            if (idmSelectYourAddressPage.IsDisplayed())
            {
                _scenarioContext.Set(idmSelectYourAddressPage.SelectAddressAndContinue(postcode), "RegistrationAddress");
                return;
            }

            Assert.That(idmWhatIsYourBusinessAddressPage.IsDisplayed(), Is.True, "Manual entry is only allowed when the lookup displays the business address form");
            var generated = Utils.GenerateOperatorDetails("Importer");
            var address = Regex.Match(generated.AddressLine1, @"^(?<number>\d+\w*)\s+(?<street>.+)$");
            var buildingNumber = address.Success ? address.Groups["number"].Value : Random.Shared.Next(1, 1000).ToString();
            var street = address.Success ? address.Groups["street"].Value : generated.AddressLine1;
            idmWhatIsYourBusinessAddressPage.EnterAddressAndContinue(buildingNumber, street, generated.CityOrTown, postcode);
            _scenarioContext.Set($"{buildingNumber} {street}, {generated.CityOrTown}, {postcode}", "RegistrationAddress");
        }
    }
}
