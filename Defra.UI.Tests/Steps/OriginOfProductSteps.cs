using Reqnroll.BoDi;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class OriginOfProductSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private IOriginOfProductPage? originOfProductPage => _objectContainer.IsRegistered<IOriginOfProductPage>() ? _objectContainer.Resolve<IOriginOfProductPage>() : null;


        public OriginOfProductSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Origin of the animal or product page should be displayed")]
        [Then("the Origin of the plants plant product or other objects page should be displayed")]
        public void ThenTheOriginOfThePlantsPlantProductOrOtherObjectsPageShouldBeDisplayed()
        {
            Assert.True(originOfProductPage?.IsPageLoaded(), "About the consignment Origin of the animal or product? page not loaded");
        }

        [When("the user chooses {string} from the dropdown for Country of origin")]
        public void WhenTheUserChoosesFromTheDropdownForCountryOfOrigin(string country)
        {
            originOfProductPage?.SelectCountryOfOrigin(country);
            _scenarioContext.Add("CountryOfOrigin", country);
        }
    }
}