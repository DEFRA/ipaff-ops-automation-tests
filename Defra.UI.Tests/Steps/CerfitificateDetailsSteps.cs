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
        private IAddIntendedUseOfBulbsPage? addIntendedUseOfBulbs => _objectContainer.IsRegistered<IAddIntendedUseOfBulbsPage>() ? _objectContainer.Resolve<IAddIntendedUseOfBulbsPage>() : null;
        private ICheckOrUpdateCommodityDetailsPage? checkOrUpdateCommodityDetails => _objectContainer.IsRegistered<ICheckOrUpdateCommodityDetailsPage>() ? _objectContainer.Resolve<ICheckOrUpdateCommodityDetailsPage>() : null;
        private IReviewYourNotificationPage? reviewYourNotificationPage => _objectContainer.IsRegistered<IReviewYourNotificationPage>() ? _objectContainer.Resolve<IReviewYourNotificationPage>() : null;
        

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
            var notificationDetails = certificateDetailsPage?.GetNotificationDetailsForCloning();

            _scenarioContext["CloningNotificationDetails"] = notificationDetails;
        }

        [Then("the user provided notification details in the search input fields")]
        public void ThenTheUserProvidedNotificationDetailsInTheSearchInputFields()
        {
            var notificationDetails = _scenarioContext.GetFromContext<NotificationDetails>("CloningNotificationDetails");

            certificateDetailsPage?.SelectCountryOfOrigin(notificationDetails.CountryOfOriginOfCertificate);
            _scenarioContext["CountryOfOrigin"] = notificationDetails.CountryOfOriginOfCertificate;
            certificateDetailsPage?.EnterCertificateReferenceNumber(notificationDetails.CertificateReferenceNumber);
            _scenarioContext["CertificateReferenceNumber"] = notificationDetails.CertificateReferenceNumber;

            DateTime certificateDateOfIssue; 
            
            bool isValidDate = DateTime.TryParseExact(notificationDetails.CertificateDateOfIssue, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out certificateDateOfIssue);

            certificateDetailsPage?.EnterCertificateDateOfIssueYear(certificateDateOfIssue.Day, certificateDateOfIssue.Month, certificateDateOfIssue.Year);
            certificateDetailsPage?.EnterConsignerConsigneeImporterName(notificationDetails.ConsignorConsigneeOrImporterName);
            _scenarioContext["ConsignorConsigneeOrImporterName"] = notificationDetails.ConsignorConsigneeOrImporterName;
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
            var notificationDetails = _scenarioContext.GetFromContext<NotificationDetails>("CloningNotificationDetails");
            var cloneCertificateDetails = phytosanitaryCertificateDetailsPage?.GetKeyAndValuesOfSummaryAndGoods();

            cloneCertificateDetails.Should().ContainKey("Health certificate number").WhoseValue.Should().Be(notificationDetails.CertificateReferenceNumber);
            cloneCertificateDetails.Should().ContainKey("Country of origin").WhoseValue.Should().Be(notificationDetails.CountryOfOriginOfCertificate);
            cloneCertificateDetails.Should().ContainKey("Purpose of the consignment").WhoseValue.Should().Be(notificationDetails.PurposeOfTheConsignment);
            cloneCertificateDetails.Should().ContainKey("Commodity code").WhoseValue.Should().Be(notificationDetails.CommodityCode);
            cloneCertificateDetails.Should().ContainKey("Description").WhoseValue.Should().Be(notificationDetails.Description);
            cloneCertificateDetails.Should().ContainKey("Genus and species").WhoseValue.Should().Be(notificationDetails.GenusAndSpecies);
            cloneCertificateDetails.Should().ContainKey("Net weight").WhoseValue.Should().Be(notificationDetails.NetWeightWithUnits);
            cloneCertificateDetails.Should().ContainKey("Packages").WhoseValue.Should().Be(notificationDetails.Packages);
            cloneCertificateDetails.Should().ContainKey("Type of package").WhoseValue.Should().Be(notificationDetails.TypeOfPackage);
            cloneCertificateDetails.Should().ContainKey("Quantity").WhoseValue.Should().Be(notificationDetails.Quantity);
            cloneCertificateDetails.Should().ContainKey("Quantity type").WhoseValue.Should().Be(notificationDetails.QuantityType);

            _scenarioContext["DocumentReference"] = notificationDetails.CertificateReferenceNumber;
            _scenarioContext["CountryOfOrigin"] = notificationDetails.CountryOfOriginOfCertificate;
            _scenarioContext["CommodityCode"] = notificationDetails.CommodityCode;
            _scenarioContext["CommodityDescription"] = notificationDetails.Description;
            _scenarioContext["GenusFirstCommodity"] = notificationDetails.GenusAndSpecies;
            _scenarioContext["TotalNetWeight"] = notificationDetails.NetWeight;
            _scenarioContext["TotalPackages"] = notificationDetails.Packages;
            _scenarioContext["FixedQuantity"] = notificationDetails.Quantity;
            _scenarioContext["FixedQuantityType"] = notificationDetails.QuantityType;
            _scenarioContext["PackageType"] = notificationDetails.TypeOfPackage;

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
            _scenarioContext["CreatingNotificationFor"] = option; 
        }

        [When("the user Clicks on Save and review button")]
        public void WhenTheUserClicksOnSaveAndReviewButton()
        {
            creatingThisNotificationForPage?.ClickSaveAndReviewButton();
        }

        [Then("the DRAFT CHEDPP notification code page should be displayed")]
        public void ThenTheDRAFTCHEDPPNotificationCodePageShouldBeDisplayed()
        {
            var notificationDetails = _scenarioContext.GetFromContext<NotificationDetails>("CloningNotificationDetails");
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

        [When("the user clicks on Check or update commodity details link")]
        public void WhenTheUserClicksOnCheckOrUpdateCommodityDetailsLink()
        {
            draftNotificationPage?.ClickCheckOrUpdateCommodityDetailsLink();
        }

        [Then("the Add intended use of bulbs page should be displayed")]
        public void ThenTheAddIntendedUseOfBulbsPageShouldBeDisplayed()
        {
            Assert.True(addIntendedUseOfBulbs?.IsPageLoaded(), "Add intended use of bulbs page is didn't load");
        }

        [Then("the user selects the Commodity from the list appeared")]
        public void ThenTheUserSelectsTheCommodityFromTheListAppeared()
        {
            addIntendedUseOfBulbs?.SelctCommodityCode();
            var commodityDetails = addIntendedUseOfBulbs?.GetCommodityDetails();
            _scenarioContext["CommodityCode"] = commodityDetails[0];
            _scenarioContext["GenusFirstCommodity"] = commodityDetails[1];
            _scenarioContext["EPPOCodeFirstCommodity"] = commodityDetails[2];
        }

        [Then("the user selects {string} for Are the commodity lines you selected intended for final users or commercial flower production?")]
        public void ThenTheUserSelectsForAreTheCommodityLinesYouSelectedIntendedForFinalUsersOrCommercialFlowerProduction(string option)
        {
            addIntendedUseOfBulbs?.SelectOptionForIntentedFinalUsers(option);
            _scenarioContext["IntendedForFinalUsers"] = option;
        }

        [When("the user clicks on Apply button")]
        public void WhenTheUserClicksOnApplyButton()
        {
            addIntendedUseOfBulbs?.ClickApplyButton();
        }

        [Then("the user can see the success message {string}")]
        public void ThenTheUserCanSeeTheSuccessMessage(string message)
        {
            Assert.True(addIntendedUseOfBulbs?.VerifyMessageOnThePage(message), "Add intended use of bulbs page is didn't load");
        }

        [When("the user clicks on Save and continue button")]
        public void WhenTheUserClicksOnSaveAndContinueButton()
        {
            addIntendedUseOfBulbs?.ClickSaveAndContinueButton();
        }

        [Then("the Check or update commodity details page should be displayed")]
        public void ThenTheCheckOrUpdateCommodityDetailsPageShouldBeDisplayed()
        {
            Assert.True(checkOrUpdateCommodityDetails?.IsPageLoaded(), "Check or update commodity details page is didn't load");
            _scenarioContext["ControlledAtmosphereContainer"] = checkOrUpdateCommodityDetails?.GetControlledAtmosphereContainer();
        }

        [When("the user Clicks on Save and review button from Additional details page")]
        public void WhenTheUserClicksOnSaveAndReviewButtonFromAdditionalDetailsPage()
        {
            checkOrUpdateCommodityDetails?.ClickSaveAndReviewButton();
        }

        [Then("the user verifies Total gross volume is displayed but it is marked as optional with the value of {string}")]
        public void ThenTheUserVerifiesTotalGrossVolumeIsDisplayedButItIsMarkedAsOptionalWithTheValueOf(string text)
        {
            Assert.True(checkOrUpdateCommodityDetails?.VerifyTotalGrossVolumeIsOptional(text), "Check or update commodity details page is didn't load");

        }

        [Then("the user enter Total Gross Weight as {string}")]
        public void ThenTheUserEnterTotalGrossWeightAs(string grossWeight)
        {
            checkOrUpdateCommodityDetails?.EnterGrossWeight(grossWeight);
            _scenarioContext["TotalGrossWeight"] = grossWeight;
            _scenarioContext["GrossVolume"] = checkOrUpdateCommodityDetails?.GetGrossVolume();

            var grossVolumeUnit = checkOrUpdateCommodityDetails?.GetGrossVolumeUnit().ToLower();

            _scenarioContext["GrossVolumetUnit"] = grossVolumeUnit.Equals("litres") ? "L": grossVolumeUnit;
        }
    }
}
