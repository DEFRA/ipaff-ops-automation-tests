using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterReviewYourAnswersSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterReviewYourAnswersPage? exporterReviewYourAnswersPage =>
            _objectContainer.IsRegistered<IExporterReviewYourAnswersPage>()
                ? _objectContainer.Resolve<IExporterReviewYourAnswersPage>()
                : null;

        public ExporterReviewYourAnswersSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Review your answers page is displayed")]
        public void ThenTheReviewYourAnswersPageIsDisplayed()
        {
            Assert.True(exporterReviewYourAnswersPage?.IsPageLoaded(), "Review your answers page is not displayed");
        }

        [When("the user clicks the Continue button on the Review your answers page")]
        public void WhenTheUserClicksTheContinueButtonOnTheReviewYourAnswersPage()
        {
            exporterReviewYourAnswersPage?.ClickContinueButton();
        }
    }
}