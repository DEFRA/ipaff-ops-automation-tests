using AventStack.ExtentReports.Gherkin.Model;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

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
            var commodityCodes = _scenarioContext.GetFromContext<List<string>>("CommodityCode", []);
            commodityCodes.Add(code);

            _scenarioContext["CommodityCode"] = commodityCodes;
            commodityPage?.EnterCommodityCode(code);
        }

        [Then("the commodity details should be populated {string} {string}")]
        public void ThenTheCommodityDetailsShouldBePopulated(string code, string description)
        {
            var commodityDescriptions = _scenarioContext.GetFromContext<List<string>>("CommodityDescription", []);
            commodityDescriptions.Add(description);

            _scenarioContext["CommodityDescription"] = commodityDescriptions;
            Assert.True(commodityPage?.VerifyCommodityDetails(code, description));
        }

        [When("the user selects the type of commodity {string}")]
        public void WhenTheUserSelectsTheTypeOfCommodity(string type)
        {
            var commoditytypes = _scenarioContext.GetFromContext<List<string>>("TypeOfCommodity", []);

            commoditytypes.Add(type);
            _scenarioContext["TypeOfCommodity"] = commoditytypes;
            commodityPage?.SelectTypeOfCommodity(type);
        }

        [When("the user selects species of commodity {string}")]
        public void WhenTheUserSelectsSpeciesOfCommodity(string species)
        {
            var speciesList = _scenarioContext.GetFromContext<List<string>>("Species", []);
            speciesList.Add(species);
            _scenarioContext["Species"] = speciesList;
            commodityPage?.SelectCommoditySpecies(species);
        }

        [When("the user selects {string} for Do you want to add another commodity?")]
        public void WhenTheUserSelectsForDoYouWantToAddAnotherCommodity(string option)
        {
            commodityPage?.AddAnotherCommodity(option);
        }

        [Then("the Commodity code and Description should be copied from the original notification")]
        [Then("the Commodity page should be displayed with the commodity and description entered")]
        public void ThenTheCommodityPageShouldBeDisplayedWithTheCommodityAndDescriptionEntered()
        {
            var code = _scenarioContext.Get<List<string>>("CommodityCode");
            var description = _scenarioContext.Get<List<string>>("CommodityDescription");
            Assert.True(commodityPage?.IsPageLoaded(), "Description of the goods Commodity page not loaded");
            Assert.True(commodityPage?.VerifyEnteredCommdityDetails(code, description));
        }

        [When("the user populates Net weight as {string}")]
        public void WhenTheUserPopulatesNetWeightAs(string weight)
        {
            List<string> values = weight.Split(',').Select(x => x.Trim()).ToList();
            commodityPage?.EnterNetWeight(values);
            _scenarioContext["NetWeight"] = values;
        }

        [When("the user changes the Number of animals to {string}")]
        [When("the user populates Number of animals as {string}")]
        public void WhenTheUserPopulatesNumberOfAnimalsAs(string quantity)
        {
            commodityPage?.EnterNumberOfAnimals(quantity);
            _scenarioContext["NumberOfAnimals"] = quantity;
        }

        [When("the user changes the Number of packages to {string}")]
        [When("the user populates Number of packages as {string}")]
        public void WhenTheUserPopulatesNumberOfPackagesAs(string packages)
        {
            List<string> values = packages.Split(',').Select(x => x.Trim()).ToList();
            commodityPage?.EnterNumberOfPackages(values);
            _scenarioContext["NumberOfPackages"] = values;
        }

        [When("the user selects type of package as {string}")]
        public void WhenTheUserSelectsTypeOfPackageAs(string type)
        {
            List<string> values = type.Split(',').Select(x => x.Trim()).ToList();
            commodityPage?.SelectPackageType(values);
            _scenarioContext["PackageType"] = values;
        }

        [When("the user clicks the Update total button")]
        public void WhenTheUserClicksTheUpdateTotalButton()
        {
            commodityPage?.ClickUpdateTotal();
            Thread.Sleep(1000);

            _scenarioContext["SubtotalNetWeight"] = commodityPage?.GetSubtotalsOfNetWeight();
            _scenarioContext["SubtotalPackages"] = commodityPage?.GetSubtotalsOfPackages();
            _scenarioContext["TotalNetWeight"] = commodityPage?.GetTotalNetWeight();
            _scenarioContext["TotalPackages"] = commodityPage?.GetTotalPackages();
        }

        [Then("the Total Net weight should be populated as {string}")]
        public void ThenTheTotalNetWeightShouldBePopulatedAs(string totalNetWeight)
        {
            commodityPage?.VerifyTotalNetWeight(totalNetWeight);
        }

        [Then("the Total Number of packages should be populated as {string}")]
        public void ThenTheTotalNumberOfPackagesShouldBePopulatedAs(string numOfPackages)
        {
            commodityPage?.VerifyNumberOfPackages(numOfPackages);
        }

        [When("the user clicks the Update total button after adding all the commodities")]
        public void WhenTheUserClicksTheUpdateTotalButtonAfterAddingAllTheCommodities()
        {
            commodityPage?.ClickUpdateTotal();
            Thread.Sleep(2000);
            _scenarioContext["SubtotalNetWeight"]=commodityPage?.GetSubtotalsOfNetWeight();
            _scenarioContext["SubtotalPackages"] = commodityPage?.GetSubtotalsOfPackages();   
            _scenarioContext["TotalNetWeight"]=commodityPage?.GetTotalNetWeight();
            _scenarioContext["TotalPackages"] = commodityPage?.GetTotalPackages();
        }

        [Then("user enters the total gross weight greater than the net weight {string}")]
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

        [When("the user clicks on the back button in the browser")]
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
            Assert.True(commodityPage?.VerifyCommodityDetails(code, description));

            _scenarioContext["CommodityCodeFirstCommodity"] = code;
            _scenarioContext["CommodityDescFirstCommodity"] = description;
        }

        [Then("the commodity details should be populated {string} {string} for second commodity")]
        public void ThenTheCommodityDetailsShouldBePopulatedForSecondCommodity(string code, string description)
        {
            Assert.True(commodityPage?.VerifyCommodityDetails(code, description));
            _scenarioContext["CommodityCodeSecondCommodity"] = code;
            _scenarioContext["CommodityDescSecondCommodity"] = description;
        }

        [When("the user populates Net weight as {string} for first commodity")]
        public void WhenTheUserPopulatesNetWeightAsForFirstCommodity(string weight)
        {
            List<string> values = weight.Split(',').Select(x => x.Trim()).ToList();
            commodityPage?.EnterNetWeight(values);
            _scenarioContext["NetWeightFirstCommodity"] = weight;
        }

        [When("the user populates Number of packages as {string} for first commodity")]
        public void WhenTheUserPopulatesNumberOfPackagesAsForFirstCommodity(string packages)
        {
            List<string> values = packages.Split(',').Select(x => x.Trim()).ToList();
            commodityPage?.EnterNumberOfPackages(values);
            _scenarioContext["NumberOfPackagesFirstCommodity"] = packages;
        }

        [When("the user selects type of package as {string} for the commodity {string} for first commodity")]
        public void WhenTheUserSelectsTypeOfPackageAsForTheCommodityForFirstCommodity(string typeOfPackage, string commodityCode)
        {
            commodityPage?.SelectPackageTypeForCommodityCode(typeOfPackage, commodityCode);
            _scenarioContext["TypeOfPackageFirstCommodity"] = typeOfPackage;
        }

        [When("the user populates Net weight as {string} for the second commodity {string}")]
        public void WhenTheUserPopulatesNetWeightAsForTheAdditionalCommodity(string netWeight, string commodityCode)
        {
            commodityPage?.AddNetWeightForCommodityCode(netWeight, commodityCode);
            _scenarioContext["NetWeightSecondCommodity"] = netWeight;
        }

        [When("the user populates Number of packages as {string} for the second commodity {string}")]
        public void WhenTheUserPopulatesNumberOfPackagesAsForTheAdditionalCommodity(string numOfPackages, string commodityCode)
        {
            commodityPage?.AddNumOfPackagesForCommodityCode(numOfPackages, commodityCode);
            _scenarioContext["NumOfPackagesSecondCommodity"] = numOfPackages;
        }

        [When("the user selects type of package as {string} for the second commodity {string}")]
        public void WhenTheUserSelectsTypeOfPackageAsForTheAdditionalCommodity(string typeOfPackage, string commodityCode)
        {
            commodityPage?.SelectPackageTypeForCommodityCode(typeOfPackage, commodityCode);
            _scenarioContext["TypeOfPackageSecondCommodity"] = typeOfPackage;
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
        public void WhenTheUserSelectsTheFirstAdditionalCommodityUnderTheParentCommodity(string additionalCommCode, string additionalCommDescription)
        {
            _scenarioContext["CommodityCodeSecondCommodity"] = additionalCommCode;
            _scenarioContext["CommodityDescSecondCommodity"] = additionalCommDescription;
            commodityPage?.SelectCommodityInTheCommTree(additionalCommDescription);
        }

        [When("the user selects the {string} {string} under the parent commodity")]
        public void WhenTheUserSelectsTheAdditionalCommodityUnderTheParentCommodity(string commCode, string commDescription)
        {
            var commodityCodes = _scenarioContext.GetFromContext<List<string>>("CommodityCode", []);
            commodityCodes.Add(commCode);
            _scenarioContext["CommodityCode"] = commodityCodes;
            commodityPage?.SelectCommodityInTheCommTree(commDescription);
        }

        [Then("the Commodity page should be displayed")]
        public void ThenTheCommodityPageShouldBeDisplayed()
        {
            Assert.True(commodityPage?.IsPageLoaded(), "Description of the goods Commodity page not loaded");
        }

        [Then("the user verifies and enters any missing data on the Commodity page")]
        public void ThenTheUserVerifiesAndEntersAnyMissingDataOnTheCommodityPage()
        {
            Assert.False(commodityPage?.GetAddedCommoditiesCount == 0, "No commodities were added on the commodity page");

            WhenTheUserPopulatesNetWeightAsForFirstCommodity("19000");
            WhenTheUserPopulatesNumberOfPackagesAsForFirstCommodity("1");
            WhenTheUserSelectsTypeOfPackageAsForTheCommodityForFirstCommodity("Case", "12024200");

            WhenTheUserPopulatesNetWeightAsForTheAdditionalCommodity("18000", "100610");
            WhenTheUserPopulatesNumberOfPackagesAsForTheAdditionalCommodity("1", "100610");
            WhenTheUserSelectsTypeOfPackageAsForTheAdditionalCommodity("Box", "100610");

            WhenTheUserClicksTheUpdateTotalButtonAfterAddingAllTheCommodities();
            ThenTheTotalGrossWeightShouldBeGreaterThanTheNetWeight("40000");
        }

        [When("the user clicks on Save and return to hub on the Commodity page")]
        [Then("the user clicks on Save and return to hub on the Commodity page")]
        public void WhenTheUserClicksOnSaveAndReturnToHubOnTheCommodityPage()
        {
            commodityPage?.ClickSaveAndReturnToHub();
        }

        [Then("Description of the goods How do you want to add your commodity details page should be displayed")]
        public void ThenDescriptionOfTheGoodsHowDoYouWantToAddYourCommodityDetailsPageShouldBeDisplayed()
        {
            Assert.True(commodityPage?.IsHowToAddCommodityPageLoaded(), "Description of the goods How do you want to add your commodity details? page not loaded");
        }

        [When("the user selects {string} option to add commodity details")]
        public void WhenTheUserSelectsOptionToAddCommodityDetails(string option)
        {
            commodityPage?.SelectHowToAddCommodityOption(option);
        }

        [When("the user clicks Commodity code search tab")]
        public void WhenTheUserClicksCommodityCodeSearchTab()
        {
            commodityPage?.ClickCommodityCodeSearchTab();
        }

        [Then("the CHED PP commodity details should be populated {string} {string} for first commodity")]
        public void ThenTheCHEDPPCommodityDetailsShouldBePopulatedForFirstCommodity(string code, string description)
        {
            Assert.True(commodityPage?.VerifyCHEDPPCommodityDetails(code, description));

            _scenarioContext["CommodityCodeFirstCommodity"] = code;
            _scenarioContext["CommodityDescFirstCommodity"] = description;
        }

        [When("the user searchs for EPPO code {string} and clicks add link")]
        public void WhenTheUserSearchsForEPPOCodeAndClicksAddLink(string eppoCode)
        {
            commodityPage?.SearchEppoCode(eppoCode);
            commodityPage?.ClickAddLink(eppoCode);
        }

        [Then("Genus\\(and Species) {string} and EPPO code {string} should be populated in commodity page")]
        public void ThenGenusAndSpeciesAndEPPOCodeShouldBePopulatedInCommodityPage(string genus, string eppoCode)
        {
            Assert.True(commodityPage?.VerifyGenusSpeciesEPPOCode(genus, eppoCode));
            _scenarioContext["GenusFirstCommodity"] = genus;
            _scenarioContext["EPPOCodeFirstCommodity"] = eppoCode;
        }

        [When("the user selects EPPO code {string} checkbox")]
        public void WhenTheUserSelectsEPPOCodeCheckbox(string eppoCodeCheckBox)
        {
            commodityPage?.SelctEPPOCode(eppoCodeCheckBox);
        }

        [Then("the Description of the goods Variety and class of commodity should be displayed")]
        public void ThenTheDescriptionOfTheGoodsVarietyAndClassOfCommodityShouldBeDisplayed()
        {
            Assert.True(commodityPage?.IsVarietyAndClassOfCommodityPageLoaded(), "Description of the goods Variety and class of commodity page not loaded");
        }

        [When("the user selects {string} variety of EPPO code {string}")]
        public void WhenTheUserSelectsVarietyofEPPOCode(string variety, string eppoCode)
        {
            commodityPage?.SelectVariety(variety, eppoCode);
            _scenarioContext["CommodityVariety"] = variety;
        }

        [When("the user select {string} class of EPPO code {string}")]
        public void WhenTheUserSelectClassofEPPOCode(string classOfEPPO, string eppoCode)
        {
            commodityPage?.SelectClass(classOfEPPO, eppoCode);
            _scenarioContext["CommodityClass"] = classOfEPPO;
        }

        [Then("the selected commodities {string} {string} should be displayed in commodity page with code {string} {string} EPPO code {string} {string} and Genus {string} {string}")]
        public void ThenTheSelectedCommoditiesShouldBeDisplayedInCommodityPageWithCodeAndEPPOCode(string firstComm, string secondComm, string firstCode, string secondCode, string firstEPPO, string secondEPPO, string firstGenus, string secondGenus)
        {
            Assert.True(commodityPage?.VerifySelectedCommoditiesDisplayed(firstComm, secondComm, firstCode, secondCode, firstEPPO, secondEPPO, firstGenus, secondGenus));
            _scenarioContext["GenusSecondCommodity"] = secondGenus;
            _scenarioContext["EPPOCodeSecondCommodity"] = secondEPPO;
        }

        [When("the user selects check boxes for the commodity codes {string} {string}")]
        public void WhenTheUserSelectsCheckBoxesForTheCommodityCodes(string firstCommCode, string secondCommCode)
        {
            commodityPage?.SelectCommodities(firstCommCode, secondCommCode);
        }

        [When("the user populates Net weight as {string} for CHED PP commodity")]
        public void WhenTheUserPopulatesNetWeightAsForCHEDPPCommodity(string weight)
        {
            commodityPage?.EnterCHEDPPNetWeight(weight);
            _scenarioContext["NetWeight"] = weight;
        }

        [When("the user populates Number of packages as {string} for CHED PP commodity")]
        public void WhenTheUserPopulatesNumberOfPackagesAsForCHEDPPCommodity(string numberOfPackages)
        {
            commodityPage?.EnterCHEDPPNumberOfPackages(numberOfPackages);
            _scenarioContext["NumberOfPackages"] = numberOfPackages;
        }

        [When("the user selects type of package as {string} for CHED PP commodity")]
        public void WhenTheUserSelectsTypeOfPackageAsForCHEDPPCommodity(string packageType)
        {
            commodityPage?.SelectCHEDPPPackageType(packageType);
            _scenarioContext["PackageType"] = packageType;
        }

        [When("the user populates Quantity as {string} for CHED PP commodity")]
        public void WhenTheUserPopulatesQuantityAsForCHEDPPCommodity(string quantity)
        {
            commodityPage?.EnterCHEDPPQuantity(quantity);
            _scenarioContext["Quantity"] = quantity;
        }

        [When("the user selects Quantity type as {string} for CHED PP commodity")]
        public void WhenTheUserSelectsQuantityTypeAsForCHEDPPCommodity(string type)
        {
            commodityPage?.SelectCHEDPPQuanityType(type);
            _scenarioContext["QuantityType"] = type;
        }

        [Then("the user clicks Apply Button")]
        public void ThenTheUserClicksApplyButton()
        {
            commodityPage?.ClickApplyButton();
            Thread.Sleep(1000);
            _scenarioContext["TotalNetWeight"] = commodityPage?.GetCHEDPPTotalNetWeight();
            _scenarioContext["TotalPackages"] = commodityPage?.GetCHEDPPTotalPackages();
        }

        [When("the user clicks Cancel link")]
        public void WhenTheUserClicksCancelLink()
        {
            commodityPage?.ClickCancelLink();
        }

        [When("the user populates Number of animals as {string} for the commodity {string}")]
        public void WhenTheUserPopulatesNumberOfAnimalsAsForTheCommodity(string numberOfAnimals, string species)
        {
            commodityPage?.EnterNumberOfAnimalsForSpecies(numberOfAnimals, species);

            // Store in unified MultiSpeciesData model
            var multiSpecies = _scenarioContext.GetOrCreateMultiSpeciesData();
            multiSpecies.GetOrCreateSpecies(species).NumberOfAnimals = numberOfAnimals;
        }

        [When("the user populates Number of packages as {string} for the commodity {string}")]
        public void WhenTheUserPopulatesNumberOfPackagesAsForTheCommodity(string numberOfPackages, string species)
        {
            commodityPage?.EnterNumberOfPackagesForSpecies(numberOfPackages, species);

            // Store in unified MultiSpeciesData model
            var multiSpecies = _scenarioContext.GetOrCreateMultiSpeciesData();
            multiSpecies.GetOrCreateSpecies(species).NumberOfPackages = numberOfPackages;
        }
    }
}