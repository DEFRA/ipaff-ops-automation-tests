using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class PermanentAddressesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IPermanentAddressesPage? permanentAddressesPage => _objectContainer.IsRegistered<IPermanentAddressesPage>()
            ? _objectContainer.Resolve<IPermanentAddressesPage>()
            : null;

        public PermanentAddressesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Permanent addresses for these animals page should be displayed")]
        public void ThenThePermanentAddressesForTheseAnimalsPageShouldBeDisplayed()
        {
            Assert.True(permanentAddressesPage?.IsPageLoaded(),
                "Permanent addresses for these animals page was not loaded");
        }

        [When("the user selects {string} for each animal")]
        public void WhenTheUserSelectsForEachAnimal(string option)
        {
            permanentAddressesPage?.SelectAddressOptionForAllAnimals(option);
        }

        [When("the user enters the permanent address details for the commodity {string} {int}")]
        public void WhenTheUserEntersThePermanentAddressDetailsForTheCommodity(string species, int animalIndex)
        {
            var details = permanentAddressesPage?.EnterPermanentAddressForAnimal(species, animalIndex);

            if (details != null)
            {
                // Store in unified model
                var multiSpecies = _scenarioContext.GetOrCreateMultiSpeciesData();
                multiSpecies.GetOrCreateSpecies(species).GetOrCreateAnimal(animalIndex).PermanentAddress = details;
            }
        }
    }
}