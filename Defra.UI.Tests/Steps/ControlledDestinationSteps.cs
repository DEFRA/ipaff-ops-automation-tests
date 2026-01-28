using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;


namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class ControlledDestinationSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IControlledDestinationPage? controlledDestinationPage => _objectContainer.IsRegistered<IControlledDestinationPage>() ? _objectContainer.Resolve<IControlledDestinationPage>() : null;


        public ControlledDestinationSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Select a controlled destination page should be displayed")]
        public void ThenTheSelectAControlledDestinationPageShouldBeDisplayed()
        {
            Assert.True(controlledDestinationPage?.IsPageLoaded(), "Select a controlled destination page not loaded");
        }

        [When("the user clicks Add a controlled destination")]
        public void WhenTheUserClicksAddAControlledDestination()
        {
            controlledDestinationPage?.ClickAddControlledDestination();
        }

        [Then("the chosen controlled destination should be displayed")]
        public void ThenTheChosenControlledDestinationShouldBeDisplayed()
        {
            var controlledDestinationName = _scenarioContext.Get<string>("ControlledDestinationName");
            var controlledDestinationAddress = _scenarioContext.Get<string>("ControlledDestinationAddress");
            var controlledDestinationType = _scenarioContext.Get<string>("ControlledDestinationType");

            Assert.True(controlledDestinationPage?.VerifySelectedControlledDestination(controlledDestinationName, controlledDestinationAddress, controlledDestinationType),
                        "Controlled destination details do not match");
        }

    }
}