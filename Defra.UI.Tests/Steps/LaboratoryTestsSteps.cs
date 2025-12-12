using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class LaboratoryTestsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ILaboratoryTestsPage? laboratoryTestsPage => _objectContainer.IsRegistered<ILaboratoryTestsPage>() ? _objectContainer.Resolve<ILaboratoryTestsPage>() : null;


        public LaboratoryTestsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Laboratory tests page should be displayed")]
        public void ThenTheLaboratoryTestsPageShouldBeDisplayed()
        {
            Assert.True(laboratoryTestsPage?.IsPageLoaded());
        }

        [When("the user select {string} radio button on the Laboratory tests page")]
        public void WhenISelectRadioButtonOnTheLaboratoryTestsPage(string labTestsOption)
        {
            laboratoryTestsPage?.SelectLabTestsRadio(labTestsOption);
        }
    }
}