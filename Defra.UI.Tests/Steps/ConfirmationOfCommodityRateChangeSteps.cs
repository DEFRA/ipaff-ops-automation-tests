using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ConfirmationOfCommodityRateChangeSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IConfirmationOfCommodityRateChangePage? confirmationOfCommodityRateChangePage =>
            _objectContainer.IsRegistered<IConfirmationOfCommodityRateChangePage>()
                ? _objectContainer.Resolve<IConfirmationOfCommodityRateChangePage>()
                : null;

        public ConfirmationOfCommodityRateChangeSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Confirmation of commodity rate change page should be displayed with the following details")]
        public void ThenTheConfirmationOfCommodityRateChangePageShouldBeDisplayedWithTheFollowingDetails(Table table)
        {
            Assert.True(confirmationOfCommodityRateChangePage?.IsPageLoaded(), "Confirmation of commodity rate change page is not displayed");

            var actual = confirmationOfCommodityRateChangePage!.GetConfirmationDetails();
            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                Assert.True(actual.ContainsKey(field), $"Field '{field}' not found on Confirmation of commodity rate change page");
                Assert.AreEqual(expected, actual[field],
                    $"Field '{field}' mismatch: expected '{expected}' but got '{actual[field]}'");
            }
        }

        [When("the user clicks the Confirm and send button on the confirmation of commodity rate change page")]
        public void WhenTheUserClicksTheConfirmAndSendButtonOnTheConfirmationOfCommodityRateChangePage()
        {
            confirmationOfCommodityRateChangePage?.ClickConfirmAndSendButton();
        }
    }
}