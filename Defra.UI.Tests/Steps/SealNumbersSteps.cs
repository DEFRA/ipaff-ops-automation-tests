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
    public class SealNumbersSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ISealNumbersPage? sealNumbersPage => _objectContainer.IsRegistered<ISealNumbersPage>() ? _objectContainer.Resolve<ISealNumbersPage>() : null;


        public SealNumbersSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Seal numbers page should be displayed")]
        public void ThenTheSealNumbersPageShouldBeDisplayed()
        {
            Assert.True(sealNumbersPage?.IsPageLoaded());
        }
    }
}