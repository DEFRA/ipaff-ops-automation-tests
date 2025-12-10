using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class AnimalIdentificationDetailsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private IAnimalIdentificationDetailsPage? animalIdentificationDetailsPage => _objectContainer.IsRegistered<IAnimalIdentificationDetailsPage>() ? _objectContainer.Resolve<IAnimalIdentificationDetailsPage>() : null;


        public AnimalIdentificationDetailsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Enter animal identification details page should be displayed")]
        public void ThenTheEnterAnimalIdentificationDetailsPageShouldBeDisplayed()
        {
            Assert.True(animalIdentificationDetailsPage?.IsPageLoaded(), "Enter animal identification details page not loaded");
        }

        [When("the user populates Idenitification details as {string}")]
        public void WhenTheUserPopulatesIdenitificationDetailsAs(string identificationDetails)
        {
            animalIdentificationDetailsPage?.EnterIdentificationDetails(identificationDetails);
        }

        [When("the user populates the Description as {string}")]
        public void WhenTheUserPopulatesTheDescriptionAs(string description)
        {
            animalIdentificationDetailsPage?.EnterDescription(description);
        }
    }
}