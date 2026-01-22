using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


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
            RemoveGVMSKeys();

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

        [When("the user enters departure time from BCP {string}")]
        public void WhenTheUserEntersDepartureTimeFromBCP(string time)
        {
            transportAfterPortOfEntryPage?.EnterDepartureTimeFromBCP(time);
            _scenarioContext["DepartureTimeFromBCP"] = time;
        }

        /// <summary>
        /// Removes GVMS context keys when Transport After BCP page is displayed
        /// (indicating airplane transport mode where GVMS page is skipped)
        /// </summary>
        private void RemoveGVMSKeys()
        {
            var keysToRemove = new[]
            {
                "IsGVMS"            };

            foreach (var key in keysToRemove)
            {
                if (_scenarioContext.ContainsKey(key))
                {
                    _scenarioContext.Remove(key);
                }
            }
        }
    }
}