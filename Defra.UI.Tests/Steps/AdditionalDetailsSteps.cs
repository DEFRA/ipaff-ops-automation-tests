using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class AdditionalDetailsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IAdditionalDetailsPage? additionalDetailsPage => _objectContainer.IsRegistered<IAdditionalDetailsPage>() ? _objectContainer.Resolve<IAdditionalDetailsPage>() : null;


        public AdditionalDetailsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Additional details page should be displayed")]
        public void ThenTheAdditionalDetailsPageShouldBeDisplayed()
        {
            Assert.True(additionalDetailsPage?.IsPageLoaded(), "Description of the goods Additional details page not loaded");
        }

        [When("the user selects {string} radio button on the Additional details page")]
        public void WhenTheUserSelectsAnyRadioButtonOnTheAdditionalDetailsPage(string option)
        {
            additionalDetailsPage?.ClickImportingProduct(option);
            _scenarioContext.Add("Temperature", option);
        }

        [Then("the corresponding radio button should be selected")]
        public void ThenTheCorrespondingRadioButtonShouldBeSelected()
        {
            throw new PendingStepException();
        }
    }
}