using Defra.UI.Tests.Pages.Classes;
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
            _scenarioContext["MicrochipNumber"] = microchipNumber;
        }

        [When("the user populates the Passport number as {string}")]
        public void WhenTheUserPopulatesThePassportNumberAs(string passportNumber)
        {
            animalIdentificationDetailsPage?.EnterPassportNumber(passportNumber);
            _scenarioContext["PassportNumber"] = passportNumber;
        }

        [When("the user populates the Ear tag as {string}")]
        public void WhenTheUserPopulatesTheEarTagAs(string earTag)
        {
            animalIdentificationDetailsPage?.EnterEarTag(earTag);
            _scenarioContext["EarTag"] = earTag;
        }

        [Then("the Ear tag should not be copied from the original notification")]
        public void ThenTheEarTagShouldNotBeCopiedFromTheOriginalNotification()
        {
            string actualEarTag = animalIdentificationDetailsPage?.GetEarTag ?? string.Empty;

            Assert.That(actualEarTag, Is.Empty,
                $"Ear tag should not be copied from the original notification, but found '{actualEarTag}'");
        }

        [Then("Number of animals should be displayed as {string} in the animal identification details page")]
        public void ThenNumberOfAnimalsShouldBeDisplayedAsInTheAnimalIdentificationDetailsPage(string expectedNumberOfAnimals)
        {
            string actualNumberOfAnimals = animalIdentificationDetailsPage?.GetNumberOfAnimals() ?? string.Empty;

            Assert.That(actualNumberOfAnimals, Is.EqualTo(expectedNumberOfAnimals),
                $"Expected number of animals to be '{expectedNumberOfAnimals}', but found '{actualNumberOfAnimals}'");
        }

        [When("the user populates the identification details for all species")]
        public void WhenTheUserPopulatesTheIdentificationDetailsForAllSpecies()
        {
            var multiSpecies = _scenarioContext.GetOrCreateMultiSpeciesData();
            var speciesList = _scenarioContext.GetFromContext<List<string>>("Species", []);

            foreach (var species in speciesList)
            {
                var speciesData = multiSpecies.GetOrCreateSpecies(species);
                var numberOfAnimals = int.TryParse(speciesData.NumberOfAnimals, out var n) ? n : 1;

                for (int animalIndex = 1; animalIndex <= numberOfAnimals; animalIndex++)
                {
                    if (animalIndex > 1)
                    {
                        animalIdentificationDetailsPage?.ClickAddAnotherForSpecies(species);
                    }

                    var microchip = $"MC-{species[..3].ToUpper()}-{animalIndex:D3}";
                    var passport = $"PP-{species[..3].ToUpper()}-{animalIndex:D3}";
                    var tattoo = $"TT-{species[..3].ToUpper()}-{animalIndex:D3}";

                    animalIdentificationDetailsPage?.EnterIdentificationForSpecies(species, animalIndex, "microchip", microchip);
                    animalIdentificationDetailsPage?.EnterIdentificationForSpecies(species, animalIndex, "passport", passport);
                    animalIdentificationDetailsPage?.EnterIdentificationForSpecies(species, animalIndex, "tattoo", tattoo);

                    // Store in unified model
                    var animal = speciesData.GetOrCreateAnimal(animalIndex);
                    animal.Identification.Microchip = microchip;
                    animal.Identification.Passport = passport;
                    animal.Identification.Tattoo = tattoo;
                }
            }

            _scenarioContext["IdentificationDetailsPopulated"] = true;
        }

        [When("the user populates the Microchip number as {string} for the species {string} animal {int}")]
        public void WhenTheUserPopulatesTheMicrochipNumberAsForTheSpeciesAnimal(string microchipNumber, string species, int animalIndex)
        {
            animalIdentificationDetailsPage?.EnterIdentificationForSpecies(species, animalIndex, "microchip", microchipNumber);
            StoreSpeciesIdentification(species, animalIndex, "Microchip", microchipNumber);
        }

        [When("the user populates the Passport number as {string} for the species {string} animal {int}")]
        public void WhenTheUserPopulatesThePassportNumberAsForTheSpeciesAnimal(string passportNumber, string species, int animalIndex)
        {
            animalIdentificationDetailsPage?.EnterIdentificationForSpecies(species, animalIndex, "passport", passportNumber);
            StoreSpeciesIdentification(species, animalIndex, "Passport", passportNumber);
        }

        [When("the user populates the Tattoo as {string} for the species {string} animal {int}")]
        public void WhenTheUserPopulatesTheTattooAsForTheSpeciesAnimal(string tattoo, string species, int animalIndex)
        {
            animalIdentificationDetailsPage?.EnterIdentificationForSpecies(species, animalIndex, "tattoo", tattoo);
            StoreSpeciesIdentification(species, animalIndex, "Tattoo", tattoo);
        }

        [When("the user clicks Add another for the species {string}")]
        public void WhenTheUserClicksAddAnotherForTheSpecies(string species)
        {
            animalIdentificationDetailsPage?.ClickAddAnotherForSpecies(species);
        }

        private void StoreSpeciesIdentification(string species, int animalIndex, string fieldType, string value)
        {
            // Store in unified model
            var multiSpecies = _scenarioContext.GetOrCreateMultiSpeciesData();
            var animal = multiSpecies.GetOrCreateSpecies(species).GetOrCreateAnimal(animalIndex);

            switch (fieldType)
            {
                case "Microchip": animal.Identification.Microchip = value; break;
                case "Passport": animal.Identification.Passport = value; break;
                case "Tattoo": animal.Identification.Tattoo = value; break;
            }
        }
    }
}