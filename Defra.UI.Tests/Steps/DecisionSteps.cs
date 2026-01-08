using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

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
        public void WhenTheUserSelectsInDecisionPage(string subOption, string decision)
        {
            decisionPage?.SelectDecision(subOption, decision);
            _scenarioContext.Add("RefusalDecision", subOption);
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
            Assert.Multiple(() =>
            {
                Assert.True(decisionPage?.IsAcceptableForRadioSelected(mainRadio), $"The main radio option {mainRadio} is not selected by default on the Decision page");
                Assert.True(decisionPage?.IsInternalMarketSubRadioSelected(subRadio), $"The sub radio option {subRadio} is not selected by default on the Decision page");
            }
            );
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
            var exitBCP = decisionPage?.GetExitBCP();
            var expectedExitBCP = _scenarioContext.Get<string>("ExitBCP");

            Assert.That(exitBCP, Does.Contain(expectedExitBCP), $"Exit BCP mismatch. Expected: {expectedExitBCP}, Actual: {exitBCP}");
        }

        [Then("the exit BCP is prepopulated with value entered in Part 1")]
        public void ThenTheExitBCPIsPrepopulatedWithValueEnteredInPart1()
        {
            var expectedExitBCP = _scenarioContext.Get<string>("ExitBCP");
            var actualExitBCP = decisionPage?.GetTransitExitBCP();

            // Extract the first word from expected (e.g., "Manchester" from "Manchester Airport (animals) - GBMNC4")
            var expectedFirstWord = expectedExitBCP.Split(' ')[0];

            Assert.That(actualExitBCP, Does.Contain(expectedFirstWord),
                $"Exit BCP is not prepopulated correctly. Expected to contain: '{expectedFirstWord}', Actual: '{actualExitBCP}'");
        }

        [Then("the destination country is prepopulated with value entered in Part 1")]
        public void ThenTheDestinationCountryIsPrepopulatedWithValueEnteredInPart1()
        {
            var expectedDestinationCountry = _scenarioContext.Get<string>("DestinationCountry");
            var actualDestinationCountry = decisionPage?.GetDestinationCountry();

            Assert.That(actualDestinationCountry, Is.EqualTo(expectedDestinationCountry),
                $"Destination country is not prepopulated correctly. Expected: '{expectedDestinationCountry}', Actual: '{actualDestinationCountry}'");
        }
    }
}