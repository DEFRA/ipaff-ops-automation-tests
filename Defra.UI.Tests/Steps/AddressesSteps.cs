using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class AddressesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IAddressesPage? addressesPage => _objectContainer.IsRegistered<IAddressesPage>() ? _objectContainer.Resolve<IAddressesPage>() : null;


        public AddressesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Addresses page should be displayed")]
        public void ThenTheAddressesPageShouldBeDisplayed()
        {
            Assert.True(addressesPage?.IsPageLoaded(), "Traders Addresses page not loaded");
        }

        [When("the user clicks Add a consignor or exporter")]
        public void WhenTheUserClicksAddAConsignorOrExporter()
        {
            addressesPage?.ClickAddConsignor();
        }


        [Then("the chosen consignor or exporter should be displayed")]
        public void ThenTheChosenConsignorOrExporterShouldBeDisplayed()
        {
            var consignorName = _scenarioContext.Get<string>("ConsignorName");
            var consignorAddress = _scenarioContext.Get<string>("ConsignorAddress");
            var consignorCountry = _scenarioContext.Get<string>("ConsignorCountry");

            Assert.True(addressesPage?.VerifySelectedConsignor(consignorName, consignorAddress, consignorCountry),
                        "Consignor details do not match");
        }

        [Then("the chosen consignor or exporter should be displayed on the Addresses page")]
        public void ThenTheChosenConsignorOrExporterShouldBeDisplayedOnTheAddressesPage()
        {
            Assert.True(addressesPage?.VerifySelectedConsignor());
        }


        [When("the user clicks Add a consignee")]
        public void WhenTheUserClicksAddAConsignee()
        {
            addressesPage?.ClickAddConsignee();
        }

        [Then("the chosen consignee should be displayed")]
        public void ThenTheChosenConsigneeShouldBeDisplayed()
        {
            var consigneeName = _scenarioContext.Get<string>("ConsigneeName");
            var consigneeAddress = _scenarioContext.Get<string>("ConsigneeAddress");
            var consigneeCountry = _scenarioContext.Get<string>("ConsigneeCountry");

            Assert.True(addressesPage?.VerifySelectedConsignee(consigneeName, consigneeAddress, consigneeCountry),
                        "Consignee details do not match");
        }

        [Then("the chosen consignee should be displayed on the Addresses page")]
        public void ThenTheChosenConsigneeShouldBeDisplayedOnTheAddressesPage()
        {
            Assert.True(addressesPage?.VerifySelectedConsignee());
        }

        [When("the user clicks Same as consignee for the Importer")]
        public void WhenTheUserClicksSameAsConsigneeForTheImporter()
        {
            addressesPage?.ClickImporterSameAsConsignee();
            _scenarioContext.Add("ImporterDetails", addressesPage.GetSelectedImporter());
        }

        [Then("the importer should be populated with the same details as the consignee on the Addresses page")]
        public void ThenTheImporterShouldBePopulatedWithTheSameDetailsAsTheConsigneeOnTheAddressesPage()
        {
            Assert.True(addressesPage?.VerifySelectedConsignee());
        }

        [Then("the importer should be populated with the same details as the consignee")]
        public void ThenTheImporterShouldBePopulatedWithTheSameDetailsAsTheConsignee()
        {
            var consigneeName = _scenarioContext.Get<string>("ConsigneeName");
            var consigneeAddress = _scenarioContext.Get<string>("ConsigneeAddress");
            var consigneeCountry = _scenarioContext.Get<string>("ConsigneeCountry");

            // Since importer uses same data as consignee, store it in context
            _scenarioContext.Add("ImporterName", consigneeName);
            _scenarioContext.Add("ImporterAddress", consigneeAddress);
            _scenarioContext.Add("ImporterCountry", consigneeCountry);

            // Verify importer shows same details as consignee
            Assert.True(addressesPage?.VerifySelectedConsignee(consigneeName, consigneeAddress, consigneeCountry),
                        "Importer details do not match consignee");
        }

        [When("the user clicks Add an importer")]
        public void WhenTheUserClicksAddAnImporter()
        {
            addressesPage?.ClickAddImporter();
        }

        [Then("the chosen importer should be displayed on the Addresses page")]
        public void ThenTheChosenImporterShouldBeDisplayedOnTheAddressesPage()
        {
            var importerName = _scenarioContext.Get<string>("ImporterName");
            var importerAddress = _scenarioContext.Get<string>("ImporterAddress");
            var importerCountry = _scenarioContext.Get<string>("ImporterCountry");

            Assert.True(addressesPage?.VerifySelectedImporter(importerName, importerAddress, importerCountry),
                        "Importer details do not match");
        }


        [When("the user clicks Add a place of destination")]
        public void WhenTheUserClicksAddAPlaceOfDestination()
        {
            addressesPage?.ClickAddDestination();
        }

        [Then("the chosen place of destination should be displayed")]
        public void ThenTheChosenPlaceOfDestinationShouldBeDisplayed()
        {
            var destinationName = _scenarioContext.Get<string>("DestinationName");
            var destinationAddress = _scenarioContext.Get<string>("DestinationAddress");
            var destinationCountry = _scenarioContext.Get<string>("DestinationCountry");

            Assert.True(addressesPage?.VerifySelectedDestination(destinationName, destinationAddress, destinationCountry),
                        "Destination details do not match");
        }

        [Then("the chosen place of destination should be displayed on the Addresses page")]
        public void ThenTheChosenPlaceOfDestinationShouldBeDisplayedOnTheAddressesPage()
        {
            Assert.True(addressesPage?.VerifySelectedDestination());
        }
    }
}