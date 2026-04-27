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
            //var chedReference = "CHEDP.GB.2025.1056538";
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

            Utils.DownloadPDF(chedReferenceFileName, pdfUrl, UserObject, _scenarioContext.Get<string>("UserRole"));
        }

        [When("the user checks that the data in the certificate matches the data entered into the notification")]
        public void WhenTheUserChecksThatTheDataInTheCertificateMatchesTheDataEnteredIntoTheNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(importNotificationsPage?.VerifyDataInCertificate(chedReference), "Certificate data verification failed");

            var json = JsonConvert.SerializeObject(_scenarioContext.ToDictionary(kvp => kvp.Key, kvp => kvp.Value), Formatting.Indented);
            var chedReferenceFileName = "\\" + chedReference + "-certificate";
            var downloadDirectory = Path.Combine(Path.GetTempPath(), "automation-downloads");
            string pdfPath = downloadDirectory + chedReferenceFileName + ".pdf";
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
                        ValidateIfExists("ConsignorName", page.Sections.ConsignorExporter.Name, ref allDataMatches, mismatches);
                        ValidateIfExists("ConsignorAddress", page.Sections.ConsignorExporter.Address, ref allDataMatches, mismatches);
                        ValidateIfExists("ConsignorCountry", page.Sections.ConsignorExporter.Country, ref allDataMatches, mismatches);
                        ValidateIfExists("ConsigneeName", page.Sections.ConsigneeImporter.Name, ref allDataMatches, mismatches);
                        ValidateIfExists("ConsigneeAddress", page.Sections.ConsigneeImporter.Address, ref allDataMatches, mismatches);
                        ValidateContains("ConsigneeCountry", page.Sections.ConsigneeImporter.Country, ref allDataMatches, mismatches, true);
                        ValidateContains("PlaceOfDestinationDetails", page.Sections.PlaceOfDestination.Name, ref allDataMatches, mismatches, true);
                        ValidateContains("PlaceOfDestinationDetails", page.Sections.PlaceOfDestination.Address, ref allDataMatches, mismatches, true);
                        ValidateContains("PlaceOfDestinationDetails", page.Sections.PlaceOfDestination.Country, ref allDataMatches, mismatches, true);

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
                        else if (_scenarioContext["CHEDReference"].ToString().Contains("CHEDA"))
                        {
                            ValidateContains("HealthDocumentType", (string?)page.Sections.AccompanyingDocuments.AdditionalData.ElementAt(1).Value, ref allDataMatches, mismatches);
                            ValidateContains("HealthCertificateReference", (string?)page.Sections.AccompanyingDocuments.AdditionalData.ElementAt(1).Value, ref allDataMatches, mismatches);
                            //ValidateIfExists("HealthCertificateDateOfIssue", (string?)page.Sections.AccompanyingDocuments.AdditionalData.ElementAt(1).Value, ref allDataMatches, mismatches);
                            ValidateContains("DocumentType", (string?)page.Sections.AccompanyingDocuments.AdditionalData.ElementAt(1).Value, ref allDataMatches, mismatches);
                            ValidateContains("DocumentReference", (string?)page.Sections.AccompanyingDocuments.AdditionalData.ElementAt(1).Value, ref allDataMatches, mismatches);
                        }


                        ValidateIfExists("CommodityIntendedFor", page.Sections.GoodsCertifiedAs?.Value, ref allDataMatches, mismatches);
                        if (_scenarioContext["CHEDReference"].ToString().Contains("CHEDA"))
                        {
                            ValidateContains("CertificationOption", page.Sections.GoodsCertifiedAs?.Value, ref allDataMatches, mismatches);
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
                        //ValidateContains("ExitBCP", page.Sections.DirectTransit?.ExitBCP, ref allDataMatches, mismatches, true);

                        var traces = page.Sections.DirectTransit?.TracesUnitNo.ToString();
                        ValidateContains("ExitBCP", page.Sections.DirectTransit?.TracesUnitNo, ref allDataMatches, mismatches, true);


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
                            ValidateContains("NetWeightSecondCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(1).Value, ref allDataMatches, mismatches);
                            ValidateContains("NumOfPackagesSecondCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(1).Value, ref allDataMatches, mismatches);
                            ValidateContains("TypeOfPackageSecondCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(1).Value, ref allDataMatches, mismatches);

                            ValidateContains("TotalNetWeight", page.Sections.TotalNetWeight?.Value, ref allDataMatches, mismatches);
                            ValidateContains("TotalPackages", page.Sections.TotalNumberOfPackages?.Value, ref allDataMatches, mismatches);
                            ValidateContains("TotalGrossWeight", page.Sections.TotalGrossWeight?.Value, ref allDataMatches, mismatches);
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

                        string? pdfLaboratoryTestNames = page.Sections.LaboratoryTests
                        switch
                        {
                            { Random: "true" } => "Random",
                            { Suspicion: "true" } => "Suspicion",
                            { IntensifiedControls: "true" } => "IntensifiedControls",
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
                            ValidateContains("ExitBorderControlPost", (string?)page.Sections.AcceptableForTransit?.AdditionalData.ElementAt(2).Value, ref allDataMatches, mismatches);
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
                        if (_scenarioContext["CHEDReference"].ToString().Contains("CHEDA"))
                        {
                            ValidateContains("PortOfEntry", page.Sections.References.Name, ref allDataMatches, mismatches);
                        }
                        else
                            ValidateContains("PortOfEntry", page.Sections.References.Name, ref allDataMatches, mismatches);

                        ValidateContains("CountryOfOrigin", (string?)page.Sections.IdentificationOfTheSample.AdditionalData.ElementAt(0).Value, ref allDataMatches, mismatches);
                        if (!_scenarioContext["CHEDReference"].ToString().Contains("CHEDA"))
                        {
                            ValidateIfExists("ContryFromWhereConsigned", (string?)page.Sections.IdentificationOfTheSample.AdditionalData.ElementAt(4).Value, ref allDataMatches, mismatches);
                        }
                        var destination = page.Sections.IdentificationOfTheSample.AdditionalData.ElementAt(3).Value.ToString();

                        ValidateContains("PlaceOfDestinationDetails", destination.Replace("Commodity", ""), ref allDataMatches, mismatches, true);
                        ValidateContains("CommodityCode", page.Sections.IdentificationOfTheSample.Commodity, ref allDataMatches, mismatches);
                        //ValidateContains("CommodityDescription", page.Sections.IdentificationOfTheSample.Commodity, ref allDataMatches, mismatches);
                        ValidateContains("Species", (string?)page.Sections.IdentificationOfTheSample.AdditionalData.ElementAt(1).Value, ref allDataMatches, mismatches);
                        ValidateIfExists("LaboratoryTestsReason", (string?)page.Sections.RequestedAnalysis.AdditionalData.ElementAt(0).Value, ref allDataMatches, mismatches);
                        ValidateContains("LabTestName", (string?)page.Sections.LabResults.Name, ref allDataMatches, mismatches, true);
                        ValidateContains("LabSampleReference", (string?)page.Sections.LabResults.AdditionalData.ElementAt(2).Value, ref allDataMatches, mismatches, true);
                        ValidateIfExists("NumberOfLabSamples", (string?)page.Sections.LabResults.AdditionalData.ElementAt(4).Value, ref allDataMatches, mismatches);
                        ValidateIfExists("LabSampleType", (string?)page.Sections.LabResults.AdditionalData.ElementAt(3).Value, ref allDataMatches, mismatches);
                        ValidateContains("LabSampleStorageTemperature", (string?)page.Sections.LabResults.AdditionalData.ElementAt(5).Value, ref allDataMatches, mismatches, true);
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

            if (File.Exists(pdfPath))
            {
                File.Delete(pdfPath);
                Console.WriteLine("File deleted successfully.");
            }
            else
            {
                Console.WriteLine("File not found to delete.");
            }
        }


        [When("the user checks that the data in the certificate matches the data entered into the CHED PP notification")]
        public void WhenTheUserChecksThatTheDataInTheCertificateMatchesTheDataEnteredIntoTheCHEDPPNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");
            Assert.True(importNotificationsPage?.VerifyDataInCertificate(chedReference), "Certificate data verification failed");

            var json = JsonConvert.SerializeObject(_scenarioContext.ToDictionary(kvp => kvp.Key, kvp => kvp.Value), Formatting.Indented);
            var chedReferenceFileName = "\\" + chedReference + "-certificate";
            var downloadDirectory = Path.Combine(Path.GetTempPath(), "automation-downloads");
            string pdfPath = downloadDirectory + chedReferenceFileName + ".pdf";
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
                        ValidateContains("CHEDReference", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);
                        ValidateContains("ConsignmentReferenceNumber", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);
                        ValidateContains("InspectionPremises", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);
                        //ValidateContains("BorderControlPost", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches, true);
                        ValidateContains("ConsignorName", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);
                        ValidateContains("ConsignorAddress", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);
                        ValidateContains("ConsignorCountry", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);
                        ValidateContains("CompanyName", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);
                        ValidateContains("ImporterAddress", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);
                        ValidateContains("ConsigneeCountry", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);
                        ValidateContains("DeliveryAddressName", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);
                        ValidateContains("DeliveryAddress", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);
                        ValidateContains("DeliveryCountry", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);

                        ValidateContains("DocumentType", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);
                        ValidateContains("DocumentReference", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);


                        ValidateIfExists("ContryFromWhereConsigned", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);
                        ValidateContains("MeansOfTransport", page.Sections.PartIDescriptionOfConsignment.Mode, ref allDataMatches, mismatches);
                        var meansOfTransport = _scenarioContext.Get<string>("MeansOfTransport");
                        if (meansOfTransport.Equals(page.Sections.PartIDescriptionOfConsignment.Mode, StringComparison.OrdinalIgnoreCase))
                            ValidateContains("EnterTransportDocRef", page.Sections.PartIDescriptionOfConsignment.InternationalTransportDocument, ref allDataMatches, mismatches);
                        else
                            ValidateContains("EnterTransportDocRef", page.Sections.PartIDescriptionOfConsignment.Mode, ref allDataMatches, mismatches);

                        ValidateContains("TransportId", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);
                        //ValidateContains("CountryOfOrigin", page.Sections.CountryOfOrigin.Value, ref allDataMatches, mismatches);

                        ValidateContains("EstimatedArrivalDate", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);
                        ValidateContains("EstimatedArrivalTime", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);

                        ValidateContains("ContactName", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);
                        ValidateContains("ConsignmentContactAddress", page.Sections.PartIDescriptionOfConsignment.Value, ref allDataMatches, mismatches);

                        if (_scenarioContext.ContainsKey("CloningHealthCertificateDetails"))
                        {
                            ValidateIfExists("ConsignorConsigneeOrImporterName", page.Sections.ConsigneeImporter.Name, ref allDataMatches, mismatches);
                            ValidateIfExists("PurposeOfTheConsignment", page.Sections.GoodsCertifiedAs.Value, ref allDataMatches, mismatches);
                        }
                    }

                    else if (pageNumber == 2)
                    {
                        if (_scenarioContext.ContainsKey("CloningHealthCertificateDetails"))
                        {
                            //Clone scenario
                            ValidateContains("CommodityCode", page.Sections.CHEDPPPageII.Value, ref allDataMatches, mismatches);
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
                            ValidateContains("CommodityCodeFirstCommodity", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("CommodityDescFirstCommodity", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("Species", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("NetWeightFirstCommodity", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("NumberOfPackagesFirstCommodity", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("TypeOfCommodity", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("TypeOfPackageFirstCommodity", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("EstablishmentListFirstName", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("CountryOfOrigin", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);

                            ValidateContains("CommodityCodeSecondCommodity", page.Sections.DescriptionOfTheGoods.ElementAt(1).Value, ref allDataMatches, mismatches);
                            ValidateContains("CommodityDescSecondCommodity", page.Sections.DescriptionOfTheGoods.ElementAt(1).Value, ref allDataMatches, mismatches);
                            ValidateContains("Species", page.Sections.DescriptionOfTheGoods.ElementAt(1).Value, ref allDataMatches, mismatches);
                            ValidateContains("NetWeightSecondCommodity", page.Sections.DescriptionOfTheGoods.ElementAt(1).Value, ref allDataMatches, mismatches);
                            ValidateContains("NumOfPackagesSecondCommodity", page.Sections.DescriptionOfTheGoods.ElementAt(1).Value, ref allDataMatches, mismatches);
                            ValidateContains("TypeOfPackageSecondCommodity", page.Sections.DescriptionOfTheGoods.ElementAt(1).Value, ref allDataMatches, mismatches);

                            ValidateContains("TotalNetWeight", page.Sections.TotalNetWeight?.Value, ref allDataMatches, mismatches);
                            ValidateContains("TotalPackages", page.Sections.TotalNumberOfPackages?.Value, ref allDataMatches, mismatches);
                            ValidateContains("TotalGrossWeight", page.Sections.TotalGrossWeight?.Value, ref allDataMatches, mismatches);

                        }
                        else
                        {
                            ValidateContains("CommodityCode", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("CommodityDescription", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("CommodityCodeFirstCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("CommodityDescFirstCommodity", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("Species", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("NetWeight", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("NumberOfPackages", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
                            ValidateContains("PackageType", page.Sections.DescriptionOfTheGoods?.ElementAt(0).Value, ref allDataMatches, mismatches);
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

            if (File.Exists(pdfPath))
            {
                File.Delete(pdfPath);
                Console.WriteLine("File deleted successfully.");
            }
            else
            {
                Console.WriteLine("File not found to delete.");
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
            if (_scenarioContext.ContainsKey("CloningHealthCertificateDetails") && (contextKey.Equals("ConsignorConsigneeOrImporterName") || contextKey.Equals("PurposeOfTheConsignment")))
            {
                string expectedValue = null;
                var details = _scenarioContext.Get<NotificationDetails>("CloningHealthCertificateDetails");
                var property = typeof(NotificationDetails).GetProperty(contextKey);
                expectedValue = property?.GetValue(details)?.ToString()?.Trim();

                if (expectedValue != null && !string.IsNullOrEmpty(expectedValue.ToString()))
                {
                    var isMatch = expectedValue.Equals(reviewValue?.Trim(), StringComparison.OrdinalIgnoreCase);
                    if (!isMatch)
                    {
                        allDataMatches = false;
                        mismatches.Add($"{contextKey}: Expected '{expectedValue}', Found '{reviewValue?.Trim()}'");
                    }
                    else
                    {
                        Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: '{expectedValue}' matches");
                    }
                }
                else
                {
                    Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (empty value in array)");
                }
            }
            else if (_scenarioContext.ContainsKey(contextKey))
            {
                object contextValue = _scenarioContext[contextKey];

                // Date validation for DocumentDateOfIssue
                if (!_scenarioContext["CHEDReference"].ToString().Contains("CHEDA") && (contextKey.Equals("HealthCertificateDateOfIssue") || contextKey.Equals("DocumentDateOfIssue")))
                {
                    try
                    {
                        // reviewValue example: "12.03.2024 00:00:00"
                        var reviewDateString = reviewValue?.Split(' ')[0]; // take only dd.MM.yyyy

                        var reviewDate = DateTime.ParseExact(reviewDateString, "dd.MM.yyyy", null);
                        var expectedDate = DateTime.ParseExact(_scenarioContext.Get<string>(contextKey), "dd MM yyyy", null);

                        var reviewFormatted = reviewDate.ToString("ddMMyyyy");
                        var expectedFormatted = expectedDate.ToString("ddMMyyyy");

                        if (!reviewFormatted.Equals(expectedFormatted))
                        {
                            allDataMatches = false;
                            mismatches.Add($"{contextKey}: Expected '{expectedFormatted}', Found '{reviewFormatted}'");
                        }
                        else
                        {
                            Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: '{expectedFormatted}' matches");
                        }
                    }
                    catch (Exception ex)
                    {
                        allDataMatches = false;
                        mismatches.Add($"{contextKey}: Date parsing failed - {ex.Message}");
                    }

                    return;
                }

                if (_scenarioContext.ContainsKey(contextKey) && contextKey is "IdentityCheck" or "IdentityCheckType")
                {
                    contextValue = (contextKey.Equals("IdentityCheck") ? _scenarioContext.Get<string>("IdentityCheck").Replace(" ", "", StringComparison.OrdinalIgnoreCase) : _scenarioContext.Get<string>("IdentityCheckType").Replace(" ", "", StringComparison.OrdinalIgnoreCase));
                    reviewValue = reviewValue.Replace(" ", "", StringComparison.OrdinalIgnoreCase);
                }

                if (contextKey.Equals("SampleTime"))
                {
                    var expectedTime = TimeSpan.Parse(_scenarioContext.Get<string>(contextKey));
                    var actualTime = TimeSpan.Parse(reviewValue);

                    if (Math.Abs((expectedTime - actualTime).TotalMinutes) == 1)
                    {
                        // Align actual time to expected time
                        actualTime = expectedTime;
                    }

                    contextValue = expectedTime.ToString(@"hh\:mm");
                    reviewValue = actualTime.ToString(@"hh\:mm");
                }

                if (_scenarioContext.ContainsKey(contextKey) && contextKey is "Temperature"
                                                                 or "DocumentaryCheckDecision"
                                                                 or "IdentityCheck"
                                                                 or "PhysicalCheck"
                                                                 or "LaboratoryTests")
                {
                    var expected = _scenarioContext.Get<string>(contextKey)?.Trim();
                    var actual = reviewValue?.Trim();

                    if (!string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase))
                    {
                        allDataMatches = false;
                        mismatches.Add($"{contextKey}: Expected '{expected}', Found '{actual}'");
                    }
                    else
                    {
                        Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: '{expected}' matches");
                    }

                    return;
                }

                if (_scenarioContext.ContainsKey(contextKey) && contextKey is "LaboratoryTestsReason")
                {
                    var expected = _scenarioContext.Get<string>(contextKey)?.Trim();
                    string actual = reviewValue?.Trim();

                    if (actual.Equals("Suspicious", StringComparison.OrdinalIgnoreCase))
                    {
                        actual = "Suspicion";
                    }

                    if (!string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase))
                    {
                        allDataMatches = false;
                        mismatches.Add($"{contextKey}: Expected '{expected}', Found '{actual}'");
                    }
                    else
                    {
                        Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: '{expected}' matches");
                    }

                    return;
                }


                // ConsignmentContactAddress special address-normalisation validation
                if (contextKey == "ConsignmentContactAddress")
                {
                    var expectedRaw = _scenarioContext.Get<string>("ConsignmentContactAddress");
                    var actualRaw = reviewValue;

                    // Normalise expected
                    var expectedFormatted = string.Join(
                        " ",
                        expectedRaw.Replace(",", " ")
                                   .Split((char[])null, StringSplitOptions.RemoveEmptyEntries)
                    ).ToLower();

                    // Normalise actual value
                    var actualFormatted = string.Join(
                        " ",
                        actualRaw?.Replace(",", " ")
                                  .Split((char[])null, StringSplitOptions.RemoveEmptyEntries)
                                  ?? Array.Empty<string>()
                    ).ToLower();

                    if (!expectedFormatted.Equals(actualFormatted))
                    {
                        allDataMatches = false;
                        mismatches.Add(
                            $"{contextKey}: Expected '{expectedFormatted}', Found '{actualFormatted}'"
                        );
                    }
                    else
                    {
                        Console.WriteLine(
                            $"[PDF VALIDATION] ✓ {contextKey}: '{expectedFormatted}' matches"
                        );
                    }

                    return; // Stop further processing
                }


                // Handle List<string> (for multiple countries)
                if (contextValue is List<string> countryList)
                {
                    // Join the list with line breaks to match the HTML format
                    var expectedValue = string.Join("\n", countryList).Trim();

                    // The review page displays countries with <br> which Selenium converts to \n
                    var actualValue = reviewValue?.Trim().Replace("\r\n", "\n").Replace("\r", "\n");

                    var isMatch = expectedValue.Equals(actualValue, StringComparison.OrdinalIgnoreCase);
                    if (!isMatch)
                    {
                        allDataMatches = false;
                        mismatches.Add($"{contextKey}: Expected '{expectedValue}', Found '{actualValue}'");
                    }
                    else
                    {
                        Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: '{expectedValue}' matches");
                    }
                }
                // Handle string[] (for documents)
                else if (contextValue is string[] stringArray)
                {
                    var expectedValue = stringArray.FirstOrDefault();
                    if (!string.IsNullOrEmpty(expectedValue))
                    {
                        var isMatch = expectedValue.Equals(reviewValue?.Trim(), StringComparison.OrdinalIgnoreCase);
                        if (!isMatch)
                        {
                            allDataMatches = false;
                            mismatches.Add($"{contextKey}: Expected '{expectedValue}', Found '{reviewValue?.Trim()}'");
                        }
                        else
                        {
                            Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: '{expectedValue}' matches");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (empty value in array)");
                    }
                }
                // Handle single string value
                else if (contextValue is string expectedValue)
                {
                    if (!string.IsNullOrEmpty(expectedValue))
                    {
                        var isMatch = expectedValue.Equals(reviewValue?.Trim(), StringComparison.OrdinalIgnoreCase);
                        if (!isMatch)
                        {
                            allDataMatches = false;
                            mismatches.Add($"{contextKey}: Expected '{expectedValue}', Found '{reviewValue?.Trim()}'");
                        }
                        else
                        {
                            Console.WriteLine($"[PDF VALIDATION] ✓ {contextKey}: '{expectedValue}' matches");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                    }
                }
                else
                {
                    Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (unsupported type: {contextValue.GetType().Name})");
                }
            }
            else
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
            }
        }

        private void ValidateContains(string contextKey, string? actual, ref bool allDataMatches, List<string> mismatches, bool contextContainsPDF = false)
        {
            (List<string> expectedWords, List<string> actualWords) result = (null, null);

            if (_scenarioContext.ContainsKey("CloningHealthCertificateDetails") && (contextKey.Equals("CommodityCode") || contextKey.Equals("Description")
                || contextKey.Equals("GenusAndSpecies") || contextKey.Equals("NetWeight")
                || contextKey.Equals("Packages") || contextKey.Equals("TypeOfPackage")
                || contextKey.Equals("CountryOfOriginOfCertificate") || contextKey.Equals("PurposeOfTheConsignment")
                || contextKey.Equals("EstablishmentListFirstName")))
            {
                string expectedValue = null;
                var details = _scenarioContext.Get<NotificationDetails>("CloningHealthCertificateDetails");
                var property = typeof(NotificationDetails).GetProperty(contextKey);
                expectedValue = property?.GetValue(details)?.ToString()?.Trim();

                if (contextKey.Equals("Packages") || contextKey.Equals("NetWeight"))
                {
                    var rawValue = property?.GetValue(details);
                    double numericValue = Convert.ToDouble(rawValue);
                    expectedValue = (numericValue * 2).ToString();
                }

                if (expectedValue != null && !string.IsNullOrEmpty(expectedValue.ToString()))
                {
                    string RemoveWhitespace(string s) => string.Concat(s.Where(c => !char.IsWhiteSpace(c)));

                    bool isMatch = contextContainsPDF
                        ? RemoveWhitespace(expectedValue).Contains(RemoveWhitespace(actual), StringComparison.OrdinalIgnoreCase)
                        : RemoveWhitespace(actual).Contains(RemoveWhitespace(expectedValue), StringComparison.OrdinalIgnoreCase);

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
            }
            else if (_scenarioContext.ContainsKey(contextKey))
            {
                object rawExpected = _scenarioContext[contextKey];
                string expectedValue;

                if (rawExpected is string s)
                {
                    expectedValue = s.Trim();
                    if (contextKey.Trim() == "IUUSubOption" && expectedValue.Trim() == "No need to inspect - exempt or not applicable") expectedValue = "IUUNA";
                    if (contextKey.Trim() == "IUUSubOption" && expectedValue.Trim() == "Compliant") expectedValue = "IUUOK";

                    if (contextKey.Trim() == "ExitBCP") actual = actual.Replace(".", "").Trim();
                    if (contextKey.Trim() == "ExitBorderControlPost" || contextKey.Trim() == "PortOfEntry") expectedValue = expectedValue.Split(new[] { '(', '-' })[0].Trim();
                    if (contextKey.Trim() == "InspectionPremises") expectedValue = expectedValue.Split('-')[0].Trim();
                    if (contextKey.Trim() == "ImporterAddress") actual = actual.Replace("Address ", "").Trim();
                    if (contextKey.Trim() == "TotalPackages") actual = Regex.Matches(actual, @"\d+").Select(m => int.Parse(m.Value)).Sum().ToString();
                    if (contextKey.Trim() == "NumberOfAnimals") expectedValue = expectedValue + " Units";
                    if (contextKey.Trim() == "EstimatedArrivalDate") expectedValue = DateTime.ParseExact(expectedValue, "dd MMM yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd.MM.yyyy");
                    if (contextKey.Trim() == "PortOfEntry")
                    {
                        result = ConvertLinesAsWords(expectedValue, actual);
                        result.actualWords.Remove("adada");
                        result.expectedWords.Remove("adada");
                    }
                }
                else if (rawExpected is string[] arr)
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
                            break;
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


                if (!string.IsNullOrEmpty(expectedValue) && !string.IsNullOrEmpty(actual))
                {
                    bool isMatch = false;

                    if (result.actualWords is List<string> actualList &&
                            result.expectedWords is List<string> expectedList &&
                            actualList != null &&
                            expectedList != null)
                    {
                        isMatch = actualList.SequenceEqual(expectedList);
                    }
                    else
                    {
                        string RemoveWhitespace(string s) => string.Concat(s.Where(c => !char.IsWhiteSpace(c)));

                        isMatch = contextContainsPDF
                            ? RemoveWhitespace(expectedValue).Contains(RemoveWhitespace(actual), StringComparison.OrdinalIgnoreCase)
                            : RemoveWhitespace(actual).Contains(RemoveWhitespace(expectedValue), StringComparison.OrdinalIgnoreCase);

                    }
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
            }
            else
            {
                Console.WriteLine($"[PDF VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
            }
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
    }
}