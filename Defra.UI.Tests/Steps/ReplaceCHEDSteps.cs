using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ReplaceCHEDSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IReplaceCHEDPage? replaceCHEDPage => _objectContainer.IsRegistered<IReplaceCHEDPage>() ? _objectContainer.Resolve<IReplaceCHEDPage>() : null;

        public ReplaceCHEDSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Replace CHED page should be displayed")]
        public void ThenTheReplaceCHEDPageShouldBeDisplayed()
        {
            Assert.True(replaceCHEDPage?.IsPageLoaded(), "Replace CHED page not loaded");
        }

        [When("the user clicks Yes, replace this CHED")]
        public void WhenTheUserClicksYesReplaceThisCHED()
        {
            replaceCHEDPage?.ClickYesReplaceThisCHED();
        }
    }
}