using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class LocalReferenceNumberSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ILocalReferenceNumberPage? localReferenceNumberPage => _objectContainer.IsRegistered<ILocalReferenceNumberPage>() ? _objectContainer.Resolve<ILocalReferenceNumberPage>() : null;


        public LocalReferenceNumberSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("Local reference number page should be displayed")]
        public void ThenLocalReferenceNumberPageShouldBeDisplayed()
        {
            Assert.True(localReferenceNumberPage?.IsPageLoaded());
        }

        [When("the user enters a local reference number and clicks Save and continue")]
        public void WhenTheUserEntersALocalReferenceNumberAndClicksSaveAndContinue()
        {
            var customDeclarionRef = _scenarioContext.Get<string>("CustomsDeclarationReference");
            _scenarioContext.Add("BorderControlPostReference", customDeclarionRef);
            localReferenceNumberPage?.EnterLocalReferenceNumber(customDeclarionRef);
            localReferenceNumberPage?.ClickSaveAndContinue();
        }

        [When("the user clicks Save and continue without entering the local reference number data")]
        public void WhenTheUserClicksSaveAndContinueWithoutEnteringTheLocalReferenceNumberData()
        {
            localReferenceNumberPage?.ClickSaveAndContinue();
        }

    }
}