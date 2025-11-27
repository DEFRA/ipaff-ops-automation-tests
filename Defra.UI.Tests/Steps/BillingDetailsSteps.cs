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
    public class BillingDetailsSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private IBillingDetailsPage? billingDetailsPage => _objectContainer.IsRegistered<IBillingDetailsPage>() ? _objectContainer.Resolve<IBillingDetailsPage>() : null;


        public BillingDetailsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }


        [Then("the Confirm billing details page should be displayed")]
        public void ThenTheConfirmBillingDetailsPageShouldBeDisplayed()
        {
            Assert.True(billingDetailsPage?.IsPageLoaded(), "Billing Confirm billing details page not loaded");
        }
    }
}