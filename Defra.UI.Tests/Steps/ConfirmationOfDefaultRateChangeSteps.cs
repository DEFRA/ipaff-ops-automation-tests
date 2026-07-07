using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class ConfirmationOfDefaultRateChangeSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IConfirmationOfDefaultRateChangePage? confirmationOfDefaultRateChangePage =>
            _objectContainer.IsRegistered<IConfirmationOfDefaultRateChangePage>()
                ? _objectContainer.Resolve<IConfirmationOfDefaultRateChangePage>()
                : null;

        public ConfirmationOfDefaultRateChangeSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Confirmation of default rate change page should be displayed")]
        public void ThenTheConfirmationOfDefaultRateChangePageShouldBeDisplayed()
        {
            Assert.True(confirmationOfDefaultRateChangePage?.IsPageLoaded(), "Confirmation of default rate change page is not displayed");
        }

        [Then("the Confirmation of default rate change page should be displayed with the following HMI \\(Import) rate details")]
        public void ThenTheConfirmationOfDefaultRateChangePageShouldBeDisplayedWithTheFollowingHMIImportRateDetails(Table table)
        {
            Assert.True(confirmationOfDefaultRateChangePage?.IsPageLoaded(), "Confirmation of default rate change page is not displayed");

            var actual = confirmationOfDefaultRateChangePage!.GetHmiImportRateConfirmationDetails();
            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                Assert.True(actual.ContainsKey(field), $"Field '{field}' not found on Confirmation of rate change page");
                Assert.AreEqual(expected, actual[field],
                    $"Field '{field}' mismatch: expected '{expected}' but got '{actual[field]}'");
            }
        }

        [Then("the Confirmation of default rate change page should be displayed with the following GMS \\(Export) rate details")]
        public void ThenTheConfirmationOfDefaultRateChangePageShouldBeDisplayedWithTheFollowingGMSExportRateDetails(Table table)
        {
            Assert.True(confirmationOfDefaultRateChangePage?.IsPageLoaded(), "Confirmation of default rate change page is not displayed");

            var actual = confirmationOfDefaultRateChangePage!.GetGmsExportRateConfirmationDetails();
            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var expected = row["Value"];
                Assert.True(actual.ContainsKey(field), $"Field '{field}' not found on Confirmation of rate change page");
                Assert.AreEqual(expected, actual[field],
                    $"Field '{field}' mismatch: expected '{expected}' but got '{actual[field]}'");
            }
        }

        [Then("the HMI \\(Import) rate should be changing to the value noted at the start")]
        public void ThenTheHMIImportRateShouldBeChangingToTheValueNotedAtTheStart()
        {
            Assert.True(confirmationOfDefaultRateChangePage?.IsPageLoaded(), "Confirmation of default rate change page is not displayed");

            var originalRate = (int)_scenarioContext["HmiImportRate"];
            var expected = $"{originalRate}%";

            var details = confirmationOfDefaultRateChangePage!.GetHmiImportRateConfirmationDetails();
            Assert.IsTrue(details.Values.Any(v => v?.Trim() == expected),
                $"Expected HMI (Import) 'To' value of '{expected}' not found in confirmation details. Actual: {string.Join(", ", details.Select(kv => $"{kv.Key}={kv.Value}"))}");
        }

        [Then("the GMS \\(Export) rate should be changing to the value noted at the start")]
        public void ThenTheGMSExportRateShouldBeChangingToTheValueNotedAtTheStart()
        {
            Assert.True(confirmationOfDefaultRateChangePage?.IsPageLoaded(), "Confirmation of default rate change page is not displayed");

            var originalRate = (int)_scenarioContext["GmsExportRate"];
            var expected = $"{originalRate}%";

            var details = confirmationOfDefaultRateChangePage!.GetGmsExportRateConfirmationDetails();
            Assert.IsTrue(details.Values.Any(v => v?.Trim() == expected),
                $"Expected GMS (Export) 'To' value of '{expected}' not found in confirmation details. Actual: {string.Join(", ", details.Select(kv => $"{kv.Key}={kv.Value}"))}");
        }

        [When("the user clicks the Confirm and send button on the confirmation of default rate change page")]
        public void WhenTheUserClicksTheConfirmAndSendButtonOnTheConfirmationOfDefaultRateChangePage()
        {
            confirmationOfDefaultRateChangePage?.ClickConfirmAndSendButton();
        }
    }
}