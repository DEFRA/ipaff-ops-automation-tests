using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class BillingDetailsSteps
    {
        private readonly IObjectContainer _objectContainer;

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


        [When("the user clicks read about rates and eligibility \\(opens in new tab) link in the confirm billing details page")]
        public void WhenTheUserClicksReadAboutRatesAndEligibilityOpensInNewTabLinkInTheConfirmBillingDetailsPage()
        {
            billingDetailsPage?.ClickRatesAndEligibilityLink();
        }


        [When("the user clicks read the terms and conditions \\(opens in new tab) link in the confirm billing details page")]
        public void WhenTheUserClicksReadTheTermsAndConditionsOpensInNewTabLinkInTheConfirmBillingDetailsPage()
        {
            billingDetailsPage?.ClickTermsAndConditionsLink();
        }


        [Then("the {string} page should be opened in new tab")]
        public void ThenThePageShouldBeOpenedInNewTab(string pageName)
        {
            Assert.True(billingDetailsPage?.VerifyPageOpensInNewTab(pageName), "Page not loaded");
        }


        [When("the user closes the tab")]
        public void WhenTheUserClosesTheTab()
        {
            billingDetailsPage?.CloseTheNewTab();
        }


        [Then("the new tab should be closed")]
        public void ThenTheNewTabShouldBeClosed()
        {
            Assert.True(billingDetailsPage?.VerifyNewTabClosed());
        }


        [When("the user clicks Save and continue in billing details page")]
        public void WhenTheUserClicksSaveAndContinueInBillingDetailsPage()
        {
            billingDetailsPage?.ClickSaveAndContinue();
        }

    }
}