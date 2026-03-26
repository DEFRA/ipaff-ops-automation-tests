using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ApprovedEstablishmentSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IApprovedEstablishmentPage? approvedEstablishmentPage => _objectContainer.IsRegistered<IApprovedEstablishmentPage>() ? _objectContainer.Resolve<IApprovedEstablishmentPage>() : null;

        public ApprovedEstablishmentSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Approved establishment of origin page should be displayed")]
        public void ThenTheApprovedEstablishmentOfOriginPageShouldBeDisplayed()
        {
            Assert.True(approvedEstablishmentPage?.IsPageLoaded(), "Approved establishment of origin (where required) page not loaded");
        }

        [When("the user clicks Search for an approved establishment")]
        public void WhenTheUserClicksSearchForAnApprovedEstablishment()
        {
            approvedEstablishmentPage?.ClickSearchForApproved();
        }

        [Then("the list of establishments should be displayed, filtered by Country of origin {string} type {string} status {string}")]
        public void ThenTheListOfEstablishmentsShouldBeDisplayedFilteredByCountryOfOriginTypeStatus(string country, string type, string status)
        {
            Assert.IsTrue(approvedEstablishmentPage?.VerifySelectedCountryOfOrigin(country));

            approvedEstablishmentPage?.SelectTypeFromDropdown(type);
            approvedEstablishmentPage?.SelectStatusFromDropdown(status);
            approvedEstablishmentPage?.ClickSearchButton();
            Thread.Sleep(1000);

            Assert.IsTrue(approvedEstablishmentPage?.VerifySelectedCountryOnlyDisplayed(country), "Search List is not displayed based on the selected country");
            Assert.IsTrue(approvedEstablishmentPage?.VerifySelectedTypeOnlyDisplayed(type), "Search List is not displayed based on the selected type");
            Assert.IsTrue(approvedEstablishmentPage?.VerifySelectedStatusOnlyDisplayed(status), "Search List is not displayed based on the selected status");
        }

        [When("the user clicks Select for one of the establishments in the list")]
        public void WhenTheUserClicksSelectForOneOfTheEstablishmentsInTheList()
        {
            _scenarioContext["EstablishmentListFirstName"] = approvedEstablishmentPage?.GetEstablishmentListFirstName();
            approvedEstablishmentPage?.ClickSelectEstablishment();
        }

        [Then("the Approved establishment of origin page should be displayed with the selected establishment")]
        public void ThenTheApprovedEstablishmentOfOriginPageShouldBeDisplayedWithTheSelectedEstablishment()
        {
            var establishmentListFirstName = _scenarioContext.Get<string>("EstablishmentListFirstName");
            approvedEstablishmentPage?.VerifySelectedEstablismentName(establishmentListFirstName);
            _scenarioContext["ApprovedEstablishmentName"] = approvedEstablishmentPage?.GetSelectedEstablishmentName();
            _scenarioContext["ApprovedEstablishmentCountry"] = approvedEstablishmentPage?.GetSelectedEstablishmentCountry();
            _scenarioContext["ApprovedEstablishmentType"] = approvedEstablishmentPage?.GetSelectedEstablishmentType();
            _scenarioContext["ApprovedEstablishmentApprovalNum"] = approvedEstablishmentPage?.GetSelectedEstablishmentApprovalNumber();
        }

        [Then("the Approved establishment of origin page should be displayed with the next selected establishment")]
        public void ThenTheApprovedEstablishmentOfOriginPageShouldBeDisplayedWithTheNextSelectedEstablishment()
        {
            var establishmentListFirstName = _scenarioContext.Get<string>("EstablishmentListFirstName");
            approvedEstablishmentPage?.VerifySelectedEstablismentName(establishmentListFirstName);
            _scenarioContext["ApprovedEstablishmentName2"] = approvedEstablishmentPage?.GetSelectedEstablishmentName();
            _scenarioContext["ApprovedEstablishmentCountry2"] = approvedEstablishmentPage?.GetSelectedEstablishmentCountry();
            _scenarioContext["ApprovedEstablishmentType2"] = approvedEstablishmentPage?.GetSelectedEstablishmentType();
            _scenarioContext["ApprovedEstablishmentApprovalNum2"] = approvedEstablishmentPage?.GetSelectedEstablishmentApprovalNumber();
        }

        [When(@"the user removes the establishment of origin")]
        public void WhenIRemoveTheEstablishmentOfOrigin()
        {
            approvedEstablishmentPage?.ClickRemoveEstablishment();
        }

        [When(@"the user searches for the approved establishment {string}")]
        public void WhenTheUserSearchesForTheApprovedEstablishment(string establishmentName)
        {
            approvedEstablishmentPage?.EnterEstablishmentName(establishmentName);
            approvedEstablishmentPage?.ClickSearchButton();
        }
    }
}