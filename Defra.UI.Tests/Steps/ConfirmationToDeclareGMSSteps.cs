using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ConfirmationToDeclareGMSSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IConfirmationToDeclareGMSPage? confirmationToDeclareGMSPage => _objectContainer.IsRegistered<IConfirmationToDeclareGMSPage>() ? _objectContainer.Resolve<IConfirmationToDeclareGMSPage>() : null;

        public ConfirmationToDeclareGMSSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Confirmation to declare GMS page should be displayed")]
        [Then("Confirmation to declare GMS page should be loaded")]
        public void ThenConfirmationToDeclareGMSPageShouldBeLoaded()
        {
            Assert.True(confirmationToDeclareGMSPage?.IsPageLoaded(), "Confirmation to declare GMS page not loaded");
        }

        [When("the user selects {string} confirmation option")]
        public void WhenTheUserSelectsConfirmationOption(string option)
        {
            confirmationToDeclareGMSPage?.SelectConfirmationOption(option);
            _scenarioContext["ConfirmationToDeclareGMS"] = option;
        }
    }
}