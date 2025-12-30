using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ReviewBorderNotificationSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IReviewBorderNotificationPage? reviewBorderNotificationPage => _objectContainer.IsRegistered<IReviewBorderNotificationPage>() ? _objectContainer.Resolve<IReviewBorderNotificationPage>() : null;

        public ReviewBorderNotificationSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("Reveiw border notification page should be displayed")]
        public void ThenReveiwBorderNotificationPageShouldBeDisplayed()
        {
            Assert.True(reviewBorderNotificationPage?.IsPageLoaded(), "Reveiw border notification page not loaded");
        }


        [When("the user download the document attached in accompanying documents")]
        public void WhenTheUserDownloadTheDocumentAttachedInAccompanyingDocuments()
        {
            reviewBorderNotificationPage?.ClickDocumentLink();
        }


        [When("the user clicks submit button")]
        public void WhenTheUserClicksSubmitButton()
        {
            reviewBorderNotificationPage?.ClickSubmitButton();
        }

        [Then("the user switch to next tab and open the browser downloads")]
        public void ThenTheUserSwitchToNextTabAndOpenTheBrowserDownloads()
        {
            reviewBorderNotificationPage?.OpenDownloadsInNewTab();
        }

        [Then("verifies the document {string} downloaded successfully")]
        public void ThenVerifiesTheDocumentDownloadedSuccessfully(string fileName)
        {
            Assert.True(reviewBorderNotificationPage?.VerifyFileDownloaded(fileName));
        }
    }
}