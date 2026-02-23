using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ImporterPackerDeliveryAddressConsignorSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IImporterPackerDeliveryAddressConsignorPage? importerPackerDeliveryAddressConsignorPage => _objectContainer.IsRegistered<IImporterPackerDeliveryAddressConsignorPage>() ? _objectContainer.Resolve<IImporterPackerDeliveryAddressConsignorPage>() : null;

        public ImporterPackerDeliveryAddressConsignorSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("Importer, Packer, Delivery address and Consignor page should be displayed")]
        public void ThenImporterPackerDeliveryAddressAndConsignorPageShouldBeDisplayed()
        {
            Assert.True(importerPackerDeliveryAddressConsignorPage?.IsPageLoaded(), "Importer, Packer, Delivery address and Consignor page not loaded");
        }

        [When("the user verifies Importer details {string} is pre-filled")]
        public void WhenTheUserVerifiesImporterDetailsIsPre_Filled(string importer)
        {
            string importerName;
            if (_scenarioContext.ContainsKey("CompanyName"))
                importerName = _scenarioContext.Get<string>("CompanyName");
            else
            {
                importerName = importer;
                _scenarioContext["CompanyName"] = importerName;
            }

            Assert.True(importerPackerDeliveryAddressConsignorPage?.VerifyImporterName(importerName));
            _scenarioContext["ImporterAddress"] = importerPackerDeliveryAddressConsignorPage?.GetImporterAddress(importerName);
        }

        [When("the user clicks Add a delivery address link")]
        public void WhenTheUserClicksAddADeliveryAddressLink()
        {
            importerPackerDeliveryAddressConsignorPage?.AddADeliveryAddress();
        }

        [Then("the chosen delivery address {string} should be displayed on the Traders page")]
        public void ThenTheChosenDeliveryAddressShouldBeDisplayedOnTheTradersPage(string dEFRA)
        {
            var deliveryAddressName = _scenarioContext.Get<string>("DeliveryAddressName");
            var deliveryAddress = _scenarioContext.Get<string>("DeliveryAddress");
            var deliveryCountry = _scenarioContext.Get<string>("DeliveryCountry");

            Assert.True(importerPackerDeliveryAddressConsignorPage?.VerifySelectedDeliveryAddress(deliveryAddressName, deliveryAddress, deliveryCountry), "Delivery Address details do not match");
        }
    }
}