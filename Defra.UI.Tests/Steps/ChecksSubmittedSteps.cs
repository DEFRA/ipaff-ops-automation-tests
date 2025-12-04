using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;


namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class ChecksSubmittedSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IChecksSubmittedPage? checksSubmittedPage => _objectContainer.IsRegistered<IChecksSubmittedPage>() ? _objectContainer.Resolve<IChecksSubmittedPage>() : null;

        public ChecksSubmittedSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Your checks have been submitted page should be displayed")]
        public void ThenTheYourChecksHaveBeenSubmittedPageShouldBeDisplayed()
        {
            Assert.True(checksSubmittedPage?.IsPageLoaded(), "Yours checks have been submitted page not loaded");
            _scenarioContext.Add("CHEDReferenceWithVersion", checksSubmittedPage.GetCHEDReferenceWithVersion());
            _scenarioContext.Add("Outcome", checksSubmittedPage.GetOutcome());
        }



       
    }
}