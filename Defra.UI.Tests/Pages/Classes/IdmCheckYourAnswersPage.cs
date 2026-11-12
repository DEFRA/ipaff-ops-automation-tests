using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmCheckYourAnswersPage : IIdmCheckYourAnswersPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Check your answers']"));
        private IWebElement btnComplete => _driver.WaitForElementClickable(By.Id("button-continue"));
        private IWebElement summaryRow(string section, string field) => _driver.WaitForElement(By.XPath($"//h2[normalize-space()='{section}']/following-sibling::div[1]//div[contains(@class,'govuk-summary-list__row')][dt[normalize-space()='{field}']]"));

        private IWebElement summaryValue(string section, string field) => summaryRow(section, field).FindElement(By.CssSelector("dd.govuk-summary-list__value"));
        private IReadOnlyCollection<IWebElement> changeLinks(string section, string field) => summaryRow(section, field).FindElements(By.CssSelector(".govuk-summary-list__actions a[id^='link-change-']"));
        private IReadOnlyCollection<IWebElement> recaptchaElements => _driver.FindElements(By.CssSelector("#recaptcha-anchor, .g-recaptcha, iframe[src*='recaptcha'], iframe[title='reCAPTCHA']"));
        #endregion

        public IdmCheckYourAnswersPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Displayed;
        public string GetSummaryValue(string section, string field) => summaryValue(section, field).Text.Trim();
        public bool HasChangeLink(string section, string field) => changeLinks(section, field).Any(a => a.Displayed);
        public bool IsRecaptchaAbsent() => recaptchaElements.Count == 0;
        public void CompleteRegistration() => btnComplete.Click();
    }
}
