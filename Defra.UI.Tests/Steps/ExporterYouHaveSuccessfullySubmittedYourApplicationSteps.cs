using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterYouHaveSuccessfullySubmittedYourApplicationSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IExporterYouHaveSuccessfullySubmittedYourApplicationPage? exporterYouHaveSuccessfullySubmittedYourApplicationPage =>
            _objectContainer.IsRegistered<IExporterYouHaveSuccessfullySubmittedYourApplicationPage>()
                ? _objectContainer.Resolve<IExporterYouHaveSuccessfullySubmittedYourApplicationPage>()
                : null;

        public ExporterYouHaveSuccessfullySubmittedYourApplicationSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the You have successfully submitted your application for a certificate of conformity page is displayed")]
        public void ThenTheYouHaveSuccessfullySubmittedYourApplicationForACertificateOfConformityPageIsDisplayed()
        {
            Assert.True(exporterYouHaveSuccessfullySubmittedYourApplicationPage?.IsPageLoaded(), "Submission confirmation page is not displayed");
        }

        [Then("the You have successfully submitted your application for a phytosanitary certificate page is displayed")]
        public void ThenTheYouHaveSuccessfullySubmittedYourApplicationForAPhytosanitaryCertificatePageIsDisplayed()
        {
            Assert.True(exporterYouHaveSuccessfullySubmittedYourApplicationPage?.IsPhytosanitaryConfirmationPageLoaded(), "Phytosanitary certificate submission confirmation page is not displayed");
        }

        [Then("the You have successfully submitted your seed sampling application page is displayed")]
        public void ThenTheYouHaveSuccessfullySubmittedYourSeedSamplingApplicationPageIsDisplayed()
        {
            Assert.True(exporterYouHaveSuccessfullySubmittedYourApplicationPage?.IsSeedSamplingConfirmationPageLoaded(), "Seed sampling submission confirmation page is not displayed");
        }

        [Then("the Application complete page is displayed")]
        public void ThenTheApplicationCompletePageIsDisplayed()
        {
            Assert.True(exporterYouHaveSuccessfullySubmittedYourApplicationPage?.IsPhytosanitaryConfirmationPageLoaded(), "Application complete page is not displayed");
        }

        [Then("the user records the APHA reference number for {string}")]
        public void ThenTheUserRecordsTheAPHAReferenceNumberFor(string iterationName)
        {
            _scenarioContext[$"{iterationName}_APHAReference"] = exporterYouHaveSuccessfullySubmittedYourApplicationPage?.GetAphaReferenceNumber();
        }

        [Then("the user records the APHA reference number")]
        public void ThenTheUserRecordsTheAPHAReferenceNumber()
        {
            var reference = exporterYouHaveSuccessfullySubmittedYourApplicationPage?.GetAphaReferenceNumber();
            _scenarioContext["APHAReference"] = reference;
        }

        [Then("the guidance {string} is displayed on the Application complete page")]
        public void ThenTheGuidanceIsDisplayedOnTheApplicationCompletePage(string guidance)
        {
            Assert.True(exporterYouHaveSuccessfullySubmittedYourApplicationPage?.IsGuidanceDisplayed(guidance), $"Guidance '{guidance}' is not displayed on the Application complete page");
        }

        [When("the user clicks the View your applications button")]
        public void WhenTheUserClicksTheViewYourApplicationsButton()
        {
            exporterYouHaveSuccessfullySubmittedYourApplicationPage?.ClickViewYourApplicationsButton();
        }
    }
}