using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class TransportAfterPortOfEntrySteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ITransportAfterPortOfEntryPage? transportAfterPortOfEntryPage => _objectContainer.IsRegistered<ITransportAfterPortOfEntryPage>() ? _objectContainer.Resolve<ITransportAfterPortOfEntryPage>() : null;

        public TransportAfterPortOfEntrySteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Transport after the BCP or Port of entry page should be displayed")]
        public void ThenTheTransportAfterTheBCPOrPortOfEntryPageShouldBeDisplayed()
        {
            // Remove GVMS keys since Transport After BCP page means airplane transport
            // (GVMS page is skipped for airplane transport)
            _scenarioContext.RemoveContextKeys("IsGVMS");

            Assert.True(transportAfterPortOfEntryPage?.IsPageLoaded(), "Transport after the BCP or Port of entry page not loaded");
        }

        [When("the user selects means of transport after BCP {string}")]
        public void WhenTheUserSelectsMeansOfTransportAfterBCP(string mode)
        {
            transportAfterPortOfEntryPage?.SelectMeansOfTransportAfterBCP(mode);
            _scenarioContext["MeansOfTransportAfterBCP"] = mode;
        }

        [When("the user enters transport identification after BCP {string}")]
        public void WhenTheUserEntersTransportIdentificationAfterBCP(string transportId)
        {
            transportAfterPortOfEntryPage?.EnterTransportIdentificationAfterBCP(transportId);
            _scenarioContext["TransportIdentificationAfterBCP"] = transportId;
        }

        [When("the user enters transport document reference after BCP {string}")]
        public void WhenTheUserEntersTransportDocumentReferenceAfterBCP(string documentRef)
        {
            transportAfterPortOfEntryPage?.EnterTransportDocumentReferenceAfterBCP(documentRef);
            _scenarioContext["TransportDocumentReferenceAfterBCP"] = documentRef;
        }

        [When("the user enters departure date from BCP {string} days later than arrival date")]
        public void WhenTheUserEntersDepartureDateFromBCPDaysLaterThanArrivalDate(string daysLater)
        {
            int days = int.Parse(daysLater);

            // Get the arrival date from ScenarioContext
            var arrivalDateString = _scenarioContext.Get<string>("EstimatedArrivalDate");
            var arrivalDate = DateTime.ParseExact(arrivalDateString, "dd MMM yyyy", System.Globalization.CultureInfo.InvariantCulture);

            var departureDate = arrivalDate.AddDays(days);

            transportAfterPortOfEntryPage?.EnterDepartureDateFromBCP(departureDate);

            var formattedDepartureDate = departureDate.ToString("dd MMM yyyy");
            _scenarioContext["DepartureDateFromBCP"] = formattedDepartureDate;
        }

        [When("the user enters departure date from BCP or Port of entry as today's date")]
        public void WhenTheUserEntersDepartureDateFromBCPOrPortOfEntryAsTodaysDate()
        {
            var departureDate = DateTime.Now;

            transportAfterPortOfEntryPage?.EnterDepartureDateFromBCP(departureDate);

            var formattedDate = departureDate.ToString("dd MMM yyyy");
            _scenarioContext["DepartureDateFromBCP"] = formattedDate;
        }

        [When("the user enters departure time from BCP {string}")]
        public void WhenTheUserEntersDepartureTimeFromBCP(string time)
        {
            transportAfterPortOfEntryPage?.EnterDepartureTimeFromBCP(time);
            _scenarioContext["DepartureTimeFromBCP"] = time;
        }

        [When("the user enters departure time from BCP or Port of entry with future time")]
        public void WhenTheUserEntersDepartureTimeFromBCPOrPortOfEntryWithFutureTime()
        {
            // Use current time + 2 hours as a future time
            var futureTime = DateTime.Now.AddHours(2);
            var hour = futureTime.Hour.ToString();
            var minutes = futureTime.Minute.ToString();
            var formattedTime = futureTime.ToString("HH:mm");

            transportAfterPortOfEntryPage?.EnterDepartureTimeFromBCP(formattedTime);

            _scenarioContext["DepartureTimeFromBCP"] = formattedTime;
        }

        [Then("the Means of transport after BCP should be copied from the original notification")]
        public void ThenTheMeansOfTransportAfterBCPShouldBeCopiedFromTheOriginalNotification()
        {
            var expectedMeansOfTransport = _scenarioContext.ContainsKey("MeansOfTransportAfterBCP")
                ? _scenarioContext["MeansOfTransportAfterBCP"]?.ToString() ?? string.Empty
                : string.Empty;

            string actualMeansOfTransport = transportAfterPortOfEntryPage?.GetMeansOfTransportAfterBCP ?? string.Empty;

            Assert.That(actualMeansOfTransport, Is.EqualTo(expectedMeansOfTransport),
                $"Expected Means of transport after BCP to be '{expectedMeansOfTransport}' but was '{actualMeansOfTransport}'");
        }

        [Then("the Transport identification should not be copied from the original notification")]
        public void ThenTheTransportIdentificationShouldNotBeCopiedFromTheOriginalNotification()
        {
            string actualTransportIdentification = transportAfterPortOfEntryPage?.GetTransportIdentificationAfterBCP ?? string.Empty;

            Assert.That(actualTransportIdentification, Is.Empty,
                $"Transport identification should not be copied from the original notification, but found '{actualTransportIdentification}'");
        }

        [Then("the Transport document reference should not be copied from the original notification")]
        public void ThenTheTransportDocumentReferenceShouldNotBeCopiedFromTheOriginalNotification()
        {
            string actualTransportDocumentReference = transportAfterPortOfEntryPage?.GetTransportDocumentReferenceAfterBCP ?? string.Empty;

            Assert.That(actualTransportDocumentReference, Is.Empty,
                $"Transport document reference should not be copied from the original notification, but found '{actualTransportDocumentReference}'");
        }

        [Then("the Departure date from BCP should not be copied from the original notification")]
        public void ThenTheDepartureDateFromBCPShouldNotBeCopiedFromTheOriginalNotification()
        {
            string actualDepartureDate = transportAfterPortOfEntryPage?.GetDepartureDateFromBCP ?? string.Empty;

            Assert.That(actualDepartureDate.Trim(), Is.Empty.Or.EqualTo("  "),
                $"Departure date from BCP should not be copied from the original notification, but found '{actualDepartureDate}'");
        }

        [Then("the Departure time from BCP should not be copied from the original notification")]
        public void ThenTheDepartureTimeFromBCPShouldNotBeCopiedFromTheOriginalNotification()
        {
            string actualDepartureTime = transportAfterPortOfEntryPage?.GetDepartureTimeFromBCP ?? string.Empty;

            Assert.That(actualDepartureTime.Trim(), Is.Empty.Or.EqualTo(":"),
                $"Departure time from BCP should not be copied from the original notification, but found '{actualDepartureTime}'");
        }
    }
}