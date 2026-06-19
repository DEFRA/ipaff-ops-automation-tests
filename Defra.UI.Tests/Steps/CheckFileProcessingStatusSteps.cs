using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CheckFileProcessingStatusSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICheckFileProcessingStatusPage? checkFileProcessingStatusPage =>
            _objectContainer.IsRegistered<ICheckFileProcessingStatusPage>()
                ? _objectContainer.Resolve<ICheckFileProcessingStatusPage>()
                : null;

        public CheckFileProcessingStatusSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Check your file processing status page should be displayed")]
        public void ThenCheckProcessingStatusPageShouldBeDisplayed()
        {
            Assert.True(checkFileProcessingStatusPage?.IsPageLoaded(), "Check your file processing status page is not displayed");
        }

        [When("the user clicks the 'view the processing status of your file here' link")]
        public void WhenTheUserClicksTheViewProcessingStatusLink()
        {
            checkFileProcessingStatusPage?.ClickViewProcessingStatusLink();
        }
    }
}