using Defra.UI.Tests.Contracts;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using Defra.UI.Tests.Tools.PDFProcessor;
using Defra.UI.Tests.Tools.PDFProcessor.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class YourImportNotificationsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IYourImportNotificationsPage? importNotificationsPage => _objectContainer.IsRegistered<IYourImportNotificationsPage>() ? _objectContainer.Resolve<IYourImportNotificationsPage>() : null;
        private IUserObject? UserObject => _objectContainer.IsRegistered<IUserObject>() ? _objectContainer.Resolve<IUserObject>() : null;

        public YourImportNotificationsSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("the user should be logged into Notification page")]
        [Then("the dashboard page should be displayed")]
        [Then("the user is taken to the Your import notifications page")]
        [Then("the Your notifications page is displayed")]
        [Then("the Your import notifications page is displayed")]
        [Then("the user is taken back to the dashboard page")]
        public void ThenTheDashboardShouldBeDisplayed()
        {
            Assert.True(importNotificationsPage?.IsPageLoaded(), "Dashboard not displayed");
        }

        [When("the user clicks Create a new notification")]
        public void WhenTheUserClicksCreateANewNotification()
        {
            importNotificationsPage?.ClickCreateNotification();
        }

        [When("the user searches for the import notification")]
        [When("user searches for the import notification")]
        public void WhenUserSearchesForTheImportNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            importNotificationsPage?.SearchForNotification(chedReference);
        }

        [Then("the notification should be present in the list")]
        public void ThenTheNotificationShouldBePresentInTheList()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(importNotificationsPage?.VerifyNotificationInList(chedReference), "Notification not found in list");
        }

        [When("the user clicks Show notification")]
        [When("the user clicks the Show notification link")]
        public void WhenTheUserClicksShowNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            importNotificationsPage?.RecordHandlesBeforePdfOpen();
            importNotificationsPage?.ClickShowNotification(chedReference);
        }

        [Then("the certificate should be displayed in a new browser tab")]
        public void ThenTheCertificateShouldBeDisplayedInANewBrowserTab()
        {
            Assert.True(importNotificationsPage?.VerifyCertificateInNewTab(), "Certificate not displayed in new browser tab");
        }

        [When("the user downloads the PDF for validation")]
        public void WhenTheUserDownloadsThePDFForValidation()
        {
            string pdfUrl = importNotificationsPage?.getPDFUrl();
            var chedReferenceFileName = _scenarioContext.Get<string>("CHEDReference") + "-certificate";
            _scenarioContext["PDFDownloadedDirectory"] = Utils.DownloadPDF(chedReferenceFileName, pdfUrl, UserObject, _scenarioContext.Get<string>("UserRole"));
        }

        [When("verifies laboratory tests should be displayed as No and Reasons for testing with no boxes selected")]
        [Then("the new Consignor, Consignee, Importer, Place of destination and Transporter should be displayed in the certificate")]
        [When("the user checks that the data in the certificate matches the data entered into the notification")]
        public void WhenTheUserChecksThatTheDataInTheCertificateMatchesTheDataEnteredIntoTheNotification()
        {

            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(importNotificationsPage?.VerifyDataInCertificate(chedReference), "Certificate data verification failed");

            var json = JsonConvert.SerializeObject(_scenarioContext.ToDictionary(kvp => kvp.Key, kvp => kvp.Value), Formatting.Indented);
            var chedReferenceFileName =  chedReference + "-certificate";

            string downloadDirectory = _scenarioContext.Get<string>("PDFDownloadedDirectory");
            string pdfPath = Path.Combine(downloadDirectory, chedReferenceFileName);
            //string pdfPath = downloadDirectory + chedReferenceFileName + ".pdf";
            var converter = new PdfToJsonConverter();
            var jsonOutput = converter.ConvertToJson(pdfPath);

            var chedDocumentPages = JsonConvert.DeserializeObject<ChedRootObject>(jsonOutput);

            var allDataMatches = true;
            var mismatches = new List<string>();

            if (chedDocumentPages != null)
            {
                for (int pageNumber = 1; pageNumber <= chedDocumentPages.Count; pageNumber++)
                {
                    var page = chedDocumentPages[pageNumber - 1];

                    if (pageNumber == 1)
                    {
                        ValidateIfExists("CHEDReference", page.Sections.ChedReference.Id, ref allDataMatches, mismatches);

                        if (!_scenarioContext["CHEDReference"].ToString().Contains("CHEDA"))
                        {
                            ValidateIfExists("ConsignmentReferenceNumber", page.Sections.LocalReference.Id, ref allDataMatches, mismatches);
                            ValidateIfExists("ContryFromWhereConsigned", page.Sections.CountryOfDispatch?.Value, ref allDataMatches, mismatches);
                        }
                        ValidateContains("PortOfEntry", page.Sections.BorderControlPost.Value, ref allDataMatches, mismatches, true);

                        if (!_scenarioContext.ContainsKey("ExporterName") || string.IsNullOrEmpty(_scenarioContext["ExporterName"]?.ToString()))
                        {
                            ValidateIfExists("ConsignorName", page.Sections.ConsignorExporter.Name, ref allDataMatches, mismatches);
                            ValidateIfExists("ConsignorAddress", page.Sections.ConsignorExporter.Address, ref allDataMatches, mismatches);
                            ValidateIfExists("ConsignorCountry", page.Sections.ConsignorExporter.Country, ref allDataMatches, mismatches);
                            ValidateIfExists("ConsigneeName", page.Sections.ConsigneeImporter.Name, ref allDataMatches, mismatches);
                            ValidateIfExists("ConsigneeAddress", page.Sections.ConsigneeImporter.Address, ref allDataMatches, mismatches);
                            ValidateContains("ConsigneeCountry", page.Sections.ConsigneeImporter.Country, ref allDataMatches, mismatches, true);
                            ValidateContains("PlaceOfDestinationDetails", page.Sections.PlaceOfDestination.Name, ref allDataMatches, mismatches, true);
                            ValidateContains("PlaceOfDestinationDetails", page.Sections.PlaceOfDestination.Address, ref allDataMatches, mismatches, true);
                            ValidateContains("PlaceOfDestinationDetails", page.Sections.PlaceOfDestination.Country, ref allDataMatches, mismatches, true);
                        }
                        var docRefEntry = page.Sections.AccompanyingDocuments.AdditionalData.FirstOrDefault(x => x.Key == "DocumentReference");
                        var documentReference = docRefEntry.Key == null ? null : docRefEntry.Value?.ToString();
                        var dateEntry = page.Sections.AccompanyingDocuments.AdditionalData.FirstOrDefault(x => x.Key == "DateOfIssue");
                        var reviewDate = dateEntry.Key == null ? null : dateEntry.Value?.ToString();

                        if (_scenarioContext["CHEDReference"].ToString().Contains("CHEDP"))
                        {
                            ValidateContains("HealthDocumentType", (string?)page.Sections.AccompanyingDocuments.AdditionalData.ElementAt(0).Value, ref allDataMatches, mismatches, true);
                            ValidateContains("HealthCertificateReference", documentReference, ref allDataMatches, mismatches, true);
                            ValidateIfExists("HealthCertificateDateOfIssue", reviewDate, ref allDataMatches, mismatches);
                        }
                        else if (_scenarioContext["CHEDReference"].ToString().Contains("CHEDD"))
                        {
                            ValidateContains("DocumentReference", documentReference, ref allDataMatches, mismatches, true);
                            ValidateIfExists("DocumentDateOfIssue", reviewDate, ref allDataMatches, mismatches);
                        }
                        else if (_scenarioContext.ContainsKey("ExporterName") && _scenarioContext["CHEDReference"].ToString().Contains("CHEDA"))
                        {
                            ValidateIfExists("ExporterName", page.Sections.ConsignorExporter.Name, ref allDataMatches, mismatches);
                            ValidateIfExists("ExporterAddress", page.Sections.ConsignorExporter.Address, ref allDataMatches, mismatches);
                            ValidateIfExists("ExporterCountry", page.Sections.ConsignorExporter.Country, ref allDataMatches, mismatches);
                            ValidateIfExists("ImporterName", page.Sections.ConsigneeImporter.Name, ref allDataMatches, mismatches);
                            ValidateIfExists("ImporterAddress", page.Sections.ConsigneeImporter.Address, ref allDataMatches, mismatches);
                            ValidateContains("ImporterCountry", page.Sections.ConsigneeImporter.Country, ref allDataMatches, mismatches, true);
                            ValidateContains("ImporterName", page.Sections.PlaceOfDestination.Name, ref allDataMatches, mismatches, true);
                            ValidateContains("ImporterAddress", page.Sections.PlaceOfDestination.Address, ref allDataMatches, mismatches, true);
                            ValidateContains("ImporterCountry", page.Sections.PlaceOfDestination.Country, ref allDataMatches, mismatches, true);
                        }
                        else
                        {
                            ValidateContains("HealthDocumentType", (string?)page.Sections.AccompanyingDocuments.AdditionalData.ElementAt(1).Value, ref allDataMatches, mismatches);
                            ValidateContains("HealthCertificateReference", (string?)page.Sections.AccompanyingDocuments.AdditionalData.ElementAt(1).Value, ref allDataMatches, mismatches);
                            ValidateContains("DocumentType", (string?)page.Sections.AccompanyingDocuments.AdditionalData?.ElementAt(1).Value, ref allDataMatches, mismatches);
                            ValidateContains("DocumentReference", (string?)page.Sections.AccompanyingDocuments.AdditionalData?.ElementAt(1).Value, ref allDataMatches, mismatches);
                        }

                        ValidateIfExists("CommodityIntendedFor", page.Sections.GoodsCertifiedAs?.Value, ref allDataMatches, mismatches);

                        if (_scenarioContext["CHEDReference"].ToString().Contains("CHEDA"))
                        {
                            ValidateContains("CertificationOption", page.Sections.GoodsCertifiedAs?.Value, ref allDataMatches, mismatches, true);
                        }


                        ValidateIfExists("CommodityIntendedFor", page.Sections.GoodsCertifiedAs?.Value, ref allDataMatches, mismatches);
                        if (_scenarioContext["CHEDReference"].ToString().Contains("CHEDA"))
                        {
                            ValidateContains("CertificationOption", page.Sections.GoodsCertifiedAs?.Value, ref allDataMatches, mismatches, true);
                        }
                        else
                        {
                            ValidateContains("Purpose", page.Sections.GoodsCertifiedAs?.Value, ref allDataMatches, mismatches);
                        }

                        ValidateContains("PlaceOfExit", page.Sections.NonInternalMarket?.Value, ref allDataMatches, mismatches);

                        ValidateContains("MeansOfTransport", page.Sections.MeansOfTransport.Mode, ref allDataMatches, mismatches);

                        var meansOfTransport = _scenarioContext.Get<string>("MeansOfTransport");
                        if (meansOfTransport.Equals(page.Sections.MeansOfTransport.Mode, StringComparison.OrdinalIgnoreCase))
                            ValidateIfExists("EnterTransportDocRef", page.Sections.MeansOfTransport.InternationalTransportDocument, ref allDataMatches, mismatches);
                        else
                            ValidateContains("EnterTransportDocRef", page.Sections.MeansOfTransport.Mode, ref allDataMatches, mismatches);

                        ValidateContains("TransportId", page.Sections.MeansOfTransport.Identification, ref allDataMatches, mismatches, true);
                        ValidateIfExists("CountryOfOrigin", page.Sections.CountryOfOrigin.Value, ref allDataMatches, mismatches);
                        ValidateContains("ApprovedEstablishmentName", page.Sections.EstablishmentsOfOrigin?.Value, ref allDataMatches, mismatches);
                        ValidateContains("ApprovedEstablishmentCountry", page.Sections.EstablishmentsOfOrigin?.Value, ref allDataMatches, mismatches);
                        ValidateContains("ApprovedEstablishmentType", page.Sections.EstablishmentsOfOrigin?.Value, ref allDataMatches, mismatches);
                        ValidateContains("ApprovedEstablishmentApprovalNum", page.Sections.EstablishmentsOfOrigin?.Value, ref allDataMatches, mismatches);
                        ValidateContains("TranshipmentDestinationCountry", page.Sections.TranshipmentOnwardTravel?.Country, ref allDataMatches, mismatches);


                        ValidateContains("SealNumber", page.Sections.ContainerNoSealNo?.Value, ref allDataMatches, mismatches);
                        ValidateContains("ContainerNumber", page.Sections.ContainerNoSealNo?.Value, ref allDataMatches, mismatches);

                        ValidateContains("DestinationCountry", page.Sections.DirectTransit?.Country, ref allDataMatches, mismatches);

                        if (page.Sections?.DirectTransit != null)
                        {
                            ValidateContains("ExitBCP", (string?)page.Sections?.DirectTransit?.TracesUnitNo, ref allDataMatches, mismatches, true);
                        }

                        ValidateContains("EstimatedArrivalDate", page.Sections.PriorNotification?.Date, ref allDataMatches, mismatches);
                        ValidateContains("EstimatedArrivalTime", page.Sections.PriorNotification?.Time, ref allDataMatches, mismatches);

                        string? pdfTemperature = page.Sections.TransportConditions
                        switch
                        {
                            { Ambient: "true" } => "Ambient",
                            { Frozen: "true" } => "Frozen",
                            { Chilled: "true" } => "Chilled",
                            _ => null
                        };
                        ValidateIfExists("Temperature", pdfTemperature, ref allDataMatches, mismatches);

                        ValidateContains("ContactName", page.Sections.OperatorResponsible?.Name, ref allDataMatches, mismatches);
                        ValidateIfExists("ConsignmentContactAddress", page.Sections.OperatorResponsible?.Address, ref allDataMatches, mismatches);
                        ValidateIfExists("TransporterName", page.Sections.Transporter?.Name, ref allDataMatches, mismatches);
                        ValidateContains("TransporterAddress", page.Sections.Transporter?.Address, ref allDataMatches, mismatches, true);
                        ValidateContains("TransporterCountry", page.Sections.Transporter?.Country, ref allDataMatches, mismatches, true);
                        ValidateIfExists("TransporterApprovalNumber", page.Sections.Transporter?.ApprovalNumber, ref allDataMatches, mismatches);

                        if (_scenarioContext.ContainsKey("CloningHealthCertificateDetails"))
                        {
                            ValidateIfExists("ConsignorConsigneeOrImporterName", page.Sections.ConsigneeImporter.Name, ref allDataMatches, mismatches);
                            ValidateIfExists("PurposeOfTheConsignment", page.Sections.GoodsCertifiedAs.Value, ref allDataMatches, mismatches);
                        }

                        if (page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value != null)
                        {
                            ValidateContains("CommodityCode", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("CommodityDescription", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("CommodityCodeFirstCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("CommodityDescFirstCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("Species", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("NumberOfAnimals", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("NetWeight", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("NumberOfPackages", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("PackageType", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("TypeOfCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("EstablishmentListFirstName", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("CountryOfOrigin", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                        }

                    }

                    else if (pageNumber == 2)
                    {
                        if (_scenarioContext["CHEDReference"].ToString().Contains("CHEDA"))
                        {
                            ValidateContains("Species", page.Sections.IdentificationDetails.Value, ref allDataMatches, mismatches);
                            ValidateContains("EarTag", page.Sections.IdentificationDetails.Value, ref allDataMatches, mismatches);
                            ValidateContains("HorseName", page.Sections.IdentificationDetails.Value, ref allDataMatches, mismatches);
                        }

                        if (_scenarioContext.ContainsKey("CloningHealthCertificateDetails"))
                        {
                            //Clone scenario
                            ValidateContains("CommodityCode", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("Description", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("GenusAndSpecies", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("NetWeight", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("Packages", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("TypeOfPackage", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("EstablishmentListFirstName", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("CountryOfOriginOfCertificate", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                        }
                        else if (page.Sections.DescriptionOfTheGoods?.Count > 1)
                        {
                            ValidateContains("CommodityCodeFirstCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("CommodityDescFirstCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("Species", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("NetWeightFirstCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("NumberOfPackagesFirstCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("TypeOfCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("TypeOfPackageFirstCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("EstablishmentListFirstName", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("CountryOfOrigin", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);

                            ValidateContains("CommodityCodeSecondCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(1).Value, ref allDataMatches, mismatches);
                            ValidateContains("CommodityDescSecondCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(1).Value, ref allDataMatches, mismatches);
                            ValidateContains("Species", page.Sections.DescriptionOfTheGoods?.ElementAt(1).Value, ref allDataMatches, mismatches);
                            ValidateContains("TypeOfCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(1).Value, ref allDataMatches, mismatches);
                            ValidateContains("NetWeightSecondCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(1).Value, ref allDataMatches, mismatches);
                            ValidateContains("NumOfPackagesSecondCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(1).Value, ref allDataMatches, mismatches);
                            ValidateContains("TypeOfPackageSecondCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(1).Value, ref allDataMatches, mismatches);

                            ValidateContains("TotalNetWeight", page.Sections.TotalNetWeight?.Value, ref allDataMatches, mismatches);
                            ValidateContains("TotalPackages", page.Sections.TotalNumberOfPackages?.Value, ref allDataMatches, mismatches);
                            ValidateContains("TotalGrossWeight", page.Sections.TotalGrossWeight?.Value, ref allDataMatches, mismatches);
                        }
                        else if (page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value == null)
                        {
                            ValidateContains("NumberOfPackages", page.Sections.TotalNumberOfPackages?.Value, ref allDataMatches, mismatches);
                        }
                        else
                        {
                            ValidateContains("CommodityCode", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("CommodityDescription", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("CommodityCodeFirstCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("CommodityDescFirstCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateAllMatchesInList("Species", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("NumberOfAnimals", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateAllMatchesInList("NetWeight", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateAllMatchesInList("NumberOfPackages", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateAllMatchesInList("PackageType", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("TypeOfCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("EstablishmentListFirstName", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("CountryOfOrigin", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("TotalNetWeight", page.Sections.TotalNetWeight?.Value, ref allDataMatches, mismatches);
                            ValidateContains("TotalPackages", page.Sections.TotalNumberOfPackages?.Value, ref allDataMatches, mismatches);
                            ValidateContains("TotalGrossWeight", page.Sections.TotalGrossWeight?.Value, ref allDataMatches, mismatches);
                        }
                    }

                    else if (pageNumber == 3)
                    {
                        ValidateIfExists("CHEDReference", page.Sections.II2ChedReference.Id, ref allDataMatches, mismatches);

                        string? pdfDocCheckDecision = page.Sections.DocumentaryCheck
                        switch
                        {
                            { Satisfactory: "true" } => "Satisfactory",
                            { SatisfactoryFollowingOfficialIntervention: "true" } => "Satisfactory Following Official Intervention",
                            { NotSatisfactory: "true" } => "Not Satisfactory",
                            { NotDone: "true" } => "Not Done",
                            _ => null
                        };
                        ValidateIfExists("DocumentaryCheckDecision", pdfDocCheckDecision, ref allDataMatches, mismatches);

                        string? pdfIdentityCheck = page.Sections.IdentityCheck
                        switch
                        {
                            { SealCheckOnly: "true" } => "SealCheckOnly",
                            { FullIdentityCheck: "true" } => "FullIdentityCheck",
                            _ => null
                        };
                        ValidateIfExists("IdentityCheck", pdfIdentityCheck, ref allDataMatches, mismatches);
                        ValidateIfExists("IdentityCheckType", pdfIdentityCheck, ref allDataMatches, mismatches);

                        string? pdfIdentityCheckDecision = page.Sections.IdentityCheck
                        switch
                        {
                            { Satisfactory: "true" } => "Satisfactory",
                            { NotSatisfactory: "true" } => "Not Satisfactory",
                            _ => null
                        };
                        ValidateIfExists("IdentityCheckDecision", pdfIdentityCheckDecision, ref allDataMatches, mismatches);

                        string? pdfPhysicalCheck = page.Sections.PhysicalCheck
                        switch
                        {
                            { Satisfactory: "true" } => "Satisfactory",
                            { NotSatisfactory: "true" } => "Not Satisfactory",
                            _ => null
                        };
                        ValidateIfExists("PhysicalCheck", pdfPhysicalCheck, ref allDataMatches, mismatches);
                        ValidateIfExists("PhysicalCheckDecision", pdfPhysicalCheck, ref allDataMatches, mismatches);

                        string? pdfLaboratoryTestRequired = page.Sections.LaboratoryTests.No;
                        if (pdfLaboratoryTestRequired.Equals("true") && page.Sections.LaboratoryTests.Random.Equals("false")
                            && page.Sections.LaboratoryTests.Suspicion.Equals("false"))
                        //&& (page.Sections.LaboratoryTests.EmergencyMeasures.Equals("false")
                        //|| page.Sections.LaboratoryTests.IntensifiedControls.Equals("false"))   ---- Need to verify once value is retrieved from pdf
                        {
                            pdfLaboratoryTestRequired = "No";
                        }
                        else
                        {
                            pdfLaboratoryTestRequired = "Yes";
                        }

                        ValidateIfExists("AreLaboratoryTestsRequired", pdfLaboratoryTestRequired, ref allDataMatches, mismatches);

                        string? pdfLaboratoryTestNames = page.Sections.LaboratoryTests
                        switch
                        {
                            { Random: "true" } => "Random",
                            { Suspicion: "true" } => "Suspicion",
                            { IntensifiedControls: "true" } => "IntensifiedControls",
                            { EmergencyMeasures: "true" } => "EmergencyMeasures",
                            _ => null
                        };
                        ValidateIfExists("LaboratoryTestsReason", pdfLaboratoryTestNames, ref allDataMatches, mismatches);

                        string? welfareCheckDecision = page.Sections.WelfareCheck
                        switch
                        {
                            { Satisfactory: "true" } => "Satisfactory",
                            { NotSatisfactory: "true" } => "Not Satisfactory",
                            _ => null
                        };
                        ValidateIfExists("WelfareCheckDecision", welfareCheckDecision, ref allDataMatches, mismatches);
                        ValidateContains("NumberOfDeadAnimals", page.Sections.ImpactOnTransportAnimals?.Value, ref allDataMatches, mismatches);
                        ValidateContains("NumberOfUnfitAnimals", page.Sections.ImpactOnTransportAnimals?.Value, ref allDataMatches, mismatches);
                        ValidateContains("NumberOfBirthsOrAbortions", page.Sections.ImpactOnTransportAnimals?.Value, ref allDataMatches, mismatches);

                        ValidateContains("LaboratoryTestName", (string?)page.Sections.LaboratoryTests?.AdditionalData?.ElementAt(0).Value, ref allDataMatches, mismatches, true);

                        ValidateContains("IUUSubOption", page.Sections.CustomsDocumentReference?.Value, ref allDataMatches, mismatches);


                        if (_scenarioContext["PortOfEntry"].ToString().Contains("London Borough of Hillingdon Heathrow Airport Imported Food Office"))
                        {
                            var combined = $"{page.Sections.IdentificationOfBcp?.AdditionalData.ElementAt(2).Value} {page.Sections.IdentificationOfBcp?.AdditionalData.ElementAt(1).Value}".Trim();
                            ValidateContains("PortOfEntry", combined, ref allDataMatches, mismatches, true);
                        }
                        else
                            ValidateContains("PortOfEntry", (string?)page.Sections.IdentificationOfBcp?.AdditionalData.ElementAt(2).Value, ref allDataMatches, mismatches, true);

                        if (page.Sections.AcceptableForTransit != null && _scenarioContext["CHEDReference"].ToString().Contains("CHEDA"))
                        {
                            ValidateContains("DestinationCountry", page.Sections.AcceptableForTransit?.Value, ref allDataMatches, mismatches);
                            ValidateContains("ExitBCP", page.Sections.AcceptableForTransit?.ExitBCP, ref allDataMatches, mismatches, true);
                            ValidateContains("ExitBCP", page.Sections.AcceptableForTransit?.TracesUnitNo, ref allDataMatches, mismatches, true);
                        }
                        else if (page.Sections.AcceptableForTransit != null && !_scenarioContext["CHEDReference"].ToString().Contains("CHEDA"))
                        {
                            ValidateContains("DestinationCountry", page.Sections.AcceptableForTransit?.Value, ref allDataMatches, mismatches);
                            ValidateContains("ExitBorderControlPost", (string?)page.Sections.AcceptableForTransit?.Value, ref allDataMatches, mismatches);
                        }
                        if (page.Sections.AcceptableForTranshipment != null)
                        {
                            ValidateContains("TranshipmentDestinationCountry", page.Sections.AcceptableForTranshipment?.Value, ref allDataMatches, mismatches);
                        }
                        if (page.Sections.NotAcceptable != null)
                        {
                            ValidateContains("AcceptableForSubOption", page.Sections.NotAcceptable?.Value, ref allDataMatches, mismatches);
                        }
                        if (page.Sections.ReasonForRefusal != null)
                        {
                            ValidateContains("ReasonForRefusal", page.Sections.ReasonForRefusal?.Value, ref allDataMatches, mismatches);
                            ValidateContains("AdditionalReasonForRefusal", page.Sections.ReasonForRefusal?.Value, ref allDataMatches, mismatches);
                        }
                        if (page.Sections.AcceptableForInternalMarket != null)
                        {
                            ValidateContains("AcceptableForSubOption", page.Sections.AcceptableForInternalMarket?.Value, ref allDataMatches, mismatches);
                        }

                        if (_scenarioContext.ContainsKey("CloningHealthCertificateDetails"))
                        {
                            ValidateContains("PurposeOfTheConsignment", page.Sections.AcceptableForInternalMarket.Value, ref allDataMatches, mismatches);
                        }
                    }
                    else if (pageNumber == 4)
                    {
                        ValidateIfExists("CHEDReference", page.Sections.III2ChedReference.Id, ref allDataMatches, mismatches);
                    }
                    else if (pageNumber == 5)
                    {
                        ValidateIfExists("CHEDReference", (string?)page.Sections.References.AdditionalData.ElementAt(2).Value, ref allDataMatches, mismatches);

                        ValidateContains("PortOfEntry", page.Sections.References.Name, ref allDataMatches, mismatches);

                        ValidateContains("CountryOfOrigin", (string?)page.Sections.IdentificationOfTheSample.AdditionalData.ElementAt(0).Value, ref allDataMatches, mismatches);
                        if (!_scenarioContext["CHEDReference"].ToString().Contains("CHEDA"))
                        {
                            ValidateIfExists("ContryFromWhereConsigned", (string?)page.Sections.IdentificationOfTheSample.AdditionalData.FirstOrDefault(x => x.Key == "CountryOfDispatch").Value, ref allDataMatches, mismatches);
                        }
                        var destination = page.Sections.IdentificationOfTheSample.AdditionalData.FirstOrDefault(x => x.Key == "PlaceOfDestination").Value.ToString();

                        _scenarioContext["LabTestNameCopy"] = _scenarioContext["LabTestName"];
                        _scenarioContext["LabSampleReferenceCopy"] = _scenarioContext["LabSampleReference"];
                        _scenarioContext["NumberOfLabSamplesCopy"] = _scenarioContext["NumberOfLabSamples"];
                        _scenarioContext["LabSampleStorageTemperatureCopy"] = _scenarioContext["LabSampleStorageTemperature"];

                        ValidateContains("PlaceOfDestinationDetails", destination.Replace("Commodity", ""), ref allDataMatches, mismatches, true);
                        ValidateContains("CommodityCode", page.Sections.IdentificationOfTheSample.Commodity, ref allDataMatches, mismatches);
                        ValidateContains("Species", (string?)page.Sections.IdentificationOfTheSample.AdditionalData.ElementAt(1).Value, ref allDataMatches, mismatches);
                        ValidateIfExists("LaboratoryTestsReason", (string?)page.Sections.RequestedAnalysis.AdditionalData.ElementAt(0).Value, ref allDataMatches, mismatches);
                        ValidateContains("LabTestNameCopy", (string?)page.Sections.LabResults.Name, ref allDataMatches, mismatches, true);
                        ValidateContains("LabSampleReferenceCopy", (string?)page.Sections.LabResults.AdditionalData.ElementAt(2).Value, ref allDataMatches, mismatches, true);
                        ValidateContains("NumberOfLabSamplesCopy", (string?)page.Sections.LabResults.AdditionalData.ElementAt(4).Value, ref allDataMatches, mismatches, true);
                        ValidateIfExists("LabSampleType", (string?)page.Sections.LabResults.AdditionalData.ElementAt(3).Value, ref allDataMatches, mismatches);
                        ValidateContains("LabSampleStorageTemperatureCopy", (string?)page.Sections.LabResults.AdditionalData.ElementAt(5).Value, ref allDataMatches, mismatches, true);
                        ValidateIfExists("SampleDate", (string?)page.Sections.LabResults.AdditionalData.ElementAt(1).Value, ref allDataMatches, mismatches);
                        ValidateIfExists("SampleTime", (string?)page.Sections.LabResults.AdditionalData.ElementAt(11).Value, ref allDataMatches, mismatches);
                        ValidateContains("LaboratoryTestName", (string?)page.Sections.LabResults.AdditionalData.ElementAt(6).Value, ref allDataMatches, mismatches, true);

                    }
                    else if (pageNumber == 6)
                    {
                        ValidateIfExists("CHEDReference", (string?)page.Sections.References.AdditionalData.ElementAt(2).Value, ref allDataMatches, mismatches);

                        ValidateContains("PortOfEntry", page.Sections.References.Name, ref allDataMatches, mismatches);

                        ValidateContains("CountryOfOrigin", (string?)page.Sections.IdentificationOfTheSample.AdditionalData.ElementAt(0).Value, ref allDataMatches, mismatches);
                        if (!_scenarioContext["CHEDReference"].ToString().Contains("CHEDA"))
                        {
                            ValidateIfExists("ContryFromWhereConsigned", (string?)page.Sections.IdentificationOfTheSample.AdditionalData.FirstOrDefault(x => x.Key == "CountryOfDispatch").Value, ref allDataMatches, mismatches);
                        }
                        var destination = page.Sections.IdentificationOfTheSample.AdditionalData.FirstOrDefault(x => x.Key == "PlaceOfDestination").Value.ToString();

                        ValidateContains("PlaceOfDestinationDetails", destination.Replace("Commodity", ""), ref allDataMatches, mismatches, true);
                        ValidateContains("CommodityCode", page.Sections.IdentificationOfTheSample.Commodity, ref allDataMatches, mismatches);
                        ValidateContains("Species", (string?)page.Sections.IdentificationOfTheSample.AdditionalData.ElementAt(1).Value, ref allDataMatches, mismatches);
                        ValidateIfExists("LaboratoryTestsReason", (string?)page.Sections.RequestedAnalysis.AdditionalData.ElementAt(0).Value, ref allDataMatches, mismatches);
                        ValidateContains("LabTestNameCopy", (string?)page.Sections.LabResults.Name, ref allDataMatches, mismatches, true);
                        ValidateContains("LabSampleReferenceCopy", (string?)page.Sections.LabResults.AdditionalData.ElementAt(2).Value, ref allDataMatches, mismatches, true);
                        ValidateContains("NumberOfLabSamplesCopy", (string?)page.Sections.LabResults.AdditionalData.ElementAt(4).Value, ref allDataMatches, mismatches, true);
                        ValidateIfExists("LabSampleType", (string?)page.Sections.LabResults.AdditionalData.ElementAt(3).Value, ref allDataMatches, mismatches);
                        ValidateContains("LabSampleStorageTemperatureCopy", (string?)page.Sections.LabResults.AdditionalData.ElementAt(5).Value, ref allDataMatches, mismatches, true);
                        ValidateIfExists("SampleDate", (string?)page.Sections.LabResults.AdditionalData.ElementAt(1).Value, ref allDataMatches, mismatches);
                        ValidateIfExists("SampleTime", (string?)page.Sections.LabResults.AdditionalData.ElementAt(11).Value, ref allDataMatches, mismatches);
                        ValidateContains("LaboratoryTestName", (string?)page.Sections.LabResults.AdditionalData.ElementAt(6).Value, ref allDataMatches, mismatches, true);

                    }
                    else if (pageNumber == 7)
                    {
                        ValidateIfExists("CHEDReference", (string?)page.Sections.References.AdditionalData.ElementAt(2).Value, ref allDataMatches, mismatches);

                        ValidateContains("PortOfEntry", page.Sections.References.Name, ref allDataMatches, mismatches);

                        ValidateContains("CountryOfOrigin", (string?)page.Sections.IdentificationOfTheSample.AdditionalData.ElementAt(0).Value, ref allDataMatches, mismatches);
                        if (!_scenarioContext["CHEDReference"].ToString().Contains("CHEDA"))
                        {
                            ValidateIfExists("ContryFromWhereConsigned", (string?)page.Sections.IdentificationOfTheSample.AdditionalData.FirstOrDefault(x => x.Key == "CountryOfDispatch").Value, ref allDataMatches, mismatches);
                        }
                        var destination = page.Sections.IdentificationOfTheSample.AdditionalData.FirstOrDefault(x => x.Key == "PlaceOfDestination").Value.ToString();

                        ValidateContains("PlaceOfDestinationDetails", destination.Replace("Commodity", ""), ref allDataMatches, mismatches, true);
                        ValidateContains("CommodityCode", page.Sections.IdentificationOfTheSample.Commodity, ref allDataMatches, mismatches);
                        ValidateContains("Species", (string?)page.Sections.IdentificationOfTheSample.AdditionalData.ElementAt(1).Value, ref allDataMatches, mismatches);
                        ValidateIfExists("LaboratoryTestsReason", (string?)page.Sections.RequestedAnalysis.AdditionalData.ElementAt(0).Value, ref allDataMatches, mismatches);
                        ValidateContains("LabTestNameCopy", (string?)page.Sections.LabResults.Name, ref allDataMatches, mismatches, true);
                        ValidateContains("LabSampleReferenceCopy", (string?)page.Sections.LabResults.AdditionalData.ElementAt(2).Value, ref allDataMatches, mismatches, true);
                        ValidateContains("NumberOfLabSamplesCopy", (string?)page.Sections.LabResults.AdditionalData.ElementAt(4).Value, ref allDataMatches, mismatches, true);
                        ValidateIfExists("LabSampleType", (string?)page.Sections.LabResults.AdditionalData.ElementAt(3).Value, ref allDataMatches, mismatches);
                        ValidateContains("LabSampleStorageTemperatureCopy", (string?)page.Sections.LabResults.AdditionalData.ElementAt(5).Value, ref allDataMatches, mismatches, true);
                        ValidateIfExists("SampleDate", (string?)page.Sections.LabResults.AdditionalData.ElementAt(1).Value, ref allDataMatches, mismatches);
                        ValidateIfExists("SampleTime", (string?)page.Sections.LabResults.AdditionalData.ElementAt(11).Value, ref allDataMatches, mismatches);
                        ValidateContains("LaboratoryTestName", (string?)page.Sections.LabResults.AdditionalData.ElementAt(6).Value, ref allDataMatches, mismatches, true);

                    }
                }
            }
            if (!allDataMatches)
            {
                Console.WriteLine("[PDF VALIDATION] Data mismatches found:");
                foreach (var mismatch in mismatches)
                {
                    Console.WriteLine($"[PDF VALIDATION] {mismatch}");
                }
            }

            Assert.True(allDataMatches, $"PDF data validation failed. Mismatches: {string.Join(", ", mismatches)}");


            if (Directory.Exists(downloadDirectory))
            {
                if (File.Exists(pdfPath))
                {
                    File.Delete(pdfPath);
                    Console.WriteLine("File deleted successfully.");
                }
                else
                {
                    Console.WriteLine("File not found to delete.");
                }

                Directory.Delete(downloadDirectory, true);
                Console.WriteLine("Deleted directory: " + downloadDirectory);
            }

        }


        [When("the user closes the newly opened tab")]
        [When("the user closes the PDF browser tab")]
        public void WhenTheUserClosesThePDFBrowserTab()
        {
            importNotificationsPage?.ClosePDFBrowserTab();
        }

        [Then("the browser tab is closed")]
        public void ThenTheBrowserTabIsClosed()
        {
            Assert.True(importNotificationsPage?.VerifyBrowserTabClosed(), "PDF browser tab not closed properly");
        }

        [When("the user clicks Amend")]
        public void WhenTheUserClicksAmend()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            importNotificationsPage?.ClickAmend(chedReference);
        }

        [When("user searches for the '(.*)' import notification")]
        public void WhenUserSearchesForTheImportNotification(string reference)
        {
            _scenarioContext["CHEDReference"] = reference;
            importNotificationsPage?.SearchForNotification(reference);
        }

        [When("the user clicks Cookies link from the footer of the page")]
        public void WhenTheUserClicksCookiesLinkFromTheFooterOfThePage()
        {
            importNotificationsPage?.ClickCookiesLink();
        }

        [Then("the notification returned in the search has the status {string}")]
        public void ThenTheNotificationReturnedInTheSearchHasTheStatus(string expectedStatus)
        {
            var actualStatus = importNotificationsPage?.GetNotificationStatus();
            Assert.AreEqual(expectedStatus.ToLower(), actualStatus.ToLower(), $"Expected status '{expectedStatus}' but found '{actualStatus}'");
        }

        [Then("the Amend link should be available for the notification")]
        public void ThenTheAmendLinkShouldBeAvailableForTheNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(importNotificationsPage?.IsAmendLinkPresent(chedReference), "Amend link is not present");
        }

        [Then("the Amend link should not be available for the notification")]
        public void ThenTheAmendLinkShouldNotBeAvailableForTheNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(importNotificationsPage?.IsAmendLinkNotPresent(chedReference), "Amend link should not be present but was found");
        }

        [Then("the Copy as new link should be available for the notification")]
        public void ThenTheCopyAsNewLinkShouldBeAvailableForTheNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(importNotificationsPage?.IsCopyAsNewLinkPresent(chedReference), "Copy as new link is not present");
        }

        [Then("the View details link should be available for the notification")]
        public void ThenTheViewDetailsLinkShouldBeAvailableForTheNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(importNotificationsPage?.IsViewDetailsLinkPresent(chedReference), "View details link is not present");
        }

        [Then("the Show notification link should be available for the notification")]
        public void ThenTheShowNotificationLinkShouldBeAvailableForTheNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(importNotificationsPage?.IsShowNotificationLinkPresent(chedReference), "Show notification link is not present");
        }

        [When("the user clicks View details for the notification")]
        public void WhenTheUserClicksViewDetailsForTheNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            importNotificationsPage?.ClickViewDetails(chedReference);
        }

        [When(@"the user clicks Address book link")]
        [When("the user clicks the Address book link on the Your import notifications page")]
        public void WhenTheUserClicksAddressBookLink()
        {
            importNotificationsPage?.ClickAddressBookLink();
        }

        [When("the user clicks Contact link on the footer")]
        public void WhenTheUserClicksContactLinkOnTheFooter()
        {
            importNotificationsPage?.ClickContactLink();
        }

        [Then("the Search notifications by section displays all the fields on the Your import notifications page")]
        public void ThenTheSearchNotificationsBySectionDisplaysAllTheFieldsOnTheYourImportNotificationsPage()
        {
            Assert.Multiple(() =>
            {
                Assert.True(importNotificationsPage?.IsSearchNotiByPanelDisplayed, "Search notifications by panel is not displayed on the Your import notifications page");
                Assert.True(importNotificationsPage?.AreAllSearchFieldsDisplayed(), "Not all search fields are displayed under Search notifications by panel");
            });
        }

        [When("the user clicks the View details link")]
        public void WhenTheUserClicksTheViewDetailsLink()
        {
            importNotificationsPage?.ClickViewDetailsLink();
        }

        [Then("the user searches for the Draft CHED reference on the dashboard")]
        public void ThenTheUserSearchesForTheDraftCHEDReferenceOnTheDashboard()
        {
            var draftCHEDReference = _scenarioContext.Get<string>("DraftCHEDReference");
            importNotificationsPage?.SearchForNotification(draftCHEDReference);
        }

        [Then("the draft notification should be present in the list")]
        public void ThenTheDraftNotificationShouldBePresentInTheList()
        {
            var draftCHEDReference = _scenarioContext.Get<string>("DraftCHEDReference");
            Assert.True(importNotificationsPage?.VerifyNotificationInList(draftCHEDReference), "Draft notification not found in the list");
        }

        [When("the user clicks the Amend link")]
        public void WhenTheUserClicksTheAmendLink()
        {
            var draftCHEDReference = _scenarioContext.Get<string>("DraftCHEDReference");
            importNotificationsPage?.ClickAmend(draftCHEDReference);
        }

        [When("the user clicks the Copy as new link for the notification")]
        public void WhenTheUserClicksTheCopyAsNewLinkForTheNotification()
        {
            importNotificationsPage?.ClickCopyAsNewLink();
        }

        [When("the user deletes all the stored values")]
        public void WhenTheUserDeletesAllTheStoredValues()
        {
            _scenarioContext.Clear();
        }

        [When("the user Clicks on Clone a certificate button")]
        public void WhenTheUserClicksOnCloneACertificateButton()
        {
            importNotificationsPage?.ClickCloneButton();
        }

        private void ValidateIfExists(string contextKey, string? reviewValue, ref bool allDataMatches, List<string> mismatches) 
        {
            if (IsCloningDetailsKey(contextKey))
            {
                ValidateCloningDetails(contextKey, reviewValue, ref allDataMatches, mismatches);
                return;
            }

            if (!_scenarioContext.ContainsKey(contextKey))
            {
                LogSkip(contextKey, "not in context");
                return;
            }

            // Special cases first
            if (IsDateField(contextKey))
            {
                ValidateDate(contextKey, reviewValue, ref allDataMatches, mismatches);
                return;
            }

            if (IsIdentityField(contextKey))
            {
                NormalizeIdentityValues(contextKey, ref reviewValue);
            }

            if (contextKey == "SampleTime")
            {
                NormalizeSampleTime(contextKey, ref reviewValue);
            }

            if (IsSimpleStringComparisonField(contextKey))
            {
                ValidateSimpleString(contextKey, reviewValue, ref allDataMatches, mismatches);
                return;
            }

            if (contextKey == "LaboratoryTestsReason")
            {
                ValidateLabReason(contextKey, reviewValue, ref allDataMatches, mismatches);
                return;
            }

            if (contextKey == "ConsignmentContactAddress")
            {
                ValidateAddress(contextKey, reviewValue, ref allDataMatches, mismatches);
                return;
            }

            // Generic handlers
            var contextValue = _scenarioContext[contextKey];

            switch (contextValue)
            {
                case List<string> list:
                    ValidateStringList(contextKey, list, reviewValue, ref allDataMatches, mismatches);
                    break;

                case string[] array:
                    ValidateStringArray(contextKey, array, reviewValue, ref allDataMatches, mismatches);
                    break;

                case string str:
                    ValidateString(contextKey, str, reviewValue, ref allDataMatches, mismatches);
                    break;

                default:
                    LogSkip(contextKey, $"unsupported type: {contextValue.GetType().Name}");
                    break;
            }
        }

        private bool IsCloningDetailsKey(string key) => _scenarioContext.ContainsKey("CloningHealthCertificateDetails") && (key is "ConsignorConsigneeOrImporterName" or "PurposeOfTheConsignment");

        private void ValidateCloningDetails(string key, string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            var details = _scenarioContext.Get<NotificationDetails>("CloningHealthCertificateDetails");
            var expected = typeof(NotificationDetails).GetProperty(key)?.GetValue(details)?.ToString()?.Trim();

            ValidateString(key, expected, reviewValue, ref allDataMatches, mismatches);
        }


        private bool IsDateField(string key) => !_scenarioContext["CHEDReference"].ToString().Contains("CHEDA") && (key is "HealthCertificateDateOfIssue" or "DocumentDateOfIssue");

        private void ValidateDate(string key, string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            try
            {
                var reviewDate = DateTime.ParseExact(reviewValue?.Split(' ')[0], "dd.MM.yyyy", null);
                var expectedDate = DateTime.ParseExact(_scenarioContext.Get<string>(key), "dd MM yyyy", null);

                if (reviewDate.ToString("ddMMyyyy") != expectedDate.ToString("ddMMyyyy"))
                    mismatches.Add($"{key}: Expected '{expectedDate:ddMMyyyy}', Found '{reviewDate:ddMMyyyy}'");
                else
                    LogMatch(key, expectedDate.ToString("ddMMyyyy"));
            }
            catch (Exception ex)
            {
                mismatches.Add($"{key}: Date parsing failed - {ex.Message}");
            }
        }


        private bool IsIdentityField(string key) =>    key is "IdentityCheck" or "IdentityCheckType";

        private void NormalizeIdentityValues(string key, ref string? reviewValue)
        {
            var expected = _scenarioContext.Get<string>(key).Replace(" ", "");
            reviewValue = reviewValue?.Replace(" ", "");
            _scenarioContext[key] = expected;
        }


        private void NormalizeSampleTime(string key, ref string? reviewValue)
        {
            var expected = TimeSpan.Parse(_scenarioContext.Get<string>(key));
            var actual = TimeSpan.Parse(reviewValue);

            if (Math.Abs((expected - actual).TotalMinutes) == 1)
                actual = expected;

            reviewValue = actual.ToString(@"hh\:mm");
            _scenarioContext[key] = expected.ToString(@"hh\:mm");
        }


        private bool IsSimpleStringComparisonField(string key) => key is "Temperature" or "DocumentaryCheckDecision" or "IdentityCheck" or "PhysicalCheck" or "LaboratoryTests";

        private void ValidateSimpleString(string key, string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            ValidateString(key, _scenarioContext.Get<string>(key), reviewValue, ref allDataMatches, mismatches);
        }


        private void ValidateLabReason(string key, string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            var expected = _scenarioContext.Get<string>(key);
            var actual = reviewValue?.Equals("Suspicious", StringComparison.OrdinalIgnoreCase) == true
                ? "Suspicion"
                : reviewValue;

            ValidateString(key, expected, actual, ref allDataMatches, mismatches);
        }


        private void ValidateAddress(string key, string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            string Normalize(string s) =>
                string.Join(" ", s.Replace(",", " ").Split((char[])null, StringSplitOptions.RemoveEmptyEntries)).ToLower();

            var expected = Normalize(_scenarioContext.Get<string>(key));
            var actual = Normalize(reviewValue ?? "");

            ValidateString(key, expected, actual, ref allDataMatches, mismatches);
        }


        private void ValidateStringList(string key, List<string> list, string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            var expected = string.Join("\n", list).Trim();
            var actual = reviewValue?.Trim().Replace("\r", "").Replace("\n\n", "\n");

            ValidateString(key, expected, actual, ref allDataMatches, mismatches);
        }


        private void ValidateStringArray(string key, string[] array, string? reviewValue, ref bool allDataMatches, List<string> mismatches)
        {
            ValidateString(key, array.FirstOrDefault(), reviewValue, ref allDataMatches, mismatches);
        }


        private void ValidateString(string key, string? expected, string? actual, ref bool allDataMatches, List<string> mismatches)
        {
            if (string.IsNullOrWhiteSpace(expected))
            {
                LogSkip(key, "empty value");
                return;
            }

            if (!string.Equals(expected.Trim(), actual?.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                allDataMatches = false;
                mismatches.Add($"{key}: Expected '{expected}', Found '{actual}'");
            }
            else
            {
                LogMatch(key, expected);
            }
        }


        private void LogMatch(string key, string value) => Console.WriteLine($"[PDF VALIDATION] ✓ {key}: '{value}' matches");

        private void LogSkip(string key, string reason) => Console.WriteLine($"[PDF VALIDATION] ⊘ {key}: Skipped ({reason})");


        private void ValidateContains(string contextKey, string? actual, ref bool allDataMatches, List<string> mismatches, bool contextContainsPDF = false)
        {
            (List<string> expectedWords, List<string> actualWords) result = (null, null);

            bool IsCloningKey =
                _scenarioContext.ContainsKey("CloningHealthCertificateDetails") &&
                contextKey is "CommodityCode" or "Description" or "GenusAndSpecies" or "NetWeight"
                          or "Packages" or "TypeOfPackage" or "CountryOfOriginOfCertificate"
                          or "PurposeOfTheConsignment" or "EstablishmentListFirstName";

            if (IsCloningKey)
            {
                ValidateCloning(contextKey, actual, ref allDataMatches, mismatches, contextContainsPDF);
                return;
            }

            if (!_scenarioContext.ContainsKey(contextKey))
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
                return;
            }

            object rawExpected = _scenarioContext[contextKey];

            if (rawExpected is List<string> list)
            {
                ValidateList(contextKey, actual, list, ref allDataMatches, mismatches);
                return;
            }

            if (rawExpected is string[] arr)
            {
                ValidateArray(contextKey, actual, arr, ref allDataMatches, mismatches);
                return;
            }

            string expectedValue = ExtractExpectedValue(contextKey, rawExpected, ref actual, ref result);

            if (string.IsNullOrEmpty(expectedValue) || string.IsNullOrEmpty(actual))
                return;

            bool isMatch = CompareValues(expectedValue, actual, contextContainsPDF, result);

            if (!isMatch)
            {
                allDataMatches = false;
                mismatches.Add($"{contextKey}: Expected '{expectedValue}', Found '{actual.Trim()}'");
            }
            else
            {
                Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: '{expectedValue}' matches");
            }
        }


        private void ValidateCloning(string contextKey, string actual, ref bool allDataMatches, List<string> mismatches, bool contextContainsPDF)
        {
            var details = _scenarioContext.Get<NotificationDetails>("CloningHealthCertificateDetails");
            var property = typeof(NotificationDetails).GetProperty(contextKey);
            string expectedValue = property?.GetValue(details)?.ToString()?.Trim();

            if (contextKey is "Packages" or "NetWeight")
            {
                double numericValue = Convert.ToDouble(property?.GetValue(details));
                expectedValue = (numericValue * 2).ToString();
            }

            if (string.IsNullOrEmpty(expectedValue))
                return;

            string RemoveWS(string s) => string.Concat(s.Where(c => !char.IsWhiteSpace(c)));

            bool isMatch = contextContainsPDF
                ? RemoveWS(expectedValue).Contains(RemoveWS(actual), StringComparison.OrdinalIgnoreCase)
                : RemoveWS(actual).Contains(RemoveWS(expectedValue), StringComparison.OrdinalIgnoreCase);

            if (!isMatch)
            {
                allDataMatches = false;
                mismatches.Add($"{contextKey}: Expected '{expectedValue}', Found '{actual.Trim()}'");
            }
            else
            {
                Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: '{expectedValue}' matches");
            }
        }


        private string ExtractExpectedValue(string contextKey, object rawExpected, ref string actual, ref (List<string> expectedWords, List<string> actualWords) result)
        {
            string expectedValue = rawExpected switch
            {
                string s => s.Trim(),
                string[] arr => string.Join(" ", arr).Trim(),
                _ => null
            };

            if (expectedValue == null)
                return null;

            // All your special-case transformations preserved:
            if (contextKey == "IUUSubOption")
            {
                if (expectedValue == "No need to inspect - exempt or not applicable") expectedValue = "IUUNA";
                if (expectedValue == "Compliant") expectedValue = "IUUOK";
            }

            if (contextKey == "ExitBCP" && actual!=null) actual = actual.Replace(".", "").Trim();
            if (contextKey is "ExitBorderControlPost" or "PortOfEntry") expectedValue = expectedValue.Split(new[] { '(', '-' })[0].Trim();
            if (contextKey == "InspectionPremises") expectedValue = expectedValue.Split('-')[0].Trim();
            if (contextKey == "ImporterAddress") actual = actual.Replace("Address ", "").Trim();
            if (contextKey == "TotalPackages") actual = Regex.Matches(actual, @"\d+").Select(m => int.Parse(m.Value)).Sum().ToString();
            if (contextKey == "NumberOfAnimals") expectedValue += " Units";
            if (contextKey == "EstimatedArrivalDate")
                expectedValue = DateTime.ParseExact(expectedValue, "dd MMM yyyy", CultureInfo.InvariantCulture).ToString("dd.MM.yyyy");

            if (expectedValue.Contains("London Borough of Hillingdon Heathrow Airport Imported Food Office"))
            {
                result = ConvertLinesAsWords(expectedValue, actual);
                result.actualWords.Remove("adada");
                result.expectedWords.Remove("adada");
            }

            return expectedValue;
        }


        private void ValidateList(string contextKey, string actual, List<string> list, ref bool allDataMatches, List<string> mismatches)
        {
            foreach (var item in list.Where(i => !string.IsNullOrWhiteSpace(i)))
            {
                if (actual.Contains(item.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: '{item}' matches");                    
                    return;
                }
            }

            mismatches.Add($"{contextKey}: None of the expected values were found in PDF");
            allDataMatches = false;
        }


        private void ValidateArray(string contextKey, string actual, string[] arr, ref bool allDataMatches, List<string> mismatches)
        {
            foreach (var rawItem in arr.Where(i => !string.IsNullOrWhiteSpace(i)))
            {
                var item = rawItem.Trim();
                if (actual.Contains(item, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: '{item}' matches");

                    if (contextKey == "LabTestNameCopy"
                        || contextKey == "LabSampleStorageTemperatureCopy"
                        || contextKey == "NumberOfLabSamplesCopy"
                        || contextKey == "LabSampleReferenceCopy")
                    {
                        if (_scenarioContext[contextKey] is string[] valuesArr)
                        {
                            _scenarioContext[contextKey] = RemoveFirstOccurrence(valuesArr, item);
                        }
                    }

                    return;
                }
            }

            mismatches.Add($"{contextKey}: None of the expected values were found in PDF");
            allDataMatches = false;
        }

        private static string[] RemoveFirstOccurrence(string[] source, string valueToRemove)
        {
            if (source == null || source.Length == 0) return source;

            int idx = Array.FindIndex(source, s => string.Equals(s?.Trim(), valueToRemove, StringComparison.OrdinalIgnoreCase));
            if (idx < 0) return source;

            var result = new string[source.Length - 1];
            if (idx > 0) Array.Copy(source, 0, result, 0, idx);
            if (idx < source.Length - 1) Array.Copy(source, idx + 1, result, idx, source.Length - idx - 1);
            return result;
        }


        private bool CompareValues(string expectedValue, string actual, bool contextContainsPDF, (List<string> expectedWords, List<string> actualWords) result)
        {
            if (result.expectedWords != null && result.actualWords != null)
                return result.expectedWords.SequenceEqual(result.actualWords);

            string RemoveWS(string s) => string.Concat(s.Where(c => !char.IsWhiteSpace(c)));

            return contextContainsPDF
                ? RemoveWS(expectedValue).Contains(RemoveWS(actual), StringComparison.OrdinalIgnoreCase)
                : RemoveWS(actual).Contains(RemoveWS(expectedValue), StringComparison.OrdinalIgnoreCase);
        }


        public (List<string> expectedWords, List<string> actualWords) ConvertLinesAsWords(string expected, string actual)
        {
            string Clean(string s) =>
                new string(s.Where(c => !char.IsPunctuation(c)).ToArray());

            var expectedWords = Clean(expected)
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(w => w.Trim().ToLower())
                .OrderBy(w => w)
                .ToList();

            var actualWords = Clean(actual)
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(w => w.Trim().ToLower())
                .OrderBy(w => w)
                .ToList();

            return (expectedWords, actualWords);
        }

        public void ValidateAllMatchesInList(string contextKey, string? actual, ref bool allDataMatches, List<string> mismatches)
        {
            if (_scenarioContext.ContainsKey(contextKey))
            {
                object rawExpected = _scenarioContext[contextKey];
                string expectedValue;

                if (rawExpected is string[] arr)
                {
                    // join array into one string (adjust to comma if needed)
                    expectedValue = string.Join(" ", arr).Trim();
                }
                else if (rawExpected is List<string> list)
                {
                    bool anyMatch = false;

                    foreach (var item in list)
                    {
                        if (string.IsNullOrWhiteSpace(item))
                            continue;

                        if (actual.Contains(item.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: '{item}' matches");
                            anyMatch = true;
                        }
                    }
                    if (!anyMatch)
                    {
                        mismatches.Add($"{contextKey}: None of the expected values were found in PDF");
                        allDataMatches = false;
                    }
                    return;
                }
                else
                {
                    mismatches.Add($"{contextKey}: Unsupported type '{rawExpected.GetType().Name}'");
                    return;
                }

            }
        }

        [When("the user clicks Manage trade partners")]
        public void WhenTheUserClicksManageTradePartners()
        {
            importNotificationsPage?.ClickManageTradePartnersLink();
        }
    }
}