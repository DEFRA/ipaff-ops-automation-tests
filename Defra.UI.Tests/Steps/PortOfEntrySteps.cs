using Reqnroll.BoDi;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class PortOfEntrySteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
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
            DateTime today = DateTime.Now;
            string day = today.Day.ToString();
            string month = today.Month.ToString();
            string year = today.Year.ToString();
            string formattedDate = today.ToString("dd MMM yyyy");

            string hour = "23";
            string minutes = "30";
            string journeyTimeHours = "8"; // Default journey time for CHED-A
            string formattedTime = hour + ":" + minutes;

            portOfEntryPage?.EnterPortOfEntry(port);
            portOfEntryPage?.SelectMeansOfTransport(mode);
            portOfEntryPage?.EnterTransportId(transId);
            portOfEntryPage?.EnterTransportDocRef(DocumentRef);
            portOfEntryPage?.EnterEstimatedArrivalDate(day, month, year);
            portOfEntryPage?.EnterEstimatedArrivalTime(hour, minutes);

            // CHED-A specific field - only filled if present
            portOfEntryPage?.EnterEstimatedJourneyTime(journeyTimeHours);

            _scenarioContext.Add("PortOfEntry", port);
            _scenarioContext.Add("MeansOfTransport", mode);
            _scenarioContext.Add("TransportId", transId);
            _scenarioContext.Add("AreContainers", option);
            _scenarioContext.Add("EnterTransportDocRef", DocumentRef);
            _scenarioContext.Add("EstimatedArrivalDate", formattedDate);
            _scenarioContext.Add("EstimatedArrivalTime", formattedTime);
            _scenarioContext.Add("EstimatedJourneyTime", journeyTimeHours);
        }
    }
}