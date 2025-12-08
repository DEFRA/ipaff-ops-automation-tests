using Reqnroll.BoDi;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using DocumentFormat.OpenXml.Drawing.Charts;
using Dynamitey.DynamicObjects;

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

        [When("the user populates Net weight as {string} for the commodity {string}")]
        public void WhenTheUserPopulatesNetWeightAsForTheCommodity(string netWeight, string commodityCode)
        {
            commodityPage?.AddNetWeightForCommodityCode(netWeight, commodityCode);
            _scenarioContext.Add("NetWeight", netWeight);
        }

        [When("the user populates Number of packages as {string} for the commodity {string}")]
        public void WhenTheUserPopulatesNumberOfPackagesAsForTheCommodity(string numOfPackages, string commodityCode)
        {
            commodityPage?.AddNumOfPackagesForCommodityCode(numOfPackages, commodityCode);
            _scenarioContext.Add("NumberOfPackages", numOfPackages);
        }

        [When("the user selects type of package as {string} for the commodity {string}")]
        public void WhenTheUserSelectsTypeOfPackageAsForTheCommodity(string typeOfPackage, string commodityCode)
        {
            commodityPage?.SelectPackageTypeForCommodityCode(typeOfPackage, commodityCode);
            _scenarioContext.Add("PackageType", typeOfPackage);
        }

        [When("the user populates Net weight as {string} for the additional commodity {string}")]
        public void WhenTheUserPopulatesNetWeightAsForTheAdditionalCommodity(string netWeight, string commodityCode)
        {
            commodityPage?.AddNetWeightForCommodityCode(netWeight, commodityCode);
            _scenarioContext.Add("NetWeight", netWeight);
        }

        [When("the user populates Number of packages as {string} for the additional commodity {string}")]
        public void WhenTheUserPopulatesNumberOfPackagesAsForTheAdditionalCommodity(string numOfPackages, string commodityCode)
        {
            commodityPage?.AddNumOfPackagesForCommodityCode(numOfPackages, commodityCode);
            _scenarioContext.Add("NumberOfPackages", numOfPackages);
        }

        [When("the user selects type of package as {string} for the additional commodity {string}")]
        public void WhenTheUserSelectsTypeOfPackageAsForTheAdditionalCommodity(string typeOfPackage, string commodityCode)
        {
            commodityPage?.SelectPackageTypeForCommodityCode(typeOfPackage, commodityCode);
            _scenarioContext.Add("PackageType", typeOfPackage);
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

        [When("I click the back button in the browser")]
        public void WhenIClickTheBackButtonInTheBrowser()
        {
            commodityPage?.ClickBrowserBackButton();
        }

        [When("the user clicks the Add commodity link")]
        public void WhenTheUserClicksTheAddCommodityLink()
        {
            commodityPage?.ClickAddCommodityLink();
        }

        [When("the user clicks the {string} in the parent commodity tree")]
        public void WhenTheUserClicksTheInTheParentCommodityTree(string commodity)
        {
            commodityPage?.SelectCommodityInTheCommTree(commodity);
        }

        [When("the sub commodity list expands")]
        public void WhenTheSubCommodityListExpands()
        {
            Assert.True(commodityPage?.IsSubCommodityListDisplayed(), "The sub commodity list is not displayed after clicking the parent commodity");
        }

        /* [When("the user clicks {string} under the parent commodity")]
         public void WhenTheUserClicksUnderTheParentCommodity(string subCommodityItem)
         {
             commodityPage?.SelectCommodityInTheCommTree(subCommodityItem);
             _scenarioContext.Add("CommodityDescription", subCommodityItem);
         }*/

        [When("the user clicks {string} {string} under the parent commodity")]
        public void WhenTheUserClicksUnderTheParentCommodity(string additionalCommCode, string additionalcommDescription)
        {
            _scenarioContext.Add("CommodityCodeAdditinalOne", additionalCommCode);
            _scenarioContext.Add("CommodityDescriptionAdditinalOne", additionalcommDescription);
            commodityPage?.SelectCommodityInTheCommTree(additionalcommDescription);
        }

        [Then("the Commodity page should be displayed")]
        public void ThenTheCommodityPageShouldBeDisplayed()
        {
            Assert.True(commodityPage?.IsPageLoaded(), "Description of the goods Commodity page not loaded");
        }



    }
}