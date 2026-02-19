using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class UploadCatchCertificatesSteps
    {

        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;


        private IUploadCatchCertificatesPage? uploadCatchCertificates => _objectContainer.IsRegistered<IUploadCatchCertificatesPage>() ? _objectContainer.Resolve<IUploadCatchCertificatesPage>() : null;

        public UploadCatchCertificatesSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("Upload catch certificates page is displayed")]
        public void ThenTheDashboardShouldBeDisplayed()
        {
            Assert.True(uploadCatchCertificates?.IsPageLoaded("Upload catch certificates"), "Upload catch certificates page is not displayed");
        }

        [When("the user verifies {string} is displayed with drag and drop functionality")]
        [Then("the user verifies {string} is displayed with drag and drop functionality")]
        public void WhenTheUserVerifiesSelectDocumentsIsDisplayedWithDragAndDropFunctionality(string expectedHeading)
        {
            var headingDisplayed = uploadCatchCertificates?.VerifySelectDocumentsHeading();
            Assert.True(headingDisplayed, $"Expected heading '{expectedHeading}' is not displayed");

            var dragAndDropExists = uploadCatchCertificates?.VerifyDragAndDropFunctionality();
            Assert.True(dragAndDropExists, "Drag and drop functionality is not available or not displayed correctly");
        }

        [When("the user uploads the documents {string} {string} {string} in the format {string}")]
        public void WhenTheUserUploadsMultipleDocuments(string file1Name, string file2Name, string file3Name, string format)
        {
            var fileName1 = file1Name + format;
            var fileName2 = file2Name + format;
            var fileName3 = file3Name + format;

            uploadCatchCertificates?.UploadMultipleCatchCertificates(fileName1, fileName2, fileName3);

            // Store the filenames in scenario context for later verification
            _scenarioContext["CatchCertificate1"] = fileName1;
            _scenarioContext["CatchCertificate2"] = fileName2;
            _scenarioContext["CatchCertificate3"] = fileName3;
        }

        [When("the user verifies all {int} files are displayed")]
        [Then("the user verifies all {int} files are displayed")]
        public void WhenTheUserVerifiesAllFilesAreDisplayed(int expectedFileCount)
        {
            // Retrieve the stored filenames from scenario context
            var expectedFileNames = new List<string>();

            for (int i = 1; i <= expectedFileCount; i++)
            {
                var key = $"CatchCertificate{i}";
                if (_scenarioContext.ContainsKey(key))
                {
                    expectedFileNames.Add(_scenarioContext[key].ToString());
                }
            }

            var filesDisplayed = uploadCatchCertificates?.VerifyFilesAreDisplayed(expectedFileCount, expectedFileNames.ToArray());

            Assert.True(filesDisplayed,
                $"Expected {expectedFileCount} files to be displayed with names: {string.Join(", ", expectedFileNames)}, but verification failed");
        }

        [When("the user clicks Continue")]
        [Then("the user clicks Continue")]
        public void WhenTheUserClicksContinue()
        {
            uploadCatchCertificates?.ClickContinue();
        }
    }
}