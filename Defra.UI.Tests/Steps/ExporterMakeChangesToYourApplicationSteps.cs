using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterMakeChangesToYourApplicationSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterMakeChangesToYourApplicationPage? exporterMakeChangesToYourApplicationPage =>
            _objectContainer.IsRegistered<IExporterMakeChangesToYourApplicationPage>()
                ? _objectContainer.Resolve<IExporterMakeChangesToYourApplicationPage>()
                : null;

        public ExporterMakeChangesToYourApplicationSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("the Make changes to your application page is displayed")]
        public void ThenTheMakeChangesToYourApplicationPageIsDisplayed()
        {
            Assert.True(exporterMakeChangesToYourApplicationPage?.IsPageLoaded(), "Make changes to your application page is not displayed");
        }

        [When("the user clicks the Change link next to the {string} field on the Make changes to your application page")]
        public void WhenTheUserClicksTheChangeLinkNextToTheFieldOnTheMakeChangesToYourApplicationPage(string fieldName)
        {
            exporterMakeChangesToYourApplicationPage?.ClickChangeLink(fieldName);
        }
    }
}
