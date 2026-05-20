using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CommodityRulesForCHEDASteps
    {
        private readonly IObjectContainer _objectContainer;

        private ICommodityRulesForCHEDAPage? commodityRulesForCHEDAPage =>
            _objectContainer.IsRegistered<ICommodityRulesForCHEDAPage>()
                ? _objectContainer.Resolve<ICommodityRulesForCHEDAPage>()
                : null;

        public CommodityRulesForCHEDASteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Commodity rules for CHED-A page should be displayed")]
        public void ThenTheCommodityRulesForCHEDAPageShouldBeDisplayed()
        {
            Assert.True(commodityRulesForCHEDAPage.IsPageLoaded(), "Commodity rules for CHED-A page is not displayed");
        }

        [Then("the Commodity rules for CHED-A page should be displayed with the File submission in progress section showing status {string}")]
        public void ThenTheCommodityRulesForCHEDAPageShouldBeDisplayedWithTheFileSubmissionInProgressSectionShowingStatus(string expectedStatus)
        {
            Assert.True(commodityRulesForCHEDAPage.IsPageLoaded(), "Commodity rules for CHED-A page is not displayed");
            Assert.True(commodityRulesForCHEDAPage.WaitForFirstInProgressStatus(expectedStatus),
                $"File submission in progress did not reach status '{expectedStatus}' within the timeout");
        }

        [When("the user clicks the Confirm and submit link for the file submission in progress on the CHED-A page")]
        public void WhenTheUserClicksTheConfirmAndSubmitLinkForTheFileSubmissionInProgressOnTheCHEDAPage()
        {
            commodityRulesForCHEDAPage.ClickConfirmAndSubmitLinkForFirstInProgress();
        }

        [Then("the File submission in progress section should be removed and the first Previous submission should have status {string} on the CHED-A page")]
        public void ThenTheFileSubmissionInProgressSectionShouldBeRemovedAndTheFirstPreviousSubmissionShouldHaveStatusOnTheCHEDAPage(string expectedStatus)
        {
            Assert.True(commodityRulesForCHEDAPage.WaitForFileSubmissionInProgressSectionToBeRemoved(),
                "File submission in progress section was not removed within the timeout");
            Assert.AreEqual(expectedStatus, commodityRulesForCHEDAPage.GetFirstPreviousSubmissionStatus(),
                $"Expected first Previous submission status '{expectedStatus}' but was '{commodityRulesForCHEDAPage.GetFirstPreviousSubmissionStatus()}'");
        }

        [When("the user clicks the View summary link for the first record in the Previous submissions section on the CHED-A page")]
        public void WhenTheUserClicksTheViewSummaryLinkForTheFirstRecordInThePreviousSubmissionsSectionOnTheCHEDAPage()
        {
            commodityRulesForCHEDAPage.ClickViewSummaryLinkForFirstPreviousSubmission();
        }

        [When("the user refreshes the Commodity rules for CHED-A page")]
        public void WhenTheUserRefreshesTheCommodityRulesForCHEDAPage()
        {
            commodityRulesForCHEDAPage.RefreshPage();
        }
    }
}