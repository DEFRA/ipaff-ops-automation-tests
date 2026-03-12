using Defra.UI.Framework.Driver;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Reqnroll.BoDi;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class TransportToBorderControlPostSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        private ITransportToBorderControlPostPage? transportToBorderControlPostPage => _objectContainer.IsRegistered<ITransportToBorderControlPostPage>() ? _objectContainer.Resolve<ITransportToBorderControlPostPage>() : null;
        private IPortOfEntryPage? portOfEntryPage => _objectContainer.IsRegistered<IPortOfEntryPage>() ? _objectContainer.Resolve<IPortOfEntryPage>() : null;
        private IWebElement btnSaveAndReview => _driver.FindElement(By.Id("save-and-review-button"));

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
        public void WhenTheUserPopulatesTheTransportToTheBCPDetails(string entryBCP, string premises, string mode, string transId, string option, string documentRef)
        {
            var currentDate = DateTime.Now.AddHours(4).AddMinutes(5);
            var day = currentDate.Day.ToString();
            var month = currentDate.Month.ToString();
            var year = currentDate.Year.ToString();
            var formattedDate = currentDate.ToString("dd MMM yyyy");

            var formattedTime = currentDate.ToString("HH:mm");
            var hour = currentDate.Hour.ToString();
            var minutes = currentDate.Minute.ToString();

            transportToBorderControlPostPage?.SelectEntryBCP(entryBCP);
            portOfEntryPage?.SelectMeansOfTransport(mode);
            portOfEntryPage?.EnterTransportId(transId);
            portOfEntryPage?.SelectAreTrailersOrContainersUsed(option);
            portOfEntryPage?.EnterTransportDocRef(documentRef);
            portOfEntryPage?.EnterEstimatedArrivalDate(day, month, year);
            portOfEntryPage?.EnterEstimatedArrivalTime(hour, minutes);
            var premisesValue = transportToBorderControlPostPage?.SelectInspectionPremises(premises);
            var premisesWithValue = premises + " - " + premisesValue;

            _scenarioContext["BorderControlPost"] = entryBCP;
            _scenarioContext["InspectionPremises"] = premisesWithValue;
            _scenarioContext["MeansOfTransport"] = mode;
            _scenarioContext["TransportId"] = transId;
            _scenarioContext["AreContainers"] = option;
            _scenarioContext["EnterTransportDocRef"] = documentRef;
            _scenarioContext["EstimatedArrivalDate"] = formattedDate;
            _scenarioContext["EstimatedArrivalTime"] = formattedTime;
        }

        [When("the user Clicks on Save and review button from port of entry page")]
        [When("the user Clicks on Save and review button from Border Control Post page")]
        public void WhenTheUserClicksOnSaveAndReviewButtonFromBorderControlPostPage()
        {
            btnSaveAndReview.Click();
        }
    }
}