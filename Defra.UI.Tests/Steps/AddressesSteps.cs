using Reqnroll.BoDi;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class AddressesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
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

        [Then("the chosen consignee should be displayed on the Addresses page")]
        public void ThenTheChosenConsigneeShouldBeDisplayedOnTheAddressesPage()
        {
            Assert.True(addressesPage?.VerifySelectedConsignee());
        }

        [When("the user clicks Same as consignee for the Importer")]
        public void WhenTheUserClicksSameAsConsigneeForTheImporter()
        {
            addressesPage?.ClickImporterSameAsConsignee();
        }

        [Then("the importer should be populated with the same details as the consignee")]
        public void ThenTheImporterShouldBePopulatedWithTheSameDetailsAsTheConsignee()
        {
            Assert.True(addressesPage?.VerifySelectedConsignee());
        }

        [When("the user clicks Add a place of destination")]
        public void WhenTheUserClicksAddAPlaceOfDestination()
        {
            addressesPage?.ClickAddDestination();
        }

        [Then("the chosen place of destination should be displayed on the Addresses page")]
        public void ThenTheChosenPlaceOfDestinationShouldBeDisplayedOnTheAddressesPage()
        {
            Assert.True(addressesPage?.VerifySelectedDestination());
        }
    }
}