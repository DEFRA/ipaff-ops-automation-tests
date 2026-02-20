using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class ManageCatchCertificatesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IManageCatchCertificatesPage? manageCatchCertificates => _objectContainer.IsRegistered<IManageCatchCertificatesPage>() ? _objectContainer.Resolve<IManageCatchCertificatesPage>() : null;

        public ManageCatchCertificatesSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("Manage catch certificates page is displayed")]
        public void ThenTheDashboardShouldBeDisplayed()
        {
            Assert.True(manageCatchCertificates?.IsPageLoaded("Manage catch certificates"), "Manage catch certificates page is not displayed");
        }

        [Then("the user verifies there are {string} certificates attached")]
        public void ThenTheUserVerifiesThereAreCertificatesAttached(string numberOfCertificates)
        {
            Assert.True(manageCatchCertificates?.VerifyNumberOfCertificates(numberOfCertificates), "The number of certificates are not equal to " + numberOfCertificates);
        }

        [When("the user selects the {string} option for Do you need to upload more catch certificates?")]
        public void WhenTheUserSelectsOption(string option)
        {
            manageCatchCertificates?.SelectOption(option.ToLower());
        }

        [When("the user clicks on Add details link")]
        public void WhenTheUserClicksOnAddDetailsLink()
        {
            manageCatchCertificates?.ClickAddDetailsLink();
        }

        [Then("the user verifies success message displays the number of files added")]
        public void ThenTheUserVerifiesSuccessMessageDisplaysTheNumberOfFilesAdded()
        {
            var expectedFileCount = _scenarioContext.GetFromContext("TotalCatchCertificateFiles", 3);

            var isDisplayed = manageCatchCertificates?.VerifySuccessMessageDisplaysNumberOfFilesAdded(expectedFileCount);
            Assert.True(isDisplayed, $"Success message does not display the correct number of files added ({expectedFileCount})");
        }

        [Then("the user verifies the number of attachments missing details based on added files")]
        public void ThenTheUserVerifiesTheNumberOfAttachmentsMissingDetailsBasedOnAddedFiles()
        {
            var expectedMissingCount = _scenarioContext.GetFromContext("TotalCatchCertificateFiles", 3);

            var isValid = manageCatchCertificates?.VerifyNumberOfAttachmentsMissingDetails(expectedMissingCount);
            Assert.True(isValid, $"The number of attachments missing details does not match expected count ({expectedMissingCount})");
        }

        [Then("the user verifies {string} heading is displayed")]
        public void ThenTheUserVerifiesHeadingIsDisplayed(string headingText)
        {
            var isDisplayed = manageCatchCertificates?.VerifyCatchCertificatesUploadedHeadingDisplayed();
            Assert.True(isDisplayed, $"Heading '{headingText}' is not displayed on the page");
        }

        [Then("the user verifies {string} is displayed for each attachment")]
        public void ThenTheUserVerifiesIsDisplayedForEachAttachment(string labelText)
        {
            var expectedAttachmentCount = _scenarioContext.GetFromContext("TotalCatchCertificateFiles", 3);

            var isDisplayed = manageCatchCertificates?.VerifyNumberOfCatchCertificatesDisplayedForEachAttachment(expectedAttachmentCount);
            Assert.True(isDisplayed, $"'{labelText}' is not displayed for each of the {expectedAttachmentCount} attachments");
        }

        [Then("the user verifies each attachment is displayed as {string} format")]
        public void ThenTheUserVerifiesEachAttachmentIsDisplayedAsFormat(string format)
        {
            var totalAttachments = _scenarioContext.GetFromContext("TotalCatchCertificateFiles", 3);

            var isValid = manageCatchCertificates?.VerifyEachAttachmentDisplayedAsXofYFormat(totalAttachments);
            Assert.True(isValid, $"Attachments are not displayed in '{format}' format with total of {totalAttachments} attachments");
        }

        [Then("the user verifies each attachment has a populated input box with the number of catch certificates in that attachment")]
        public void ThenTheUserVerifiesEachAttachmentHasAPopulatedInputBox()
        {
            var expectedAttachmentCount = _scenarioContext.GetFromContext("TotalCatchCertificateFiles", 3);

            var isValid = manageCatchCertificates?.VerifyEachAttachmentHasPopulatedInputBox(expectedAttachmentCount);
            Assert.True(isValid, $"Not all {expectedAttachmentCount} attachments have populated input boxes with catch certificate numbers");
        }

        [Then("the user verifies each attachment has an {string} button")]
        public void ThenTheUserVerifiesEachAttachmentHasAnUpdateButton(string buttonText)
        {
            var expectedAttachmentCount = _scenarioContext.GetFromContext("TotalCatchCertificateFiles", 3);

            var isValid = manageCatchCertificates?.VerifyEachAttachmentHasUpdateButton(expectedAttachmentCount);
            Assert.True(isValid, $"Not all {expectedAttachmentCount} attachments have an '{buttonText}' button");
        }

        [Then("the user verifies each attachment has {string} and {string} links")]
        public void ThenTheUserVerifiesEachAttachmentHasAddDetailsAndRemoveLinks(string link1, string link2)
        {
            var expectedAttachmentCount = _scenarioContext.GetFromContext("TotalCatchCertificateFiles", 3);

            var isValid = manageCatchCertificates?.VerifyEachAttachmentHasAddDetailsAndRemoveLinks(expectedAttachmentCount);
            Assert.True(isValid, $"Not all {expectedAttachmentCount} attachments have '{link1}' and '{link2}' links");
        }

        [When("the user clicks View or amend details link for attachment {int}")]
        public void WhenTheUserClicksViewOrAmendDetailsLinkForAttachment(int attachmentNumber)
        {
            manageCatchCertificates?.ClickViewOrAmendDetailsLinkForAttachment(attachmentNumber);
        }
    }
}