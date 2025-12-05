using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class DeclarationSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IDeclarationPage? declarationPage => _objectContainer.IsRegistered<IDeclarationPage>() ? _objectContainer.Resolve<IDeclarationPage>() : null;

        public DeclarationSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Declaration page should be displayed")]
        public void ThenTheDeclarationPageShouldBeDisplayed()
        {
            Assert.True(declarationPage?.IsPageLoaded(), "Declaration page not loaded");
        }

        [When("the user ticks the checkbox to declare that the information is true and correct")]
        public void WhenTheUserTicksTheCheckboxToDeclareThatTheInformationIsTrueAndCorrect()
        {
            declarationPage?.CheckDeclarationAgreement();
        }

        [When("the user clicks Submit notification")]
        public void WhenTheUserClicksSubmitNotification()
        {
            declarationPage?.ClickSubmitNotification();
        }
    }
}