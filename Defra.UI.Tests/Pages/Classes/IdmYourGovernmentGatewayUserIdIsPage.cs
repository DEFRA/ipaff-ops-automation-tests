using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;
using System.Text.RegularExpressions;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmYourGovernmentGatewayUserIdIsPage : IIdmYourGovernmentGatewayUserIdIsPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private IWebElement pageTitle => _driver.WaitForElement(By.CssSelector(".govuk-panel--confirmation h1"));
        private IWebElement lblGatewayId => _driver.WaitForElement(By.CssSelector(".govuk-panel--confirmation .govuk-panel__body"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("continue"));

        public IdmYourGovernmentGatewayUserIdIsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Text.Contains("Your Government Gateway user ID is");
        public string GetGatewayId() => Regex.Replace(lblGatewayId.Text, @"\s", "");
        public void ClickContinueButton() => btnContinue.Click();
    }
}
