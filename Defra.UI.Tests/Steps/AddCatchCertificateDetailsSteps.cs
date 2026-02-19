using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;
using System.Globalization;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class AddCatchCertificateDetailsSteps
    {

        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IAddCatchCertificateDetailsPage? addCatchCertificateDetails => _objectContainer.IsRegistered<IAddCatchCertificateDetailsPage>() ? _objectContainer.Resolve<IAddCatchCertificateDetailsPage>() : null;


        public AddCatchCertificateDetailsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("Add catch certificate details page should be displayed")]
        public void ThenAddCatchCertificatesDetailsPageShouldBeDisplayed()
        {
            Assert.True(addCatchCertificateDetails?.IsPageLoaded("Add catch certificate details"), "Add catch certificate details page not loaded");
        }

        [Then("{string} is displayed in Add catch certificate page")]
        public void ThenTheUserVerifiesThereAreCertificatesAttached(string text)
        {
            Assert.True(addCatchCertificateDetails?.VerifyContent(text), text + " is not displayed");
        }

        [When("the user selects the {string} species under Select species being imported under this catch certificate")]
        public void WhenTheUserSelectAllUnderSelectSpecies(string species)
        {
            Thread.Sleep(2000);

            var speciesList = species.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var commodityCodes = _scenarioContext.GetFromContext<List<string>>("CatchCertificateCommodityCode", []);
            var speciesNames = _scenarioContext.GetFromContext<List<string>>("CatchCertificateSpeciesDescription", []);
            var commodityCode = _scenarioContext.GetFromContext<List<string>>("CommodityCode", []);

            foreach (var item in speciesList)
            {
                addCatchCertificateDetails?.SelectSpeciesByName(item);
                commodityCodes.Add(string.Join(",", commodityCode));
                speciesNames.Add(item);
            }

            _scenarioContext["CatchCertificateCommodityCode"] = commodityCodes;
            _scenarioContext["CatchCertificateSpeciesDescription"] = speciesNames;
        }

        [Then("the calendar icon is displayed in Add catch certificate page")]
        public void ThenTheCalendarIconIsDisplayedInAddCatchCertificatePage()
        {
            Assert.True(addCatchCertificateDetails?.VerifyCalendar(), "Calendar icon is not displayed");
        }

        [When("the user clicks on Change link in Add catch certificate details page")]
        public void WhenTheUserClicksOnChangeLinkInAddCatchCertificateDetailsPage()
        {
            addCatchCertificateDetails?.ClickChangeLink();
        }

        [When("the user enters {string} for Number of catch certificates in this attachment")]
        public void WhenTheUserEntersForNumberOfCatchCertificatesInThisAttachment(string noOfCertificateRef)
        {
            addCatchCertificateDetails?.EnterNumberOfCatchCertificates(noOfCertificateRef);
            Utils.AppendStringToScenarioContextArray(_scenarioContext, "NoOfCertificateReference", noOfCertificateRef);
        }

        [When("the user clicks on Update button")]
        public void WhenTheUserClicksOnUpdateButton()
        {
            addCatchCertificateDetails?.ClickUpdate();
        }

        [Then("the user can see {int} Catch certificate reference details sections for input")]
        public void ThenTheUserCanSeeCatchCertificateReferenceDetailsSectionsForInput(int noOfRefSections)
        {
            Assert.True(addCatchCertificateDetails?.VerifyNoOfCatchReferenceSections(noOfRefSections), $"The expected number of catch reference section would be {noOfRefSections}");
        }

        [When("the user enters {string} in catch certificate reference {int}")]
        public void WhenTheUserEntersInCatchCertificateReference(string reference, int index)
        {
            addCatchCertificateDetails?.EnterCatchCertificateReference(reference, index);
            Utils.AppendStringToScenarioContextArray(_scenarioContext, $"CatchCertificateReference{index}", reference);
        }

        [When("the user enters Data of issue {int} as {string}{string}{string} in Add catch certificate page")]
        public void WhenTheUserEntersDataOfIssueAsInAddCatchCertificatePage(int index, string day, string month, string year)
        {
            addCatchCertificateDetails?.EnterDateOfIssue(day, month, year, index);

            var date = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
            var dateOfIssueCatchCertificate = date.ToString("d MMMM yyyy", CultureInfo.InvariantCulture);

            Utils.AppendStringToScenarioContextArray(_scenarioContext, $"CatchCertificateDateOfIssue{index}", dateOfIssueCatchCertificate);
        }

        [When("the user enters {string} in Flag state of catching vessels {int}")]
        public void WhenTheUserEntersInFlagStateOfCatchingVessels(string flagState, int index)
        {
            addCatchCertificateDetails?.EnterFlagStateOfCatchingVessel(flagState, index);
            Utils.AppendStringToScenarioContextArray(_scenarioContext, $"FlagStateOfCatchingVessel{index}", flagState);
        }

        [When("the user Clicks on Update details {int}")]
        public void WhenTheUserClicksOnUpdateDetails(int index)
        {
            addCatchCertificateDetails?.ClickUpdate(index);
        }

        [When("the user clicks on Save and return to manage catch certificates link")]
        public void WhenTheUserClicksOnSaveAndReturnToManageCatchCertificatesLink()
        {
            addCatchCertificateDetails?.ClickSaveAndReturnToManageCertificateLink();
        }

        [Then("the user verifies Attachment {int} is displayed underneath Add catch certificate details")]
        public void ThenTheUserVerifiesAttachmentNumberIsDisplayedUnderneathAddCatchCertificateDetails(int attachmentNumber)
        {
            var totalAttachments = _scenarioContext.GetFromContext("TotalCatchCertificateFiles", 3);

            var isDisplayed = addCatchCertificateDetails?.VerifyAttachmentNumberDisplayed(attachmentNumber, totalAttachments);
            Assert.True(isDisplayed, $"'Attachment {attachmentNumber} of {totalAttachments}' is not displayed underneath Add catch certificate details");
        }

        [Then("the user verifies {string} with {string} link is displayed")]
        public void ThenTheUserVerifiesNumberOfCatchCertificatesWithChangeLinkIsDisplayed(string labelText, string linkText)
        {
            var isDisplayed = addCatchCertificateDetails?.VerifyNumberOfCatchCertificatesWithChangeLinkDisplayed();
            Assert.True(isDisplayed, $"'{labelText}' with '{linkText}' link is not displayed");
        }

        [Then("the user verifies {string} field is displayed")]
        public void ThenTheUserVerifiesFieldIsDisplayed(string fieldName)
        {
            bool isDisplayed = false;

            if (fieldName.Contains("Catch certificate reference"))
            {
                isDisplayed = addCatchCertificateDetails?.VerifyCatchCertificateReferenceFieldDisplayed() ?? false;
            }
            else if (fieldName.Contains("Flag state of catching vessel"))
            {
                isDisplayed = addCatchCertificateDetails?.VerifyFlagStateFieldDisplayed() ?? false;
            }

            Assert.True(isDisplayed, $"'{fieldName}' field is not displayed");
        }

        [Then("the user verifies {string} with Day, Month, Year fields and calendar icon is displayed")]
        public void ThenTheUserVerifiesDateOfIssueWithFieldsAndCalendarIconIsDisplayed(string labelText)
        {
            var isDisplayed = addCatchCertificateDetails?.VerifyDateOfIssueFieldsWithCalendarDisplayed();
            Assert.True(isDisplayed, $"'{labelText}' with Day, Month, Year fields and calendar icon is not displayed");
        }

        [Then("the user verifies {string} with {string} option is displayed")]
        public void ThenTheUserVerifiesSelectSpeciesWithSelectAllOptionIsDisplayed(string headingText, string optionText)
        {
            var isDisplayed = addCatchCertificateDetails?.VerifySelectSpeciesWithSelectAllDisplayed();
            Assert.True(isDisplayed, $"'{headingText}' with '{optionText}' option is not displayed");
        }

        [Then("the user verifies {string} button is displayed")]
        public void ThenTheUserVerifiesButtonIsDisplayed(string buttonText)
        {
            var isDisplayed = addCatchCertificateDetails?.VerifySaveAndContinueButtonDisplayed();
            Assert.True(isDisplayed, $"'{buttonText}' button is not displayed");
        }

        [Then("the user verifies {string} link is displayed")]
        public void ThenTheUserVerifiesLinkIsDisplayed(string linkText)
        {
            bool isDisplayed = false;

            if (linkText.Contains("Save and return to manage catch certificates"))
            {
                isDisplayed = addCatchCertificateDetails?.VerifySaveAndReturnToManageCatchCertificatesLinkDisplayed() ?? false;
            }
            else if (linkText.Contains("Save and return to hub"))
            {
                isDisplayed = addCatchCertificateDetails?.VerifySaveAndReturnToHubLinkDisplayed() ?? false;
            }

            Assert.True(isDisplayed, $"'{linkText}' link is not displayed");
        }
    }
}