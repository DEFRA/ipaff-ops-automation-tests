using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class AreYouAPlantsImporterOrAgencySteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IAreYouAPlantsImporterOrAgencyPage? areYouAPlantsImporterOrAgencyPage => _objectContainer.IsRegistered<IAreYouAPlantsImporterOrAgencyPage>() ? _objectContainer.Resolve<IAreYouAPlantsImporterOrAgencyPage>() : null;

        public AreYouAPlantsImporterOrAgencySteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Are you a plants importer or agency? page should be displayed")]
        public void ThenTheAreYouAPlantsImporterOrAgencyPageShouldBeDisplayed()
        {
            Assert.True(areYouAPlantsImporterOrAgencyPage?.IsPageLoaded(), "Are you a plants importer or agency? page not loaded");
        }

        [When("the user selects Yes and clicks Continue")]
        public void WhenTheUserSelectsYesAndClicksContinue()
        {
            areYouAPlantsImporterOrAgencyPage?.SelectYesAndClickContinue();
        }
    }
}