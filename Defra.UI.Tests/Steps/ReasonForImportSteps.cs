using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using System.Globalization;


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


        [When("the user chooses {string} and the sub-option {string}")]
        public void WhenTheUserChoosesAndTheSub_Option(string option, string subOption)
        {
            reasonForImportPage?.SelectReasonForImport(option);
            _scenarioContext.Add("MainReasonForImport", option);
            if (!string.IsNullOrWhiteSpace(subOption))
            {
                reasonForImportPage?.SelectReasonForImportSubOption(subOption);
                _scenarioContext.Add("Purpose", subOption);
            }
        }

        [When("the user chooses {string} as the main reason for importing the consignment")]
        public void WhenTheUserChoosesAsTheMainReasonForImportingTheConsignment(string option)
        {
            reasonForImportPage?.SelectReasonForImport(option);
            _scenarioContext.Add("MainReasonForImport", option);
        }

        [When("the user chooses exit BCP {string} time {string} transited country {string} and destination country {string}")]
        public void WhenTheUserChoosesBCPTimeTransitedCountryAndDestinationCountry(string exitBCP, string timeString, string transitedCountry, string destinationCountry)
        {
            reasonForImportPage?.SelectExitBorderControlPost(exitBCP);
            _scenarioContext.Add("ExitBorderControlPost", exitBCP);

            DateTime futureDate = DateTime.Now.AddDays(7);
            reasonForImportPage?.EnterConsignmentLeavingDate(futureDate.Day.ToString(), futureDate.Month.ToString(), futureDate.Year.ToString());
            var monthName = futureDate.ToString("MMMM", CultureInfo.InvariantCulture);
            var leavingFromGBDate = futureDate.Day.ToString() + " " + monthName + " " + futureDate.Year.ToString();
            _scenarioContext.Add("ConsignmentLeavingFromGBDate", leavingFromGBDate);

            var hours = timeString.Substring(0,2);
            var minutes = timeString.Substring(3, 2);
            reasonForImportPage?.EnterConsignmentLeavingTime(hours, minutes);
            _scenarioContext.Add("ConsignmentLeavingFromGBTime", timeString);

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
    }
}