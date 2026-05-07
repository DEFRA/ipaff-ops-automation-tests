using System.Collections.Generic;
using System.Linq;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class AreCommodityRuleChangesCorrectPage : IAreCommodityRuleChangesCorrectPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        private IWebElement pageHeading => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Are the commodity rule changes correct?']"), true);
        private IWebElement summaryList => _driver.WaitForElement(By.Id("commodity-information"));
        private IWebElement btnSubmit => _driver.WaitForElement(By.XPath("//button[normalize-space()='Submit']"));
        private IWebElement radioLabelByText(string label) =>
            _driver.WaitForElement(By.XPath($"//label[normalize-space()='{label}']"));

        public AreCommodityRuleChangesCorrectPage(IObjectContainer container) => _objectContainer = container;

        public bool IsPageLoaded() => pageHeading.Displayed;

        public IDictionary<string, string> GetSummaryDetails()
        {
            var rows = summaryList.FindElements(By.XPath(".//div[contains(@class,'govuk-summary-list__row')]"));
            return rows.ToDictionary(
                r => r.FindElement(By.XPath("./dt")).Text.Trim(),
                r => r.FindElement(By.XPath("./dd")).Text.Trim());
        }

        public void SelectConfirmChangesOption(string optionLabel) => radioLabelByText(optionLabel).Click();
        public void ClickSubmitButton() => btnSubmit.Click();
    }
}