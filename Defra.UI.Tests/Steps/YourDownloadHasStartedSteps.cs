using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class YourDownloadHasStartedSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IYourDownloadHasStarted? yourDownloadHasStarted => _objectContainer.IsRegistered<IYourDownloadHasStarted>() ? _objectContainer.Resolve<IYourDownloadHasStarted>() : null;

        public YourDownloadHasStartedSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("Your download has started message should be displayed")]
        public void ThenYourDownloadHasStartedMessageShouldBeDisplayed()
        {
            Assert.True(yourDownloadHasStarted?.IsPageLoaded(), "Your download has started message didn't appear");
        }

        [Then("the user verifies that Documents is downloaded into a .zip file with the title of the CHED certificates in Downloaded folder")]
        public void ThenTheUserVerifiesThatDocumentsIsDownloadedIntoA_ZipFileWithTheTitleOfTheCHEDCertificatesInDownloadedFolder()
        {
            var chedReference = _scenarioContext.GetFromContext<string>("CHEDReference");

            Assert.IsTrue(yourDownloadHasStarted?.IsDownloadedZipFile(chedReference), "Zip file didn't downloaded in automation-download folder");
        }

        [When("the user clicks Return to documents button")]
        public void WhenTheUserClicksReturnToDocumentsButton()
        {
            yourDownloadHasStarted?.ClickReturnToDocuments();
        }

    }
}
