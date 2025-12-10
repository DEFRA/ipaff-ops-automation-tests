using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class AdditionalDetailsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IAdditionalDetailsPage? additionalDetailsPage => _objectContainer.IsRegistered<IAdditionalDetailsPage>() ? _objectContainer.Resolve<IAdditionalDetailsPage>() : null;


        public AdditionalDetailsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Additional details page should be displayed")]
        public void ThenTheAdditionalDetailsPageShouldBeDisplayed()
        {
            Assert.True(additionalDetailsPage?.IsPageLoaded(), "Description of the goods Additional details page not loaded");
        }

        [Then("the Additional animal details page should be displayed")]
        public void ThenTheAdditionalAnimalDetailsPageShouldBeDisplayed()
        {
            Assert.True(additionalDetailsPage?.IsAdditionalAnimalDetailsPageLoaded(), "Description of the goods Additional animal details page not loaded");
        }

        [When("the user selects {string} radio button on the Additional details page")]
        public void WhenTheUserSelectsAnyRadioButtonOnTheAdditionalDetailsPage(string option)
        {
            additionalDetailsPage?.ClickImportingProduct(option);
            _scenarioContext.Add("Temperature", option);
        }

        [When("the user selects {string} radio button under Commodity intended for on the Additional details page")]
        public void WhenTheUserSelectsRadioButtonUnderCommodityIntendedForOnTheAdditionalDetailsPage(string commIntendedForOption)
        {
            Assert.True(additionalDetailsPage?.SelectCommodityIntendedForRadio(commIntendedForOption), "Commodity intended for radio is not selected on the Additional details page");
            _scenarioContext.Add("CommodityIntendedFor", commIntendedForOption);
        }

        [When("the user selects {string} for What are the animals certified for?")]
        public void WhenTheUserSelectsForWhatAreTheAnimalsCertifiedFor(string certificationOption)
        {
            additionalDetailsPage?.SelectAnimalCertification(certificationOption);
            _scenarioContext.Add("CertificationOption", certificationOption);
        }

        [When("the user selects {string} for Does the consignment contain any unweaned animals?")]
        public void WhenTheUserSelectsForDoesTheConsignmentContainAnyUnweanedAnimals(string unweanedAnimalsOption)
        {
            additionalDetailsPage?.SelectUnweanedAnimalsOption(unweanedAnimalsOption);
            _scenarioContext.Add("UnweanedAnimalsOption", unweanedAnimalsOption);
        }

    }
}