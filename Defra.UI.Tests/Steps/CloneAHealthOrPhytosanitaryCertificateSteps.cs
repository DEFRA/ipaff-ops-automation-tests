using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class CloneAHealthOrPhytosanitaryCertificateSteps
    {

        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICloneAHealthOrPhytosanitaryCertificatePage? cloneAHealthOrPhytosanitaryCertificate => _objectContainer.IsRegistered<ICloneAHealthOrPhytosanitaryCertificatePage>() ? _objectContainer.Resolve<ICloneAHealthOrPhytosanitaryCertificatePage>() : null;

        public CloneAHealthOrPhytosanitaryCertificateSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Clone a health or phytosanitary certificate page should be displayed")]
        public void ThenTheCloneAHealthOrPhytosanitaryCertificatePageShouldBeDisplayed()
        {
            Assert.True(cloneAHealthOrPhytosanitaryCertificate?.IsPageLoaded(), "Clone a health or phytosanitary certificate page not loaded");
        }

        [Then("the user verifies all the content in Clone a health or phytosanitary certificate page")]
        public void ThenTheUserVerifiesAllTheContentInCloneAHealthOrPhytosanitaryCertificatePage()
        {
            Assert.True(cloneAHealthOrPhytosanitaryCertificate?.VerifyClonePageDisplayText(), "Clone a health or phytosanitary certificate page not loaded");
        }

        [Then("the user selected the importing option as {string}")]
        public void ThenTheUserSelectedTheImportingOptionAs(string importOption)
        {
            cloneAHealthOrPhytosanitaryCertificate?.SelectImportingOption(importOption);
            _scenarioContext["ImportType"] = importOption;
        }

        [When("the user clicks on continue button")]
        public void WhenTheUserClicksOnContinueButton()
        {
            cloneAHealthOrPhytosanitaryCertificate?.ContinueButton();
        }

    }
}

