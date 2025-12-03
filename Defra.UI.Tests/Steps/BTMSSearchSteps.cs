using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;


namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class BTMSSearchSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IBTMSSearchPage? btmsSearchPage => _objectContainer.IsRegistered<IBTMSSearchPage>() ? _objectContainer.Resolve<IBTMSSearchPage>() : null;

        public BTMSSearchSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the BTMS search screen should be displayed")]
        public void ThenTheBTMSSearchScreenShouldBeDisplayed()
        {
            Assert.True(btmsSearchPage?.IsPageLoaded(), "Search for an MRN, CHED, GMR or DUCR page not loaded");
        }


        [When("the user searches for the CHED created earlier")]
        public void WhenTheUserSearchesForTheCHEDCreatedEarlierAndChecksAllDetailsMatch()
        {
            //var chedRef = _scenarioContext.Get<string>("CHEDReference");
            btmsSearchPage?.SearchForChed("CHEDP.GB.2025.1055883");
        }
    }
}