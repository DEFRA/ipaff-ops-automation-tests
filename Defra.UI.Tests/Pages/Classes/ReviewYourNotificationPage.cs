using Defra.UI.Tests.Configuration;
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
        private By CopyAsNewButtonBy => By.XPath("//button[@type='submit' and @value='Copy as new']");
        private By ViewCHEDButtonBy => By.Id("show-notification");
        private By ChangeLinksBy => By.XPath("//a[text()='Change']");
        private IWebElement lnkChange(string section) => _driver.FindElement(By.XPath($"(//*[normalize-space()='{section}']/following::a)[1]"));
        private By DashboardLinkBy => By.XPath("//a[@class='govuk-breadcrumbs__link' and @href='/notification/pre/protected/notifications']");

        // About the consignment
        private IWebElement importType => _driver.FindElement(By.Id("importing"));
        private IWebElement countryOfOrigin => _driver.FindElement(By.Id("country-of-origin"));
        private IWebElement countryFromWhereConsigned => _driver.FindElement(By.XPath("//dt[text()='Country from where consigned']//following-sibling::dd"));
        private IWebElement mainReasonForImport => _driver.FindElement(By.Id("purpose-of-consignment-value"));
        private IWebElement purpose => _driver.FindElement(By.XPath("//dt[text()='Purpose in the internal market']//following-sibling::dd"));
        private IWebElement consignmentReferenceNumber => _driver.FindElement(By.XPath("//dt[text()='Consignment reference number']//following-sibling::dd"));
        private IWebElement exitDate => _driver.FindElement(By.Id("exit-date-value"));
        private By exitBCPTemporaryAdmissionLocator => By.Id("designated-bip-horses-value");
        private By exitBCPTransitLocator => By.XPath("//dt[@id='exit-border-control-post-header']/following-sibling::dd");
        private IWebElement destinationCountry => _driver.FindElement(By.XPath("//dt[@id='destination-country-header']/following-sibling::dd"));

        // Commodity details
        private IWebElement commodityCode => _driver.FindElement(By.XPath("//td[text()='Commodity code']//following-sibling::td[1]"));
        private IWebElement species => _driver.FindElement(By.XPath("//table[@id='review-table-consignment-0']//tbody//tr[1]//td[1]"));
        private IWebElement numberOfAnimals => _driver.FindElement(By.XPath("//table[@id='review-consignment-total-table']//tbody//tr//td[1]"));
        private IWebElement numberOfPackages => _driver.FindElement(By.XPath("//table[@id='review-consignment-total-table']//tbody//tr//td[2]"));
        
        private List<IWebElement> commodityCodeList => _driver.WaitForElements(By.XPath("//table[contains(@id,'review-table-commodity-attributes')]//td[text()='Commodity code']//following-sibling::td[1]")).ToList();
        private List<IWebElement> netWeightList => _driver.WaitForElements(By.XPath("//table[contains(@class,'data-table-commodities')]//tr/td[1]")).ToList();
        private List<IWebElement> numPackagesList => _driver.WaitForElements(By.XPath("//table[contains(@class,'data-table-commodities')]//tr/td[2]")).ToList();
        private List<IWebElement> typeOfPackagesList => _driver.WaitForElements(By.XPath("//table[contains(@class,'data-table-commodities')]//tr/td[3]")).ToList();
        private IWebElement totalNetWeight => _driver.FindElement(By.XPath("//td[text()='Total net weight']//following-sibling::td[1]"));
        private IWebElement totalPackages => _driver.FindElement(By.XPath("//td[text()='Total packages']//following-sibling::td[1]"));
        private IWebElement totalGrossWeight => _driver.FindElement(By.XPath("//td[text()='Total gross weight ']//following-sibling::td[1]"));

        //Additional details
        private IWebElement commodityIntendedFor => _driver.FindElement(By.XPath("//dt[text()='Commodity intended for']//following-sibling::dd"));
        private IWebElement temperature => _driver.FindElement(By.XPath("//dt[text()='Temperature']//following-sibling::dd"));

        // Animal details
        private IWebElement certificationOption => _driver.FindElement(By.XPath("//td[text()='Certified for']//following-sibling::td"));
        private IWebElement GetHorseNameElement(int index) => _driver.FindElement(By.XPath($"//table[@id='animal-identification-details-table']//tbody//tr[{index + 1}]//td[@headers='horseName-01']"));
        private IWebElement GetMicrochipElement(int index) => _driver.FindElement(By.XPath($"//table[@id='animal-identification-details-table']//tbody//tr[{index + 1}]//td[@headers='microchip-01']"));
        private IWebElement GetPassportElement(int index) => _driver.FindElement(By.XPath($"//table[@id='animal-identification-details-table']//tbody//tr[{index + 1}]//td[@headers='passport-01']"));
        private IWebElement GetEarTagElement(int index) => _driver.FindElement(By.XPath($"//table[contains(@aria-describedby,'animalProduct')]//tbody//tr[{index + 1}]//td[@headers='earTag-01']"));
        private IWebElement unweanedAnimalsOption => _driver.FindElement(By.XPath("//td[contains(text(),'Includes unweaned animals')]/following-sibling::td"));

        // Documents
        private IWebElement healthCertificateReference => _driver.FindElement(By.Id("latest-health-document-reference"));
        private IWebElement healthCertificateDateOfIssue => _driver.FindElement(By.Id("latest-health-document-issue-date"));
        private IWebElement healthCertificateFileName => _driver.FindElement(By.XPath("//table[@id='latest-health-certificate-table']//a[contains(@id,'attachment-view')]"));
        private IWebElement additionalDocumentType => _driver.FindElement(By.Id("veterinary-document-type-1"));
        private IWebElement additionalDocumentReference => _driver.FindElement(By.Id("veterinary-document-reference-1"));
        private IWebElement additionalDocumentDateOfIssue => _driver.FindElement(By.Id("veterinary-document-issue-date-1"));
        private IWebElement additionalDocumentFileName => _driver.FindElement(By.XPath("//table[@id='additional-documents-table']//a[contains(@id,'attachment-view')]"));

        // Addresses
        private IWebElement consignorDetails => _driver.FindElement(By.Id("consignor"));
        private IWebElement consigneeDetails => _driver.FindElement(By.Id("consignee"));
        private IWebElement importerDetails => _driver.FindElement(By.Id("importer"));
        private IWebElement destinationDetails => _driver.FindElement(By.Id("final-destination"));

        // Transport details
        private IWebElement portOfEntry => _driver.FindElement(By.XPath("//th[contains(text(),'Port of entry')]//following-sibling::td"));
        private IWebElement meansOfTransport => _driver.FindElement(By.XPath("//th[contains(text(),'Means of transport')]//following-sibling::td"));
        private IWebElement transportId => _driver.FindElement(By.XPath("//th[text()='Transport identification']//following-sibling::td"));
        private IWebElement containerUsage => _driver.FindElement(By.Id("imported-in-containers"));
        private IWebElement transportDocumentReference => _driver.FindElement(By.XPath("//th[text()='Transport document reference']//following-sibling::td"));
        private IWebElement estimatedArrivalDate => _driver.FindElement(By.XPath("//th[text()='Estimated arrival date at BCP or Port of entry']//following-sibling::td"));
        private IWebElement estimatedArrivalTime => _driver.FindElement(By.XPath("//th[contains(text(),'Estimated arrival time at')]//following-sibling::td"));
        private IWebElement estimatedJourneyTime => _driver.FindElement(By.XPath("//th[text()='Estimated total journey time of the animals']//following-sibling::td"));
        private IWebElement ctcUsage => _driver.FindElement(By.XPath("//td[contains(text(),'Using the Common Transit Convention')]/following-sibling::td"));
        private IWebElement gvmsUsage => _driver.FindElement(By.Id("goods-movement-services-route"));
        private IWebElement GetContainerNumberElement(int index) => _driver.FindElement(By.Id($"container-number-{index}"));
        private IWebElement GetSealNumberElement(int index) => _driver.FindElement(By.Id($"seal-number-{index}"));
        private IWebElement GetOfficialSealElement(int index) => _driver.FindElement(By.Id($"official-seal-present-{index}"));
        private IWebElement meansOfTransportAfterBCP => _driver.FindElement(By.XPath("//th[contains(text(),'Means of transport after BCP or Port of entry')]//following-sibling::td"));
        private IWebElement transportIdentificationAfterBCP => _driver.FindElement(By.XPath("//table[@id='review-table-transport']//th[text()='Transport identification']//following-sibling::td"));
        private IWebElement transportDocumentReferenceAfterBCP => _driver.FindElement(By.XPath("//table[@id='review-table-transport']//th[text()='Transport document reference']//following-sibling::td"));
        private IWebElement departureDateFromBCP => _driver.FindElement(By.XPath("//th[text()='Departure date from port of entry']//following-sibling::td"));
        private IWebElement departureTimeFromBCP => _driver.FindElement(By.XPath("//th[text()='Departure time from port of entry']//following-sibling::td"));

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

        //Error Message
        private IReadOnlyCollection<IWebElement> lblErrorMessages => _driver.FindElements(By.XPath("//div[@class='govuk-error-summary']/div/ul/li"));
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

        public string? GetPartOfImportType()
        {
            var importTypeText = string.Empty;
            try
            {
                importTypeText = importType.Text.Trim();
                var words = importTypeText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                importTypeText = string.Join(" ", words.Take(3)); 
            }
            catch(Exception ex)
            {
                Console.WriteLine($"GetPartOfImportType failed: {ex}");   
            }
            return importTypeText;
        }

        public string? GetCountryOfOrigin()
        {
            try { return countryOfOrigin.Text.Trim(); } catch { return null; }
        }

        public string? GetCountryFromWhereConsigned()
        {
            try { return countryFromWhereConsigned.Text.Trim(); } catch { return null; }
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

        public string? GetCommodityCodeList(int index)
        {
            try
            {
                var allcommodityCodesList = GetItemsList(commodityCodeList);

                if (allcommodityCodesList == null || allcommodityCodesList.Count == 0)
                    return null;

                var commCode = allcommodityCodesList[index];
                return string.IsNullOrWhiteSpace(commCode) ? null : commCode;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"GetCommodityCodeList failed: {ex}");
                return null;
            }
        }

        public string? GetNetWeightList(int index)
        {
            var netWeight = string.Empty;
            try
            {
                var allNetWeightList = GetItemsList(netWeightList);

                if (allNetWeightList == null || allNetWeightList.Count == 0)
                    return null;

                netWeight = allNetWeightList[index];
                netWeight = netWeight.Replace("kg/units", "").Trim();
                return string.IsNullOrWhiteSpace(netWeight) ? null : netWeight;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"GetNetWeightList failed: {ex}");
                return netWeight;
            }
        }

        public string? GetNumPackagesList(int index)
        {
            var numOfPackage = string.Empty;
            try
            {
                var allNumPackagesList = GetItemsList(numPackagesList);

                if (allNumPackagesList == null || allNumPackagesList.Count == 0)
                    return null;

                numOfPackage = allNumPackagesList[index];
                return string.IsNullOrWhiteSpace(numOfPackage) ? null : numOfPackage;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"GetNumPackagesList failed: {ex}");
                return numOfPackage;
            }
        }

        public string? GetTypeOfPackagesList(int index)
        {
            var typeOfPackage = string.Empty;
            try
            {
                var allTypeOfPackagesList = GetItemsList(typeOfPackagesList);

                if (allTypeOfPackagesList == null || allTypeOfPackagesList.Count == 0)
                    return null;

                typeOfPackage = allTypeOfPackagesList[index];
                return string.IsNullOrWhiteSpace(typeOfPackage) ? null : typeOfPackage;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"GetTypeOfPackagesList failed: {ex}");
                return typeOfPackage;
            }
        }

        private List<string>? GetItemsList(List<IWebElement> inputList)
        {
            List<string> itemsList = new List<string>();
            foreach (var item in inputList)
            {
                itemsList.Add(item.Text.Trim());
            }
            return itemsList;
        }

        public string? GetTotalNetWeight()
        {
            string totalNetWeightText = string.Empty;
            try
            {
                totalNetWeightText = totalNetWeight.Text.Replace("kg/units", "").Trim();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"GetTotalNetWeight failed: {ex.Message}");
            }
            return totalNetWeightText;
        }

        public string? GetTotalPackages()
        {
            string totalPackagesText = string.Empty;
            try
            {
                totalPackagesText = totalPackages.Text.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTotalPackages failed: {ex.Message}");
            }
            return totalPackagesText;
        }

        public string? GetTotalGrossWeight()
        {
            string totalGrossWeightText = string.Empty;
            try
            {
                totalGrossWeightText = totalGrossWeight.Text.Replace("kg/units", "").Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTotalGrossWeight failed: {ex.Message}");
            }
            return totalGrossWeightText;
        }

        public string? GetExitDate()
        {
            try
            {
                var text = exitDate.Text.Trim();
                // Convert "14 January 2026" to "14 January 2026" format (matches stored format)
                if (DateTime.TryParse(text, out DateTime date))
                {
                    return date.ToString("dd MMMM yyyy");
                }
                return text;
            }
            catch { return null; }
        }

        public string? GetExitBCP()
        {
            try
            {
                // First, check what the main reason for import is
                var mainReason = GetMainReasonForImport();

                if (mainReason?.Contains("Transit", StringComparison.OrdinalIgnoreCase) == true)
                {
                    // For Transit, use the exit-border-control-post-header locator
                    var transitExitBCP = _driver.FindElement(exitBCPTransitLocator);
                    return transitExitBCP.Text.Trim();
                }
                else if (mainReason?.Contains("Temporary admission", StringComparison.OrdinalIgnoreCase) == true)
                {
                    // For Temporary Admission Horses, use the designated-bip-horses-value locator
                    var tempAdmissionExitBCP = _driver.FindElement(exitBCPTemporaryAdmissionLocator);
                    return tempAdmissionExitBCP.Text.Trim();
                }
                else
                {
                    // Fallback: Try both locators
                    try
                    {
                        var transitExitBCP = _driver.FindElement(exitBCPTransitLocator);
                        if (!string.IsNullOrEmpty(transitExitBCP.Text))
                            return transitExitBCP.Text.Trim();
                    }
                    catch { }

                    try
                    {
                        var tempAdmissionExitBCP = _driver.FindElement(exitBCPTemporaryAdmissionLocator);
                        if (!string.IsNullOrEmpty(tempAdmissionExitBCP.Text))
                            return tempAdmissionExitBCP.Text.Trim();
                    }
                    catch { }

                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetExitBCP failed: {ex.Message}");
                return null;
            }
        }

        public string? GetDestinationCountry()
        {
            try { return destinationCountry.Text.Trim(); } catch { return null; }
        }

        // Animal details
        public string? GetCertificationOption()
        {
            try { return certificationOption.Text.Trim(); } catch { return null; }
        }

        public string? GetHorseName(int index = 0)
        {
            try { return GetHorseNameElement(index).Text.Trim(); } catch { return null; }
        }

        public string? GetMicrochipNumber(int index = 0)
        {
            try { return GetMicrochipElement(index).Text.Trim(); } catch { return null; }
        }

        public string? GetPassportNumber(int index = 0)
        {
            try { return GetPassportElement(index).Text.Trim(); } catch { return null; }
        }

        public string? GetEarTag(int index = 0)
        {
            try { return GetEarTagElement(index).Text.Trim(); } catch { return null; }
        }

        //Additional details

        public string? GetCommodityIntendedFor()
        {
            try { return commodityIntendedFor.Text.Trim(); } catch { return null; }
        }

        public string? GetTemperature()
        {
            try { return temperature.Text.Trim(); } catch { return null; }
        }

        public string? GetUnweanedAnimalsOption()
        {
            try { return unweanedAnimalsOption.Text.Trim(); } catch { return null; }
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

        public string? GetHealthCertificateFileName()
        {
            try { return healthCertificateFileName.Text.Trim(); } catch { return null; }
        }

        public string? GetAdditionalDocumentFileName()
        {
            try { return additionalDocumentFileName.Text.Trim(); } catch { return null; }
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

        public string? GetCTCUsage()
        {
            try { return ctcUsage.Text.Trim(); } catch { return null; }
        }

        public string? GetGVMSUsage()
        {
            try { return gvmsUsage.Text.Trim(); } catch { return null; }
        }

        // Transport to BCP - Containers
        public string? GetContainerNumber(int index = 0)
        {
            try { return GetContainerNumberElement(index).Text.Trim(); } catch { return null; }
        }

        public string? GetSealNumber(int index = 0)
        {
            try { return GetSealNumberElement(index).Text.Trim(); } catch { return null; }
        }

        public string? GetOfficialSeal(int index = 0)
        {
            try { return GetOfficialSealElement(index).Text.Trim(); } catch { return null; }
        }

        // Transport after BCP
        public string? GetMeansOfTransportAfterBCP()
        {
            try { return meansOfTransportAfterBCP.Text.Trim(); } catch { return null; }
        }

        public string? GetTransportIdentificationAfterBCP()
        {
            try { return transportIdentificationAfterBCP.Text.Trim(); } catch { return null; }
        }

        public string? GetTransportDocumentReferenceAfterBCP()
        {
            try { return transportDocumentReferenceAfterBCP.Text.Trim(); } catch { return null; }
        }

        public string? GetDepartureDateFromBCP()
        {
            try
            {
                var text = departureDateFromBCP.Text.Trim();
                // Convert "17 January 2026" to "17 Jan 2026" format
                if (DateTime.TryParse(text, out DateTime date))
                {
                    return date.ToString("dd MMM yyyy");
                }
                return text;
            }
            catch { return null; }
        }

        public string? GetDepartureTimeFromBCP()
        {
            try { return departureTimeFromBCP.Text.Trim(); } catch { return null; }
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

        private string ExtractAddressFromAddressText(string fullText)
        {
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

        public bool IsError(string errorMessage)
        {
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

        public void ClickDashboardLink()
        {
            _driver.FindElement(DashboardLinkBy).Click();
        }
    }
}