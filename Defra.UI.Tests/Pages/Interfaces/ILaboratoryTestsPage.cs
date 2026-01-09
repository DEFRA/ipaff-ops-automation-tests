using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ILaboratoryTestsPage
    {
        bool IsPageLoaded();
        void SelectLabTestsRadio(string labTestsOption);
        bool IsLabTestsNoPreselected();
        void SelectLabTestsReason(string labTestReason);
        void ClickSelectForCommodityCode(string commodityCode);
        void SelectTest(string testName);
        void SelectLaboratoryTestCategory(string category);
        void SelectLaboratoryTestSubCategory(string category);
        void ClickSearch();
        void SelectLaboratoryTest(string test);
        void SelectAnalysisType(string analysisType);
        void SelectLaboratoryTesting(string labTest);
        void EnterSampleReferenceNumber(string sampleReference);
        void EnterNumberOfSamples(string numberOfSamples);
        void SelectStorageTemperature(string storageTemperature);
        void SelectSampleType(string sampleType);
        string GetSelectedCommoditySampledCode();
        string GetSelectedCommoditySampledDescription();
        string GetSelectedCommoditySampledSpecies();
        bool IsCommoditySampledPageLoaded();
        void ClickSelectLaboratoryTest();
        string GetLaboratoryTestName();
        bool IsReviewPageLoaded();
        bool VerifyLabTestsReviewPage(string commodityCode, string commodityDescription, string commoditySpecies, string labTestName);
        bool IsReasonForTestingPageLoaded();
        bool IsSelectCommoditySampledPageLoaded();
        bool IsCommodityToBeTestedPageLoaded();        
        string GetSampleDate();
        string GetSampleTime();
        bool IsAddAnotherTestLinkDisplayed();
        string GetLabTestResult(int index = 0);
        void ClickAddAnotherTest();
        int GetLabTestCount();
        bool VerifyMultipleLabTestsWithPendingResults(int expectedMinimumCount = 2);
    }
}