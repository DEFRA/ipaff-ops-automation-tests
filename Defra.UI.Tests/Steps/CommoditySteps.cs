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
    public class CommoditySteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private ICommodityPage? commodityPage => _objectContainer.IsRegistered<ICommodityPage>() ? _objectContainer.Resolve<ICommodityPage>() : null;


        public CommoditySteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Description of the goods\\/Commodity page should be displayed")]
        public void ThenTheDescriptionOfTheGoodsCommodityPageShouldBeDisplayed()
        {
            Assert.True(commodityPage?.IsPageLoaded(), "Description of the goods Commodity page not loaded");
        }


        [When("the user searches {string} commodity code")]
        public void WhenTheUserSearchesCommodityCode(string code)
        {
            commodityPage?.EnterCommodityCode(code);
            _scenarioContext.Add("CommodityCode", code);
        }


        [Then("the commodity details should be populated {string} {string}")]
        public void ThenTheCommodityDetailsShouldBePopulated(string code, string description)
        {
            Assert.True(commodityPage?.VerifyCommdityDetails(code,description));
            _scenarioContext.Add("CommodityDescription", description);
        }


        [When("the user selects species of commodity {string}")]
        public void WhenTheUserSelectsSpeciesOfCommodity(string species)
        {
            commodityPage?.SelectCommoditySpecies(species);
            _scenarioContext.Add("Species", species);
        }


        [When("the user selects {string} for Do you want to add another commodity?")]
        public void WhenTheUserSelectsForDoYouWantToAddAnotherCommodity(string option)
        {
            commodityPage?.AddAnotherCommodity(option);
        }


        [Then("the Commodity page should be displayed with the commodity and description entered")]
        public void ThenTheCommodityPageShouldBeDisplayedWithTheCommodityAndDescriptionEntered()
        {
            var code = _scenarioContext.Get<string>("CommodityCode");
            var description = _scenarioContext.Get<string>("CommodityDescription");
            Assert.True(commodityPage?.IsPageLoaded(), "Description of the goods Commodity page not loaded");
            Assert.True(commodityPage?.VerifyEnteredCommdityDetails(code, description));
        }


        [When("the user populates Net weight as {string}")]
        public void WhenTheUserPopulatesNetWeightAs(string weight)
        {
            commodityPage?.EnterNetWeight(weight);
            _scenarioContext.Add("NetWeight", weight);
        }

        [When("the user populates Number of animals as {string}")]
        public void WhenTheUserPopulatesNumberOfAnimalsAs(string quantity)
        {
            commodityPage?.EnterNumberOfAnimals(quantity);
            _scenarioContext.Add("NumberOfAnimals", quantity);
        }

        [When("the user populates Number of packages as {string}")]
        public void WhenTheUserPopulatesNumberOfPackagesAs(string packages)
        {
            commodityPage?.EnterNumberOfPackages(packages);
            _scenarioContext.Add("NumberOfPackages", packages);
        }


        [When("the user selects type of package as {string}")]
        public void WhenTheUserSelectsTypeOfPackageAs(string type)
        {
            commodityPage?.SelectPackageType(type);
            _scenarioContext.Add("PackageType", type);
        }


        [When("the user clicks the Update total button")]
        public void WhenTheUserClicksTheUpdateTotalButton()
        {
            commodityPage?.ClickUpdateTotal();
        }

        [Then("the total gross weight should be greater than the net weight {string}")]
        public void ThenTheTotalGrossWeightShouldBeGreaterThanTheNetWeight(string weight)
        {
            commodityPage?.EnterTotalGrossWeight(weight);
            _scenarioContext.Add("TotalGrossWeight", weight);
        }

        [When("the user clicks Save and continue in commodity page")]
        public void WhenTheUserClicksSaveAndContinueInCommodityPage()
        {
            commodityPage?.ClickSaveAndContinue();
        }

        [When("the user populates the Ear tag as {string}")]
        public void WhenTheUserPopulatesTheEarTagAs(string earTag)
        {
            commodityPage?.EnterEarTag(earTag);
            _scenarioContext.Add("EarTag", earTag);
        }
    }
}