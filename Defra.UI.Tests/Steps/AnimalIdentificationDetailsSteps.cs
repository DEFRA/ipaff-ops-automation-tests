using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
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

        [When("the user populates the Horse name as {string}")]
        public void WhenTheUserPopulatesTheHorseNameAs(string horseName)
        {
            animalIdentificationDetailsPage?.EnterHorseName(horseName);
            _scenarioContext["HorseName"] = horseName;
        }

        [When("the user populates the Microchip number as {string}")]
        public void WhenTheUserPopulatesTheMicrochipNumberAs(string microchipNumber)
        {
            animalIdentificationDetailsPage?.EnterMicrochipNumber(microchipNumber);
            _scenarioContext["MicrochipNumber"]=microchipNumber;
        }

        [When("the user populates the Passport number as {string}")]
        public void WhenTheUserPopulatesThePassportNumberAs(string passportNumber)
        {
            animalIdentificationDetailsPage?.EnterPassportNumber(passportNumber);
            _scenarioContext["PassportNumber"] = passportNumber;
        }
    }
}