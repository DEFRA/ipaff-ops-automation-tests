using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using Defra.UI.Tests.Tools.PDFProcessor.Models;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.Xrm.Sdk.Deployment;
using Newtonsoft.Json;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

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
            //var chedReference = "CHEDP.GB.2025.1056538";
            importNotificationsPage?.ClickShowNotification(chedReference);
        }

        [Then("the certificate should be displayed in a new browser tab")]
        public void ThenTheCertificateShouldBeDisplayedInANewBrowserTab()
        {
            //var chedRef = "CHEDP.GB.2025.1056538";
            Assert.True(importNotificationsPage?.VerifyCertificateInNewTab(), "Certificate not displayed in new browser tab");
        }

        [When("the user downloads the PDF for validation")]
        public void WhenTheUserDownloadsThePDFForValidation()
        {
            string pdfUrl = importNotificationsPage?.getPDFUrl();
            var chedReferenceFileName = _scenarioContext.Get<string>("CHEDReference") + "-certificate";

            Utils.DownloadPDF(chedReferenceFileName, pdfUrl, UserObject);
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
                for (int pageNumber = 0; pageNumber < chedDocumentPages.Count; pageNumber++)
                {
                    var page = chedDocumentPages[pageNumber];

                    if (pageNumber == 0)
                    {
                        ValidateIfExists("CHEDReference", page.Sections.ChedReference.Id, ref allDataMatches, mismatches);
                        ValidateIfExists("ConsignmentReferenceNumber", page.Sections.LocalReference.Id, ref allDataMatches, mismatches);
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
                        ValidateContains("DocumentReference", documentReference, ref allDataMatches, mismatches, true);

                        var dateEntry = page.Sections.AccompanyingDocuments.AdditionalData.FirstOrDefault(x => x.Key == "DateOfIssue");
                        var reviewDate = dateEntry.Key == null ? null : dateEntry.Value?.ToString();
                        ValidateIfExists("DocumentDateOfIssue", reviewDate, ref allDataMatches, mismatches);

                        ValidateIfExists("MeansOfTransport", page.Sections.MeansOfTransport.Mode, ref allDataMatches, mismatches);
                        ValidateIfExists("EnterTransportDocRef", page.Sections.MeansOfTransport.InternationalTransportDocument, ref allDataMatches, mismatches);
                        ValidateIfExists("TransportId", page.Sections.MeansOfTransport.Identification, ref allDataMatches, mismatches);
                        ValidateIfExists("CountryOfOrigin", page.Sections.CountryOfOrigin.Value, ref allDataMatches, mismatches);
                        ValidateContains("ApprovedEstablishmentName", page.Sections.EstablishmentsOfOrigin.ApprovalNumber, ref allDataMatches, mismatches);
                        ValidateContains("ApprovedEstablishmentCountry", page.Sections.EstablishmentsOfOrigin.ApprovalNumber, ref allDataMatches, mismatches);
                        ValidateContains("ApprovedEstablishmentType", page.Sections.EstablishmentsOfOrigin.ApprovalNumber, ref allDataMatches, mismatches);
                        ValidateContains("ApprovedEstablishmentApprovalNum", page.Sections.EstablishmentsOfOrigin.ApprovalNumber, ref allDataMatches, mismatches);

                        string? pdfTemperature = page.Sections.TransportConditions switch
                        {
                            { Ambient: "true" } => "Ambient",
                            { Frozen: "true" } => "Frozen",
                            { Chilled: "true" } => "Chilled",
                            _ => null
                        };
                        ValidateIfExists("Temperature", pdfTemperature, ref allDataMatches, mismatches);

                        ValidateContains("ContactName", page.Sections.OperatorResponsible.Name, ref allDataMatches, mismatches);
                        ValidateIfExists("ConsignmentContactAddress", page.Sections.OperatorResponsible.Address, ref allDataMatches, mismatches);
                    }

                    else if (pageNumber == 1)
                    {
                        var x = page.Sections.DescriptionOfTheGoods.ElementAt(0).Commodity;
                        ValidateIfExists("CommodityCode", page.Sections.DescriptionOfTheGoods.ElementAt(0).Commodity, ref allDataMatches, mismatches);
                        ValidateContains("CommodityDescription", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                        ValidateContains("Species", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                        ValidateContains("NetWeight", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                        ValidateContains("NumberOfPackages", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                        ValidateContains("PackageType", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                        ValidateContains("TypeOfCommodity", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                        ValidateContains("EstablishmentOfOrigin", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                        ValidateContains("CountryOfOrigin", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                        ValidateContains("TotalNetWeight", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                        ValidateContains("TotalPackages", page.Sections.DescriptionOfTheGoods.ElementAt(0).Value, ref allDataMatches, mismatches);
                        ValidateContains("TotalGrossWeight", page.Sections.TotalGrossWeight.Value, ref allDataMatches, mismatches);
                    }
                    else if (pageNumber == 2)
                    {
                        ValidateIfExists("CHEDReference", page.Sections.ChedReference.Id, ref allDataMatches, mismatches);
                    }
                    else if (pageNumber == 3)
                    {
                        ValidateIfExists("CHEDReference", page.Sections.ChedReference.Id, ref allDataMatches, mismatches);
                    }
                }
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
            Assert.AreEqual(expectedStatus, actualStatus, $"Expected status '{expectedStatus}' but found '{actualStatus}'");
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
            if (_scenarioContext.ContainsKey(contextKey))
            {
                object contextValue = _scenarioContext[contextKey];

                // Date validation for DocumentDateOfIssue
                if (contextKey == "DocumentDateOfIssue")
                {
                    try
                    {
                        // reviewValue example: "12.03.2024 00:00:00"
                        var reviewDateString = reviewValue?.Split(' ')[0]; // take only dd.MM.yyyy

                        var reviewDate = DateTime.ParseExact(reviewDateString, "dd.MM.yyyy", null);
                        var expectedDate = DateTime.ParseExact(_scenarioContext.Get<string>("DocumentDateOfIssue"), "dd MM yyyy", null);

                        var reviewFormatted = reviewDate.ToString("ddMMyyyy");
                        var expectedFormatted = expectedDate.ToString("ddMMyyyy");

                        if (!reviewFormatted.Equals(expectedFormatted))
                        {
                            allDataMatches = false;
                            mismatches.Add($"{contextKey}: Expected '{expectedFormatted}', Found '{reviewFormatted}'");
                        }
                        else
                        {
                            Console.WriteLine($"[REVIEW VALIDATION] ✓ {contextKey}: '{expectedFormatted}' matches");
                        }
                    }
                    catch (Exception ex)
                    {
                        allDataMatches = false;
                        mismatches.Add($"{contextKey}: Date parsing failed - {ex.Message}");
                    }

                    return; // stop further processing
                }


                // Temperature validation
                if (contextKey == "Temperature")
                {
                    var expectedTemp = _scenarioContext.Get<string>("Temperature")?.Trim();

                    bool isMatch =
                        (expectedTemp.Equals("Ambient", StringComparison.OrdinalIgnoreCase) && reviewValue == "Ambient") ||
                        (expectedTemp.Equals("Frozen", StringComparison.OrdinalIgnoreCase) && reviewValue == "Frozen") ||
                        (expectedTemp.Equals("Chilled", StringComparison.OrdinalIgnoreCase) && reviewValue == "Chilled");

                    if (!isMatch)
                    {
                        allDataMatches = false;
                        mismatches.Add($"{contextKey}: Expected '{expectedTemp}', Found '{reviewValue}'");
                    }
                    else
                    {
                        Console.WriteLine($"[REVIEW VALIDATION] ✓ {contextKey}: '{expectedTemp}' matches");
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
                            $"[REVIEW VALIDATION] ✓ {contextKey}: '{expectedFormatted}' matches"
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
                        Console.WriteLine($"[REVIEW VALIDATION] ✓ {contextKey}: '{expectedValue}' matches");
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
                            Console.WriteLine($"[REVIEW VALIDATION] ✓ {contextKey}: '{expectedValue}' matches");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[REVIEW VALIDATION] ⊘ {contextKey}: Skipped (empty value in array)");
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
                            Console.WriteLine($"[REVIEW VALIDATION] ✓ {contextKey}: '{expectedValue}' matches");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[REVIEW VALIDATION] ⊘ {contextKey}: Skipped (empty value in context)");
                    }
                }
                else
                {
                    Console.WriteLine($"[REVIEW VALIDATION] ⊘ {contextKey}: Skipped (unsupported type: {contextValue.GetType().Name})");
                }
            }
            else
            {
                Console.WriteLine($"[REVIEW VALIDATION] ⊘ {contextKey}: Skipped (not in context)");
            }
        }

        private void ValidateContains(string contextKey, string? actual, ref bool allDataMatches, List<string> mismatches, bool contextNotContainsPDF = false)
        {
            if (_scenarioContext.ContainsKey(contextKey))
            {
                //var expectedValue = _scenarioContext.Get<string>(contextKey);
                object rawExpected = _scenarioContext[contextKey];
                string expectedValue;

                if (rawExpected is string s)
                {
                    expectedValue = s.Trim();
                }
                else if (rawExpected is string[] arr)
                {
                    // join array into one string (adjust to comma if needed)
                    expectedValue = string.Join(" ", arr).Trim();
                }
                else
                {
                    mismatches.Add($"{contextKey}: Unsupported type '{rawExpected.GetType().Name}'");
                    return;
                }


                if (!string.IsNullOrEmpty(expectedValue) && !string.IsNullOrEmpty(actual))
                {

                    bool isMatch = contextNotContainsPDF
                        ? expectedValue.Contains(actual.Trim(), StringComparison.OrdinalIgnoreCase)
                        : actual.Trim().Contains(expectedValue, StringComparison.OrdinalIgnoreCase);

                    if (!isMatch)
                    {
                        allDataMatches = false;
                        mismatches.Add($"{contextKey}: Expected '{expectedValue}', Found '{actual.Trim()}'");
                    }
                    else
                    {
                        Console.WriteLine($"[REVIEW VALIDATION] ✓ {contextKey}: '{expectedValue}' matches");
                    }
                }
            }
        }
    }
}