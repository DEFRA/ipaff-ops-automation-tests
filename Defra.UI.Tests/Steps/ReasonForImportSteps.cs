using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
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
            _scenarioContext["MainReasonForImport"] = option;
            if (!string.IsNullOrWhiteSpace(subOption))
            {
                reasonForImportPage?.SelectReasonForImportSubOption(subOption);
                _scenarioContext["Purpose"] = subOption;
            }
        }

        [When("the user chooses {string} as the main reason for importing the consignment")]
        public void WhenTheUserChoosesAsTheMainReasonForImportingTheConsignment(string option)
        {
            reasonForImportPage?.SelectReasonForImport(option);
            _scenarioContext["MainReasonForImport"] = option;
        }

        [When("the user chooses exit BCP {string} transited country {string} and destination country {string}")]
        public void WhenTheUserChoosesBCPTransitedCountryAndDestinationCountry(string exitBCP, string transitedCountry, string destinationCountry)
        {
            reasonForImportPage?.SelectExitBorderControlPost(exitBCP);
            _scenarioContext.Add("ExitBorderControlPost", exitBCP);

            var futureDate = DateTime.Now.AddDays(7);
            reasonForImportPage?.EnterConsignmentLeavingDate(futureDate.Day.ToString(), futureDate.Month.ToString(), futureDate.Year.ToString());
            var leavingFromGBDate = futureDate.ToString("dd MMMM yyyy");
            _scenarioContext.Add("ConsignmentLeavingFromGBDate", leavingFromGBDate);

            var hours = futureDate.Hour.ToString();
            var minutes = futureDate.Minute.ToString();
            var formattedTime = futureDate.ToString("HH:mm");
            reasonForImportPage?.EnterConsignmentLeavingTime(hours, minutes);
            _scenarioContext.Add("ConsignmentLeavingFromGBTime", formattedTime);

            reasonForImportPage?.SelectTransitedCountry(transitedCountry);
            _scenarioContext.Add("TransitedCountry", transitedCountry);

            reasonForImportPage?.SelectDestinationCountry(destinationCountry);
            _scenarioContext.Add("DestinationCountry", destinationCountry);
        }

        [Then("What is the main reason for importing the animals? page should be displayed with radio buttons")]
        public void ThenWhatIsTheMainReasonForImportingTheAnimalsPageShouldBeDisplayedWithRadioButtons()
        {
            Assert.True(reasonForImportPage?.IsReasonForImportingAnimalsPageLoaded(), "About the consignment What is the main reason for importing the animals? page not loaded");
            Assert.True(reasonForImportPage?.AreImportAnimalsReasonsPresent(), "Expected import options are not present on the page.");
        }

        [When("the user chooses destination country {string}")]
        public void WhenTheUserChoosesDestinationCountry(string transhipmentCountry)
        {
            reasonForImportPage?.SelectTranshipmentDestination(transhipmentCountry);
            _scenarioContext.Add("TranshipmentDestinationCountry", transhipmentCountry);
        }

        [When("the user enters exit date {string} days from today")]
        public void WhenTheUserEntersExitDateDaysFromToday(string daysFromToday)
        {
            int days = int.Parse(daysFromToday);
            reasonForImportPage?.EnterExitDate(days);

            var exitDate = DateTime.Now.AddDays(days);
            var formattedExitDate = exitDate.ToString("dd MMMM yyyy");
            _scenarioContext.Add("ExitDate", formattedExitDate);
        }

        [When("the user selects exit BCP {string}")]
        public void WhenTheUserSelectsExitBCP(string exitBCP)
        {
            reasonForImportPage?.SelectExitBCPBasedOnContext(exitBCP);
            _scenarioContext.Add("ExitBCP", exitBCP);
        }

        [When("the user verifies {string} radio button exists with {int} sub-options for {string}")]
        public void WhenTheUserVerifiesRadioButtonExistsWithSubOptionsFor(string radioButton, int subOptionCount, string subOptionSection)
        {
            reasonForImportPage?.SelectReasonForImport(radioButton);
            Assert.True(reasonForImportPage?.VerifyInternalMarketHasSubOptions(subOptionCount),
                $"{radioButton} should have {subOptionCount} sub-options for {subOptionSection}");
        }

        [When("the user verifies {string} radio button exists with {string} dropdown")]
        public void WhenTheUserVerifiesRadioButtonExistsWithDropdown(string radioButton, string dropdownName)
        {
            reasonForImportPage?.SelectReasonForImport(radioButton);
            Assert.True(reasonForImportPage?.VerifyTranshipmentHasDestinationCountryDropdown(),
                $"{radioButton} should have {dropdownName} dropdown");
        }

        [When("the user verifies {string} radio button exists with {string} dropdown and {string} dropdown")]
        public void WhenTheUserVerifiesRadioButtonExistsWithTwoDropdowns(string radioButton, string dropdown1, string dropdown2)
        {
            reasonForImportPage?.SelectReasonForImport(radioButton);
            Assert.True(reasonForImportPage?.VerifyTransitHasExitBCPAndDestinationDropdowns(),
                $"{radioButton} should have {dropdown1} and {dropdown2} dropdowns");
        }

        [When("the user verifies {string} radio button exists with no sub-options")]
        public void WhenTheUserVerifiesRadioButtonExistsWithNoSubOptions(string radioButton)
        {
            reasonForImportPage?.SelectReasonForImport(radioButton);
            Assert.True(reasonForImportPage?.VerifyReentryHasNoSubOptions(),
                $"{radioButton} should exist and have no sub-options");
        }

        [When("the user verifies {string} radio button exists with {string} fields and {string} dropdown")]
        public void WhenTheUserVerifiesRadioButtonExistsWithFieldsAndDropdown(string radioButton, string fields, string dropdown)
        {
            reasonForImportPage?.SelectReasonForImport(radioButton);
            Assert.True(reasonForImportPage?.VerifyTemporaryAdmissionHasExitDateAndBCPDropdown(),
                $"{radioButton} should have {fields} fields and {dropdown} dropdown");
        }

        [When("the user selects destination country {string}")]
        public void WhenTheUserSelectsDestinationCountry(string destinationCountry)
        {
            reasonForImportPage?.SelectDestinationCountryBasedOnContext(destinationCountry);
            _scenarioContext.Add("DestinationCountry", destinationCountry);
        }
    }
}