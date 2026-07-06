using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ConfirmationOfRateChangeSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IConfirmationOfRateChangePage? confirmationOfRateChangePage =>
            _objectContainer.IsRegistered<IConfirmationOfRateChangePage>()
                ? _objectContainer.Resolve<IConfirmationOfRateChangePage>()
                : null;

        public ConfirmationOfRateChangeSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Confirmation of rate change page should be displayed")]
        public void ThenTheConfirmationOfRateChangePageShouldBeDisplayed()
        {
            Assert.True(confirmationOfRateChangePage?.IsPageLoaded(), "Confirmation of rate change page is not displayed");
        }

        [Then("the Confirmation of rate change page should be displayed with the following details")]
        public void ThenTheConfirmationOfRateChangePageShouldBeDisplayedWithTheFollowingDetails(Table table)
        {
            Assert.True(confirmationOfRateChangePage?.IsPageLoaded(), "Confirmation of rate change page is not displayed");

            var actual = confirmationOfRateChangePage!.GetConfirmationDetails();
            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                Assert.True(actual.ContainsKey(field), $"Field '{field}' not found on Confirmation of rate change page");
                Assert.AreEqual(expected, actual[field],
                    $"Field '{field}' mismatch: expected '{expected}' but got '{actual[field]}'");
            }
        }

        [Then("the Confirmation of rate change page should be displayed with the following HMI Import Rate details")]
        public void ThenTheConfirmationOfRateChangePageShouldBeDisplayedWithTheFollowingHMIImportRateDetails(Table table)
        {
            Assert.True(confirmationOfRateChangePage?.IsDefaultRateChangePageLoaded(), "Confirmation of default rate change page is not displayed");

            var actual = confirmationOfRateChangePage!.GetHmiImportRateConfirmationDetails();
            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                Assert.True(actual.ContainsKey(field), $"Field '{field}' not found on Confirmation of rate change page");
                Assert.AreEqual(expected, actual[field],
                    $"Field '{field}' mismatch: expected '{expected}' but got '{actual[field]}'");
            }
        }

        [When("the user clicks the Confirm and send button on the confirmation of rate change page")]
        public void WhenTheUserClicksTheConfirmAndSendButtonOnTheConfirmationOfRateChangePage()
        {
            confirmationOfRateChangePage?.ClickConfirmAndSendButton();
        }
    }
}