using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ReviewYourNotificationPage : IReviewYourNotificationPage
    {
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private By CopyAsNewButtonBy => By.XPath("//button[@type='submit' and @value='Copy as new']");
        private By ViewCHEDButtonBy => By.Id("show-notification");
        private By ChangeLinksBy => By.XPath("//a[text()='Change']");
        private IWebElement lnkChange(string section) => _driver.FindElement(By.XPath($"(//*[normalize-space()='{section}']/following::a)[1]"));
        private By DashboardLinkBy => By.XPath("//a[@class='govuk-breadcrumbs__link' and @href='/notification/pre/protected/notifications']");
        private By chedReferenceBy => By.Id("reference-number");

        // About the consignment
        private By importTypeBy => By.Id("importing");
        private By countryOfOriginBy => By.Id("country-of-origin");
        private By countryFromWhereConsignedBy => By.XPath("//dt[text()='Country from where consigned']//following-sibling::dd");
        private By mainReasonForImportBy => By.Id("purpose-of-consignment-value");
        private By pointOfExitBy => By.XPath("//dt[@id='point-of-exit-header']/following-sibling::dd");
        private By consignmentDepartureDateTimeBy => By.Id("estimated-departure-date-time-value");
        private By consignmentReferenceNumberBy => By.XPath("//dt[text()='Consignment reference number' or text()='Add a reference number for this consignment']//following-sibling::dd[1]");
        private By exitDateBy => By.Id("exit-date-value");
        private By exitBCPTemporaryAdmissionLocator => By.Id("designated-bip-horses-value");
        private By exitBCPTransitLocator => By.XPath("//dt[@id='exit-border-control-post-header']/following-sibling::dd");
        private By destinationCountryBy => By.XPath("//dt[@id='destination-country-header']/following-sibling::dd");

        // Commodity details
        private By commodityCodeBy => By.XPath("//td[text()='Commodity code']//following-sibling::td[1]");
        private By speciesBy => By.XPath("//table[@id='review-table-consignment-0']//tbody//tr[1]//td[1]");
        private By numberOfAnimalsBy => By.XPath("//table[@id='review-consignment-total-table']//tbody//tr//td[1]");
        private By numberOfPackagesBy => By.XPath("//table[@id='review-consignment-total-table']//tbody//tr//td[2]");

        private List<IWebElement> commodityCodeList => _driver.WaitForElements(By.XPath("//table[contains(@id,'review-table-commodity-attributes')]//td[text()='Commodity code']//following-sibling::td[1]")).ToList();
        private List<IWebElement> netWeightList => _driver.WaitForElements(By.XPath("//table[contains(@class,'data-table-commodities')]//tr/td[1]")).ToList();
        private List<IWebElement> numPackagesList => _driver.WaitForElements(By.XPath("//table[contains(@class,'data-table-commodities')]//tr/td[2]")).ToList();
        private List<IWebElement> typeOfPackagesList => _driver.WaitForElements(By.XPath("//table[contains(@class,'data-table-commodities')]//tr/td[3]")).ToList();
        private By totalNetWeightBy => By.XPath("//td[text()='Total net weight']//following-sibling::td[1]");
        private By totalPackagesBy => By.XPath("//td[text()='Total packages']//following-sibling::td[1]");
        private By totalGrossWeightBy => By.XPath("//td[contains(text(),'Total gross weight ')]//following-sibling::td[1]");
        private By confirmationToDeclareGMSBy => By.XPath("//td[contains(text(),'Confirmation to declare GMS')]//following-sibling::td[1]");
        private List<IWebElement> genusAndSpeciesList => _driver.FindElements(By.XPath("//th[text()='Genus and species']/following-sibling::td")).ToList();
        private List<IWebElement> descriptionList => _driver.FindElements(By.XPath("//td[text()='Description']/following-sibling::td[1]")).ToList();
        private List<IWebElement> netWeightCHEDPPList => _driver.FindElements(By.XPath("//*[contains(@class,'govuk-table chedpp-species-table')]//tr[2]/td[3]")).ToList();
        private List<IWebElement> numPackagesCHEDPPList => _driver.FindElements(By.XPath("//*[contains(@class, 'govuk-table chedpp-species-table')]//tr[2]/td[4]")).ToList();
        private List<IWebElement> typeOfPackagesCHEDPPList => _driver.FindElements(By.XPath("//*[contains(@class, 'govuk-table chedpp-species-table')]//tr[2]/td[5]")).ToList();
        private List<IWebElement> varietyBy => _driver.FindElements(By.XPath("//*[contains(@class,'govuk-table chedpp-species-table')]//tr[2]/td[1]")).ToList();
        private List<IWebElement> classBy => _driver.FindElements(By.XPath("//*[contains(@class,'govuk-table chedpp-species-table')]//tr[2]/td[2]")).ToList();
        private By forTestAndTrial => By.XPath(".//*[normalize-space()='For test and trial']");
        private IWebElement firstCommodityTable => _driver.FindElement(By.XPath("//*[@id='page-primary-title']/following-sibling::div[4]"));
        private IWebElement secondCommodityTable => _driver.FindElement(By.XPath("//*[@id='page-primary-title']/following-sibling::div[5]"));

        // Multi-species commodity rows
        private IReadOnlyCollection<IWebElement> speciesConsignmentRows => _driver.FindElements(By.XPath("//table[contains(@id,'review-table-consignment')]//tbody//tr | //table[contains(@id,'review-table-consignment')]//tr[td[@class='govuk-table__cell']]"));

        // Animal identification details — per-species sub-tables inside the main identification table
        private IReadOnlyCollection<IWebElement> identificationSpeciesHeaders => _driver.FindElements(By.XPath("//table[@id='animal-identification-details-table']//th[contains(text(),'identification details')]"));
        private IReadOnlyCollection<IWebElement> identificationSubTables => _driver.FindElements(By.XPath("//table[@id='animal-identification-details-table']//div[contains(@id,'identifiers-')]//table"));

        // Permanent address rows
        private IReadOnlyCollection<IWebElement> permanentAddressRows => _driver.FindElements(By.XPath("//td[@id='animal-name']/parent::tr"));

        //Additional details
        private By commodityIntendedForBy => By.XPath("//dt[text()='Commodity intended for']//following-sibling::dd");
        private By temperatureBy => By.XPath("//dt[text()='Temperature']//following-sibling::dd");

        // Animal details
        private By certificationOptionBy => By.XPath("//td[text()='Certified for']//following-sibling::td");
        private By GetHorseNameBy(int index) => By.XPath($"//table[@id='animal-identification-details-table']//tbody//tr[{index + 1}]//td[@headers='horseName-01']");
        private By GetMicrochipBy(int index) => By.XPath($"//table[@id='animal-identification-details-table']//tbody//tr[{index + 1}]//td[@headers='microchip-01']");
        private By GetPassportBy(int index) => By.XPath($"//table[@id='animal-identification-details-table']//tbody//tr[{index + 1}]//td[@headers='passport-01']");
        private By GetEarTagBy(int index) => By.XPath($"//table[contains(@aria-describedby,'animalProduct')]//tbody//tr[{index + 1}]//td[@headers='earTag-01']");
        private By unweanedAnimalsOptionBy => By.XPath("//td[contains(text(),'Includes unweaned animals')]/following-sibling::td");

        // Documents
        private By healthCertificateReferenceBy => By.Id("latest-health-document-reference");
        private By healthCertificateDateOfIssueBy => By.Id("latest-health-document-issue-date");
        private By healthCertificateFileNameBy => By.XPath("//table[@id='latest-health-certificate-table']//a[contains(@id,'attachment-view')]");
        private By additionalDocumentTypeBy => By.Id("veterinary-document-type-1");
        private By additionalDocumentReferenceBy => By.Id("veterinary-document-reference-1");
        private By additionalDocumentDateOfIssueBy => By.Id("veterinary-document-issue-date-1");
        private By additionalDocumentFileNameBy => By.XPath("//table[@id='additional-documents-table']//a[contains(@id,'attachment-view')]");
        private By catchCertificateHeadingBy => By.Id("catch-certificate-details-heading");
        private By catchCertificateSummaryTableBy => By.Id("catch-certificate-summary-table");
        private By catchCertificateSummaryRowsBy => By.XPath("//table[@id='catch-certificate-summary-table']//tbody//tr");

        // Addresses
        private By consignorDetailsBy => By.Id("consignor");
        private By consigneeDetailsBy => By.Id("consignee");
        private By importerDetailsBy => By.Id("importer");
        private By destinationDetailsBy => By.Id("final-destination");

        // Transport details
        private By portOfEntryBy => By.XPath("//th[contains(text(),'Port of entry') or contains(text(),'Border Control Post')]//following-sibling::td");
        private By inspectionPremisesBy => By.XPath("//th[text()='Inspection Premises']//following-sibling::td");
        private By meansOfTransportBy => By.XPath("//th[contains(text(),'Means of transport')]//following-sibling::td");
        private By transportIdBy => By.XPath("//th[text()='Transport identification']//following-sibling::td");
        private By containerUsageBy => By.Id("imported-in-containers");
        private By transportDocumentReferenceBy => By.XPath("//th[text()='Transport document reference']//following-sibling::td");
        private By estimatedArrivalDateBy => By.XPath("//th[contains(text(),'Estimated arrival date at BCP')]//following-sibling::td");
        private By estimatedArrivalTimeBy => By.XPath("//th[contains(text(),'Estimated arrival time at')]//following-sibling::td");
        private By estimatedJourneyTimeBy => By.XPath("//th[text()='Estimated total journey time of the animals']//following-sibling::td");
        private By ctcUsageBy => By.XPath("//td[contains(text(),'Using the Common Transit Convention')]/following-sibling::td");
        private By movementReferenceNumberBy => By.Id("goods-movement-services-mrn");
        private By gvmsUsageBy => By.Id("goods-movement-services-route");
        private By GetContainerNumberBy(int index) => By.Id($"container-number-{index}");
        private By GetSealNumberBy(int index) => By.Id($"seal-number-{index}");
        private By GetOfficialSealBy(int index) => By.Id($"official-seal-present-{index}");
        private By meansOfTransportAfterBCPBy => By.XPath("//th[contains(text(),'Means of transport after BCP or Port of entry')]//following-sibling::td");
        private By transportIdentificationAfterBCPBy => By.XPath("//table[@id='review-table-transport']//th[text()='Transport identification']//following-sibling::td");
        private By transportDocumentReferenceAfterBCPBy => By.XPath("//table[@id='review-table-transport']//th[text()='Transport document reference']//following-sibling::td");
        private By departureDateFromBCPBy => By.XPath("//th[text()='Departure date from port of entry']//following-sibling::td");
        private By departureTimeFromBCPBy => By.XPath("//th[text()='Departure time from port of entry']//following-sibling::td");

        // Transporter details
        private By transporterNameBy => By.Id("transporter-name");
        private By transporterAddressBy => By.Id("transporter-address");
        private By transporterCountryBy => By.Id("transporter-country");
        private By transporterApprovalNumberBy => By.Id("transporter-approval-number");
        private By transporterTypeBy => By.Id("transporter-type");

        // Route and contacts
        private By routeCountriesBy => By.XPath("//td[text()='Route']//following-sibling::td");
        private By notifyTransportContactsBy => By.Id("transporter-contact-yesnoindicator");
        private By consignmentContactAddressBy => By.Id("organisation-branch-address-address");

        //Error Message
        private IReadOnlyCollection<IWebElement> lblErrorMessages => _driver.FindElements(By.XPath("//div[@class='govuk-error-summary']/div/ul/li"));
        private IWebElement lblCatchCertificateHeader => _driver.FindElement(By.Id("catch-certificate-details-heading"));
        private IWebElement lblCatchCertificateRowNoneAttached => _driver.FindElement(By.Id("catch-certificates-none-attached"));
        private IWebElement catchCertificateFlagState(int row, int column) => _driver.FindElement(By.XPath($"//table[@id='catch-certificate-summary-table']//tr[{row}]/td[{column}]"));
        private IWebElement catchCertificateDocumentReference(int row, int column) => _driver.FindElement(By.XPath($"//table[@id='catch-certificate-summary-table']//tr[{row}]/td[{column}]"));
        private IWebElement catchCertificateDocumentDateOfIssue(int row, int column) => _driver.FindElement(By.XPath($"//table[@id='catch-certificate-summary-table']//tr[{row}]/td[{column}]"));
        private IWebElement catchCertificateCommodityCode(int row, int column) => _driver.FindElement(By.XPath($"(//table[@id='catch-certificate-details-table'])[1]//tbody/tr[{row}]/td[{column}]"));
        private IWebElement catchCertificateSpeciesDescription(int row, int column) => _driver.FindElement(By.XPath($"(//table[@id='catch-certificate-details-table'])[1]//tbody/tr[{row}]/td[{column}]"));
        private IReadOnlyCollection<IWebElement> lnkChangeCatchCertificateLinks => _driver.FindElements(By.Id("add-catch-certificate-details-change-link"));
        private IReadOnlyCollection<IWebElement> lnkChangeCatchCertificateLinks => _driver.FindElements(By.Id("add-catch-certificate-details-change-link"));
        private IWebElement lnkChangeLinkForTransportBCP => _driver.FindElement(By.Id("transport-to-bip-change-link"));
        private IWebElement lnkChangeLinkForContactDetailsChange => _driver.FindElement(By.Id("responsible-person-contact-details-change-link"));
        private IWebElement lnkGoodsMovementServicesChange => _driver.FindElement(By.Id("goods-movement-services-change-link"));
        private IWebElement lnkTraderDeliveryAddressChange => _driver.FindElement(By.Id("traders-change-link"));
        private IWebElement pImporterName => _driver.FindElement(By.XPath("(//dd[@id='importer']/p)[1]"));
        private IWebElement btnViewChed => _driver.FindElement(By.Id("show-notification"));
        private IWebElement ddContactName => _driver.FindElement(By.Id("responsible-contact-name"));
        private IWebElement ddContactEmail => _driver.FindElement(By.Id("responsible-contact-email"));
        private IWebElement ddContactTelephone => _driver.FindElement(By.Id("responsible-contact-telephone"));
        private IWebElement tdIntendedForUsers => _driver.FindElement(By.XPath("//th[normalize-space()='Intended for final users or commercial flower production'] /parent::tr/following-sibling::tr[1]/td[2]"));
        private IWebElement tdControlledAtmosphereContainer => _driver.FindElement(By.XPath("//th[normalize-space()='Controlled atmosphere container']/parent::tr/following-sibling::tr[1]/td[6]"));
        private IWebElement tdQuantity => _driver.FindElement(By.XPath("//th[normalize-space()='Quantity']/parent::tr/following-sibling::tr[1]/td[3]"));
        private IWebElement tdQuantityType => _driver.FindElement(By.XPath("//th[normalize-space()='Quantity type']/parent::tr/following-sibling::tr[1]/td[4]"));
        private IWebElement tdGrossVolume => _driver.FindElement(By.XPath("//td[normalize-space()='Total gross volume']/following-sibling::td"));
        private IWebElement tdGrossVolumeUnit => _driver.FindElement(By.XPath("//td[normalize-space()='Total gross volume unit']/following-sibling::td"));
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

        // About the consignment - Simple getters using SafelyGetText
        public string GetImportType() => _driver.SafelyGetText(importTypeBy);
        public string GetCountryOfOrigin() => _driver.SafelyGetText(countryOfOriginBy);
        public string GetCountryFromWhereConsigned() => _driver.SafelyGetText(countryFromWhereConsignedBy);
        public string GetConsignmentReferenceNumber() => _driver.SafelyGetText(consignmentReferenceNumberBy);
        public string GetDestinationCountry() => _driver.SafelyGetText(destinationCountryBy);


        // Complex methods with logic - Keep try-catch
        public string GetPartOfImportType()
        {
            try
            {
                var importTypeText = _driver.SafelyGetText(importTypeBy);
                if (string.IsNullOrEmpty(importTypeText))
                    return string.Empty;

                var words = importTypeText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                return string.Join(" ", words.Take(3));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetPartOfImportType failed: {ex}");
                return string.Empty;
            }
        }

        public string GetMainReasonForImport()
        {
            try
            {
                var text = _driver.SafelyGetText(mainReasonForImportBy);
                if (string.IsNullOrEmpty(text))
                    return string.Empty;

                // Convert "For internal market" to "Internal market"
                if (text.StartsWith("For "))
                {
                    return text.Substring(4); // Remove "For " prefix
                }
                return text;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetPurpose()
        {
            try
            {
                // First, check what the main reason is to determine the purpose locator
                var mainReason = GetMainReasonForImport();

                if (mainReason?.Contains("internal market") == true)
                {
                    // For internal market, look for "Purpose in the internal market"
                    return _driver.SafelyGetText(By.XPath("//dt[contains(text(),'Purpose in the internal market')]//following-sibling::dd"));
                }
                else
                {
                    // Try to find any purpose-related value after the main reason
                    var purposeElements = _driver.FindElements(By.XPath("//dt[contains(text(),'Purpose') or contains(text(),'purpose') ]/following-sibling::dd"));
                    return purposeElements.FirstOrDefault()?.SafelyGetText() ?? string.Empty;
                }
            }
            catch
            {
                // If specific locators fail, try a more generic approach
                try
                {
                    // Look for the next review-summary-list__value after the main reason
                    return _driver.SafelyGetText(By.XPath("//dd[@id='purpose-of-consignment-value']//following::div[@class='review-summary-list__row'][1]//dd[@class='review-summary-list__value']"));
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public string GetPointOfExit => _driver.SafelyGetText(pointOfExitBy);

        public (string departureDate, string departureTime) GetConsignmentDepartureDateTime()
        {
            try
            {
                var departureDateTime = _driver.SafelyGetText(consignmentDepartureDateTimeBy);
                if (string.IsNullOrEmpty(departureDateTime))
                    return (string.Empty, string.Empty);

                var dateTimeParts = departureDateTime.Split(',', StringSplitOptions.TrimEntries);
                return (DateTime.Parse(dateTimeParts[0]).ToString("dd MMMM yyyy"), dateTimeParts.Length > 1 ? dateTimeParts[1] : string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while getting date and time: {ex.Message}");
                return (string.Empty, string.Empty);
            }
        }

        // Commodity details - Simple getters
        public string GetCommodityCode() => _driver.SafelyGetText(commodityCodeBy);
        public string GetSpecies() => _driver.SafelyGetText(speciesBy);
        public string GetNumberOfAnimals() => _driver.SafelyGetText(numberOfAnimalsBy);
        public string GetNumberOfPackages() => _driver.SafelyGetText(numberOfPackagesBy);

        /// <summary>
        /// Returns all species rows from the review-table-consignment tables.
        /// Each tuple contains (species name, number of animals, number of packages).
        /// HTML structure: each species row has 3 td cells in tbody rows of tables
        /// with id containing 'review-table-consignment'.
        /// </summary>
        public List<(string species, string numberOfAnimals, string numberOfPackages)> GetAllSpeciesDetails()
        {
            var results = new List<(string species, string numberOfAnimals, string numberOfPackages)>();

            foreach (var row in speciesConsignmentRows)
            {
                var cells = row.FindElements(By.TagName("td"));
                if (cells.Count >= 3)
                {
                    var speciesName = cells[0].SafelyGetText();
                    var animals = cells[1].SafelyGetText();
                    var packages = cells[2].SafelyGetText();

                    if (!string.IsNullOrWhiteSpace(speciesName) && !speciesName.Equals("Subtotal", StringComparison.OrdinalIgnoreCase))
                        results.Add((speciesName, animals, packages));
                }
            }

            return results;
        }

        /// <summary>
        /// Returns identification details for a specific species from the review page.
        /// The HTML has a header row with text like "Canis familiaris identification details"
        /// followed by a sub-table with columns: Animal, Microchip, Passport, Tattoo.
        /// </summary>
        public List<(string animal, string microchip, string passport, string tattoo)> GetIdentificationDetailsForSpecies(string species)
        {
            var results = new List<(string animal, string microchip, string passport, string tattoo)>();

            var headers = identificationSpeciesHeaders.ToList();
            var tables = identificationSubTables.ToList();

            for (int i = 0; i < headers.Count && i < tables.Count; i++)
            {
                if (!headers[i].Text.Contains(species, StringComparison.OrdinalIgnoreCase))
                    continue;

                var bodyRows = tables[i].FindElements(By.XPath(".//tbody/tr"));
                foreach (var row in bodyRows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count >= 4)
                    {
                        results.Add((
                            cells[0].SafelyGetText(),
                            cells[1].SafelyGetText(),
                            cells[2].SafelyGetText(),
                            cells[3].SafelyGetText()
                        ));
                    }
                }

                break;
            }

            return results;
        }

        /// <summary>
        /// Returns all permanent address rows from the review page.
        /// Each row has td[@id='animal-name'] and td[@id='animal-permanent-address'].
        /// Returns tuples of (animalName, full address text).
        /// </summary>
        public List<(string animalName, string addressText)> GetAllPermanentAddresses()
        {
            var results = new List<(string animalName, string addressText)>();

            foreach (var row in permanentAddressRows)
            {
                var name = row.FindElement(By.XPath(".//td[@id='animal-name']")).SafelyGetText();
                var address = row.FindElement(By.XPath(".//td[@id='animal-permanent-address']")).SafelyGetText();

                if (!string.IsNullOrWhiteSpace(name))
                    results.Add((name, address));
            }

            return results;
        }

        public string GetCommodityCodeList(int index)
        {
            try
            {
                var allcommodityCodesList = GetItemsList(commodityCodeList);

                if (allcommodityCodesList == null || allcommodityCodesList.Count == 0)
                    return string.Empty;

                var commCode = allcommodityCodesList[index];
                return string.IsNullOrWhiteSpace(commCode) ? string.Empty : commCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetCommodityCodeList failed: {ex}");
                return string.Empty;
            }
        }

        public string GetDescriptionList(int index)
        {
            try
            {
                var allcommodityDescList = GetItemsList(descriptionList);

                if (allcommodityDescList == null || allcommodityDescList.Count == 0)
                    return string.Empty;

                var commCode = allcommodityDescList[index];
                return string.IsNullOrWhiteSpace(commCode) ? string.Empty : commCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetCommodityDescList failed: {ex}");
                return string.Empty;
            }
        }

        public string GetGenusListCHEDPP(int index)
        {
            try
            {
                var allGenusAndSpeciesList = GetItemsList(genusAndSpeciesList);

                if (allGenusAndSpeciesList == null || allGenusAndSpeciesList.Count == 0)
                    return string.Empty;

                var commGenusAndEPPOCode = allGenusAndSpeciesList[index];
                var commGenus = commGenusAndEPPOCode.Split(',')[0].Trim();
                return string.IsNullOrWhiteSpace(commGenus) ? string.Empty : commGenus;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetGenusListCHEDPP failed: {ex}");
                return string.Empty;
            }
        }

        public string GetEPPOCodeListCHEDPP(int index)
        {
            try
            {
                var allGenusAndSpeciesList = GetItemsList(genusAndSpeciesList);

                if (allGenusAndSpeciesList == null || allGenusAndSpeciesList.Count == 0)
                    return string.Empty;

                var commGenusAndEPPOCode = allGenusAndSpeciesList[index];
                var commEPPOCode = commGenusAndEPPOCode.Split(',')[1].Trim();
                return string.IsNullOrWhiteSpace(commEPPOCode) ? string.Empty : commEPPOCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetEPPOCodeListCHEDPP failed: {ex}");
                return string.Empty;
            }
        }

        public string GetNetWeightList(int index)
        {
            try
            {
                var allNetWeightList = GetItemsList(netWeightList);

                if (allNetWeightList == null || allNetWeightList.Count == 0)
                    return string.Empty;

                var netWeight = allNetWeightList[index];
                netWeight = netWeight.Replace("kg/units", "").Trim();
                return string.IsNullOrWhiteSpace(netWeight) ? string.Empty : netWeight;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetNetWeightList failed: {ex}");
                return string.Empty;
            }
        }

        public string GetNumPackagesList(int index)
        {
            try
            {
                var allNumPackagesList = GetItemsList(numPackagesList);

                if (allNumPackagesList == null || allNumPackagesList.Count == 0)
                    return string.Empty;

                var numOfPackage = allNumPackagesList[index];
                return string.IsNullOrWhiteSpace(numOfPackage) ? string.Empty : numOfPackage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetNumPackagesList failed: {ex}");
                return string.Empty;
            }
        }

        public string GetTypeOfPackagesList(int index)
        {
            try
            {
                var allTypeOfPackagesList = GetItemsList(typeOfPackagesList);

                if (allTypeOfPackagesList == null || allTypeOfPackagesList.Count == 0)
                    return string.Empty;

                var typeOfPackage = allTypeOfPackagesList[index];
                return string.IsNullOrWhiteSpace(typeOfPackage) ? string.Empty : typeOfPackage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTypeOfPackagesList failed: {ex}");
                return string.Empty;
            }
        }

        public string GetNetWeightListCHEDPP(int index)
        {
            try
            {
                var allNetWeightList = GetItemsList(netWeightCHEDPPList);

                if (allNetWeightList == null || allNetWeightList.Count == 0)
                    return string.Empty;

                var netWeight = allNetWeightList[index];
                netWeight = netWeight.Replace("kg", "").Trim();
                return string.IsNullOrWhiteSpace(netWeight) ? string.Empty : netWeight;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetNetWeightList failed: {ex}");
                return string.Empty;
            }
        }

        public string GetNumPackagesListCHEDPP(int index)
        {
            try
            {
                var allNumPackagesList = GetItemsList(numPackagesCHEDPPList);

                if (allNumPackagesList == null || allNumPackagesList.Count == 0)
                    return string.Empty;

                var numOfPackage = allNumPackagesList[index];
                return string.IsNullOrWhiteSpace(numOfPackage) ? string.Empty : numOfPackage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetNumPackagesList failed: {ex}");
                return string.Empty;
            }
        }

        public string GetTypeOfPackagesListCHEDPP(int index)
        {
            try
            {
                var allTypeOfPackagesList = GetItemsList(typeOfPackagesCHEDPPList);

                if (allTypeOfPackagesList == null || allTypeOfPackagesList.Count == 0)
                    return string.Empty;

                var typeOfPackage = allTypeOfPackagesList[index];
                return string.IsNullOrWhiteSpace(typeOfPackage) ? string.Empty : typeOfPackage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTypeOfPackagesList failed: {ex}");
                return string.Empty;
            }
        }

        private List<string>? GetItemsList(List<IWebElement> inputList)
        {
            List<string> itemsList = new List<string>();
            foreach (var item in inputList)
            {
                itemsList.Add(item.SafelyGetText());
            }
            return itemsList;
        }

        public string GetTotalNetWeight()
        {
            try
            {
                var text = _driver.SafelyGetText(totalNetWeightBy);
                return text.Replace("kg/units", "").Replace("kg", "").Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTotalNetWeight failed: {ex.Message}");
                return string.Empty;
            }
        }

        public string GetTotalPackages()
        {
            try
            {
                return _driver.SafelyGetText(totalPackagesBy);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTotalPackages failed: {ex.Message}");
                return string.Empty;
            }
        }

        public string GetTotalGrossWeight()
        {
            try
            {
                var text = _driver.SafelyGetText(totalGrossWeightBy);
                return text.Replace("kg/units", "").Replace("kg", "").Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTotalGrossWeight failed: {ex.Message}");
                return string.Empty;
            }
        }

        public string GetConfirmationToDeclareGMS()
        {
            try
            {
                return _driver.SafelyGetText(confirmationToDeclareGMSBy);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetConfirmationToDeclareGMS failed: {ex.Message}");
                return string.Empty;
            }
        }

        public string GetCommodityVariety(int index)
        {
            try
            {
                var allVarietyList = GetItemsList(varietyBy);

                if (allVarietyList == null || allVarietyList.Count == 0)
                    return string.Empty;

                var varietyOfCommodity = allVarietyList[index];
                return string.IsNullOrWhiteSpace(varietyOfCommodity) ? string.Empty : varietyOfCommodity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetCommodityVariety failed: {ex}");
                return string.Empty;
            }
        }

        public string GetCommodityClass(int index)
        {
            try
            {
                var allClassList = GetItemsList(classBy);

                if (allClassList == null || allClassList.Count == 0)
                    return string.Empty;

                var classOfCommodity = allClassList[index];
                return string.IsNullOrWhiteSpace(classOfCommodity) ? string.Empty : classOfCommodity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetCommodityClass failed: {ex}");
                return string.Empty;
            }
        }

        public string GetQuantityListCHEDPP(int index)
        {
            try
            {
                var commodityTable = index == 0 ? firstCommodityTable : secondCommodityTable;

                int quantityIndex = commodityTable.FindElements(forTestAndTrial).Count > 0 ? 3 : 2;

                var quantityList = commodityTable.FindElements(By.XPath(".//tbody/tr[4]/td[" + quantityIndex + "]")).ToList();

                var allQuantityList = quantityList
                                      .Select(e => e.Text?.Trim())
                                      .Where(t => !string.IsNullOrEmpty(t))
                                      .ToList();

                if (allQuantityList.Count == 0)
                    return string.Empty;

                var commQuantity = allQuantityList.FirstOrDefault();
                return string.IsNullOrWhiteSpace(commQuantity) ? string.Empty : commQuantity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetQuantityListCHEDPP failed: {ex}");
                return string.Empty;
            }
        }

        public string GetQuantityTypeListCHEDPP(int index)
        {
            try
            {

                var commodityTable = index == 0 ? firstCommodityTable : secondCommodityTable;

                int quantityTypeIndex = commodityTable.FindElements(forTestAndTrial).Count > 0 ? 4 : 3;

                var quantityTypeList = commodityTable.FindElements(By.XPath(".//tbody/tr[4]/td[" + quantityTypeIndex + "]")).ToList();

                var allQuantityTypeList = quantityTypeList
                      .Select(e => e.Text?.Trim())
                      .Where(t => !string.IsNullOrEmpty(t))
                      .ToList();

                if (allQuantityTypeList.Count == 0)
                    return string.Empty;

                var commQuantityType = allQuantityTypeList.FirstOrDefault();
                return string.IsNullOrWhiteSpace(commQuantityType) ? string.Empty : commQuantityType;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetQuantityTypeListCHEDPP failed: {ex}");
                return string.Empty;
            }
        }

        public string GetExitDate()
        {
            try
            {
                var text = _driver.SafelyGetText(exitDateBy);
                if (string.IsNullOrEmpty(text))
                    return string.Empty;

                // Convert "14 January 2026" to "14 January 2026" format (matches stored format)
                if (DateTime.TryParse(text, out DateTime date))
                {
                    return date.ToString("dd MMMM yyyy");
                }
                return text;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetExitBCP()
        {
            try
            {
                // First, check what the main reason for import is
                var mainReason = GetMainReasonForImport();

                if (mainReason?.Contains("Transit", StringComparison.OrdinalIgnoreCase) == true)
                {
                    // For Transit, use the exit-border-control-post-header locator
                    return _driver.SafelyGetText(exitBCPTransitLocator);
                }
                else if (mainReason?.Contains("Temporary admission", StringComparison.OrdinalIgnoreCase) == true)
                {
                    // For Temporary Admission Horses, use the designated-bip-horses-value locator
                    return _driver.SafelyGetText(exitBCPTemporaryAdmissionLocator);
                }
                else
                {
                    // Fallback: Try both locators
                    var transitExitBCP = _driver.SafelyGetText(exitBCPTransitLocator);
                    if (!string.IsNullOrEmpty(transitExitBCP))
                        return transitExitBCP;

                    var tempAdmissionExitBCP = _driver.SafelyGetText(exitBCPTemporaryAdmissionLocator);
                    if (!string.IsNullOrEmpty(tempAdmissionExitBCP))
                        return tempAdmissionExitBCP;

                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetExitBCP failed: {ex.Message}");
                return string.Empty;
            }
        }

        // Animal details - Simple getters
        public string GetCertificationOption() => _driver.SafelyGetText(certificationOptionBy);
        public string GetHorseName(int index = 0) => _driver.SafelyGetText(GetHorseNameBy(index));
        public string GetMicrochipNumber(int index = 0) => _driver.SafelyGetText(GetMicrochipBy(index));
        public string GetPassportNumber(int index = 0) => _driver.SafelyGetText(GetPassportBy(index));
        public string GetEarTag(int index = 0) => _driver.SafelyGetText(GetEarTagBy(index));

        // Additional details - Simple getters
        public string GetCommodityIntendedFor() => _driver.SafelyGetText(commodityIntendedForBy);
        public string GetTemperature() => _driver.SafelyGetText(temperatureBy);
        public string GetUnweanedAnimalsOption() => _driver.SafelyGetText(unweanedAnimalsOptionBy);

        // Documents - Simple getters
        public string GetHealthCertificateReference() => _driver.SafelyGetText(healthCertificateReferenceBy);
        public string GetAdditionalDocumentType() => _driver.SafelyGetText(additionalDocumentTypeBy);
        public string GetAdditionalDocumentReference() => _driver.SafelyGetText(additionalDocumentReferenceBy);
        public string GetHealthCertificateFileName() => _driver.SafelyGetText(healthCertificateFileNameBy);
        public string GetAdditionalDocumentFileName() => _driver.SafelyGetText(additionalDocumentFileNameBy);

        //Catched Documents
        public string GetCatchedDocumentType() => _driver.SafelyGetText(additionalDocumentTypeBy);
        public string GetCatchedDocumentReference() => _driver.SafelyGetText(additionalDocumentReferenceBy);
        public string GetCatchedCertificateFileName() => _driver.SafelyGetText(healthCertificateFileNameBy);
        public string GetCatchedDocumentFileName() => _driver.SafelyGetText(additionalDocumentFileNameBy);

        // Date methods with parsing logic
        public string GetHealthCertificateDateOfIssue()
        {
            try
            {
                var text = _driver.SafelyGetText(healthCertificateDateOfIssueBy);
                if (string.IsNullOrEmpty(text))
                    return string.Empty;

                // Convert "1 December 2025" to "01 12 2025" format
                if (DateTime.TryParse(text, out DateTime date))
                {
                    return date.ToString("dd MM yyyy");
                }
                return text;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetAdditionalDocumentDateOfIssue()
        {
            try
            {
                var text = _driver.SafelyGetText(additionalDocumentDateOfIssueBy);
                if (string.IsNullOrEmpty(text))
                    return string.Empty;

                // Convert "24 November 2025" to "24 11 2025" format  
                if (DateTime.TryParse(text, out DateTime date))
                {
                    return date.ToString("dd MM yyyy");
                }
                return text;
            }
            catch
            {
                return string.Empty;
            }
        }

        public bool VerifyCatchCertificateHeadingDisplaysCount(int expectedCount)
        {
            var heading = _driver.FindElement(catchCertificateHeadingBy);
            var expectedText = $"{expectedCount} catch certificate{(expectedCount != 1 ? "s" : "")}";
            return heading.Displayed && heading.Text.Trim().Equals(expectedText, StringComparison.OrdinalIgnoreCase);
        }

        public (bool isValid, List<string> mismatches) VerifyCatchCertificateSummaryTable(
            int totalAttachments,
            Dictionary<int, (string reference, string flagState, string dateOfIssue, string fileName)> expectedData)
        {
            var mismatches = new List<string>();

            var table = _driver.FindElement(catchCertificateSummaryTableBy);
            if (!table.Displayed)
            {
                mismatches.Add("Catch certificate summary table is not displayed");
                return (false, mismatches);
            }

            var rows = _driver.FindElements(catchCertificateSummaryRowsBy);
            if (rows.Count != totalAttachments)
            {
                mismatches.Add($"Expected {totalAttachments} rows, found {rows.Count}");
                return (false, mismatches);
            }

            for (int i = 0; i < totalAttachments; i++)
            {
                var rowIndex = i + 1;
                var row = rows[i];
                var cells = row.FindElements(By.TagName("td"));

                if (cells.Count < 4)
                {
                    mismatches.Add($"Row {rowIndex}: Expected 4 cells, found {cells.Count}");
                    continue;
                }

                if (expectedData.TryGetValue(rowIndex, out var expected))
                {
                    var actualReference = cells[0].Text.Trim();
                    var actualFlagState = cells[1].Text.Trim();
                    var actualDateOfIssue = cells[2].Text.Trim();
                    var actualFileName = cells[3].Text.Trim();

                    if (!actualReference.Equals(expected.reference, StringComparison.OrdinalIgnoreCase))
                    {
                        mismatches.Add($"Row {rowIndex} Reference: Expected '{expected.reference}', Found '{actualReference}'");
                    }

                    if (!actualFlagState.Equals(expected.flagState, StringComparison.OrdinalIgnoreCase))
                    {
                        mismatches.Add($"Row {rowIndex} Flag State: Expected '{expected.flagState}', Found '{actualFlagState}'");
                    }

                    if (!actualDateOfIssue.Equals(expected.dateOfIssue, StringComparison.OrdinalIgnoreCase))
                    {
                        mismatches.Add($"Row {rowIndex} Date of Issue: Expected '{expected.dateOfIssue}', Found '{actualDateOfIssue}'");
                    }

                    if (!actualFileName.Contains(expected.fileName, StringComparison.OrdinalIgnoreCase))
                    {
                        mismatches.Add($"Row {rowIndex} File Name: Expected to contain '{expected.fileName}', Found '{actualFileName}'");
                    }
                }
            }

            return (mismatches.Count == 0, mismatches);
        }

        public (bool isValid, List<string> mismatches) VerifyCatchCertificateDetails(
            int totalAttachments,
            Dictionary<int, (string reference, string commodityCode, string species)> expectedData)
        {
            var mismatches = new List<string>();

            for (int i = 1; i <= totalAttachments; i++)
            {
                // Verify "Catch certificate X of Y" caption
                var captionXPath = $"(//span[contains(@class, 'govuk-caption-m') and contains(text(), 'Catch certificate')])[{i}]";
                var captionElements = _driver.FindElements(By.XPath(captionXPath));

                if (captionElements.Count == 0)
                {
                    mismatches.Add($"Caption for Catch certificate {i} of {totalAttachments} not found");
                    continue;
                }

                var expectedCaption = $"Catch certificate {i} of {totalAttachments}";
                var actualCaption = captionElements[0].Text.Trim();

                if (!actualCaption.Equals(expectedCaption, StringComparison.OrdinalIgnoreCase))
                {
                    mismatches.Add($"Caption {i}: Expected '{expectedCaption}', Found '{actualCaption}'");
                }

                if (expectedData.TryGetValue(i, out var expected))
                {
                    // Verify Reference heading
                    var referenceXPath = $"(//span[contains(@class, 'govuk-caption-m') and contains(text(), 'Catch certificate {i} of')])/following-sibling::div//h3[contains(@class, 'govuk-heading-s')]";
                    var referenceElements = _driver.FindElements(By.XPath(referenceXPath));

                    if (referenceElements.Count > 0)
                    {
                        var actualReference = referenceElements[0].Text.Trim();
                        var expectedReference = $"Reference: {expected.reference}";

                        if (!actualReference.Equals(expectedReference, StringComparison.OrdinalIgnoreCase))
                        {
                            mismatches.Add($"Certificate {i} Reference: Expected '{expectedReference}', Found '{actualReference}'");
                        }
                    }
                    else
                    {
                        mismatches.Add($"Certificate {i}: Reference heading not found");
                    }

                    // Verify Commodity Code - using the table within that section
                    var commodityCodeXPath = $"(//table[@id='catch-certificate-details-table'])[{i}]//td[@id='species-commodity-code']";
                    var commodityCodeElements = _driver.FindElements(By.XPath(commodityCodeXPath));

                    if (commodityCodeElements.Count > 0)
                    {
                        var actualCommodityCode = commodityCodeElements[0].Text.Trim();

                        if (!actualCommodityCode.Equals(expected.commodityCode, StringComparison.OrdinalIgnoreCase))
                        {
                            mismatches.Add($"Certificate {i} Commodity Code: Expected '{expected.commodityCode}', Found '{actualCommodityCode}'");
                        }
                    }
                    else
                    {
                        mismatches.Add($"Certificate {i}: Commodity code not found");
                    }

                    // Verify Species - using the table within that section
                    var speciesXPath = $"(//table[@id='catch-certificate-details-table'])[{i}]//td[@id='species-and-description-information']";
                    var speciesElements = _driver.FindElements(By.XPath(speciesXPath));

                    if (speciesElements.Count > 0)
                    {
                        var actualSpecies = speciesElements[0].Text.Trim();

                        if (!actualSpecies.Contains(expected.species, StringComparison.OrdinalIgnoreCase))
                        {
                            mismatches.Add($"Certificate {i} Species: Expected to contain '{expected.species}', Found '{actualSpecies}'");
                        }
                    }
                    else
                    {
                        mismatches.Add($"Certificate {i}: Species not found");
                    }
                }
            }

            return (mismatches.Count == 0, mismatches);
        }

        // Addresses - Using helper methods with extraction logic
        public string GetConsignorName()
        {
            var fullText = _driver.SafelyGetText(consignorDetailsBy);
            return ExtractNameFromAddressText(fullText);
        }

        public string GetConsignorAddress()
        {
            var fullText = _driver.SafelyGetText(consignorDetailsBy);
            return ExtractAddressFromAddressText(fullText);
        }

        public string GetCHEDPPConsignorAddress()
        {
            var fullText = _driver.SafelyGetText(consignorDetailsBy);
            return ExtractCHEDPPAddressFromAddressText(fullText);
        }

        public string GetConsigneeName()
        {
            var fullText = _driver.SafelyGetText(consigneeDetailsBy);
            return ExtractNameFromAddressText(fullText);
        }

        public string GetConsigneeAddress()
        {
            var fullText = _driver.SafelyGetText(consigneeDetailsBy);
            return ExtractAddressFromAddressText(fullText);
        }

        public string GetImporterName()
        {
            var fullText = _driver.SafelyGetText(importerDetailsBy);
            return ExtractNameFromAddressText(fullText);
        }

        public string GetImporterAddress()
        {
            var fullText = _driver.SafelyGetText(importerDetailsBy);
            return ExtractAddressFromAddressText(fullText);
        }

        public string GetCHEDPPImporterAddress()
        {
            var fullText = _driver.SafelyGetText(importerDetailsBy);
            return ExtractCHEDPPAddressFromAddressText(fullText);
        }

        public string GetDestinationName()
        {
            var fullText = _driver.SafelyGetText(destinationDetailsBy);
            return ExtractNameFromAddressText(fullText);
        }

        public string GetDestinationAddress()
        {
            var fullText = _driver.SafelyGetText(destinationDetailsBy);
            return ExtractAddressFromAddressText(fullText);
        }

        public string GetDeliveryAddress()
        {
            var fullText = _driver.SafelyGetText(destinationDetailsBy);
            return ExtractCHEDPPAddressFromAddressText(fullText);
        }

        public string GetConsignorCountry()
        {
            var fullText = _driver.SafelyGetText(consignorDetailsBy);
            return ExtractCountryFromAddressText(fullText);
        }

        public string GetConsigneeCountry()
        {
            var fullText = _driver.SafelyGetText(consigneeDetailsBy);
            return ExtractCountryFromAddressText(fullText);
        }

        public string GetImporterCountry()
        {
            var fullText = _driver.SafelyGetText(importerDetailsBy);
            return ExtractCountryFromAddressText(fullText);
        }

        public string GetPlaceOfDestinationCountry()
        {
            var fullText = _driver.SafelyGetText(destinationDetailsBy);
            return ExtractCountryFromAddressText(fullText);
        }

        // Transport details - Simple getters
        public string GetPortOfEntry() => _driver.SafelyGetText(portOfEntryBy);
        public string GetInspectionPremises() => _driver.SafelyGetText(inspectionPremisesBy);
        public string GetMeansOfTransport() => _driver.SafelyGetText(meansOfTransportBy);
        public string GetTransportId() => _driver.SafelyGetText(transportIdBy);
        public string GetTransportDocumentReference() => _driver.SafelyGetText(transportDocumentReferenceBy);
        public string GetEstimatedArrivalTime() => _driver.SafelyGetText(estimatedArrivalTimeBy);
        public string GetCTCUsage() => _driver.SafelyGetText(ctcUsageBy);
        public string GetGVMSUsage() => _driver.SafelyGetText(gvmsUsageBy);
        public string GetContainerUsage() => _driver.SafelyGetText(containerUsageBy);

        // Transport dates with parsing
        public string GetEstimatedArrivalDate()
        {
            try
            {
                var text = _driver.SafelyGetText(estimatedArrivalDateBy);
                if (string.IsNullOrEmpty(text))
                    return string.Empty;

                // Convert "4 December 2025" to "04 Dec 2025" format
                if (DateTime.TryParse(text, out DateTime date))
                {
                    return date.ToString("dd MMM yyyy");
                }
                return text;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetEstimatedJourneyTime()
        {
            try
            {
                var text = _driver.SafelyGetText(estimatedJourneyTimeBy);
                if (string.IsNullOrEmpty(text))
                    return string.Empty;

                // Extract just the number from "8 hrs" to return "8"
                return text.Replace(" hrs", "").Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        // Transport to BCP - Containers
        public string GetContainerNumber(int index = 0) => _driver.SafelyGetText(GetContainerNumberBy(index));
        public string GetSealNumber(int index = 0) => _driver.SafelyGetText(GetSealNumberBy(index));
        public string GetOfficialSeal(int index = 0) => _driver.SafelyGetText(GetOfficialSealBy(index));

        // Transport after BCP - Simple getters
        public string GetMeansOfTransportAfterBCP() => _driver.SafelyGetText(meansOfTransportAfterBCPBy);
        public string GetTransportIdentificationAfterBCP() => _driver.SafelyGetText(transportIdentificationAfterBCPBy);
        public string GetTransportDocumentReferenceAfterBCP() => _driver.SafelyGetText(transportDocumentReferenceAfterBCPBy);
        public string GetDepartureTimeFromBCP() => _driver.SafelyGetText(departureTimeFromBCPBy);

        public string GetDepartureDateFromBCP()
        {
            try
            {
                var text = _driver.SafelyGetText(departureDateFromBCPBy);
                if (string.IsNullOrEmpty(text))
                    return string.Empty;

                // Convert "17 January 2026" to "17 Jan 2026" format
                if (DateTime.TryParse(text, out DateTime date))
                {
                    return date.ToString("dd MMM yyyy");
                }
                return text;
            }
            catch
            {
                return string.Empty;
            }
        }

        // Transporter details - Simple getters
        public string GetTransporterName() => _driver.SafelyGetText(transporterNameBy);
        public string GetTransporterAddress() => _driver.SafelyGetText(transporterAddressBy);
        public string GetTransporterCountry() => _driver.SafelyGetText(transporterCountryBy);
        public string GetTransporterApprovalNumber() => _driver.SafelyGetText(transporterApprovalNumberBy);
        public string GetTransporterType() => _driver.SafelyGetText(transporterTypeBy);

        public string GetTransporterAddressWithoutContact()
        {
            try
            {
                var fullAddress = _driver.SafelyGetText(transporterAddressBy);
                if (string.IsNullOrEmpty(fullAddress))
                    return string.Empty;

                // Transporter address format: "street, city, postcode, phone, email"
                // We only want: "street, city, postcode"
                var parts = fullAddress.Split(',', StringSplitOptions.TrimEntries);

                if (parts.Length >= 3)
                {
                    // Take first 3 parts (street, city, postcode) - exclude phone and email
                    return string.Join(", ", parts.Take(3));
                }

                return fullAddress;
            }
            catch
            {
                return string.Empty;
            }
        }

        // Route and contacts - Simple getters
        public string GetRouteCountries() => _driver.SafelyGetText(routeCountriesBy);
        public string GetNotifyTransportContacts() => _driver.SafelyGetText(notifyTransportContactsBy);

        public string GetConsignmentContactAddress()
        {
            try
            {
                var text = _driver.SafelyGetText(consignmentContactAddressBy);
                if (string.IsNullOrEmpty(text))
                    return string.Empty;

                // Convert comma-separated to multi-line format to match expected
                return text.Replace(", ", "\n");
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetCHEDReference() => _driver.SafelyGetText(chedReferenceBy);

        // Helper methods to extract name and address from combined text
        private string ExtractNameFromAddressText(string fullText)
        {
            if (string.IsNullOrEmpty(fullText))
                return string.Empty;

            var lines = fullText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            return lines.Length > 0 ? lines[0].Trim() : string.Empty;
        }

        private string ExtractAddressFromAddressText(string fullText)
        {
            if (string.IsNullOrEmpty(fullText))
                return string.Empty;

            var lines = fullText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length > 1)
            {
                var addressLine = lines[1].Trim();

                // The review page shows: "street, city, postcode, country, phone"
                // We only want: "street, city, postcode"

                var parts = addressLine.Split(',', StringSplitOptions.TrimEntries);

                // Take only the first 3 parts (street, city/region, postcode)
                // This excludes both country and phone number
                if (parts.Length >= 3)
                {
                    var addressWithoutCountryAndPhone = string.Join(", ", parts.Take(3));
                    return addressWithoutCountryAndPhone;
                }

                return addressLine;
            }
            return string.Empty;
        }

        private string ExtractCHEDPPAddressFromAddressText(string fullText)
        {
            if (string.IsNullOrEmpty(fullText))
                return string.Empty;

            var lines = fullText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length > 1)
            {
                var addressLine = lines[1].Trim();

                // The review page shows: "street, city, postcode, country, phone"
                // We only want: "street, city, postcode"

                var parts = addressLine.Split(',', StringSplitOptions.TrimEntries);

                // Take only the first 3 parts (street, city/region, postcode)
                // This excludes both country and phone number
                if (parts.Length >= 3)
                {
                    if (parts[0].Equals("DEPARTMENT FOR ENVIRONMENT FOOD & RURAL AFFAIRS (D E F R A)"))
                    {
                        var addressWithoutCountryAndPhone = string.Join(", ", parts.Take(5));
                        return addressWithoutCountryAndPhone;
                    }
                    else
                    {
                        var addressWithoutCountryAndPhone = string.Join(", ", parts.Take(4));
                        return addressWithoutCountryAndPhone;
                    }
                }

                return addressLine;
            }
            return string.Empty;
        }

        private string ExtractCountryFromAddressText(string fullText)
        {
            if (string.IsNullOrEmpty(fullText))
                return string.Empty;

            var lines = fullText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length > 1)
            {
                var addressLine = lines[1].Trim();

                // The review page shows: "street, city, postcode, country, phone"
                // Country is the 4th part (index 3)

                var parts = addressLine.Split(',', StringSplitOptions.TrimEntries);

                // Return the 4th part (country) if it exists
                if (parts.Length >= 4)
                {
                    return parts[3].Trim();
                }
            }
            return string.Empty;
        }

        public bool IsError(string errorMessage)
        {
            Thread.Sleep(1000);
            foreach (var element in lblErrorMessages)
            {
                if (element.Text.Contains(errorMessage))
                {
                    return true;
                }
            }
            return false;
        }

        public void ClickChangeLink(string heading)
        {
            lnkChange(heading).Click();
        }

        public (bool hasError, string errorMessages) VerifyErrorMsgDisplayed(string errorMessage)
        {
            // Check if error message banner is present
            if (lblErrorMessages.Count == 0)
            {
                // No error banner present - this is what we want (test should pass)
                return (false, string.Empty);
            }

            // Error banner is present - collect all error messages
            var errorMessagesList = new List<string>();
            foreach (var element in lblErrorMessages)
            {
                errorMessagesList.Add(element.Text.Trim());
            }

            var allErrorMessages = string.Join("; ", errorMessagesList);

            // Check if the specific error message we're looking for is in the list
            var containsSpecificError = errorMessagesList.Any(msg =>
                msg.Contains(errorMessage, StringComparison.OrdinalIgnoreCase));

            return (containsSpecificError, allErrorMessages);
        }

        public bool IsCopyAsNewButtonDisplayed()
        {
            return _driver.IsElementDisplayed(CopyAsNewButtonBy);
        }

        public bool IsViewCHEDButtonDisplayed()
        {
            return _driver.IsElementDisplayed(ViewCHEDButtonBy);
        }

        public bool AreChangeLinksNotDisplayed()
        {
            try
            {
                var changeLinks = _driver.FindElements(ChangeLinksBy);
                var displayedLinks = changeLinks.Where(link => link.IsElementDisplayed()).ToList();

                if (displayedLinks.Count == 0)
                {
                    return true;
                }

                Console.WriteLine($"✗ Found {displayedLinks.Count} Change link(s) displayed when expecting none");
                return false;
            }
            catch (NoSuchElementException)
            {
                return true;
            }
        }

        public string GetMovementReferenceNumber() => _driver.SafelyGetText(movementReferenceNumberBy);

        public void ClickDashboardLink()
        {
            _driver.FindElement(DashboardLinkBy).Click();
        }

        public bool VerifyCatchCertificateHeader(string message)
        {
            return lblCatchCertificateHeader.Text.Equals(message);
        }

        public bool VerifyCatchCertificateForNoneAttached(string message)
        {
            return lblCatchCertificateRowNoneAttached.Text.Equals(message);
        }

        public string GetCatchCertificateDocumentReference(int row, int column = 1)
        {
            return catchCertificateDocumentReference(row, column).Text;
        }

        public string GetCatchCertificateFlagState(int row, int column = 2)
        {
            return catchCertificateFlagState(row, column).Text;
        }

        public string GetCatchCertificateDocumentDateOfIssue(int row, int column = 3)
        {
            return catchCertificateDocumentDateOfIssue(row, column).Text;
        }

        public void ClickChangeCatchCertificateReferences(int index)
        {
            lnkChangeCatchCertificateLinks.ElementAt(index).Click();
        }

        public string GetCatchCertificateCommodityCode(int row, int column = 1)
        {
            return catchCertificateCommodityCode(row, column).Text;
        }

        public string GetCatchCertificateSpeciesDescription(int row, int column = 2)
        {
            return catchCertificateSpeciesDescription(row, column).Text;
        }

        public void ClickChangeLinkForTransportToTheBCP()
        {
            lnkChangeLinkForTransportBCP.Click();
        }

        public void ClickChangeLinkForContactDetails()
        {
            lnkChangeLinkForContactDetailsChange.Click();
        }

        public void ClickChangeLinkForGoodsMovementServices()
        {
            lnkGoodsMovementServicesChange.Click();
        }

        public void ClickChangeLinkForAddDeliveryAddress()
        {
            lnkTraderDeliveryAddressChange.Click();
        }

        public string GetImporterNameByChangeLink()
        {
            return pImporterName.Text.Trim();
        }

        public void ClickViewCHEDButton()
        {
            btnViewChed.Click();
        }

        public string GetContactName()=>ddContactName.Text;

        public string GetContactEmail() => ddContactEmail.Text;

        public string GetContactTelephone() => ddContactTelephone.Text;

        public string GetIntendedForFinalUsers() => tdIntendedForUsers.Text.Trim();

        public string GetControlledAtmosphereContainer() => tdControlledAtmosphereContainer.Text.Trim();
        public string GetQuantity() => tdQuantity.Text.Trim();

        public string GetQuantityType()=> tdQuantityType.Text.Trim();
        public string GetGrossVolume() => tdGrossVolume.Text.Trim();

        public string GetGrossVolumeUnit() => tdGrossVolumeUnit.Text.Trim();
    }
}