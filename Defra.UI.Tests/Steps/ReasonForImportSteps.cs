using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class ReasonForImportSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IReasonForImportPage? reasonForImportPage => _objectContainer.IsRegistered<IReasonForImportPage>() ? _objectContainer.Resolve<IReasonForImportPage>() : null;


        public ReasonForImportSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("What is the main reason for importing the consignment? page should be displayed with radio buttons")]
        public void ThenWhatIsTheMainReasonForImportingTheConsignmentPageShouldBeDisplayedWithRadioButtons()
        {
            Assert.True(reasonForImportPage?.IsPageLoaded(), "About the consignment What is the main reason for importing the consignment? page not loaded");
            Assert.True(reasonForImportPage?.AreImportReasonsPresent(), "Expected import options are not present on the page.");
        }

        [Then("What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD")]
        public void ThenWhatIsTheMainReasonForImportingTheConsignmentPageShouldBeDisplayedWithRadioButtonsForCHEDD()
        {
            Assert.True(reasonForImportPage?.IsPageLoaded(), "About the consignment What is the main reason for importing the consignment? page not loaded");
            Assert.True(reasonForImportPage?.AreImportReasonsForCHEDDPresent(), "I port options for CHED D are not displayed on the Reason for import page");
        }

        [When("The user selects {string} radio option")]
        public void WhenTheUserSelectsRadioOption(string reasonForImport)
        {
            reasonForImportPage?.SelectReasonForImport(reasonForImport);
            _scenarioContext.Add("MainReasonForImport", reasonForImport);
        }

        [When("the user chooses {string} and the sub-option {string}")]
        public void WhenTheUserChoosesAndTheSub_Option(string option, string subOption)
        {
            reasonForImportPage?.SelectReasonForImport(option);
            reasonForImportPage?.SelectReasonForImportSubOption(subOption);
            _scenarioContext.Add("MainReasonForImport", option);
            _scenarioContext.Add("Purpose", subOption);
        }

        [Then("What is the main reason for importing the animals? page should be displayed with radio buttons")]
        public void ThenWhatIsTheMainReasonForImportingTheAnimalsPageShouldBeDisplayedWithRadioButtons()
        {
            Assert.True(reasonForImportPage?.IsReasonForImportingAnimalsPageLoaded(), "About the consignment What is the main reason for importing the animals? page not loaded");
            Assert.True(reasonForImportPage?.AreImportAnimalsReasonsPresent(), "Expected import options are not present on the page.");
        }
    }
}