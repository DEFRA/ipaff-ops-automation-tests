using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class LaboratoryTestsPage : ILaboratoryTestsPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.XPath("//h2[@class='govuk-heading-s govuk-!-margin-bottom-1  ']"), true);
        private IWebElement secondaryTitleReview => _driver.WaitForElement(By.XPath("//h2[@class='govuk-heading-m']"), true);
        private IWebElement labTestsRadio(string labTestsOption) => _driver.FindElement(By.XPath($"//input[@class='govuk-radios__input']/following-sibling::label[contains(text(),'{labTestsOption}')]"));
        private IWebElement labTestsReasonRadio(string labTestsReason) => _driver.FindElement(By.XPath($"//input[@value='{labTestsReason}']"));
        private IWebElement selectForCommodityCode(string commodityCode) => _driver.FindElement(By.XPath($"(//td[text()='{commodityCode}']/following::a[text()='Select'])[1]"));
        private IWebElement lnkTestName(string testName) => _driver.FindElement(By.XPath($"//td/button[normalize-space(text())='{testName}']"));
        private IWebElement lnkSelectLabTest(string testName) => _driver.FindElement(By.XPath($"//td[text()='{testName}']/following::a[1]"));
        private IWebElement rdoLabTestsYes => _driver.FindElement(By.Id("radio-lab-tests-required-yes"));
        private IWebElement rdoLabTestsNo => _driver.FindElement(By.Id("radio-lab-tests-required-no"));
        private IWebElement selectLabTestCategory => _driver.FindElement(By.Id("laboratory-tests-category"));
        private IWebElement selectLabTestSubCategory => _driver.FindElement(By.Id("laboratory-tests-subcategory"));
        private IWebElement btnSearch => _driver.FindElement(By.Id("search-button"));
        private IWebElement selectAnalysisType => _driver.FindElement(By.Id("analysisType"));
        private IWebElement selectLabTest => _driver.FindElement(By.Id("laboratory"));
        private IWebElement selectSampleReferenceNumber => _driver.FindElement(By.Id("sampleReferenceNumber"));
        private IWebElement selectNumberOfSamples => _driver.FindElement(By.Id("numberOfSamples"));
        private IWebElement selectSamplesType => _driver.FindElement(By.Id("sampleType"));
        private IWebElement selectStorageTemperature => _driver.FindElement(By.Id("conservationOfSample"));
        private IWebElement txtCommodityCode => _driver.FindElement(By.XPath("//*[@class='govuk-table__body']/tr[1]/td[1]"));
        private IWebElement txtDescription => _driver.FindElement(By.XPath("//*[@class='govuk-table__body']/tr[1]/td[2]"));
        private IWebElement txtSpecies => _driver.FindElement(By.XPath("//*[@class='govuk-table__body']/tr[1]/td[3]"));  
        private IWebElement lnkLabTestSelect => _driver.FindElement(By.Id("choose-laboratory-test-0"));
        private IWebElement txtLabTestName => _driver.FindElement(By.XPath("//*[@class='govuk-table__body']/tr[1]/td[1]"));
        private IReadOnlyCollection<IWebElement> reviewTableFirstRow => _driver.FindElements(By.XPath("//*[@class='govuk-table__body']"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public LaboratoryTestsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Laboratory tests");
        }

        public void SelectLabTestsRadio(string labTestsOption)
        {
            labTestsRadio(labTestsOption).Click();
        }
              
        public bool IsLabTestsNoPreselected()
        {
            return rdoLabTestsNo.GetAttribute("checked") != null;
        }

        public void SelectLabTestsReason(string labTestsReason)
        {
            labTestsReasonRadio(labTestsReason).Click();
        }

        public void ClickSelectForCommodityCode(string commodityCode)
        {
            selectForCommodityCode(commodityCode).Click();
        }
        
        public void SelectTest(string testName)
        {
            lnkTestName(testName).Click();
        }
        
        public void ClickSearch()
        {
            btnSearch.Click();
        }

        public void SelectLaboratoryTestCategory(string category)
        {
            new SelectElement(selectLabTestCategory).SelectByText(category);
        }
        
        public void SelectLaboratoryTestSubCategory(string category)
        {
            new SelectElement(selectLabTestSubCategory).SelectByText(category);
        }
        
        public void SelectLaboratoryTest(string test)
        {
            lnkSelectLabTest(test).Click();
        }

        public void SelectAnalysisType(String analysisType)
        {
            new SelectElement(selectAnalysisType).SelectByText(analysisType);
        }

        public void SelectLaboratoryTesting(string labTest)
        {
            new SelectElement(selectLabTest).SelectByText(labTest);
        }

        public void EnterSampleReferenceNumber(string sampleReference)
        {
            selectSampleReferenceNumber.SendKeys(sampleReference);
        }

        public void EnterNumberOfSamples(string numberOfSamples)
        {
            selectNumberOfSamples.SendKeys(numberOfSamples);

        }

        public void SelectStorageTemperature(string storageTemp)
        {
            new SelectElement(selectStorageTemperature).SelectByText(storageTemp);
        }

        public void SelectSampleType(string sampleType)
        {
            new SelectElement(selectSamplesType).SelectByText(sampleType);
        }

        public bool IsCommoditySampledPageLoaded()
        {
            return secondaryTitle.Text.Trim().Contains("Commodity sampled");
        }

        public string GetLaboratoryTestName()
        {
            return txtLabTestName.Text.Trim();
        }

        public void ClickSelectLaboratoryTest()
        {
            lnkLabTestSelect.Click();
        }

        public string GetSelectedCommoditySampledCode()
        {
            return txtCommodityCode.Text.Trim();
        }

        public string GetSelectedCommoditySampledDescription()
        {
            return txtDescription.Text.Trim();
        }

        public string GetSelectedCommoditySampledSpecies()
        {
            return txtSpecies.Text.Trim();
        }


        public bool IsReviewPageLoaded()
        {
            return secondaryTitleReview.Text.Trim().Contains("Review");
        }

        public bool VerifyLabTestsReviewPage(string commodityCode, string commodityDescription, string commoditySpecies, string labTestName)
        {
            foreach(var row in reviewTableFirstRow)
            {
                string rowText = row.Text;
                return rowText.Contains(commodityCode)
                    && rowText.Contains(commodityDescription)
                    && rowText.Contains(labTestName)
                    && rowText.Contains("Pending")
                    && rowText.Trim().Contains("Remove");
            }
            return false;
        }
    }
}