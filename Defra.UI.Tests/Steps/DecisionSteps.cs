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
    }
}