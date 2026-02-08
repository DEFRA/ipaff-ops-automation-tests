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

        [When("the user selects the {int} species under Select species being imported under this catch certificate")]
        public void WhenTheUserSelectAllUnderSelectSpecies(int species)
        {
            Thread.Sleep(2000);
            addCatchCertificateDetails?.SelectSpecies(species);

            var speciesDetails = addCatchCertificateDetails?.GetSpeciesDetailsList(species);

            if (speciesDetails != null && speciesDetails.Length > 0)
            {
                _scenarioContext.AppendStringToScenarioContextArray("CatchCertificateCommodityCode1", speciesDetails[0]);
                _scenarioContext.AppendStringToScenarioContextArray("CatchCertificateSpeciesDescription1", speciesDetails[1]);
            }
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
        public void ThenTheUserCanSeeCatchCertificateReferenceDetailsSectionsForInput(int numberOfRefBlocks)
        {
            Assert.True(addCatchCertificateDetails?.VerifyNoOfCatchReferenceSections(numberOfRefBlocks), $"The expected number of catch certificate reference sections should be {numberOfRefBlocks}");
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
    }
}
