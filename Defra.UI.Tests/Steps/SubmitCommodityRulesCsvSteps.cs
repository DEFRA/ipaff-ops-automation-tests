using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class SubmitCommodityRulesCsvSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ISubmitCommodityRulesCsvPage? submitCommodityRulesCsvPage =>
            _objectContainer.IsRegistered<ISubmitCommodityRulesCsvPage>()
                ? _objectContainer.Resolve<ISubmitCommodityRulesCsvPage>()
                : null;

        public SubmitCommodityRulesCsvSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Submit multiple commodity rules using a CSV file page is displayed with the first record status {string}")]
        public void ThenSubmitCsvPageWithFirstRecordStatus(string expectedStatus)
        {
            Assert.True(submitCommodityRulesCsvPage?.IsPageLoaded(), "Submit multiple commodity rules using a CSV file page is not displayed");
            Assert.True(submitCommodityRulesCsvPage?.WaitForFirstRecordStatus(expectedStatus),
                $"First record on Submit page did not reach status '{expectedStatus}' within timeout");
        }

        [When("the user clicks the Confirm and submit link for the first record in the list")]
        [When("the user clicks the View summary link for the first record in the list")]
        public void WhenTheUserClicksTheFirstRecordActionLink()
        {
            submitCommodityRulesCsvPage?.ClickFirstRecordActionLink();
        }
    }
}