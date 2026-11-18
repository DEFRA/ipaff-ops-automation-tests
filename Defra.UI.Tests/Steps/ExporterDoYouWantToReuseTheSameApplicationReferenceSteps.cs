using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterDoYouWantToReuseTheSameApplicationReferenceSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterDoYouWantToReuseTheSameApplicationReferencePage? exporterDoYouWantToReuseTheSameApplicationReferencePage =>
            _objectContainer.IsRegistered<IExporterDoYouWantToReuseTheSameApplicationReferencePage>()
                ? _objectContainer.Resolve<IExporterDoYouWantToReuseTheSameApplicationReferencePage>()
                : null;

        public ExporterDoYouWantToReuseTheSameApplicationReferenceSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("the Do you want to reuse the same application reference? page is displayed")]
        public void ThenTheDoYouWantToReuseTheSameApplicationReferencePageIsDisplayed()
        {
            Assert.True(exporterDoYouWantToReuseTheSameApplicationReferencePage?.IsPageLoaded(), "Do you want to reuse the same application reference? page is not displayed");
        }

        [When("the user selects {string} and enters a new reference")]
        public void WhenTheUserSelectsAndEntersANewReference(string option)
        {
            exporterDoYouWantToReuseTheSameApplicationReferencePage?.SelectOption(option);

            // Max 20 characters allowed for the reference
            var reference = DateTime.Now.ToString("ddMMyyyyHHmmss");
            _scenarioContext["Reference"] = reference;
            exporterDoYouWantToReuseTheSameApplicationReferencePage?.EnterNewReference(reference);
        }

        [When("the user clicks the Save and continue button on the Do you want to reuse the same application reference? page")]
        public void WhenTheUserClicksTheSaveAndContinueButtonOnTheDoYouWantToReuseTheSameApplicationReferencePage()
        {
            exporterDoYouWantToReuseTheSameApplicationReferencePage?.ClickSaveAndContinueButton();
        }
    }
}
