using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class NotificationOverviewSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private INotificationOverviewPage? notificationOverviewPage => _objectContainer.IsRegistered<INotificationOverviewPage>() ? _objectContainer.Resolve<INotificationOverviewPage>() : null;


        public NotificationOverviewSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Notification overview page should be displayed")]
        public void ThenTheNotificationOverviewPageShouldBeDisplayed()
        {
            Assert.True(notificationOverviewPage?.IsPageLoaded(), "Notification overview page not loaded");
        }

        [Then("the notification status should be {string}")]
        [Then("the status should be {string}")]
        public void ThenTheStatusShouldBe(string status)
        {
            Assert.True(notificationOverviewPage?.VerifyStatus(status));
        }

        [Then("the user records replaced CHED Reference number, Customs declaration reference and document code")]
        public void ThenTheUserRecordsReplacedCHEDReferenceNumberCustomsDeclarationReferenceAndDocumentCode()
        {
            _scenarioContext["ReplacementCHEDReference"] = notificationOverviewPage?.GetCHEDReference();
            _scenarioContext["ReplacementCustomsDeclarationReference"] = notificationOverviewPage?.GetCustomsDeclarationReference();
            _scenarioContext["ReplacementCustomsDocumentCode"] = notificationOverviewPage?.GetCustomsDocumentCode();
        }

        [When("the user clicks change in commodity section")]
        public void WhenTheUserClicksChangeInCommoditySection()
        {
            notificationOverviewPage?.ClickChangeInCommoditySection();
        }

        [Then("the total net weight should be updated to {string}")]
        public void ThenTheTotalNetWeightShouldBeUpdatedTo(string netWeight)
        {
            Assert.True(notificationOverviewPage?.VerifyTotalNetWeight(netWeight));
        }

        [Then("the total gross weight should be updated to {string}")]
        public void ThenTheTotalGrossWeightShouldBeUpdatedTo(string grossWeight)
        {
            Assert.True(notificationOverviewPage?.VerifyTotalGrossWeight(grossWeight));
        }

        [When("the user clicks Set to in Progress button")]
        public void WhenTheUserClicksSetToInProgressButton()
        {
            notificationOverviewPage?.ClickSetToInProgressButton();
        }

        [When("the user clicks Record checks button")]
        public void WhenTheUserClicksRecordChecksButton()
        {
            notificationOverviewPage?.ClickRecordChecksButton();
        }

        [When("the user clicks Request amendment")]
        public void WhenTheUserClicksRequestAmendment()
        {
            notificationOverviewPage?.ClickRequestAmendment();
        }
    }
}