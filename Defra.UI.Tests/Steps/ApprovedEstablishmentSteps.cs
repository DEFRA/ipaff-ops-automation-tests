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

        [Then("the list of establishments should be displayed, filtered by Country of origin to {string}")]
        public void ThenTheListOfEstablishmentsShouldBeDisplayedFilteredByCountryOfOriginToChina(string country)
        {
            Assert.IsTrue(approvedEstablishmentPage?.VerifySelectedCountryOfOrigin(country));
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
            _scenarioContext["ApprovedEstablishmentName"] = approvedEstablishmentPage?.GetSubtotalNetWeight();
            _scenarioContext["ApprovedEstablishmentCountry"] = approvedEstablishmentPage?.GetSubtotalPackages();
            _scenarioContext["ApprovedEstablishmentType"] = approvedEstablishmentPage?.GetTotalNetWeight();
            _scenarioContext["ApprovedEstablishmentApprovalNum"] = approvedEstablishmentPage?.GetTotalPackages();
        }

        [When(@"the user removes the establishment of origin")]
        public void WhenIRemoveTheEstablishmentOfOrigin()
        {
            approvedEstablishmentPage?.ClickRemoveEstablishment();
        }

    }
}