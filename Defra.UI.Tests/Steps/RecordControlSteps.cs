using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class RecordControlSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IRecordControlPage? recordControlPage => _objectContainer.IsRegistered<IRecordControlPage>() ? _objectContainer.Resolve<IRecordControlPage>() : null;

        public RecordControlSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("Record control page should be displayed")]
        public void ThenRecordControlPageShouldBeDisplayed()
        {
            Assert.True(recordControlPage?.IsPageLoaded(), "Record control page is not displayed");
        }

        [When("the user selects {string} for Did the consignment leave the UK?")]
        public void WhenTheUserSelectsForDidTheConsignmentLeaveTheUK(string consignmentLeaveOption)
        {
            recordControlPage?.SelectConsignmentLeaveRadio(consignmentLeaveOption);
        }

        [When("the user selects {string} as Means of transport")]
        public void WhenTheUserSelectsAsMeansOfTransport(string transport)
        {
            recordControlPage?.SelectMeansOfTransport(transport);
        }

        [When("the user enters {string} as Identification")]
        public void WhenTheUserEntersAsIdentification(string identification)
        {
            recordControlPage?.EnterIdentification(identification);
        }

        [When("the user enters {string} in Documentaion")]
        public void WhenTheUserEntersInDocumentaion(string documentation)
        {
            recordControlPage?.EnterDocumentation(documentation);
        }

        [When("the user selects Date of departure using date picker")]
        public void WhenTheUserSelectsDateOfDepartureUsingDatePicker()
        {
            recordControlPage?.SelectDateFromDatePicker();
        }

        [When("the user selects {string} as Exit BCP")]
        public void WhenTheUserSelectsAsExitBCP(string bcp)
        {
            recordControlPage?.SelectExitBCP(bcp);
        }

        [When("the user selects {string} as Destination country")]
        public void WhenTheUserSelectsAsDestinationCountry(string destination)
        {
            recordControlPage?.SelectDestinationCountry(destination);
        }

        [When("the user clicks the Submit control button")]
        public void WhenTheUserClicksTheSubmitControlButton()
        {
            recordControlPage?.ClickSubmitControlButton();
        }
    }
}