using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.CP
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

        [When("the user populates the transport details {string} {string} {string} {string}")]
        public void WhenTheUserPopulatesTheTransportDetails(string port, string mode, string transId, string DocumentRef)
        {
            DateTime today = DateTime.Now;
            string day = today.Day.ToString();
            string month = today.Month.ToString();
            string year = today.Year.ToString();
            string formattedDate = today.ToString("dd MMM yyyy");
            
            string hour = "23";
            string minutes = "30";
            string formattedTime = hour + ":" + minutes;

            portOfEntryPage?.EnterPortOfEntry(port);
            portOfEntryPage?.SelectMeansOfTransport(mode);
            portOfEntryPage?.EnterTransportId(transId);
            portOfEntryPage?.EnterTransportDocRef(DocumentRef);
            portOfEntryPage?.EnterEstimatedArrivalDate(day, month, year);
            portOfEntryPage?.EnterEstimatedArrivalTime(hour, minutes);

            _scenarioContext.Add("PortOfEntry", port);
            _scenarioContext.Add("MeansOfTransport", mode);
            _scenarioContext.Add("TransportId", transId);
            _scenarioContext.Add("EnterTransportDocRef", DocumentRef);
            _scenarioContext.Add("EstimatedArrivalDate", formattedDate);
            _scenarioContext.Add("EstimatedArrivalTime", formattedTime);
        }
    }
}