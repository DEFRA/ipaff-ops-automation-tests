using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ConsignmentsRequiringSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IConsignmentsRequiringControlPage? consignmentsRequiringControlPage => _objectContainer.IsRegistered<IConsignmentsRequiringControlPage>() ? _objectContainer.Resolve<IConsignmentsRequiringControlPage>() : null;

        public ConsignmentsRequiringSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Consignments requiring control page should be displayed")]
        public void ThenTheConsignmentsRequiringControlPageShouldBeDisplayed()
        {
            Assert.True(consignmentsRequiringControlPage?.IsPageLoaded(), "Consignments requiring control page not loaded");
        }


        [Then("the notification should be found with the status {string}")]
        public void ThenTheNotificationShouldBeFoundWithTheStatus(string status)
        {
            var chedRef = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(consignmentsRequiringControlPage?.VerifyNotificationStatus(chedRef, status));
        }

        [When("the user clicks CHEDP reference number")]
        public void WhenTheUserClicksCHEDPReferenceNumber()
        {
            consignmentsRequiringControlPage?.ClickCHEDReferencNum();
        }
        
        [Then("the user verifies the control status is {string}")]
        public void WhenTheUserVerifiesTheControlStatus(string controlStatus)
        {
            Assert.IsTrue(consignmentsRequiringControlPage?.VerifyControlStatus(controlStatus), "The Control status is not " +controlStatus);
        }
        
        [Then("the user verifies {string} link in Consignments requiring control page")]
        public void WhenTheUserVerifiesLinkInConsignmentsRequiringConrolPage(String link)
        {
            Assert.IsTrue(consignmentsRequiringControlPage?.VerifyLink(link), "The link" +link+" is not available");
        }
        
        [Then("the user verifies {string} is selected in {string} field")]
        public void WhenTheUserVerifiesTheSelectedValues(String value, String field)
        {
            Assert.IsTrue(consignmentsRequiringControlPage?.VerifyDropdownFieldValue(field, value), value + " is not selected in " + field + " ");
        }
        
        [When("the user selects {string} from {string} field")]
        public void WhenTheUserSelectsFromDropdown(String field, String value)
        {
            consignmentsRequiringControlPage?.SelectControlStatus(field, value);
        }
        
        [Then("the user validates the Control status is {string}")]
        public void WhenTheUserValidatesControlStatus(String value)
        {
            Assert.IsTrue(consignmentsRequiringControlPage?.VerifyTheControlStatus(value), "The control status is not found or not equal to " + value);
        }
        
        [Then("the user validates the result is within the date range")]
        public void WhenTheUserValidatesTheResultIsWithinTheDateRange()
        {
            Assert.IsTrue(consignmentsRequiringControlPage?.VerifyTheResultsInTheDateRange(_scenarioContext.Get<string>("StartDate"), _scenarioContext.Get<string>("EndDate")), "The control record is not in the date range " + _scenarioContext.Get<string>("StartDate") + " " + _scenarioContext.Get<string>("EndDate"));
        }

        [When("the user clicks on Search in Consignments requiring control page")]
        public void WhenTheUserClicksSearch()
        {
           consignmentsRequiringControlPage?.ClickSearchButton();
        }
        
        [When("the user enters the Start date as {string}")]
        public void WhenTheUserEntersStartDate(string dateString)
        {
            var (day, month, year) = Utils.GetDayMonthYear(dateString);
            _scenarioContext["StartDate"] = $"{day}/{month}/{year}";
            consignmentsRequiringControlPage?.EnterStartDate(day,month,year);
        }
        
        [When("the user enters the End date as {string}")]
        public void WhenTheUserEntersEndDate(string dateString)
        {
            var (day, month, year) = Utils.GetDayMonthYear(dateString);
            _scenarioContext["EndDate"] = $"{day}/{month}/{year}"; ;
            consignmentsRequiringControlPage?.EnterEndDate(day,month,year);
        }

        [Then("the Date of decision is sorted by {string}")]
        public void ThenTheDateOfDecisionIsSortedBy(String sortBy)
        {
            Assert.IsTrue(consignmentsRequiringControlPage?.VerifySortByDropdown(sortBy), "The Decisions are not sort by " + sortBy);
        }
        
        [When("the user clicks on {string} in Consignment requiring control page")]
        public void WhenTheUserClicksInConsignmentRequiringControlPage(String link)
        {
            consignmentsRequiringControlPage?.ClickLink(link);
        }
        
        [When("the user opens the first notification in the consignments requiring control page")]
        public void WhenTheUserClicksInConsignmentRequiringControlPage()
        {
            consignmentsRequiringControlPage?.ClickFirstNotification();
        }
    }
}