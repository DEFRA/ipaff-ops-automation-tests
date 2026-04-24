using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class RecordHmiChecksSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IRecordHmiChecksPage? recordHmiChecksPage => _objectContainer.IsRegistered<IRecordHmiChecksPage>()
            ? _objectContainer.Resolve<IRecordHmiChecksPage>()
            : null;

        public RecordHmiChecksSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Record HMI checks page should be displayed")]
        public void ThenTheRecordHmiChecksPageShouldBeDisplayed()
        {
            Assert.True(recordHmiChecksPage?.IsPageLoaded(), "Record HMI checks page not loaded.");
        }

        [Then("the Commodities HMI check status should be {string}")]
        public void ThenTheCommoditiesHmiCheckStatusShouldBe(string expectedStatus)
        {
            Assert.True(
                recordHmiChecksPage?.VerifyCommodityHmiStatus(expectedStatus),
                $"Expected all commodity HMI check statuses to be '{expectedStatus}'.");
        }

        [When("the user sets the Commodities status to {string}")]
        public void WhenTheUserSetsTheCommoditiesStatusTo(string status)
        {
            recordHmiChecksPage?.SetAllCommoditiesStatus(status);
        }

        [When("the Validity period is {int} days")]
        public void WhenTheValidityPeriodIsDays(int days)
        {
            recordHmiChecksPage?.SetValidityPeriod(days);
        }

        [When("the user clicks Save and return to work order")]
        public void WhenTheUserClicksSaveAndReturnToWorkOrder()
        {
            recordHmiChecksPage?.ClickSaveAndReturnToWorkOrder();
        }
    }
}