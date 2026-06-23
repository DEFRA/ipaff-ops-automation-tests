using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ConfirmationOfCountryRateChangeSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IConfirmationOfCountryRateChangePage? ConfirmationOfCountryRateChangePage =>
            _objectContainer.IsRegistered<IConfirmationOfCountryRateChangePage>()
                ? _objectContainer.Resolve<IConfirmationOfCountryRateChangePage>()
                : null;

        public ConfirmationOfCountryRateChangeSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Confirmation of country rate change page should be displayed with the following details")]
        public void ThenTheConfirmationOfCountryRateChangePageShouldBeDisplayedWithTheFollowingDetails(Table table)
        {
            Assert.True(ConfirmationOfCountryRateChangePage?.IsPageLoaded(), "Confirmation of country rate change page is not displayed");

            var actual = ConfirmationOfCountryRateChangePage!.GetConfirmationDetails();
            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                Assert.True(actual.ContainsKey(field), $"Field '{field}' not found on Confirmation of country rate change page");
                Assert.AreEqual(expected, actual[field],
                    $"Field '{field}' mismatch: expected '{expected}' but got '{actual[field]}'");
            }
        }

        [When("the user clicks the Confirm and send button on the confirmation of country rate change page")]
        public void WhenTheUserClicksTheConfirmAndSendButtonOnTheConfirmationOfCountryRateChangePage()
        {
            ConfirmationOfCountryRateChangePage?.ClickConfirmAndSendButton();
        }
    }
}