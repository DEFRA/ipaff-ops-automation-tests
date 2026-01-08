using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CommoditySteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

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

        [When("the user selects the type of commodity {string}")]
        public void WhenTheUserSelectsTheTypeOfCommodity(string type)
        {
            commodityPage?.SelectTypeOfCommodity(type);
            _scenarioContext["TypeOfCommodity"] = type;
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
            _scenarioContext["NetWeight"]= weight;
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
            _scenarioContext["NumberOfPackages"] = packages;
        }


        [When("the user selects type of package as {string}")]
        public void WhenTheUserSelectsTypeOfPackageAs(string type)
        {
            commodityPage?.SelectPackageType(type);
            _scenarioContext["PackageType"] = type;
        }

        [When("the user clicks the Update total button")]
        public void WhenTheUserClicksTheUpdateTotalButton()
        {
            commodityPage?.ClickUpdateTotal();
            Thread.Sleep(1000);
            _scenarioContext["SubtotalNetWeight"] = commodityPage?.GetSubtotalNetWeight();
            _scenarioContext["SubtotalPackages"] = commodityPage?.GetSubtotalPackages();
            _scenarioContext["TotalNetWeight"] = commodityPage?.GetTotalNetWeight();
            _scenarioContext["TotalPackages"] = commodityPage?.GetTotalPackages();
        }

        [When("the user populates Net weight as {string} for the second commodity {string}")]
        public void WhenTheUserPopulatesNetWeightAsForTheAdditionalCommodity(string netWeight, string commodityCode)
        {
            commodityPage?.AddNetWeightForCommodityCode(netWeight, commodityCode);
            _scenarioContext.Add("NetWeightSecondCommodity", netWeight);
        }

        [When("the user populates Number of packages as {string} for the second commodity {string}")]
        public void WhenTheUserPopulatesNumberOfPackagesAsForTheAdditionalCommodity(string numOfPackages, string commodityCode)
        {
            commodityPage?.AddNumOfPackagesForCommodityCode(numOfPackages, commodityCode);
            _scenarioContext.Add("NumOfPackagesSecondCommodity", numOfPackages);
        }

        [When("the user selects type of package as {string} for the second commodity {string}")]
        public void WhenTheUserSelectsTypeOfPackageAsForTheAdditionalCommodity(string typeOfPackage, string commodityCode)
        {
            commodityPage?.SelectPackageTypeForCommodityCode(typeOfPackage, commodityCode);
            _scenarioContext.Add("TypeOfPackageSecondCommodity", typeOfPackage);
        }

        [When("the user clicks the Update total button after adding all the commodities")]
        public void WhenTheUserClicksTheUpdateTotalButtonAfterAddingAllTheCommodities()
        {
            commodityPage?.ClickUpdateTotal();
            Thread.Sleep(2000);
            _scenarioContext.Add("TotalNetWeight", commodityPage.GetTotalNetWeight());
            _scenarioContext.Add("TotalPackages", commodityPage.GetTotalPackages());
        }

        [When("the total gross weight should be greater than the net weight {string}")]
        [Then("the total gross weight should be greater than the net weight {string}")]
        public void ThenTheTotalGrossWeightShouldBeGreaterThanTheNetWeight(string weight)
        {
            commodityPage?.EnterTotalGrossWeight(weight);
            _scenarioContext["TotalGrossWeight"] = weight;
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

        [When("the user searches for first commodity code {string}")]
        public void WhenTheUserSearchesForFirstCommodityCode(string code)
        {
            commodityPage?.EnterCommodityCode(code);
        }

        [Then("the commodity details should be populated {string} {string} for first commodity")]
        public void ThenTheCommodityDetailsShouldBePopulatedForFirstCommodity(string code, string description)
        {
            Assert.True(commodityPage?.VerifyCommdityDetails(code, description));
            _scenarioContext.Add("CommodityCodeFirstCommodity", code);
            _scenarioContext.Add("CommodityDescFirstCommodity", description);
        }

        [When("the user populates Net weight as {string} for first commodity")]
        public void WhenTheUserPopulatesNetWeightAsForFirstCommodity(string weight)
        {
            commodityPage?.EnterNetWeight(weight);
            _scenarioContext.Add("NetWeightFirstCommodity", weight);
        }

        [When("the user populates Number of packages as {string} for first commodity")]
        public void WhenTheUserPopulatesNumberOfPackagesAsForFirstCommodity(string packages)
        {
            commodityPage?.EnterNumberOfPackages(packages);
            _scenarioContext.Add("NumberOfPackagesFirstCommodity", packages);
        }

        [When("the user selects type of package as {string} for the commodity {string} for first commodity")]
        public void WhenTheUserSelectsTypeOfPackageAsForTheCommodityForFirstCommodity(string typeOfPackage, string commodityCode)
        {
            commodityPage?.SelectPackageTypeForCommodityCode(typeOfPackage, commodityCode);
            _scenarioContext.Add("TypeOfPackageFirstCommodity", typeOfPackage);
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

        [When("the user clicks {string} {string} under the parent commodity")]
        public void WhenTheUserClicksUnderTheParentCommodity(string additionalCommCode, string additionalcommDescription)
        {
            commodityPage?.SelectCommodityInTheCommTree(additionalcommDescription);
        }

        [When("the user selects the second commodity {string} {string} under the parent commodity")]
        public void WhenTheUserSelectsTheFirstAdditionalCommodityUnderTheParentCommodity(string additionalCommCode, string additionalcommDescription)
        {
            _scenarioContext.Add("CommodityCodeSecondCommodity", additionalCommCode);
            _scenarioContext.Add("CommodityDescSecondCommodity", additionalcommDescription);
            commodityPage?.SelectCommodityInTheCommTree(additionalcommDescription);
        }

        [Then("the Commodity page should be displayed")]
        public void ThenTheCommodityPageShouldBeDisplayed()
        {
            Assert.True(commodityPage?.IsPageLoaded(), "Description of the goods Commodity page not loaded");
        }

        [When("the user populates the Ear tag as {string}")]
        public void WhenTheUserPopulatesTheEarTagAs(string earTag)
        {
            commodityPage?.EnterEarTag(earTag);
            _scenarioContext.Add("EarTag", earTag);
        }
    }
}