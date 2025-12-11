using Reqnroll.BoDi;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class LatestHealthCertificateSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private ILatestHealthCertificatePage? latestHealthCertificatePage => _objectContainer.IsRegistered<ILatestHealthCertificatePage>() ? _objectContainer.Resolve<ILatestHealthCertificatePage>() : null;

        public LatestHealthCertificateSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Latest Health Certificate page should be displayed")]
        public void ThenTheLatestHealthCertificatePageShouldBeDisplayed()
        {
            Assert.True(latestHealthCertificatePage?.IsPageLoaded(), "Latest Health Certificate page not loaded");
        }

        [When("the user enters Latest Health Certificate Document reference {string}")]
        public void WhenTheUserEntersDocumentReference(string reference)
        {
            latestHealthCertificatePage?.EnterDocumentReference(reference);
            _scenarioContext.Add("HealthCertificateReference", reference);
        }

        [When("the user enters Latest Health Certificate date of issue {string}{string}{string}")]
        public void WhenTheUserEntersDateOfIssue(string day, string month, string year)
        {
            latestHealthCertificatePage?.EnterDateOfIssue(day, month, year);
            var dateofIssue = day + " " + month + " " + year;
            _scenarioContext.Add("HealthCertificateDateOfIssue", dateofIssue);
        }

        [When("the user clicks on Add attachment link on the Latest Health Certificate page")]
        public void WhenTheUserClicksOnAddAttachmentLinkOnTheLatestHealthCertificatePage()
        {
            latestHealthCertificatePage?.ClickAddAttachmentLink();
        }

        [When("the user uploads the Veterinary Health Certificate {string} in the format {string}")]
        public void WhenTheUserUploadsTheVeterinaryHealthCertificateInTheFormat(string name, string format)
        {
            var filename = name + format;
            latestHealthCertificatePage?.AddHealthCertificate(filename);
            _scenarioContext.Add("HealthCertificateFileName", filename);
        }

        [Then("the Veterinary Health Certificate {string} {string} is uploaded successfully")]
        public void ThenTheVeterinaryHealthCertificateIsUploadedSuccessfully(string name, string format)
        {
            var expectedFileName = _scenarioContext.Get<string>("HealthCertificateFileName");
            var displayedFileName = latestHealthCertificatePage?.GetFileName;

            // Handle filename truncation - the UI truncates long filenames but keeps the extension
            // Based on observed behaviour: "IPAFFS Test Health Certificate.docx" becomes "IPAFFS Test Health Cer.docx"
            var isMatch = false;

            if (!string.IsNullOrEmpty(displayedFileName) && !string.IsNullOrEmpty(expectedFileName))
            {
                var displayedExtension = Path.GetExtension(displayedFileName);
                var expectedExtension = Path.GetExtension(expectedFileName);

                var displayedNameWithoutExt = Path.GetFileNameWithoutExtension(displayedFileName);
                var expectedNameWithoutExt = Path.GetFileNameWithoutExtension(expectedFileName);

                // Check if extensions match and displayed name is the start of expected name (handles truncation)
                isMatch = displayedExtension.Equals(expectedExtension, StringComparison.OrdinalIgnoreCase) &&
                          expectedNameWithoutExt.StartsWith(displayedNameWithoutExt, StringComparison.OrdinalIgnoreCase);
            }

            Assert.True(
                isMatch,
                $"The Veterinary Health Certificate upload has failed. Expected '{expectedFileName}', but got '{displayedFileName}'"
            );
        }
    }
}