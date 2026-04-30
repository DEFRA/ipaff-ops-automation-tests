using AventStack.ExtentReports.Gherkin.Model;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class LaboratoryTestsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ILaboratoryTestsPage? laboratoryTestsPage => _objectContainer.IsRegistered<ILaboratoryTestsPage>() ? _objectContainer.Resolve<ILaboratoryTestsPage>() : null;

        public LaboratoryTestsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Laboratory tests page should be displayed")]
        public void ThenTheLaboratoryTestsPageShouldBeDisplayed()
        {
            Assert.True(laboratoryTestsPage?.IsPageLoaded(), "Laboratory tests page is not displayed");
        }

        [Then("{string} is pre-selected for Would you like to record laboratory tests?")]
        public void ThenIsPreSelectedForWouldYouLikeToRecordLaboratoryTests(string expectedSelection)
        {
            if (expectedSelection.Equals("No", StringComparison.OrdinalIgnoreCase))
            {
                _scenarioContext["AreLaboratoryTestsRequired"] = expectedSelection;
                Assert.True(laboratoryTestsPage?.IsLabTestsNoPreselected(), "No is not pre-selected for Would you like to record laboratory tests?");
            }
        }

        [When("the user selects {string} radio button for Would you like to record laboratory tests?")]
        [When("the user select {string} radio button on the Laboratory tests page")]
        public void WhenISelectRadioButtonOnTheLaboratoryTestsPage(string labTestsOption)
        {
            // Remove laboratory test data when user selects "No" to record lab tests
            // These keys would have been set in a previous selection and are no longer valid
            if (labTestsOption.Equals("No", StringComparison.OrdinalIgnoreCase))
            {
                _scenarioContext.RemoveContextKeys(
                    "LaboratoryTestsReason",
                    "AnalysisType",
                    "CommoditySampled",
                    "LaboratoryTestName",
                    "SampleDate",
                    "SampleTime",
                    "SampleUseByDate",
                    "ReleasedDate",
                    "Conclusion",
                    "SelectedCommoditySampledCode",
                    "SelectedCommoditySampledDescription",
                    "SelectedCommoditySampledSpecies",
                    "LabTestSelectedDate",
                    "LabTestSelectedTime"
                );
            }

            _scenarioContext["AreLaboratoryTestsRequired"] = labTestsOption;
            laboratoryTestsPage?.SelectLabTestsRadio(labTestsOption);
        }

        [When("the user selects {string} radio button for Reason for testing")]
        [When("the user select {string} reason radio button on the Laboratory tests page")]
        public void WhenISelectReasonRadioButtonOnTheLaboratoryTestsPage(string labTestsReason)
        {
            _scenarioContext["LaboratoryTestsReason"] = labTestsReason;
            laboratoryTestsPage?.SelectLabTestsReason(labTestsReason);
        }

        [When("{string} is pre-selected for Reason for testing")]
        [Then("{string} is pre-selected for Reason for testing")]
        public void WhenIsPre_SelectedForReasonForTesting(string labTestsReasonOption)
        {
            Assert.True(laboratoryTestsPage?.IsReasonForTestingRadioSelected(labTestsReasonOption), $"Unweaned animals radio is not pre-selected with {labTestsReasonOption} option");
            _scenarioContext["LaboratoryTestsReason"] = labTestsReasonOption;
        }

        [Then("the user verifies {string} {string} and {string} radio buttons are displayed")]
        public void ThenTheUserVerifiesRadioButtonsAreDisplayed(string reason1, string reason2, string reason3)
        {
            var isDisplayed = laboratoryTestsPage?.AreLabTestReasonRadioButtonsDisplayed(reason1, reason2, reason3);
            Assert.True(isDisplayed,
                $"Not all lab test reason radio buttons are displayed. Expected: '{reason1}', '{reason2}', '{reason3}'");
        }

        [When("the user clicks the Select link for the {string} commodity code")]
        public void WhenIClicksTheSelectLinkForTheCommodityCode(string commodityCode)
        {
            _scenarioContext["SelectedCommoditySampledCode"] = laboratoryTestsPage?.GetSelectedCommoditySampledCode();
            _scenarioContext["SelectedCommoditySampledDescription"] = laboratoryTestsPage?.GetSelectedCommoditySampledDescription();
            _scenarioContext["SelectedCommoditySampledSpecies"] = laboratoryTestsPage?.GetSelectedCommoditySampledSpecies();
            laboratoryTestsPage?.ClickSelectForCommodityCode(commodityCode);
        }

        [When("the user clicks on the Test {string}")]
        public void WhentheUserClicksOnTest(string testName)
        {
            laboratoryTestsPage?.SelectTest(testName);
        }

        [When("the user clicks on the name of the test")]
        public void WhenTheUserClicksOnTheNameOfTheTest()
        {
            var testName = _scenarioContext.Get<string[]>("LaboratoryTestName");
            laboratoryTestsPage?.SelectTest(testName[0]);
        }

        [When("the user selects {string} in Laboratory test category")]
        public void WhenTheUserSelectsLaboratoryTestCategory(string category)
        {
            laboratoryTestsPage?.SelectLaboratoryTestCategory(category);
        }

        [When("the user selects {string} in Laboratory test subcategory")]
        public void WhenTheUserSelectsLaboratoryTestSubCategory(string category)
        {
            laboratoryTestsPage?.SelectLaboratoryTestSubCategory(category);
        }

        [When("the user selects {string} from the list of Laboratory tests")]
        public void WhenTheUserSelectsFromTheListOfLaboratoryTests(string test)
        {
            //_scenarioContext["LaboratoryTestName"] = test;
            Utils.AppendStringToScenarioContextArray(_scenarioContext, "LaboratoryTestName", test);
            laboratoryTestsPage?.SelectLaboratoryTest(test);
            // Capture system date and time AFTER clicking the Select link
            var labTestSelectedDateTime = DateTime.UtcNow;
            _scenarioContext["LabTestSelectedDate"] = labTestSelectedDateTime.ToString("d MMMM yyyy");
            _scenarioContext["LabTestSelectedTime"] = labTestSelectedDateTime.ToString("HH:mm");
        }

        [When("the user clicks on Search")]
        public void WhenTheUserClicksSearch()
        {
            laboratoryTestsPage?.ClickSearch();
        }

        [When("the user populates the commodity sample details {string} {string} {string} {string} {string} {string}")]
        public void WhenIPopulateTheCommoditySampleDetails(string analysisType, string labTest, string sampleReference, string numberOfSamples, string sampleType, string storageTemperature)
        {
            laboratoryTestsPage?.SelectAnalysisType(analysisType);
            laboratoryTestsPage?.SelectLaboratoryTesting(labTest);
            laboratoryTestsPage?.EnterSampleReferenceNumber(sampleReference);
            laboratoryTestsPage?.EnterNumberOfSamples(numberOfSamples);
            laboratoryTestsPage?.SelectSampleType(sampleType);
            laboratoryTestsPage?.SelectStorageTemperature(storageTemperature);

            _scenarioContext["AnalysisType"] = analysisType;
            Utils.AppendStringToScenarioContextArray(_scenarioContext, "LabTestName", labTest);
            Utils.AppendStringToScenarioContextArray(_scenarioContext, "LabSampleReference", sampleReference);
            Utils.AppendStringToScenarioContextArray(_scenarioContext, "NumberOfLabSamples", numberOfSamples);
            Utils.AppendStringToScenarioContextArray(_scenarioContext, "LabSampleType", sampleType);
            Utils.AppendStringToScenarioContextArray(_scenarioContext, "LabSampleStorageTemperature", storageTemperature);
            _scenarioContext["SampleDate"] = laboratoryTestsPage?.GetSampleDate();
            _scenarioContext["SampleTime"] = laboratoryTestsPage?.GetSampleTime();
            /*            _scenarioContext["LabTestName"] = labTest;
                        _scenarioContext["LabSampleReference"] = sampleReference;
                        _scenarioContext["NumberOfLabSamples"] = numberOfSamples;
                        _scenarioContext["LabSampleType"] = sampleType;
                        _scenarioContext["LabSampleStorageTemperature"] = storageTemperature;*/
        }

        [When("the user selects any Laboratory test from the displayed list")]
        [When("the user clicks select link of one of the Laboratory test")]
        public void WhenTheUserClicksSelectLinkOfOneOfTheLaboratoryTest()
        {
            //_scenarioContext["LaboratoryTestName"] = laboratoryTestsPage?.GetLaboratoryTestName();
            Utils.AppendStringToScenarioContextArray(_scenarioContext, "LaboratoryTestName", laboratoryTestsPage?.GetLaboratoryTestName());
            laboratoryTestsPage?.ClickSelectLaboratoryTest();

            // Capture system date and time AFTER clicking the Select link
            var labTestSelectedDateTime = DateTime.UtcNow;
            _scenarioContext["LabTestSelectedDate"] = labTestSelectedDateTime.ToString("d MMMM yyyy");
            _scenarioContext["LabTestSelectedTime"] = labTestSelectedDateTime.ToString("HH:mm");
        }

        [Then("the Laboratory tests Commodity sampled page should be displayed")]
        public void ThenTheLaboratoryTestsCommoditySampledPageShouldBeDisplayed()
        {
            Assert.True(laboratoryTestsPage?.IsCommoditySampledPageLoaded(), "Laboratory tests Commodity sampled page is not displayed");
        }

        [Then("the Sample date and time is todays date with the time the lab test was selected")]
        public void ThenTheSampleDateAndTimeIsTodaysDateWithTheTimeTheLabTestWasSelected()
        {
            // Get the expected values from when the lab test was selected
            var expectedDate = _scenarioContext.Get<string>("LabTestSelectedDate");
            var expectedTimeStr = _scenarioContext.Get<string>("LabTestSelectedTime");

            // Get the actual values from the page
            var actualDate = laboratoryTestsPage?.GetSampleDate();
            var actualTimeStr = laboratoryTestsPage?.GetSampleTime();

            // Validate the sample date matches exactly
            Assert.That(actualDate, Is.EqualTo(expectedDate),
                $"Sample date mismatch. Expected: {expectedDate}, Actual: {actualDate}");

            // Parse times for comparison with tolerance
            if (TimeSpan.TryParse(expectedTimeStr, out var expectedTime) &&
                TimeSpan.TryParse(actualTimeStr, out var actualTime))
            {
                var timeDifference = Math.Abs((actualTime - expectedTime).TotalMinutes);

                // Allow up to 1 minutes tolerance for timing differences
                Assert.That(timeDifference, Is.LessThanOrEqualTo(1),
                    $"Sample time is outside acceptable range. Expected: {expectedTimeStr}, Actual: {actualTimeStr}, Difference: {timeDifference:F1} minutes");
            }
            else
            {
                // Fallback to exact match if parsing fails
                Assert.That(actualTimeStr, Is.EqualTo(expectedTimeStr),
                    $"Sample time mismatch. Expected: {expectedTimeStr}, Actual: {actualTimeStr}");
            }
        }

        [Then("the Laboratory tests Review page should be displayed")]
        public void ThenTheLaboratoryTestsReviewPageShouldBeDisplayed()
        {
            Assert.True(laboratoryTestsPage?.IsReviewPageLoaded(), "Laboratory tests Review page is not displayed");
        }

        [Then("the user verifies the data in Laboratory tests review page")]
        public void ThenTheUserVerifiesTheDataInLaboratoryTestsReviewPage()
        {
            var commodityCode = _scenarioContext.Get<string>("SelectedCommoditySampledCode");
            var commodityDescription = _scenarioContext.Get<string>("SelectedCommoditySampledDescription");
            var commoditySpecies = _scenarioContext.Get<string>("SelectedCommoditySampledSpecies");
            var labTestName = _scenarioContext.Get<string[]>("LaboratoryTestName");
            var analysisType = _scenarioContext.Get<string>("AnalysisType");
            var result = laboratoryTestsPage?.GetLabTestResult(0);

            Assert.True(laboratoryTestsPage?.VerifyLabTestsReviewPage(commodityCode, commodityDescription, commoditySpecies, labTestName[0]));
            Assert.True(laboratoryTestsPage?.IsAddAnotherTestLinkDisplayed(), "Add another test link is not displayed");
            Assert.That(result, Is.EqualTo("Pending"), $"Lab test result mismatch. Expected: Pending, Actual: {result}");
        }

        [Then("the user verifies the data and the conclusion as {string} on the Laboratory Tests Review page")]
        public void ThenTheUserVerifiesTheDataAndTheConclusionAsOnTheLaboratoryTestsReviewPage(string conclusion)
        {
            var commodityCode = _scenarioContext.Get<string>("SelectedCommoditySampledCode");
            var commodityDescription = _scenarioContext.Get<string>("SelectedCommoditySampledDescription");
            var commoditySpecies = _scenarioContext.Get<string>("SelectedCommoditySampledSpecies");
            var labTestName = _scenarioContext.Get<string[]>("LaboratoryTestName");
            var analysisType = _scenarioContext.Get<string>("AnalysisType");
            var result = laboratoryTestsPage?.GetLabTestResult(0);

            Assert.True(laboratoryTestsPage?.VerifyLabTestsReviewPage(commodityCode, commodityDescription, commoditySpecies, labTestName[0], conclusion));
            Assert.True(laboratoryTestsPage?.IsAddAnotherTestLinkDisplayed(), "Add another test link is not displayed");
            Assert.That(result, Is.EqualTo(conclusion), $"Lab test result mismatch. Expected: {conclusion}, Actual: {result}");
        }

        [Then("the Laboratory tests Reason for testing page should be displayed")]
        public void ThenTheLaboratoryTestsReasonForTestingPageShouldBeDisplayed()
        {
            Assert.True(laboratoryTestsPage?.IsReasonForTestingPageLoaded(), "Laboratory tests Reason for testing page is not displayed");
        }

        [Then("the Laboratory tests Select the commodity sampled page should be displayed")]
        public void ThenTheLaboratoryTestsSelectTheCommoditySampledPageShouldBeDisplayed()
        {
            Assert.True(laboratoryTestsPage?.IsSelectCommoditySampledPageLoaded(), "Laboratory tests Select the commodity sampled page is not displayed");
        }

        [Then("the Laboratory tests Commodity to be tested page should be displayed")]
        public void ThenTheLaboratoryTestsCommodityToBeTestedPageShouldBeDisplayed()
        {
            Assert.True(laboratoryTestsPage?.IsCommodityToBeTestedPageLoaded(), "Laboratory tests Commodity to be tested page is not displayed");
        }

        [When("the user clicks Add another test")]
        [When("the user clicks the Add a laboratory test link")]
        public void WhenTheUserClicksTheAddALaboratoryTestLink()
        {
            laboratoryTestsPage?.ClickAddAnotherTest();
        }

        [Then("the user verifies multiple Laboratory tests are displayed with Results {string}")]
        [Then("the user verifies multiple Laboratory tests are entered with Results {string}")]
        public void ThenTheUserVerifiesMultipleLaboratoryTestsAreEnteredWithResults(string expectedResult)
        {
            if (expectedResult.Equals("Pending", StringComparison.OrdinalIgnoreCase))
            {
                Assert.True(laboratoryTestsPage?.VerifyMultipleLabTestsWithPendingResults(2),
                    "Failed to verify multiple laboratory tests with Pending results");
            }
            else if (expectedResult.Equals("Satisfactory", StringComparison.OrdinalIgnoreCase))
            {
                Assert.True(laboratoryTestsPage?.VerifyMultipleLabTestsWithSatisfactoryResults(2),
                    "Failed to verify multiple laboratory tests with Satisfactory results");
            }
            else
            {
                Assert.Fail($"Verification for result '{expectedResult}' is not implemented");
            }
        }

        [Then("the Laboratory tests screen is displayed with the correct Commodity code")]
        public void ThenTheLaboratoryTestsScreenIsDisplayedWithTheCorrectCommodityCode()
        {
            var commodityCodes = _scenarioContext.GetFromContext("CommodityCode", new List<string>());
            var commodityCode = commodityCodes.FirstOrDefault() ?? string.Empty;

            Assert.True(laboratoryTestsPage?.VerifyCommodityCodeDisplayedInLabTests(commodityCode),
                $"Commodity code '{commodityCode}' is not correctly displayed on the Laboratory tests screen");
        }

        [Then("the Laboratory tests screen is displayed with the correct Description")]
        public void ThenTheLaboratoryTestsScreenIsDisplayedWithTheCorrectDescription()
        {
            var commodityCodes = _scenarioContext.GetFromContext("CommodityCode", new List<string>());
            var commodityCode = commodityCodes.FirstOrDefault() ?? string.Empty;

            var commodityDescriptions = _scenarioContext.GetFromContext("CommodityDescription", new List<string>());
            var description = commodityDescriptions.FirstOrDefault() ?? string.Empty;

            Assert.True(laboratoryTestsPage?.VerifyDescriptionDisplayedInLabTests(commodityCode, description),
                $"Description '{description}' is not correctly displayed on the Laboratory tests screen");
        }

        [Then("the Laboratory tests screen is displayed with the correct Species")]
        public void ThenTheLaboratoryTestsScreenIsDisplayedWithTheCorrectSpecies()
        {
            var commodityCodes = _scenarioContext.GetFromContext("CommodityCode", new List<string>());
            var commodityCode = commodityCodes.FirstOrDefault() ?? string.Empty;

            var speciesList = _scenarioContext.GetFromContext("Species", new List<string>());
            var species = speciesList.FirstOrDefault() ?? string.Empty;

            Assert.True(laboratoryTestsPage?.VerifySpeciesDisplayedInLabTests(commodityCode, species),
                $"Species '{species}' is not correctly displayed on the Laboratory tests screen");
        }

        [Then("the Laboratory tests screen is displayed with the Select hyperlink for the commodity sampled")]
        public void ThenTheLaboratoryTestsScreenIsDisplayedWithTheSelectHyperlinkForTheCommoditySampled()
        {
            var commodityCodes = _scenarioContext.GetFromContext("CommodityCode", new List<string>());
            var commodityCode = commodityCodes.FirstOrDefault() ?? string.Empty;

            Assert.True(laboratoryTestsPage?.VerifySelectHyperlinkDisplayedInLabTests(commodityCode),
                $"Select hyperlink for commodity code '{commodityCode}' is not correctly displayed on the Laboratory tests screen");
        }

        [Then("the Laboratory tests list should be filtered by the Laboratory test subcategory {string}")]
        public void ThenTheLaboratoryTestsListShouldBeFilteredByTheLaboratoryTestSubcategory(string subcategory)
        {
            Assert.True(laboratoryTestsPage?.VerifyLabTestsFilteredBySubcategory(subcategory),
                $"Laboratory tests list is not correctly filtered by subcategory '{subcategory}'");
        }

        [When("the user verifies the Analysis type dropdown displays options")]
        public void WhenTheUserVerifiesTheAnalysisTypeDropdownDisplaysOptions()
        {
            Assert.True(laboratoryTestsPage?.VerifyAnalysisTypeDropdownHasOptions(),
                "Analysis type dropdown does not display options");
        }

        [When("the user verifies the Laboratory dropdown displays options")]
        public void WhenTheUserVerifiesTheLaboratoryDropdownDisplaysOptions()
        {
            Assert.True(laboratoryTestsPage?.VerifyLaboratoryDropdownHasOptions(),
                "Laboratory dropdown does not display options");
        }

        [When("the user verifies the Sample type dropdown displays options")]
        public void WhenTheUserVerifiesTheSampleTypeDropdownDisplaysOptions()
        {
            Assert.True(laboratoryTestsPage?.VerifySampleTypeDropdownHasOptions(),
                "Sample type dropdown does not display options");
        }

        [When("the user verifies the Storage temperature dropdown displays options")]
        public void WhenTheUserVerifiesTheStorageTemperatureDropdownDisplaysOptions()
        {
            Assert.True(laboratoryTestsPage?.VerifyStorageTemperatureDropdownHasOptions(),
                "Storage temperature dropdown does not display options");
        }

        [When("the user searches for the laboratory test {string}")]
        public void WhenTheUserSearchesForTheLaboratoryTest(string testName)
        {
            laboratoryTestsPage?.EnterLaboratoryTestName(testName);
            laboratoryTestsPage?.ClickSearch();
        }

        [When(@"the user populates the sample date and time as {int} day\(s\) ago")]
        public void WhenTheUserPopulatesTheSampleDateAndTimeAsDaysAgo(int daysAgo)
        {
            laboratoryTestsPage?.PopulateSampleDateAndTime(daysAgo);
        }
    }
}