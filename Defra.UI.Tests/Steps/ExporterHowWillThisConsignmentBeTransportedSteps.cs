using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterHowWillThisConsignmentBeTransportedSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterHowWillThisConsignmentBeTransportedPage? exporterHowWillThisConsignmentBeTransportedPage =>
            _objectContainer.IsRegistered<IExporterHowWillThisConsignmentBeTransportedPage>()
                ? _objectContainer.Resolve<IExporterHowWillThisConsignmentBeTransportedPage>()
                : null;

        public ExporterHowWillThisConsignmentBeTransportedSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the How will this consignment be transported? page is displayed")]
        public void ThenTheHowWillThisConsignmentBeTransportedPageIsDisplayed()
        {
            Assert.True(exporterHowWillThisConsignmentBeTransportedPage?.IsPageLoaded(), "How will this consignment be transported? page is not displayed");
        }

        [When("the user selects a transport method and clicks the Save and continue button")]
        public void WhenTheUserSelectsATransportMethodAndClicksTheSaveAndContinueButton()
        {
            exporterHowWillThisConsignmentBeTransportedPage?.SelectAnyTransportMethod();
            exporterHowWillThisConsignmentBeTransportedPage?.ClickSaveAndContinueButton();
        }

        [When("the user enters the value of the consignment in pounds {float}")]
        public void WhenTheUserEntersTheValueOfTheConsignmentInPounds(decimal value)
        {
            _scenarioContext["ExporterConsignmentValue"] = value.ToString("0.00");
            exporterHowWillThisConsignmentBeTransportedPage?.EnterConsignmentValue(value);
        }

        [When("the user selects {string} for Do you know the point of entry?")]
        public void WhenTheUserSelectsForDoYouKnowThePointOfEntry(string option)
        {
            exporterHowWillThisConsignmentBeTransportedPage?.SelectDoYouKnowThePointOfEntry(option);
        }

        [When("the user enters the point of entry {string}")]
        public void WhenTheUserEntersThePointOfEntry(string pointOfEntry)
        {
            _scenarioContext["ExporterPointOfEntry"] = pointOfEntry;
            exporterHowWillThisConsignmentBeTransportedPage?.EnterPointOfEntry(pointOfEntry);
        }

        [When("the user selects a transport method {string}")]
        public void WhenTheUserSelectsATransportMethod(string method)
        {
            _scenarioContext["ExporterTransportMode"] = method;
            exporterHowWillThisConsignmentBeTransportedPage?.SelectTransportMethod(method);
        }

        [When("the user clicks the Save and continue button on the How will this consignment be transported? page")]
        public void WhenTheUserClicksTheSaveAndContinueButtonOnTheHowWillThisConsignmentBeTransportedPage()
        {
            exporterHowWillThisConsignmentBeTransportedPage?.ClickSaveAndContinueButton();
        }
    }
}