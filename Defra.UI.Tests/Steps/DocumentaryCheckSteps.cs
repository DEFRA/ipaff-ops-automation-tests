using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;
using System.ComponentModel;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class DocumentaryCheckSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IDocumentaryCheckPage? documentaryCheckPage => _objectContainer.IsRegistered<IDocumentaryCheckPage>() ? _objectContainer.Resolve<IDocumentaryCheckPage>() : null;


        public DocumentaryCheckSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Documentary check page should be displayed")]
        public void ThenTheDocumentaryCheckPageShouldBeDisplayed()
        {
            Assert.True(documentaryCheckPage?.IsPageLoaded());
        }


        [When("the user selects {string} for the documentary check and clicks Save and continue")]
        public void WhenTheUserSelectsForTheDocumentaryCheckAndClicksSaveAndContinue(string decision)
        {
            _scenarioContext.Add("DocumentaryCheckDecision", decision);
            documentaryCheckPage?.SelectDocumentaryCheckDecision(decision);
            documentaryCheckPage?.ClickSaveAndContinue();
        }
    }
}