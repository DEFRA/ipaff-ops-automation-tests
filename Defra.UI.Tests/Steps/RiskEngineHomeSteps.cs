using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class RiskEngineHomeSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IRiskEngineHomePage? riskEngineHomePage =>
            _objectContainer.IsRegistered<IRiskEngineHomePage>()
                ? _objectContainer.Resolve<IRiskEngineHomePage>()
                : null;

        public RiskEngineHomeSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Given("that I navigate to the Risk Engine application")]
        [When("I navigate to the Risk Engine application")]
        public void GivenThatINavigateToTheRiskEngineApplication()
        {
            var riskEngineConfig = ConfigSetup.BaseConfiguration.RiskEngineAdmin;
            var driver = _objectContainer.Resolve<IWebDriver>();
            driver.Navigate().GoToUrl(riskEngineConfig.Url);
        }

        [Then("the Risk Engine Home page should be displayed")]
        public void ThenTheRiskEngineHomePageShouldBeDisplayed()
        {
            Assert.True(riskEngineHomePage?.IsPageLoaded(), "Risk Engine Home page is not displayed");
        }

        [When("the user clicks the {string} link from the Risk Engine header menu")]
        public void WhenTheUserClicksTheLinkFromTheRiskEngineHeaderMenu(string linkText)
        {
            riskEngineHomePage?.ClickHeaderLink(linkText);
        }
    }
}