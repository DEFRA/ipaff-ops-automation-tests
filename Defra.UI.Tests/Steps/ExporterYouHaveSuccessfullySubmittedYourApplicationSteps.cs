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

        [Then("the user records the APHA reference number for {string}")]
        public void ThenTheUserRecordsTheAPHAReferenceNumberForSPS9510(string contextKey)
        {
            var aphaReference = exporterYouHaveSuccessfullySubmittedYourApplicationPage?.GetAphaReferenceNumber();
            Assert.False(string.IsNullOrWhiteSpace(aphaReference), "APHA reference number is not found");
            _scenarioContext[contextKey] = aphaReference;
        }
    }
}