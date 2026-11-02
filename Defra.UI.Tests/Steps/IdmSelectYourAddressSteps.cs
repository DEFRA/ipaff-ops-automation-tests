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
        private IIdmSelectYourAddressPage page => _objectContainer.Resolve<IIdmSelectYourAddressPage>();
        private IIdmWhatIsYourBusinessAddressPage manualAddressPage => _objectContainer.Resolve<IIdmWhatIsYourBusinessAddressPage>();
        public IdmSelectYourAddressSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the address selection or manual business address page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(page.IsDisplayed() || manualAddressPage.IsDisplayed(), Is.True, "Expected address selection or manual address entry after postcode lookup");

        [When("the user selects a random business address or enters the generated address manually and continues")]
        public void WhenAddressIsSelected()
        {
            var business = _scenarioContext.Get<OperatorDetails>("RegistrationBusiness");
            if (page.IsDisplayed())
            {
                _scenarioContext.Set(page.SelectAddressAndContinue(business.Postcode), "RegistrationAddress");
                return;
            }

            Assert.That(manualAddressPage.IsDisplayed(), Is.True, "Manual entry is only allowed when the lookup displays the business address form");
            var address = Regex.Match(business.AddressLine1, @"^(?<number>\d+\w*)\s+(?<street>.+)$");
            var buildingNumber = address.Success ? address.Groups["number"].Value : Random.Shared.Next(1, 1000).ToString();
            var street = address.Success ? address.Groups["street"].Value : business.AddressLine1;
            manualAddressPage.EnterAddressAndContinue(buildingNumber, street, business.CityOrTown, business.Postcode);
            _scenarioContext.Set($"{buildingNumber} {street}, {business.CityOrTown}, {business.Postcode}", "RegistrationAddress");
        }
    }
}
