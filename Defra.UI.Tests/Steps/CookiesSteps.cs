using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CookiesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICookiesPage? cookiesPage => _objectContainer.IsRegistered<ICookiesPage>() ? _objectContainer.Resolve<ICookiesPage>() : null;

        public CookiesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Cookies page should be displayed")]
        public void ThenTheCookiesPageShouldBeDisplayed()
        {
            Assert.True(cookiesPage?.IsPageLoaded(), "Cookies page not loaded");
        }

        [When("the user clicks Import of products, animals, food and feed service link on the header")]
        public void WhenTheUserClicksImportOfProductsAnimalsFoodAndFeedServiceLinkOnTheHeader()
        {
            cookiesPage?.ClickImportOfProduct();
        }
    }
}