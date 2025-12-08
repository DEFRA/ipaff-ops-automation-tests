using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Contracts;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ReviewYourNotificationPage : IReviewYourNotificationPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);

        // About the consignment
        private IWebElement importType => _driver.FindElement(By.Id("importing"));
        private IWebElement countryOfOrigin => _driver.FindElement(By.Id("country-of-origin"));
        private IWebElement mainReasonForImport => _driver.FindElement(By.Id("purpose-of-consignment-value"));
        private IWebElement purpose => _driver.FindElement(By.XPath("//dt[text()='Purpose in the internal market']//following-sibling::dd"));
        private IWebElement consignmentReferenceNumber => _driver.FindElement(By.XPath("//dt[text()='Consignment reference number']//following-sibling::dd"));

        // Commodity details
        private IWebElement commodityCode => _driver.FindElement(By.XPath("//td[text()='Commodity code']//following-sibling::td[1]"));
        private IWebElement species => _driver.FindElement(By.XPath("//table[@id='review-table-consignment-0']//tbody//tr[1]//td[1]"));
        private IWebElement numberOfAnimals => _driver.FindElement(By.XPath("//table[@id='review-consignment-total-table']//tbody//tr//td[1]"));
        private IWebElement numberOfPackages => _driver.FindElement(By.XPath("//table[@id='review-consignment-total-table']//tbody//tr//td[2]"));

        // Animal details
        private IWebElement certificationOption => _driver.FindElement(By.XPath("//td[text()='Certified for']//following-sibling::td"));

        // Documents
        private IWebElement healthCertificateReference => _driver.FindElement(By.Id("latest-health-document-reference"));
        private IWebElement healthCertificateDateOfIssue => _driver.FindElement(By.Id("latest-health-document-issue-date"));
        private IWebElement additionalDocumentType => _driver.FindElement(By.Id("veterinary-document-type-1"));
        private IWebElement additionalDocumentReference => _driver.FindElement(By.Id("veterinary-document-reference-1"));
        private IWebElement additionalDocumentDateOfIssue => _driver.FindElement(By.Id("veterinary-document-issue-date-1"));

        // Addresses
        private IWebElement consignorDetails => _driver.FindElement(By.Id("consignor"));
        private IWebElement consigneeDetails => _driver.FindElement(By.Id("consignee"));
        private IWebElement importerDetails => _driver.FindElement(By.Id("importer"));
        private IWebElement destinationDetails => _driver.FindElement(By.Id("final-destination"));

        // Transport details
        private IWebElement portOfEntry => _driver.FindElement(By.XPath("//th[text()='BCP or Port of entry']//following-sibling::td"));
        private IWebElement meansOfTransport => _driver.FindElement(By.XPath("//th[text()='Means of transport to BCP or Port of entry']//following-sibling::td"));
        private IWebElement transportId => _driver.FindElement(By.XPath("//th[text()='Transport identification']//following-sibling::td"));
        private IWebElement containerUsage => _driver.FindElement(By.Id("imported-in-containers"));
        private IWebElement transportDocumentReference => _driver.FindElement(By.XPath("//th[text()='Transport document reference']//following-sibling::td"));
        private IWebElement estimatedArrivalDate => _driver.FindElement(By.XPath("//th[text()='Estimated arrival date at BCP or Port of entry']//following-sibling::td"));
        private IWebElement estimatedArrivalTime => _driver.FindElement(By.XPath("//th[text()='Estimated arrival time at BCP']//following-sibling::td"));
        private IWebElement estimatedJourneyTime => _driver.FindElement(By.XPath("//th[text()='Estimated total journey time of the animals']//following-sibling::td"));
        private IWebElement gvmsUsage => _driver.FindElement(By.Id("goods-movement-services-route"));

        // Transporter details
        private IWebElement transporterName => _driver.FindElement(By.Id("transporter-name"));
        private IWebElement transporterAddress => _driver.FindElement(By.Id("transporter-address"));
        private IWebElement transporterCountry => _driver.FindElement(By.Id("transporter-country"));
        private IWebElement transporterApprovalNumber => _driver.FindElement(By.Id("transporter-approval-number"));
        private IWebElement transporterType => _driver.FindElement(By.Id("transporter-type"));

        // Route and contacts
        private IWebElement routeCountries => _driver.FindElement(By.XPath("//td[text()='Route']//following-sibling::td"));
        private IWebElement notifyTransportContacts => _driver.FindElement(By.Id("transporter-contact-yesnoindicator"));
        private IWebElement consignmentContactAddress => _driver.FindElement(By.Id("organisation-branch-address-address"));
        private IReadOnlyCollection<IWebElement> divAboutTheConsignmentDetails => _driver.WaitForElements(By.XPath("//div[@id='document-pet-card']//dl/div"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ReviewYourNotificationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Contains("Review your notification");
        }

        // About the consignment
        public string? GetImportType()
        {
            try { return importType.Text.Trim(); } catch { return null; }
        }

        public string? GetCountryOfOrigin()
        {
            try { return countryOfOrigin.Text.Trim(); } catch { return null; }
        }

        public string? GetMainReasonForImport()
        {
            try
            {
                var text = mainReasonForImport.Text.Trim();
                // Convert "For internal market" to "Internal market"
                if (text.StartsWith("For "))
                {
                    return text.Substring(4); // Remove "For " prefix
                }
                return text;
            }
            catch { return null; }
        }


        public string? GetPurpose()
        {
            try
            {
                // First, check what the main reason is to determine the purpose locator
                var mainReason = GetMainReasonForImport();

                if (mainReason?.Contains("internal market") == true)
                {
                    // For internal market, look for "Purpose in the internal market"
                    var internalMarketPurpose = _driver.FindElement(By.XPath("//dt[contains(text(),'Purpose in the internal market')]//following-sibling::dd"));
                    return internalMarketPurpose.Text.Trim();
                }                
                else
                {
                    // Try to find any purpose-related value after the main reason
                    var purposeElements = _driver.FindElements(By.XPath("//dt[contains(text(),'Purpose')]//following-sibling::dd"));
                    return purposeElements.FirstOrDefault()?.Text.Trim();
                }
            }
            catch
            {
                // If specific locators fail, try a more generic approach
                try
                {
                    // Look for the next review-summary-list__value after the main reason
                    var nextValue = _driver.FindElement(By.XPath("//dd[@id='purpose-of-consignment-value']//following::div[@class='review-summary-list__row'][1]//dd[@class='review-summary-list__value']"));
                    return nextValue?.Text.Trim();
                }
                catch
                {
                    return null;
                }
            }
        }


        public string? GetCommodityCode()
        {
            try
            {
                // Look for the commodity code value in the table cell next to "Commodity code" header
                var codeElement = commodityCode;
                return codeElement.Text.Trim();
            }
            catch { return null; }
        }

        public string? GetSpecies()
        {
            try { return species.Text.Trim(); } catch { return null; }
        }

        public string? GetNumberOfAnimals()
        {
            try { return numberOfAnimals.Text.Trim(); } catch { return null; }
        }

        public string? GetNumberOfPackages()
        {
            try { return numberOfPackages.Text.Trim(); } catch { return null; }
        }

        // Animal details
        public string? GetCertificationOption()
        {
            try { return certificationOption.Text.Trim(); } catch { return null; }
        }

        // Documents
        public string? GetHealthCertificateReference()
        {
            try { return healthCertificateReference.Text.Trim(); } catch { return null; }
        }

        public string? GetHealthCertificateDateOfIssue()
        {
            try
            {
                var text = healthCertificateDateOfIssue.Text.Trim();
                // Convert "1 December 2025" to "01 12 2025" format
                if (DateTime.TryParse(text, out DateTime date))
                {
                    return date.ToString("dd MM yyyy");
                }
                return text;
            }
            catch { return null; }
        }

        public string? GetAdditionalDocumentType()
        {
            try { return additionalDocumentType.Text.Trim(); } catch { return null; }
        }

        public string? GetAdditionalDocumentReference()
        {
            try { return additionalDocumentReference.Text.Trim(); } catch { return null; }
        }

        public string? GetAdditionalDocumentDateOfIssue()
        {
            try
            {
                var text = additionalDocumentDateOfIssue.Text.Trim();
                // Convert "24 November 2025" to "24 11 2025" format  
                if (DateTime.TryParse(text, out DateTime date))
                {
                    return date.ToString("dd MM yyyy");
                }
                return text;
            }
            catch { return null; }
        }

        // Addresses
        public string? GetConsignorName()
        {
            try
            {
                var fullText = consignorDetails.Text.Trim();
                return ExtractNameFromAddressText(fullText);
            }
            catch { return null; }
        }

        public string? GetConsignorAddress()
        {
            try
            {
                var fullText = consignorDetails.Text.Trim();
                return ExtractAddressFromAddressText(fullText);
            }
            catch { return null; }
        }

        public string? GetConsigneeName()
        {
            try
            {
                var fullText = consigneeDetails.Text.Trim();
                return ExtractNameFromAddressText(fullText);
            }
            catch { return null; }
        }

        public string? GetConsigneeAddress()
        {
            try
            {
                var fullText = consigneeDetails.Text.Trim();
                return ExtractAddressFromAddressText(fullText);
            }
            catch { return null; }
        }

        public string? GetImporterName()
        {
            try
            {
                var fullText = importerDetails.Text.Trim();
                return ExtractNameFromAddressText(fullText);
            }
            catch { return null; }
        }

        public string? GetImporterAddress()
        {
            try
            {
                var fullText = importerDetails.Text.Trim();
                return ExtractAddressFromAddressText(fullText);
            }
            catch { return null; }
        }

        public string? GetDestinationName()
        {
            try
            {
                var fullText = destinationDetails.Text.Trim();
                return ExtractNameFromAddressText(fullText);
            }
            catch { return null; }
        }

        public string? GetDestinationAddress()
        {
            try
            {
                var fullText = destinationDetails.Text.Trim();
                return ExtractAddressFromAddressText(fullText);
            }
            catch { return null; }
        }

        // Transport details
        public string? GetPortOfEntry()
        {
            try { return portOfEntry.Text.Trim(); } catch { return null; }
        }

        public string? GetMeansOfTransport()
        {
            try { return meansOfTransport.Text.Trim(); } catch { return null; }
        }

        public string? GetTransportId()
        {
            try { return transportId.Text.Trim(); } catch { return null; }
        }

        public string? GetTransportDocumentReference()
        {
            try { return transportDocumentReference.Text.Trim(); } catch { return null; }
        }

        public string? GetEstimatedArrivalDate()
        {
            try
            {
                var text = estimatedArrivalDate.Text.Trim();
                // Convert "4 December 2025" to "04 Dec 2025" format
                if (DateTime.TryParse(text, out DateTime date))
                {
                    return date.ToString("dd MMM yyyy");
                }
                return text;
            }
            catch { return null; }
        }

        public string? GetEstimatedArrivalTime()
        {
            try { return estimatedArrivalTime.Text.Trim(); } catch { return null; }
        }

        public string? GetEstimatedJourneyTime()
        {
            try
            {
                var text = estimatedJourneyTime.Text.Trim();
                // Extract just the number from "8 hrs" to return "8"
                return text.Replace(" hrs", "").Trim();
            }
            catch { return null; }
        }

        public string? GetGVMSUsage()
        {
            try { return gvmsUsage.Text.Trim(); } catch { return null; }
        }

        // Transporter details
        public string? GetTransporterName()
        {
            try { return transporterName.Text.Trim(); } catch { return null; }
        }

        public string? GetTransporterAddress()
        {
            try { return transporterAddress.Text.Trim(); } catch { return null; }
        }

        public string? GetTransporterCountry()
        {
            try { return transporterCountry.Text.Trim(); } catch { return null; }
        }

        public string? GetTransporterApprovalNumber()
        {
            try { return transporterApprovalNumber.Text.Trim(); } catch { return null; }
        }

        public string? GetTransporterType()
        {
            try { return transporterType.Text.Trim(); } catch { return null; }
        }

        // Route and contacts
        public string? GetRouteCountries()
        {
            try { return routeCountries.Text.Trim(); } catch { return null; }
        }

        public string? GetNotifyTransportContacts()
        {
            try { return notifyTransportContacts.Text.Trim(); } catch { return null; }
        }

        public string? GetConsignmentContactAddress()
        {
            try
            {
                var text = consignmentContactAddress.Text.Trim();
                // Convert comma-separated to multi-line format to match expected
                return text.Replace(", ", "\n");
            }
            catch { return null; }
        }

        // Helper methods to extract name and address from combined text
        private string ExtractNameFromAddressText(string fullText)
        {
            var lines = fullText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            return lines.Length > 0 ? lines[0].Trim() : "";
        }

        // Update address extraction methods to only return the street address part:
        private string ExtractAddressFromAddressText(string fullText)
        {
            var lines = fullText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length > 1)
            {
                var addressLine = lines[1].Trim();
                // Extract only the street address portion before additional details
                var parts = addressLine.Split(',');
                if (parts.Length >= 4)
                {
                    // Return only the first 4 parts: street, city, region, postcode
                    return string.Join(", ", parts.Take(4).Select(p => p.Trim()));
                }
                return addressLine;
            }
            return "";
        }

        public string? GetConsignmentReferenceNumber()
        {
            try { return consignmentReferenceNumber.Text.Trim(); } catch { return null; }
        }

        public string? GetContainerUsage()
        {
            try { return containerUsage.Text.Trim(); } catch { return null; }
        }
    }
}