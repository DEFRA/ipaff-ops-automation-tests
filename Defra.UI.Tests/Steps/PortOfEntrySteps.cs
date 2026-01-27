using Reqnroll.BoDi;
using NUnit.Framework;
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
            portOfEntryPage?.SelectAreTrailersOrContainersUsed(option);
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

        [When("the user enters BCP or Port of entry {string}")]
        public void WhenTheUserEntersBCPOrPortOfEntry(string port)
        {
            portOfEntryPage?.EnterPortOfEntry(port);
            _scenarioContext["PortOfEntry"] = port;
        }

        [When("the user selects means of transport to BCP or Port of entry {string}")]
        public void WhenTheUserSelectsMeansOfTransportToBCPOrPortOfEntry(string mode)
        {
            portOfEntryPage?.SelectMeansOfTransport(mode);
            _scenarioContext["MeansOfTransport"] = mode;
        }

        [When("the user enters transport identification {string}")]
        public void WhenTheUserEntersTransportIdentification(string transId)
        {
            portOfEntryPage?.EnterTransportId(transId);
            _scenarioContext["TransportId"] = transId;
        }

        [When("the user selects {string} for Are any road trailers or shipping containers being used to transport the consignment")]
        public void WhenTheUserSelectsForAreAnyRoadTrailersOrShippingContainersBeingUsedToTransportTheConsignment(string option)
        {
            portOfEntryPage?.SelectAreTrailersOrContainersUsed(option);
            _scenarioContext["AreContainers"] = option;
        }

        [When("the user enters transport document reference {string}")]
        public void WhenTheUserEntersTransportDocumentReference(string documentRef)
        {
            portOfEntryPage?.EnterTransportDocRef(documentRef);
            _scenarioContext["EnterTransportDocRef"] = documentRef;
        }

        [When("the user enters arrival date at BCP or Port of entry {string} days from now")]
        public void WhenTheUserEntersArrivalDateAtBCPOrPortOfEntryDaysFromNow(string daysFromNow)
        {
            int days = int.Parse(daysFromNow);
            var arrivalDate = DateTime.Now.AddDays(days);

            var day = arrivalDate.Day.ToString();
            var month = arrivalDate.Month.ToString();
            var year = arrivalDate.Year.ToString();

            portOfEntryPage?.EnterEstimatedArrivalDate(day, month, year);

            var formattedDate = arrivalDate.ToString("dd MMM yyyy");
            _scenarioContext["EstimatedArrivalDate"] = formattedDate;
        }

        [When("the user enters estimated arrival time at BCP with future time")]
        public void WhenTheUserEntersEstimatedArrivalTimeAtBCPWithFutureTime()
        {
            // Use current time + 2 hours as a future time
            var futureTime = DateTime.Now.AddHours(2);
            var hour = futureTime.Hour.ToString();
            var minutes = futureTime.Minute.ToString();

            portOfEntryPage?.EnterEstimatedArrivalTime(hour, minutes);

            var formattedTime = futureTime.ToString("HH:mm");
            _scenarioContext["EstimatedArrivalTime"] = formattedTime;
        }

        [When("the user enters estimated arrival time at BCP {string}")]
        public void WhenTheUserEntersEstimatedArrivalTimeAtBCP(string time)
        {
            var timeParts = time.Split(':');
            var hour = timeParts[0];
            var minutes = timeParts[1];

            portOfEntryPage?.EnterEstimatedArrivalTime(hour, minutes);
            _scenarioContext["EstimatedArrivalTime"] = time;
        }

        [When("the user enters estimated total journey time of the animals {string} hours")]
        public void WhenTheUserEntersEstimatedTotalJourneyTimeOfTheAnimalsHours(string hours)
        {
            portOfEntryPage?.EnterEstimatedJourneyTime(hours);
            _scenarioContext["EstimatedJourneyTime"] = hours;
        }

        [When("the user enters Container Number {string}")]
        public void WhenTheUserEntersContainerNumber(string containerNumber)
        {
            portOfEntryPage?.EnterContainerNumber(containerNumber);
            _scenarioContext.Add("ContainerNumber", containerNumber);
        }

        [When("the user enters Seal Number {string}")]
        public void WhenTheUserEntersSealNumber(string sealNumber)
        {
            portOfEntryPage?.EnterSealNumber(sealNumber);
            _scenarioContext.Add("SealNumber", sealNumber);
        }

        [When("the user ticks the checkbox to confirm an official seal is affixed")]
        public void WhenTheUserTicksTheCheckboxToConfirmAnOfficialSealIsAffixed()
        {
            portOfEntryPage?.TickOfficialSealCheckbox();
            _scenarioContext.Add("OfficialSealAffixed", "Yes");
        }

        [Then("the user verifies and enters any missing data on the Transport to the port of entry page")]
        public void ThenTheUserVerifiesAndEntersAnyMissingDataOnTheTransportToThePortOfEntryPage()
        {
            Assert.True(portOfEntryPage?.IsPageLoaded(), "Transport Transport to the port of entry page not loaded");
            if (portOfEntryPage.VerifyPortOfEntryIfNotAlreadyPopulated())
                WhenTheUserPopulatesTheTransportDetails("LONDON GATEWAY (GBLGP)", "No", "Road vehicle", "123456", "Doc1234");
            else
                Assert.Fail("Port of entry data should not be prefilled");
        }
    }
}