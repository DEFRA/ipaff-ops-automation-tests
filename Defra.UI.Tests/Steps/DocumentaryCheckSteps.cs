using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
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
            Assert.True(documentaryCheckPage?.IsPageLoaded(), "Documentary check page is not displayed");
        }


        [When("the user selects {string} for the Documentary check and clicks Save and continue")]
        [When("the user selects {string} for the documentary check and clicks Save and continue")]
        public void WhenTheUserSelectsForTheDocumentaryCheckAndClicksSaveAndContinue(string decision)
        {
            _scenarioContext["DocumentaryCheckDecision"] = decision;
            documentaryCheckPage?.SelectDocumentaryCheckDecision(decision);
            documentaryCheckPage?.ClickSaveAndContinue();
        }
    }
}