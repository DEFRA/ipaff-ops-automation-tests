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

        #region Page Objects
        private IWebElement pageHeading => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Are the commodity rule changes correct?']"), true);
        private IWebElement summaryList => _driver.WaitForElement(By.Id("commodity-information"));
        private IWebElement btnSubmit => _driver.WaitForElement(By.XPath("//button[normalize-space()='Submit']"));
        private IWebElement radioLabelByText(string label) =>
            _driver.WaitForElement(By.XPath($"//label[normalize-space()='{label}']"));
        private By summaryRowsBy => By.XPath(".//div[contains(@class,'govuk-summary-list__row')]");
        private By summaryKeyBy => By.XPath("./dt");
        private By summaryValueBy => By.XPath("./dd");
        #endregion

        public AreCommodityRuleChangesCorrectPage(IObjectContainer container) => _objectContainer = container;

        public bool IsPageLoaded() => pageHeading.Displayed;

        public IDictionary<string, string> GetSummaryDetails()
        {
            var rows = summaryList.FindElements(summaryRowsBy);
            return rows.ToDictionary(
                r => r.FindElement(summaryKeyBy).Text.Trim(),
                r => r.FindElement(summaryValueBy).Text.Trim());
        }

        public void SelectConfirmChangesOption(string optionLabel) => radioLabelByText(optionLabel).Click();
        public void ClickSubmitButton() => btnSubmit.Click();
    }
}