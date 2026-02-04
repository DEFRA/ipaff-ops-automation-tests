using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using FluentAssertions.Execution;
using Microsoft.VisualBasic.FileIO;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class DecisionSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IDecisionPage? decisionPage => _objectContainer.IsRegistered<IDecisionPage>() ? _objectContainer.Resolve<IDecisionPage>() : null;


        public DecisionSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Decision page should be displayed")]
        public void ThenTheDecisionPageShouldBeDisplayed()
        {
            Assert.True(decisionPage?.IsPageLoaded(), "Decision page is not displayed");
        }

        [When("the user selects Acceptable for {string} {string}")]
        public void WhenTheUserSelectsAcceptableFor(string acceptableFor, string subOption)
        {
            _scenarioContext["AcceptableFor"] = acceptableFor;
            _scenarioContext["AcceptableForSubOption"] = subOption;
            decisionPage?.SelectNotAcceptableFor(acceptableFor, subOption);
        }

        [Then("the user verifies the Transit radio button option is pre populated")]
        public void ThenTheUserVerifiesTheTransitRadioButtonOptionIsPrePopulated()
        {
            Assert.True(decisionPage?.VerifyTransitRadioButtonPrePopulated());
        }

        [Then("verifies exit BCP Transited country and Destination country are pre populated from part {int}")]
        public void ThenVerifiesExitBCPTransitedCountryAndDestinationCountryArePrePopulatedFromPart(int p0)
        {
            var exitBCP = _scenarioContext.Get<string>("ExitBorderControlPost");
            var transitedCountry = _scenarioContext.Get<string>("TransitedCountry");
            var destinationCountry = _scenarioContext.Get<string>("DestinationCountry");

            Assert.True(decisionPage?.VerifyPrepopulatedTransitDetails(exitBCP, transitedCountry, destinationCountry));
        }

        [When("the user selects {string} {string} in decision page")]
        [When("the user changes the selection to {string} {string} in the decision page")]
        public void WhenTheUserSelectsInDecisionPage(string subOption, string decision)
        {
            _scenarioContext["AcceptableFor"] = decision;
            _scenarioContext["AcceptableForSubOption"] = subOption;
            subOption = subOption == "Use for other purpose" ? "Other" : subOption;
            decisionPage?.SelectDecision(subOption, decision);
        }

        [When("the user provides the reason as {string} for destruction option in decision page")]
        public void WhenTheUserProvidesTheReasonAsForDestructionOptionInDecisionPage(string reason)
        {
            decisionPage?.EnterReason(reason);
        }

        [When("the user enters currendate in decision page")]
        public void WhenTheUserEntersCurrendateInDecisionPage()
        {
            var currentDate = DateTime.Now;
            var day = currentDate.Day.ToString();
            var month = currentDate.Month.ToString();
            var year = currentDate.Year.ToString();
            decisionPage?.EnterCurrentDateInDecisionPage(day, month, year);
        }

        [Then("the main radio option {string} and the sub radio option {string} are selected by default")]
        public void ThenTheMainRadioOptionAndTheSubRadioOptionAreSelectedByDefault(string mainRadio, string subRadio)
        {
            _scenarioContext["AcceptableFor"] = mainRadio;
            _scenarioContext["AcceptableForSubOption"] = subRadio;
            Assert.Multiple(() =>
            {
                Assert.True(decisionPage?.IsAcceptableForRadioSelected(mainRadio), $"The main radio option {mainRadio} is not selected by default on the Decision page");
                Assert.True(decisionPage?.IsInternalMarketSubRadioSelected(subRadio), $"The sub radio option {subRadio} is not selected by default on the Decision page");
            }
            );
        }

        [Then("{string} radio is pre-selected under Acceptable for")]
        public void ThenRadioIsPre_SelectedUnderAcceptableFor(string mainRadio)
        {
            Assert.True(decisionPage?.IsAcceptableForRadioSelected(mainRadio), $"The main radio option {mainRadio} is not selected by default on the Decision page");
        }

        [When("the user enters reason {string} and selects By date")]
        public void WhenTheUserEntersReasonAndSelectsByDate(string reason)
        {
            decisionPage?.EnterReason(reason);
            var currentDate = DateTime.Now;
            var day = currentDate.Day.ToString();
            var month = currentDate.Month.ToString();
            var year = currentDate.Year.ToString();
            decisionPage?.EnterCurrentDateInDecisionPage(day, month, year);
        }

        [Then("the {string} radio button option is pre populated")]
        public void ThenTheRadioButtonOptionIsPrePopulated(string radioButtonOption)
        {
            _scenarioContext["AcceptableFor"] = radioButtonOption;
            Assert.True(decisionPage?.IsRadioButtonPreSelected(radioButtonOption),
                        $"'{radioButtonOption}' radio button is not pre-selected");
        }

        [Then("the exit date is pre populated")]
        public void ThenTheExitDateIsPrePopulated()
        {
            var exitDate = decisionPage?.GetExitDate();
            var expectedExitDate = _scenarioContext.Get<string>("ExitDate");

            // Convert expectedExitDate from "dd MMMM yyyy" to "dd/MM/yyyy" format for comparison
            var parsedDate = DateTime.ParseExact(expectedExitDate, "dd MMMM yyyy", System.Globalization.CultureInfo.InvariantCulture);
            var formattedExpectedDate = parsedDate.ToString("dd/MM/yyyy");

            Assert.That(exitDate, Is.EqualTo(formattedExpectedDate), $"Exit date mismatch. Expected: {formattedExpectedDate}, Actual: {exitDate}");
        }

        [Then("the exit BCP is correct")]
        public void ThenTheExitBCPIsCorrect()
        {
            var actualExitBCP = decisionPage?.GetExitBCP();
            var expectedExitBCP = _scenarioContext.Get<string>("ExitBCP");

            // Extract the name part by finding the last " - " and taking everything before it
            // Example: "London Borough of Hillingdon Heathrow Airport Imported Food Office - ADADA" 
            //          → "London Borough of Hillingdon Heathrow Airport Imported Food Office"
            var lastDashIndex = expectedExitBCP.LastIndexOf(" - ");
            var expectedName = lastDashIndex >= 0
                ? expectedExitBCP.Substring(0, lastDashIndex).Trim()
                : expectedExitBCP;

            Assert.That(actualExitBCP, Is.EqualTo(expectedName),
                $"Exit BCP mismatch. Expected: '{expectedName}', Actual: '{actualExitBCP}'");
        }

        [Then("the exit BCP is prepopulated with value entered in Part 1")]
        public void ThenTheExitBCPIsPrepopulatedWithValueEnteredInPart1()
        {
            var expectedExitBCP = _scenarioContext.Get<string>("ExitBCP");
            var actualExitBCP = decisionPage?.GetTransitExitBCP();

            // Extract the name part by finding the last " - " and taking everything before it
            // Example: "Manchester Airport (animals) - GBMNC4" → "Manchester Airport (animals)"
            // Example: "Heathrow Airport - HARC (animals) - GBLHR4A" → "Heathrow Airport - HARC (animals)"
            var lastDashIndex = expectedExitBCP.LastIndexOf(" - ");
            var expectedName = lastDashIndex >= 0
                ? expectedExitBCP.Substring(0, lastDashIndex).Trim()
                : expectedExitBCP;

            Assert.That(actualExitBCP, Is.EqualTo(expectedName),
                $"Exit BCP is not prepopulated correctly. Expected: '{expectedName}', Actual: '{actualExitBCP}'");
        }

        [Then("the destination country is prepopulated with value entered in Part 1")]
        public void ThenTheDestinationCountryIsPrepopulatedWithValueEnteredInPart1()
        {
            var expectedDestinationCountry = _scenarioContext.Get<string>("DestinationCountry");
            var actualDestinationCountry = decisionPage?.GetDestinationCountry();

            Assert.That(actualDestinationCountry, Is.EqualTo(expectedDestinationCountry),
                $"Destination country is not prepopulated correctly. Expected: '{expectedDestinationCountry}', Actual: '{actualDestinationCountry}'");
        }

        [When("the user enters {string} in reason under Destruction")]
        public void WhenTheUserEntersInReasonUnderDestruction(String reason)
        {
            decisionPage?.EnterDestructionReason(reason);
        }

        [When("the user enters {string} date in By date")]
        public void WhenTheUserEntersDate(string dateString)
        {
            var (day, month, year) = Utils.GetDayMonthYear(dateString);
            decisionPage?.EnterCurrentDateInDecisionPage(day, month, year);            
        }
    }
}