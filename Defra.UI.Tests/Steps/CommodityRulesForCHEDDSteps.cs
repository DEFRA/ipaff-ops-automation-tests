using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CommodityRulesForCHEDDSteps
    {
        private readonly IObjectContainer _objectContainer;

        private ICommodityRulesForCHEDDPage? commodityRulesForCHEDDPage =>
            _objectContainer.IsRegistered<ICommodityRulesForCHEDDPage>()
                ? _objectContainer.Resolve<ICommodityRulesForCHEDDPage>()
                : null;

        public CommodityRulesForCHEDDSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Commodity rules for CHED-D page should be displayed")]
        public void ThenTheCommodityRulesForCHEDDPageShouldBeDisplayed()
        {
            Assert.True(commodityRulesForCHEDDPage.IsPageLoaded(), "Commodity rules for CHED-D page is not displayed");
        }

        [Then("the Commodity rules for CHED-D page should be displayed with the File submission in progress section showing status {string}")]
        public void ThenTheCommodityRulesForCHEDDPageShouldBeDisplayedWithTheFileSubmissionInProgressSectionShowingStatus(string expectedStatus)
        {
            Assert.True(commodityRulesForCHEDDPage.IsPageLoaded(), "Commodity rules for CHED-D page is not displayed");
            Assert.True(commodityRulesForCHEDDPage.WaitForFirstInProgressStatus(expectedStatus),
                $"File submission in progress did not reach status '{expectedStatus}' within the timeout");
        }

        [When("the user clicks the Confirm and submit link for the file submission in progress on the CHED-D page")]
        public void WhenTheUserClicksTheConfirmAndSubmitLinkForTheFileSubmissionInProgressOnTheCHEDDPage()
        {
            commodityRulesForCHEDDPage.ClickConfirmAndSubmitLinkForFirstInProgress();
        }

        [Then("the File submission in progress section should be removed and the first Previous submission should have status {string} on the CHED-D page")]
        public void ThenTheFileSubmissionInProgressSectionShouldBeRemovedAndTheFirstPreviousSubmissionShouldHaveStatusOnTheCHEDDPage(string expectedStatus)
        {
            Assert.True(commodityRulesForCHEDDPage.WaitForFileSubmissionInProgressSectionToBeRemoved(),
                "File submission in progress section was not removed within the timeout");
            Assert.AreEqual(expectedStatus, commodityRulesForCHEDDPage.GetFirstPreviousSubmissionStatus(),
                $"Expected first Previous submission status '{expectedStatus}' but was '{commodityRulesForCHEDDPage.GetFirstPreviousSubmissionStatus()}'");
        }

        [When("the user clicks the View summary link for the first record in the Previous submissions section on the CHED-D page")]
        public void WhenTheUserClicksTheViewSummaryLinkForTheFirstRecordInThePreviousSubmissionsSectionOnTheCHEDDPage()
        {
            commodityRulesForCHEDDPage.ClickViewSummaryLinkForFirstPreviousSubmission();
        }

        [When("the user refreshes the Commodity rules for CHED-D page")]
        public void WhenTheUserRefreshesTheCommodityRulesForCHEDDPage()
        {
            commodityRulesForCHEDDPage.RefreshPage();
        }
    }
}