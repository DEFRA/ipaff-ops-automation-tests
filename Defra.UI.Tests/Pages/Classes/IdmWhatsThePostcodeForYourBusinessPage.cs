using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmWhatsThePostcodeForYourBusinessPage : IIdmWhatsThePostcodeForYourBusinessPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private By pageTitleBy => By.XPath("//h1[normalize-space()='What’s the postcode for your business?']");
        private IWebElement pageTitle => _driver.WaitForElement(pageTitleBy);
        private IWebElement txtPostcode => _driver.WaitForElement(By.Id("postcode"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("button-continue"));

        public IdmWhatsThePostcodeForYourBusinessPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Displayed;
        public bool IsDisplayed() => _driver.FindElements(pageTitleBy).Any(element => element.Displayed);
        public bool IsErrorSummaryDisplayed() => _driver.FindElements(By.CssSelector("div.govuk-error-summary")).Any(element => element.Displayed);
        public string CurrentUrl => _driver.Url;

        public bool ReloadPostcodePage(string url)
        {
            _driver.Navigate().GoToUrl(url);
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                return wait.Until(_ => IsDisplayed());
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public void EnterPostcodeAndContinue(string postcode)
        {
            txtPostcode.Clear();
            txtPostcode.SendKeys(postcode);
            btnContinue.Click();
        }
    }
}
