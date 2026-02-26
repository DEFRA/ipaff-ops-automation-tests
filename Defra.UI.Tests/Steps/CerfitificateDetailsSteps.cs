using Defra.UI.Tests.Contracts;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using FluentAssertions;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;
using System.Globalization;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class CerfitificateDetailsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICertificateDetailsPage? certificateDetailsPage => _objectContainer.IsRegistered<ICertificateDetailsPage>() ? _objectContainer.Resolve<ICertificateDetailsPage>() : null;
        private IPhytosanitaryCertificateDetailsPage? phytosanitaryCertificateDetailsPage => _objectContainer.IsRegistered<IPhytosanitaryCertificateDetailsPage>() ? _objectContainer.Resolve<IPhytosanitaryCertificateDetailsPage>() : null;
        private ICreatingThisNotificationForPage? creatingThisNotificationForPage => _objectContainer.IsRegistered<ICreatingThisNotificationForPage>() ? _objectContainer.Resolve<ICreatingThisNotificationForPage>() : null;
        private IDraftNotificationPage? draftNotificationPage => _objectContainer.IsRegistered<IDraftNotificationPage>() ? _objectContainer.Resolve<IDraftNotificationPage>() : null;

        public CerfitificateDetailsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Certificate details page should be displayed")]
        public void ThenTheCertificateDetailsPageShouldBeDisplayed()
        {
            Assert.True(certificateDetailsPage?.IsPageLoaded(), "Certificate details page not loaded");
        }

        [Then("the user searches for the notification for cloning which is not more than {int} days from creation")]
        public void ThenTheUserSearchesForTheNotificationForCloningWhichIsNotMoreThanDaysFromCreation(int notificateionCreatedDaysFrom)
        {
            var notificateionDetails = certificateDetailsPage?.GetNotificationDetailsForCloning();

            _scenarioContext["CloningNotificationDetails"] = notificateionDetails;
        }

        [Then("the user provided notification details in the search input fields")]
        public void ThenTheUserProvidedNotificationDetailsInTheSearchInputFields()
        {
            var notificateionDetails = _scenarioContext.GetFromContext<NotificateionDetails>("CloningNotificationDetails");

            certificateDetailsPage?.SelectCountryOfOrigin(notificateionDetails.CountryOfOriginOfCertificate);
            certificateDetailsPage?.EnterCertificateReferenceNumber(notificateionDetails.CertificateReferenceNumber);

            DateTime certificateDateOfIssue; 
            
            bool isValidDate = DateTime.TryParseExact(notificateionDetails.CertificateDateOfIssue, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out certificateDateOfIssue);

            certificateDetailsPage?.EnterCertificateDateOfIssueYear(certificateDateOfIssue.Day, certificateDateOfIssue.Month, certificateDateOfIssue.Year);
            certificateDetailsPage?.EnterConsignerConsigneeImporterName(notificateionDetails.ConsignorConsigneeOrImporterName);
        }

        [When("the user Clicks on Search button")]
        public void WhenTheUserClicksOnSearchButton()
        {
            certificateDetailsPage?.ClickSearchButton();
        }

        [Then("the Phytosanitary certificate details page should be displayed")]
        public void ThenThePhytosanitaryCertificateDetailsPageShouldBeDisplayed()
        {
            Assert.True(phytosanitaryCertificateDetailsPage?.IsPageLoaded(), "Phytosanitary certificate details page not loaded");
        }

        [Then("the user verified all the details on Phytosanitary certificate details page")]
        public void ThenTheUserVerifiedAllTheDetailsOnPhytosanitaryCertificateDetailsPage()
        {
            var notificateionDetails = _scenarioContext.GetFromContext<NotificateionDetails>("CloningNotificationDetails");
            var cloneCertificateDetails = phytosanitaryCertificateDetailsPage?.GetKeyAndValuesOfSummaryAndGoods();

            cloneCertificateDetails.Should().ContainKey("Health certificate number").WhoseValue.Should().Be(notificateionDetails.CertificateReferenceNumber);
            cloneCertificateDetails.Should().ContainKey("Country of origin").WhoseValue.Should().Be(notificateionDetails.CountryOfOriginOfCertificate);
            cloneCertificateDetails.Should().ContainKey("Purpose of the consignment").WhoseValue.Should().Be(notificateionDetails.PurposeOfTheConsignment);
            cloneCertificateDetails.Should().ContainKey("Commodity code").WhoseValue.Should().Be(notificateionDetails.CommodityCode);
            cloneCertificateDetails.Should().ContainKey("Description").WhoseValue.Should().Be(notificateionDetails.Description);
            cloneCertificateDetails.Should().ContainKey("Genus and species").WhoseValue.Should().Be(notificateionDetails.GenusAndSpecies);
            cloneCertificateDetails.Should().ContainKey("Net weight").WhoseValue.Should().Be(notificateionDetails.NetWeight);
            cloneCertificateDetails.Should().ContainKey("Packages").WhoseValue.Should().Be(notificateionDetails.Packages);
            cloneCertificateDetails.Should().ContainKey("Type of package").WhoseValue.Should().Be(notificateionDetails.TypeOfPackage);
            cloneCertificateDetails.Should().ContainKey("Quantity").WhoseValue.Should().Be(notificateionDetails.Quantity);
            cloneCertificateDetails.Should().ContainKey("Quantity type").WhoseValue.Should().Be(notificateionDetails.QuantityType);

            Assert.IsTrue(phytosanitaryCertificateDetailsPage?.VerifyContentAndTitlesOnPage(),"H2 titles didn't match");
            Assert.IsTrue(phytosanitaryCertificateDetailsPage?.IsCloneAndCancelButtonExists(), "Clone or Cancel button doesn't exists");
        }

        [When("the user Clicks on Clone button")]
        public void WhenTheUserClicksOnCloneButton()
        {
            phytosanitaryCertificateDetailsPage?.ClickCloneButton();
        }

        [Then("the Who are you creating this notification for page should be displayed")]
        public void ThenTheWhoAreYouCreatingThisNotificationForPageShouldBeDisplayed()
        {
            Assert.True(creatingThisNotificationForPage?.IsPageLoaded(), "Who are you creating this notification for? page not loaded");
        }

        [Then("the user selects the option of creating notification for as {string}")]
        public void ThenTheUserSelectsTheOptionOfCreatingNotificationForAs(string option)
        {
            creatingThisNotificationForPage?.SelectNotificationForOption(option);
        }

        [When("the user Clicks on Save and review button")]
        public void WhenTheUserClicksOnSaveAndReviewButton()
        {
            creatingThisNotificationForPage?.ClickSaveAndReviewButton();
        }

        [Then("the DRAFT CHEDPP notification code page should be displayed")]
        public void ThenTheDRAFTCHEDPPNotificationCodePageShouldBeDisplayed()
        {
            var notificateionDetails = _scenarioContext.GetFromContext<NotificateionDetails>("CloningNotificationDetails");
            Assert.True(draftNotificationPage?.IsPageLoaded(), "Draft notification page is not loaded");
        }

        [Then("the user records the Draft notification number")]
        public void ThenTheUserRecordsTheDraftNotificationNumber()
        {
            var draftReferenceNumber = draftNotificationPage?.GetDraftNotificationNumber();
            _scenarioContext["DraftCHEDNotificationRefNumber"] = draftReferenceNumber;
        }

        [Then("the user verifies the following information is displayed within a red outlined box")]
        public void ThenTheUserVerifiesTheFollowingInformationIsDisplayedWithinARedOutlinedBox(DataTable dataTable)
        {
            var expectedList = dataTable.Rows.Select(r => r["MissingOrIncorrect"].Trim()).ToList();

            Assert.True(draftNotificationPage?.VerifyAllMissingOrErrorLinksExists(expectedList), "Missing or incorrect links didn't match or exists");
        }

        [When("the user clicks on each red error message to enter the information which is missing")]
        public void WhenTheUserClicksOnEachRedErrorMessageToEnterTheInformationWhichIsMissing()
        {
            draftNotificationPage?.ClickEachLinkAndEnterMissingInformation();
        }

    }
}
