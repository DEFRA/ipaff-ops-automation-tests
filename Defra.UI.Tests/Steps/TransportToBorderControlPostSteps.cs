using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class TransportToBorderControlPostSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ITransportToBorderControlPostPage? transportToBorderControlPostPage => _objectContainer.IsRegistered<ITransportToBorderControlPostPage>() ? _objectContainer.Resolve<ITransportToBorderControlPostPage>() : null;
        private IPortOfEntryPage? portOfEntryPage => _objectContainer.IsRegistered<IPortOfEntryPage>() ? _objectContainer.Resolve<IPortOfEntryPage>() : null;

        public TransportToBorderControlPostSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("Transport to the Border Control Post \\(BCP) page should be dislayed")]
        public void ThenTransportToTheBorderControlPostBCPPageShouldBeDislayed()
        {
            Assert.True(transportToBorderControlPostPage?.IsPageLoaded(), "Transport to the Border Control Post (BCP) page not loaded");
        }

        [When("the user populates the transport to the BCP details {string} {string} {string} {string} {string} {string}")]
        public void WhenTheUserPopulatesTheTransportToTheBCPDetails(string entryBCP, string premises, string mode, string transId, string option, string DocumentRef)
        {
            var currentDate = DateTime.Now.AddHours(4).AddMinutes(5);
            var day = currentDate.Day.ToString();
            var month = currentDate.Month.ToString();
            var year = currentDate.Year.ToString();
            var formattedDate = currentDate.ToString("dd MMM yyyy");

            var formattedTime = currentDate.ToString("HH:mm");
            var hour = currentDate.Hour.ToString();
            var minutes = currentDate.Minute.ToString();

            //portOfEntryPage?.EnterPortOfEntry(entryBCP);
            transportToBorderControlPostPage?.SelectEntryBCP(entryBCP);
            transportToBorderControlPostPage?.SelectInspectionPremises(premises);
            portOfEntryPage?.SelectMeansOfTransport(mode);
            portOfEntryPage?.EnterTransportId(transId);
            portOfEntryPage?.SelectAreTrailersOrContainersUsed(option);
            portOfEntryPage?.EnterTransportDocRef(DocumentRef);
            portOfEntryPage?.EnterEstimatedArrivalDate(day, month, year);
            portOfEntryPage?.EnterEstimatedArrivalTime(hour, minutes);

            _scenarioContext["PortOfEntry"] = entryBCP;
            _scenarioContext["MeansOfTransport"] = mode;
            _scenarioContext["TransportId"] = transId;
            _scenarioContext["AreContainers"] = option;
            _scenarioContext["EnterTransportDocRef"] = DocumentRef;
            _scenarioContext["EstimatedArrivalDate"] = formattedDate;
            _scenarioContext["EstimatedArrivalTime"] = formattedTime;
        }
    }
}