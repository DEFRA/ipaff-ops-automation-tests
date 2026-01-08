using AventStack.ExtentReports.Gherkin.Model;
using Defra.UI.Tests.Pages.Interfaces;
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

        [When("the user select {string} radio button on the Laboratory tests page")]
        public void WhenISelectRadioButtonOnTheLaboratoryTestsPage(string labTestsOption)
        {
            _scenarioContext["AreLaboratoryTestsRequired"] = labTestsOption;
            laboratoryTestsPage?.SelectLabTestsRadio(labTestsOption);
        }

        [When("the user select {string} reason radio button on the Laboratory tests page")]
        public void WhenISelectReasonRadioButtonOnTheLaboratoryTestsPage(string labTestsReason)
        {
            _scenarioContext["LaboratoryTestsReason"] = labTestsReason;
            laboratoryTestsPage?.SelectLabTestsReason(labTestsReason);
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
            _scenarioContext["LaboratoryTestName"] = test;
            laboratoryTestsPage?.SelectLaboratoryTest(test);
            // Capture system date and time AFTER clicking the Select link
            var labTestSelectedDateTime = DateTime.Now;
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
            _scenarioContext["SampleDate"] = laboratoryTestsPage?.GetSampleDate();
            _scenarioContext["SampleTime"] = laboratoryTestsPage?.GetSampleTime();
        }

        [When("the user clicks select link of one of the Laboratory test")]
         public void WhenTheUserClicksSelectLinkOfOneOfTheLaboratoryTest()
         {
             _scenarioContext["LaboratoryTestName"] = laboratoryTestsPage?.GetLaboratoryTestName();
             laboratoryTestsPage?.ClickSelectLaboratoryTest();

            // Capture system date and time AFTER clicking the Select link
            var labTestSelectedDateTime = DateTime.Now;
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
            var expectedTime = _scenarioContext.Get<string>("LabTestSelectedTime");

            // Get the actual values from the page
            var actualDate = laboratoryTestsPage?.GetSampleDate();
            var actualTime = laboratoryTestsPage?.GetSampleTime();

            // Validate the sample date matches exactly
            Assert.That(actualDate, Is.EqualTo(expectedDate),
                $"Sample date mismatch. Expected: {expectedDate}, Actual: {actualDate}");

            // Validate the sample time matches exactly
            Assert.That(actualTime, Is.EqualTo(expectedTime),
                $"Sample time mismatch. Expected: {expectedTime}, Actual: {actualTime}");
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
            var labTestName = _scenarioContext.Get<string>("LaboratoryTestName");
            var analysisType = _scenarioContext.Get<string>("AnalysisType");
            var result = laboratoryTestsPage?.GetLabTestResult(0);

            Assert.True(laboratoryTestsPage?.VerifyLabTestsReviewPage(commodityCode, commodityDescription, commoditySpecies, labTestName));
            Assert.True(laboratoryTestsPage?.IsAddAnotherTestLinkDisplayed(), "Add another test link is not displayed");
            Assert.That(result, Is.EqualTo("Pending"), $"Lab test result mismatch. Expected: Pending, Actual: {result}");
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
    }
}