using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class AboutConsignmentSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IAboutConsignmentPage? aboutConsignmentPage => _objectContainer.IsRegistered<IAboutConsignmentPage>() ? _objectContainer.Resolve<IAboutConsignmentPage>() : null;

        public AboutConsignmentSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the About the consignment\\/What are you importing? page should be displayed with radio buttons")]
        public void ThenTheAboutTheConsignmentWhatAreYouImportingPageShouldBeDisplayedWithRadioButtons()
        {
            Assert.True(aboutConsignmentPage?.IsPageLoaded(), "About the consignment What are you importing? page not loaded");
            Assert.True(aboutConsignmentPage?.AreImportOptionsPresent(), "Expected import options are not present on the page.");
        }

        [When("the user chooses {string} option")]
        public void WhenTheUserChoosesOption(string option)
        {
            aboutConsignmentPage?.ClickImportingProduct(option);
            _scenarioContext["ImportType"] = option;
        }

        [Then("the user should be able to click Save and continue")]
        [When("the user clicks Save and continue")]
        [Then("the user clicks Save and continue")]
        public void WhenTheUserClicksSaveAndContinue()
        {
            aboutConsignmentPage?.ClickSaveAndContinue();
        }
    }
}