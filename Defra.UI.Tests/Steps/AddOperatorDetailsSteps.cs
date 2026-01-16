using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class AddOperatorDetailsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IAddOperatorDetailsPage? addOperatorDetailsPage => _objectContainer.IsRegistered<IAddOperatorDetailsPage>() ? _objectContainer.Resolve<IAddOperatorDetailsPage>() : null;

        public AddOperatorDetailsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then(@"the Add operator details page should be displayed")]
        public void ThenTheAddOperatorDetailsPageShouldBeDisplayed()
        {
            Assert.True(addOperatorDetailsPage?.IsPageLoaded(), "Add Operator Details page not loaded");
        }

        [When(@"the user adds the operator '([^']*)' details")]
        public void WhenTheUserAddsTheOperatorDetails(string operatorType)
        {
            // Generate random operator details based on operator type
            var operatorDetails = Utils.GenerateOperatorDetails(operatorType);

            // Enter the details into the form
            addOperatorDetailsPage?.EnterOperatorDetails(operatorDetails);

            // Store in scenario context with keys for easy access
            _scenarioContext[$"{operatorType}Name"] = operatorDetails.OperatorName;
            _scenarioContext[$"{operatorType}Address"] = operatorDetails.Address;
            _scenarioContext[$"{operatorType}Country"] = operatorDetails.Country;
        }

        [When(@"the user clicks Save and Continue")]
        public void WhenTheUserClicksSaveAndContinue()
        {
            addOperatorDetailsPage?.ClickSaveAndContinue();
        }
    }
}