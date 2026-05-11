using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ConfirmationSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private IConfirmationPage? confirmationPage => _objectContainer.IsRegistered<IConfirmationPage>() ? _objectContainer.Resolve<IConfirmationPage>() : null;

        /// <summary>
        /// IPAFFS notification-specific context keys that are set during each iteration.
        /// These are archived with an iteration prefix and then removed before the next iteration.
        /// </summary>
        private static readonly string[] IpaffsIterationKeys =
        [
            "ImportType",
            "CountryOfOrigin",
            "ContryFromWhereConsigned",
            "CommodityCode",
            "CommodityDescription",
            "CommodityCodeFirstCommodity",
            "CommodityDescFirstCommodity",
            "CommodityCodeSecondCommodity",
            "CommodityDescSecondCommodity",
            "GenusFirstCommodity",
            "GenusSecondCommodity",
            "EPPOCodeFirstCommodity",
            "EPPOCodeSecondCommodity",
            "CommodityVariety",
            "CommodityClass",
            "MainReasonForImport",
            "Purpose",
            "ConsignmentReferenceNumber",
            "NumberOfAnimals",
            "NumberOfPackages",
            "PackageType",
            "Quantity",
            "QuantityType",
            "NetWeight",
            "NetWeightFirstCommodity",
            "NumberOfPackagesFirstCommodity",
            "TypeOfPackageFirstCommodity",
            "NetWeightSecondCommodity",
            "NumOfPackagesSecondCommodity",
            "TypeOfPackageSecondCommodity",
            "SubtotalNetWeight",
            "SubtotalPackages",
            "TotalNetWeight",
            "TotalPackages",
            "TotalGrossWeight",
            "IntendedForFinalUsers",
            "ControlledAtmosphereContainer",
            "GrossVolume",
            "GrossVolumetUnit",
            "Temperature",
            "ConfirmationToDeclareGMS",
            "BorderControlPost",
            "PortOfEntry",
            "InspectionPremises",
            "MeansOfTransport",
            "TransportId",
            "AreContainers",
            "ContainerNumber",
            "SealNumber",
            "OfficialSealAffixed",
            "EnterTransportDocRef",
            "EstimatedArrivalDate",
            "EstimatedArrivalTime",
            "EstimatedJourneyTime",
            "IsCTC",
            "IsGVMS",
            "MovementReferenceNumber",
            "MeansOfTransportAfterBCP",
            "TransportIdentificationAfterBCP",
            "TransportDocumentReferenceAfterBCP",
            "DepartureDateFromBCP",
            "DepartureTimeFromBCP",
            "TransporterName",
            "TransporterAddress",
            "TransporterCountry",
            "TransporterApprovalNumber",
            "TransporterType",
            "CountriesConsignmentWillTravelThrough",
            "ShouldNotifyTransportContacts",
            "ContactName",
            "ContactEmail",
            "ContactTelephone",
            "ConsignmentContactAddress",
            "DocumentType",
            "DocumentReference",
            "DocumentDateOfIssue",
            "DocumentName",
            "HealthDocumentType",
            "HealthCertificateReference",
            "HealthCertificateDateOfIssue",
            "HealthCertificateFileName",
            "CompanyName",
            "ImporterAddress",
            "ConsignorName",
            "ConsignorAddress",
            "ConsignorCountry",
            "ConsignorDetails",
            "ConsigneeName",
            "ConsigneeAddress",
            "ConsigneeDetails",
            "ImporterDetails",
            "DeliveryAddressName",
            "DeliveryAddress",
            "DeliveryCountry",
            "DeliveryAddressDetails",
            "PlaceOfDestinationName",
            "PlaceOfDestinationAddress",
            "PlaceOfDestinationAddressTextOnly",
            "PlaceOfDestinationDetails",
            "CHEDReference",
            "CustomsDeclarationReference",
            "CustomsDocumentCode",
            "BulkUploadFileName",
            "RiskDecisionRequestsJson",
            "RiskDecisionJson",
            "NewRuleId",
        ];

        public ConfirmationSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Confirmation page should be displayed with the initial risk assessment")]
        public void ThenTheConfirmationPageShouldBeDisplayedWithTheInitialRiskAssessment()
        {
            Assert.True(confirmationPage?.VerifyInitialAssessmentPage(), "Initial rist assessment page not loaded");
        }

        [When("the user records the IPAFFS User details and CHED Reference")]
        [Then("the user records the IPAFFS User details and CHED Reference")]
        public void WhenTheUserRecordsTheIPAFFSUserDetailsAndCHEDReference()
        {
            _scenarioContext["CHEDReference"] = confirmationPage.GetCHEDReference();
            _scenarioContext["CustomsDeclarationReference"] = confirmationPage.GetCustomsDeclarationReference();
            _scenarioContext["CustomsDocumentCode"] = confirmationPage.GetCustomsDocumentCode();
        }

        [When("the user records the IPAFFS User details and CHED Reference for notification {int}")]
        [Then("the user records the IPAFFS User details and CHED Reference for notification {int}")]
        public void WhenTheUserRecordsTheIPAFFSUserDetailsAndCHEDReferenceForNotification(int notificationNumber)
        {
            var prefix = $"Notification_{notificationNumber}_";
            _scenarioContext[$"{prefix}CHEDReference"] = confirmationPage.GetCHEDReference();
            _scenarioContext[$"{prefix}CustomsDeclarationReference"] = confirmationPage.GetCustomsDeclarationReference();
            _scenarioContext[$"{prefix}CustomsDocumentCode"] = confirmationPage.GetCustomsDocumentCode();
        }

        [When("the user clicks Return to your dashboard")]
        [When("the user clicks return to your dashboard link")]
        public void WhenTheUserClicksReturnToYourDashboard()
        {
            confirmationPage?.ClickReturnToDashboard();
        }

        [Then("the details should be recorded")]
        public void ThenTheDetailsShouldBeRecorded()
        {
            //No Implementation
        }

        [Then("the user verified the banner message {string}")]
        public void ThenTheUserVerifiedTheBannerMessage(string message)
        {
            Assert.True(confirmationPage?.VerifyBannerMessage(message), $"Banner doesn't contain a message '{message}'");
        }

        [When("the user clicks Return to your dashboard link")]
        public void WhenTheUserClicksReturnToYourDashboardLink()
        {
            confirmationPage?.ClickReturnToDashboardLink();
        }

        [Then("{string} is complete")]
        [When("{string} is complete")]
        public void ThenIterationIsComplete(string iterationName)
        {
            Console.WriteLine($"[ITERATION] Archiving context keys for '{iterationName}'");

            // 1. Archive each IPAFFS key with the iteration prefix
            foreach (var key in IpaffsIterationKeys)
            {
                if (_scenarioContext.ContainsKey(key))
                {
                    var archivedKey = $"{iterationName}_{key}";
                    _scenarioContext[archivedKey] = _scenarioContext[key];
                    Console.WriteLine($"[ITERATION] Archived '{key}' → '{archivedKey}'");
                }
            }

            // 2. Remove the prefix-free keys so the next iteration starts clean
            _scenarioContext.RemoveContextKeys(IpaffsIterationKeys);
            Console.WriteLine($"[ITERATION] Cleared {IpaffsIterationKeys.Length} IPAFFS keys. '{iterationName}' complete.");
        }
    }
}