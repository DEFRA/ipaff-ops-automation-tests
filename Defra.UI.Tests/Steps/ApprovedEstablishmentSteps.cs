using Reqnroll.BoDi;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class ApprovedEstablishmentSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private IApprovedEstablishmentPage? approvedEstablishmentPage => _objectContainer.IsRegistered<IApprovedEstablishmentPage>() ? _objectContainer.Resolve<IApprovedEstablishmentPage>() : null;


        public ApprovedEstablishmentSteps(IObjectContainer container)
        {
            _objectContainer = container;
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
            approvedEstablishmentPage?.VerifySelectedCountryOfOrigin(country);
        }

        [When("the user clicks Select for one of the establishments in the list")]
        public void WhenTheUserClicksSelectForOneOfTheEstablishmentsInTheList()
        {
            approvedEstablishmentPage?.ClickSelectEstablishment();
        }

        [Then("the Approved establishment of origin page should be displayed with the selected establishment")]
        public void ThenTheApprovedEstablishmentOfOriginPageShouldBeDisplayedWithTheSelectedEstablishment()
        {
            approvedEstablishmentPage?.VerifySelectedEstablismentName();
        }

    }
}