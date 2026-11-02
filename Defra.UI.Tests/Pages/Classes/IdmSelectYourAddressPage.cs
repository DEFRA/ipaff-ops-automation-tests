using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmSelectYourAddressPage : IIdmSelectYourAddressPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private By pageTitleBy => By.XPath("//h1[normalize-space()='Select your address']");
        private IWebElement pageTitle => _driver.WaitForElement(pageTitleBy);
        private IWebElement ddlAddress => _driver.WaitForElement(By.Id("address"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("button-continue"));

        public IdmSelectYourAddressPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Displayed;
        public bool IsDisplayed() => _driver.FindElements(pageTitleBy).Any(element => element.Displayed);
        public bool WaitForPage(int timeoutSeconds)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds));
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                return wait.Until(_ => IsDisplayed());
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public string SelectAddressAndContinue(string postcode)
        {
            var dropdown = new SelectElement(ddlAddress);
            var options = dropdown.Options.Where(o => o.Enabled && !string.IsNullOrWhiteSpace(o.GetAttribute("value")) && o.Text.Contains(postcode, StringComparison.OrdinalIgnoreCase)).ToList();
            if (options.Count == 0)
                throw new InvalidOperationException($"No address was returned for postcode {postcode}");
            var option = options[Random.Shared.Next(options.Count)];
            dropdown.SelectByValue(option.GetAttribute("value"));
            var address = dropdown.SelectedOption.Text.Trim();
            btnContinue.Click();
            return address;
        }
    }
}
