using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class SelectTheTransporterTypeSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ISelectTheTransporterTypePage? selectTheTransporterTypePage => _objectContainer.IsRegistered<ISelectTheTransporterTypePage>() ? _objectContainer.Resolve<ISelectTheTransporterTypePage>() : null;

        public SelectTheTransporterTypeSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then(@"the Select the transporter type page should be displayed")]
        public void ThenTheTransporterTypeSelectionPageShouldBeDisplayed()
        {
            Assert.True(selectTheTransporterTypePage?.IsPageLoaded(), "Select the transporter type page not loaded");
        }

        [When(@"the user selects transporter type '([^']*)' and clicks Continue")]
        public void WhenTheUserSelectsTransporterTypeAndClicksContinue(string transporterType)
        {
            selectTheTransporterTypePage?.SelectTransporterType(transporterType);
            selectTheTransporterTypePage?.ClickSaveAndContinue();
        }
    }
}