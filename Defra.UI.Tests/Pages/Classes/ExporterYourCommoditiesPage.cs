using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterYourCommoditiesPage : IExporterYourCommoditiesPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Your commodities']"), true);
        private IWebElement tblCommodities => _driver.WaitForElement(By.Id("commodity-table"), true);
        private IWebElement finishedAddingOption(string option) => _driver.WaitForElement(By.XPath($"//label[contains(normalize-space(),'{option}')]"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));
        private IReadOnlyCollection<IWebElement> tblCommodityHeaders => _driver.WaitForElements(By.XPath("//table[@id='commodity-table']//th"));
        private IWebElement addMoreQuestion(string question) => _driver.WaitForElement(By.XPath($"//h2[contains(@class,'govuk-fieldset__heading') and normalize-space()='{question}']"), true);
        private IReadOnlyCollection<IWebElement> rdoAddMoreCommodities => _driver.FindElements(By.XPath("//input[@name='add_more_commodities' and @type='radio']"));
        private By btnSaveAndContinueLaterBy => By.Id("Button-SaveAndResumeLater");
        #endregion

        public ExporterYourCommoditiesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Your commodities");

        public bool IsCommodityLineDisplayed(string commodityName) => tblCommodities.Text.Contains(commodityName);

        public void SelectFinishedAddingCommoditiesOption(string option) => finishedAddingOption(option).Click();

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();

        public List<string> GetCommodityTableHeaders() =>
            tblCommodityHeaders.Select(h => System.Text.RegularExpressions.Regex.Replace(h.Text, @"\s+", " ").Trim()).ToList();

        public bool IsAddMoreQuestionDisplayed(string question) => addMoreQuestion(question).Displayed;

        public bool AreAddMoreRadioButtonsPresent() => rdoAddMoreCommodities.Count > 0;

        public bool IsSaveAndContinueLaterDisplayed() => _driver.IsElementDisplayed(btnSaveAndContinueLaterBy);
    }
}