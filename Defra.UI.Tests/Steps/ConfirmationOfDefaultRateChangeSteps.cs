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

        [Then("the Confirmation of default rate change page should be displayed with the following HMI Import Rate details")]
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

        [When("the user clicks the Confirm and send button on the confirmation of default rate change page")]
        public void WhenTheUserClicksTheConfirmAndSendButtonOnTheConfirmationOfDefaultRateChangePage()
        {
            confirmationOfDefaultRateChangePage?.ClickConfirmAndSendButton();
        }
    }
}