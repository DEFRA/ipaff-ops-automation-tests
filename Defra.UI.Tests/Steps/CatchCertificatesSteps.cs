using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CatchCertificatesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICatchCertificatesPage? catchCertificatesPage => _objectContainer.IsRegistered<ICatchCertificatesPage>() ? _objectContainer.Resolve<ICatchCertificatesPage>() : null;

        public CatchCertificatesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Catch cerificates page should be displayed")]
        public void ThenTheCatchCerificatesPageShouldBeDisplayed()
        {
            Assert.True(catchCertificatesPage?.IsPageLoaded(), "Documents Catch certificates page not loaded");
        }

        [When("the user selects {string} option for add catch certificate")]
        [Then("the user selects {string} option for add catch certificate")]
        public void WhenTheUserSelectsOption(string option)
        {
            catchCertificatesPage?.SelectAddCatchCertificate(option);
        }

        [When("the user verifies {string} is displayed")]
        [Then("the user verifies {string} is displayed")]
        public void ThenTheUserVerifiesQuestionIsDisplayed(string questionText)
        {
            var isDisplayed = catchCertificatesPage?.VerifyQuestionDisplayed(questionText);
            Assert.True(isDisplayed, $"Expected question '{questionText}' is not displayed on the page");
        }

        [When("the user verifies the radio buttons are {string} and {string}")]
        [Then("the user verifies the radio buttons are {string} and {string}")]
        public void ThenTheUserVerifiesRadioButtonsAre(string yesText, string noText)
        {
            var areDisplayed = catchCertificatesPage?.VerifyRadioButtonsDisplayed(yesText, noText);
            Assert.True(areDisplayed, $"Expected radio buttons '{yesText}' and '{noText}' are not displayed correctly");
        }
    }
}