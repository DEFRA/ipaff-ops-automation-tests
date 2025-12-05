using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;
using System.ComponentModel;

namespace Defra.UI.Tests.Steps.CP
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
            localReferenceNumberPage?.EnterLocalReferenceNumber(customDeclarionRef);
            localReferenceNumberPage?.ClickSaveAndContinue();
        }
    }
}