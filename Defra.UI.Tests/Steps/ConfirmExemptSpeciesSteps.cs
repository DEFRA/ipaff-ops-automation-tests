using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class ConfirmExemptSpeciesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IConfirmExemptSpeciesPage? confirmExemptSpeciesPage => _objectContainer.IsRegistered<IConfirmExemptSpeciesPage>() ? _objectContainer.Resolve<IConfirmExemptSpeciesPage>() : null;

        public ConfirmExemptSpeciesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("Confirm exempt species page should be displayed")]
        public void ThenConfirmExemptSpeciesPageShouldBeDisplayed()
        {
            Assert.True(confirmExemptSpeciesPage?.IsPageLoaded("Confirm exempt species"), "Confirm exempt species page not loaded");
        }

        [Then("the user selected {string} option")]
        public void ThenTheUserSelectedOption(string option)
        {
            confirmExemptSpeciesPage?.SelectSpeciesOption(option);
            Utils.AppendStringToScenarioContextArray(_scenarioContext, "SpeciesExemptFromIUU", option);
        }
    }
}
