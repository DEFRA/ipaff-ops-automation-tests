using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class PortOfEntrySteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IPortOfEntryPage? portOfEntryPage => _objectContainer.IsRegistered<IPortOfEntryPage>() ? _objectContainer.Resolve<IPortOfEntryPage>() : null;


        public PortOfEntrySteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Transport to the port of entry page should be displayed")]
        public void ThenTheTransportToThePortOfEntryPageShouldBeDisplayed()
        {
            Assert.True(portOfEntryPage?.IsPageLoaded(), "Transport Transport to the port of entry page not loaded");
        }

        [Then("the Transport to the BCP or Port of entry page should be displayed")]
        public void ThenTheTransportToTheBCPOrPortOfEntryPageShouldBeDisplayed()
        {
            Assert.True(portOfEntryPage?.IsBCPOrPortOfEntryPageLoaded(), "Transport Transport to the BCP or port of entry page not loaded");
        }

        [When("the user populates the transport details {string} {string} {string} {string} {string}")]
        public void WhenTheUserPopulatesTheTransportDetails(string port, string option, string mode, string transId, string DocumentRef)
        {
            var futureDate = DateTime.Now.AddDays(5);
            var day = futureDate.Day.ToString();
            var month = futureDate.Month.ToString();
            var year = futureDate.Year.ToString();
            var formattedDate = futureDate.ToString("dd MMM yyyy");

            var journeyTimeHours = "8"; // Default journey time for CHED-A
            var formattedTime = futureDate.ToString("HH:mm");
            var hour = futureDate.Hour.ToString();
            var minutes = futureDate.Minute.ToString();

            portOfEntryPage?.EnterPortOfEntry(port);
            portOfEntryPage?.SelectMeansOfTransport(mode);
            portOfEntryPage?.EnterTransportId(transId);
            portOfEntryPage?.EnterTransportDocRef(DocumentRef);
            portOfEntryPage?.EnterEstimatedArrivalDate(day, month, year);
            portOfEntryPage?.EnterEstimatedArrivalTime(hour, minutes);

            // CHED-A specific field - only filled if present
            portOfEntryPage?.EnterEstimatedJourneyTime(journeyTimeHours);

            
            _scenarioContext["PortOfEntry"] = port;
            _scenarioContext["MeansOfTransport"] = mode;
            _scenarioContext["TransportId"] = transId;
            _scenarioContext["AreContainers"] = option;
            _scenarioContext["EnterTransportDocRef"] = DocumentRef;
            _scenarioContext["EstimatedArrivalDate"] = formattedDate;
            _scenarioContext["EstimatedArrivalTime"] = formattedTime;
            _scenarioContext["EstimatedJourneyTime"] = journeyTimeHours;           
        }
    }
}