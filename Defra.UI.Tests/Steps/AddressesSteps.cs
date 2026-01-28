using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using AventStack.ExtentReports.Gherkin.Model;
using Defra.UI.Tests.Tools;

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

        [Then("the chosen consignor or exporter {string} should be displayed on the Addresses page")]
        public void ThenTheChosenConsignorOrExporterShouldBeDisplayedOnTheAddressesPage(string consignor)
        {
            Assert.True(addressesPage?.VerifySelectedConsignor(consignor));
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

        [Then("the chosen consignee {string} should be displayed on the Addresses page")]
        public void ThenTheChosenConsigneeShouldBeDisplayedOnTheAddressesPage(string consigneeName)
        {
            Assert.True(addressesPage?.VerifySelectedConsignee(consigneeName));
        }

        [When("the user clicks Same as consignee for the Importer")]
        public void WhenTheUserClicksSameAsConsigneeForTheImporter()
        {
            addressesPage?.ClickImporterSameAsConsignee();
            _scenarioContext["ImporterDetails"] = addressesPage.GetSelectedImporter();
        }

        [Then("the importer should be populated with the same details as the consignee {string} on the Addresses page")]
        public void ThenTheImporterShouldBePopulatedWithTheSameDetailsAsTheConsigneeOnTheAddressesPage(string consigneeName)
        {
            Assert.True(addressesPage?.VerifySelectedConsignee(consigneeName));
        }

        [Then("the importer should be populated with the same details as the consignee")]
        public void ThenTheImporterShouldBePopulatedWithTheSameDetailsAsTheConsignee()
        {
            var consigneeName = _scenarioContext.Get<string>("ConsigneeName");
            var consigneeAddress = _scenarioContext.Get<string>("ConsigneeAddress");
            var consigneeCountry = _scenarioContext.Get<string>("ConsigneeCountry");

            // Since importer uses same data as consignee, store it in context
            _scenarioContext["ImporterName"] = consigneeName;
            _scenarioContext["ImporterAddress"] = consigneeAddress;
            _scenarioContext["ImporterCountry"] = consigneeCountry;

            // Verify importer shows same details as consignee
            Assert.True(addressesPage?.VerifySelectedImporter(consigneeName, consigneeAddress, consigneeCountry),
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
            var destinationName = _scenarioContext.Get<string>("PlaceOfDestinationName");
            var destinationAddress = _scenarioContext.Get<string>("PlaceOfDestinationAddress");
            var destinationCountry = _scenarioContext.Get<string>("PlaceOfDestinationCountry");

            Assert.True(addressesPage?.VerifySelectedDestination(destinationName, destinationAddress, destinationCountry),
            "Destination details do not match");
        }

        [Then ("the place of destination should be populated with the same details as the consignee {string} on the Addresses page")]
        [Then("the chosen place of destination {string} should be displayed on the Addresses page")]
        public void ThenTheChosenPlaceOfDestinationShouldBeDisplayedOnTheAddressesPage(string destination)
        {
            Assert.True(addressesPage?.VerifySelectedDestination(destination));
        }

        [When("the user clicks Same as consignee for Place of destination")]
        public void WhenTheUserClicksSameAsConsigneeForPlaceOfDestination()
        {
            addressesPage?.ClickPlaceOfDestinationSameAsConsignee();
            _scenarioContext["PlaceOfDestinationDetails"]=addressesPage?.GetSelectedPlaceOfDestination();
        }

        [When(@"the user clicks on Change link under '(.*)'")]
        public void WhenTheUserClicksOnChangeLinkUnderConsignorOrExporter(string link)
        {
            addressesPage?.ClickChangeInAddressPage(link);
        }

        [Then("the place of destination should be populated with the same details as the consignee")]
        public void ThenThePlaceOfDestinationShouldBePopulatedWithTheSameDetailsAsTheConsignee()
        {
            var consigneeName = _scenarioContext.Get<string>("ConsigneeName");
            var consigneeAddress = _scenarioContext.Get<string>("ConsigneeAddress");
            var consigneeCountry = _scenarioContext.Get<string>("ConsigneeCountry");

            // Since place of destination uses same data as consignee, store it in context
            // Use indexer assignment instead of .Add() to handle cases where key might already exist
            _scenarioContext["PlaceOfDestinationName"] = consigneeName;
            _scenarioContext["PlaceOfDestinationAddress"] = consigneeAddress;
            _scenarioContext["PlaceOfDestinationCountry"] = consigneeCountry;

            // Verify place of destination shows same details as consignee
            Assert.True(addressesPage?.VerifySelectedDestination(consigneeName, consigneeAddress, consigneeCountry),
                        "Place of destination details do not match consignee");
        }

        [Then("the user verifies and enters any missing data on the Addresses page")]
        public void ThenTheUserVerifiesAndEntersAnyMissingDataOnTheAddressesPage()
        {
            if (addressesPage?.GetConsignorRowsCount() == 1)
                _scenarioContext["ConsignorDetails"] = addressesPage.GetSelectedConsignor();
            else
                Assert.Fail($"Unexpected consignor rows count: {addressesPage?.GetConsignorRowsCount()}. Add consignor flow logic not implemented.");

            if (addressesPage?.GetConsigneeRowsCount() == 1)
                _scenarioContext["ConsigneeDetails"] = addressesPage.GetSelectedConsignee();
            else
                Assert.Fail($"Unexpected consignee rows count: {addressesPage?.GetConsigneeRowsCount()}. Add consignee flow logic not implemented.");

            if (addressesPage?.GetImporterRowsCount() == 1)
                _scenarioContext["ImporterDetails"] = addressesPage.GetSelectedImporter();
            else
                Assert.Fail($"Unexpected importer rows count: {addressesPage?.GetImporterRowsCount()}. Add importer flow logic not implemented.");

            if (addressesPage?.GetDestinationRowsCount() == 1)
                _scenarioContext["PlaceOfDestinationDetails"] = addressesPage.GetSelectedPlaceOfDestination();
            else
                Assert.Fail($"Unexpected place of destination rows count: {addressesPage?.GetDestinationRowsCount()}. Add place of destination flow logic not implemented.");
        }

        [Then("the chosen consignor from the address book should be displayed on the Addresses page {string}")]
        public void ThenTheChosenConsignorFromTheAddressBookShouldBeDisplayedOnTheAddressesPage(string operatorType)
        {
            // Get the ORIGINAL operator details from address book (source of truth)
            var expectedName = _scenarioContext[$"{operatorType}Name"]?.ToString();
            var expectedAddress = _scenarioContext[$"{operatorType}Address"]?.ToString();
            var expectedCountry = _scenarioContext[$"{operatorType}Country"]?.ToString();

            // Verify using existing page method
            var isDisplayed = addressesPage?.VerifySelectedConsignor(expectedName, expectedAddress, expectedCountry);

            Assert.IsTrue(isDisplayed,
                $"Consignor from address book ({operatorType}) not displayed correctly. Expected: {expectedName}, {expectedAddress}, {expectedCountry}");
        }

        [Then("the chosen consignee from the address book should be displayed on the Addresses page {string}")]
        public void ThenTheChosenConsigneeFromTheAddressBookShouldBeDisplayedOnTheAddressesPage(string operatorType)
        {
            // Get the ORIGINAL operator details from address book (source of truth)
            var expectedName = _scenarioContext[$"{operatorType}Name"]?.ToString();
            var expectedAddress = _scenarioContext[$"{operatorType}Address"]?.ToString();
            var expectedCountry = _scenarioContext[$"{operatorType}Country"]?.ToString();

            // Verify using existing page method
            var isDisplayed = addressesPage?.VerifySelectedConsignee(expectedName, expectedAddress, expectedCountry);

            Assert.IsTrue(isDisplayed,
                $"Consignee from address book ({operatorType}) not displayed correctly. Expected: {expectedName}, {expectedAddress}, {expectedCountry}");
        }

        [Then("the chosen importer from the address book should be displayed on the Addresses page {string}")]
        public void ThenTheChosenImporterFromTheAddressBookShouldBeDisplayedOnTheAddressesPage(string operatorType)
        {
            // Get the ORIGINAL operator details from address book (source of truth)
            var expectedName = _scenarioContext[$"{operatorType}Name"]?.ToString();
            var expectedAddress = _scenarioContext[$"{operatorType}Address"]?.ToString();
            var expectedCountry = _scenarioContext[$"{operatorType}Country"]?.ToString();

            // Verify using existing page method
            var isDisplayed = addressesPage?.VerifySelectedImporter(expectedName, expectedAddress, expectedCountry);

            Assert.IsTrue(isDisplayed,
                $"Importer from address book ({operatorType}) not displayed correctly. Expected: {expectedName}, {expectedAddress}, {expectedCountry}");
        }

        [Then("the chosen place of destination from the address book should be displayed on the Addresses page {string}")]
        public void ThenTheChosenPlaceOfDestinationFromTheAddressBookShouldBeDisplayedOnTheAddressesPage(string operatorType)
        {
            // Get the ORIGINAL operator details from address book (source of truth)
            var expectedName = _scenarioContext[$"{operatorType}Name"]?.ToString();
            var expectedAddress = _scenarioContext[$"{operatorType}Address"]?.ToString();
            var expectedCountry = _scenarioContext[$"{operatorType}Country"]?.ToString();

            // Verify using existing page method
            var isDisplayed = addressesPage?.VerifySelectedDestination(expectedName, expectedAddress, expectedCountry);

            Assert.IsTrue(isDisplayed,
                $"Place of destination from address book ({operatorType}) not displayed correctly. Expected: {expectedName}, {expectedAddress}, {expectedCountry}");
        }
    }
}