using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class IOCDetailsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IIOCDetailsPage? iocDetailsPage => _objectContainer.IsRegistered<IIOCDetailsPage>() ? _objectContainer.Resolve<IIOCDetailsPage>() : null;

        public IOCDetailsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the IOC details page should be displayed")]
        public void ThenTheIOCDetailsPageShouldBeDisplayed()
        {
            Assert.True(iocDetailsPage?.IsPageLoaded(), "IOC details page is not displayed");
        }

        [Then("notification {int} is under the associated checks header")]
        public void ThenNotificationIsUnderTheAssociatedChecksHeader(int notificationNumber)
        {
            var chedRef = _scenarioContext.Get<string>($"Notification_{notificationNumber}_CHEDReference");
            Assert.True(iocDetailsPage?.IsUnderAssociatedChedP(chedRef),
                $"Notification {notificationNumber} (CHED ref: {chedRef}) was not found under the Associated CHED-P table");
        }

        [Then("notification {int} is under the checked consignments header with count {string}")]
        public void ThenNotificationIsUnderTheCheckedConsignmentsHeaderWithCount(int notificationNumber, string count)
        {
            var chedRef = _scenarioContext.Get<string>($"Notification_{notificationNumber}_CHEDReference");
            Assert.True(iocDetailsPage?.IsUnderCheckedConsignmentsWithCount(chedRef, count),
                $"Notification {notificationNumber} (CHED ref: {chedRef}) was not found under the Checked consignments table with count '{count}'");
        }
    }
}