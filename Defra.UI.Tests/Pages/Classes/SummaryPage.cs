using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Contracts;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class SummaryPage : ISummaryPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IReadOnlyCollection<IWebElement> divAboutConsignment => _driver.WaitForElements(By.XPath("//*[@id='review-table-about-notification']/div"));
        private IReadOnlyCollection<IWebElement> txtCommodityCode => _driver.FindElements(By.XPath("//*[normalize-space()='Commodity code']//following-sibling::td[1]"));
        private IReadOnlyCollection<IWebElement> txtCommodityType => _driver.FindElements(By.XPath("//*[normalize-space()='Type of commodity']/following::td[1]"));
        private IReadOnlyCollection<IWebElement> txtCommodityType1 => _driver.FindElements(By.XPath("(//*[normalize-space()='Description of the goods']/following::td)[1]"));
        private IReadOnlyCollection<IWebElement> txtSpecies => _driver.FindElements(By.XPath("//*[normalize-space()='Species']/../../following-sibling::tbody//td[1]"));
        private IReadOnlyCollection<IWebElement> txtNetWeight => _driver.FindElements(By.XPath("//*[@class='govuk-table__cell govuk-table__cell--numeric consignment-net-weight']"));
        private IReadOnlyCollection<IWebElement> txtPackages => _driver.FindElements(By.XPath("//*[@class='govuk-table__cell govuk-table__cell--numeric consignment-net-weight']//following-sibling::td[1]"));
        private IReadOnlyCollection<IWebElement> txtPackageType => _driver.FindElements(By.XPath("//*[@class='govuk-table__cell govuk-table__cell--numeric consignment-net-weight']//following-sibling::td[2]"));
        private IWebElement txtSubtotalNetWeight => _driver.FindElement(By.XPath("//*[normalize-space()='Subtotal']//following-sibling::td[1]"));
        private IWebElement txtSubtotalPackages => _driver.FindElement(By.XPath("//*[normalize-space()='Subtotal']//following-sibling::td[2]"));
        private IWebElement txtTotalNetWeight => _driver.FindElement(By.XPath("//*[normalize-space()='Total net weight']//following-sibling::td"));
        private IWebElement txtTotalPackages => _driver.FindElement(By.XPath("//*[normalize-space()='Total packages']//following-sibling::td"));
        private IWebElement txtTotalGrossWeight => _driver.FindElement(By.XPath("//*[normalize-space()='Total gross weight']//following-sibling::td[1]"));
        private IWebElement txtTemperature => _driver.FindElement(By.XPath("//*[normalize-space()='Temperature']//following-sibling::dd"));
        private IReadOnlyCollection<IWebElement> txtDocumentType => _driver.FindElements(By.XPath("//td[contains(@id,'veterinary-document-type')]"));
        private IReadOnlyCollection<IWebElement> txtDocumentReference => _driver.FindElements(By.XPath("//td[contains(@id,'veterinary-document-reference')]"));
        private IReadOnlyCollection<IWebElement> txtDateOfIssue => _driver.FindElements(By.XPath("//td[contains(@id,'veterinary-document-issue-date')]"));
        private IWebElement txtApprovedEstablishmentName => _driver.FindElement(By.XPath("//*[@id='establishments-row-1']/td[1]"));
        private IWebElement txtApprovedEstablishmentCountry => _driver.FindElement(By.XPath("//*[@id='establishments-row-1']/td[2]"));
        private IWebElement txtApprovedEstablishmentType => _driver.FindElement(By.XPath("//*[@id='establishments-row-1']/td[3]"));
        private IWebElement txtApprovedEstablishmentApprovalNum => _driver.FindElement(By.XPath("//*[@id='establishments-row-1']/td[4]"));
        private IWebElement txtConsignorDetails => _driver.FindElement(By.Id("consignor"));
        private IWebElement txtConsigneeDetails => _driver.FindElement(By.Id("consignee"));
        private IWebElement txtImporterDetails => _driver.FindElement(By.Id("importer"));
        private IWebElement txtplaceOfDestination => _driver.FindElement(By.Id("final-destination"));

        //Inspector
        private IWebElement txtBCPRefNum => _driver.WaitForElement(By.XPath("//*[@id='reference-row-1']/td[1]"));
        private IWebElement txtDocumentaryChk => _driver.WaitForElement(By.XPath("//*[@id='parttwo/consignmentcheck']/td[1]"));
        private IWebElement txtIdentityChkType => _driver.WaitForElement(By.XPath("//*[@id='consignmentcheck/identitycheckdone']/th"));
        private IWebElement txtIdentityChkDecision => _driver.WaitForElement(By.XPath("//*[@id='consignmentcheck/identitycheckdone']/td[1]"));
        private IWebElement txtPhysicalChkDecision => _driver.WaitForElement(By.XPath("//*[@id='consignmentcheck/physicalcheckdone']/td[1]"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public SummaryPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public Summary GetSummaryDetails()
        {
            var summary = new Summary();

            foreach (var element in divAboutConsignment)
            {
                var elementTitle = element.FindElement(By.TagName("dt"))?.Text?.Replace("\r\n", string.Empty).Trim()?.ToUpper();
                var elementValue = element.FindElements(By.TagName("dd"))?[0].Text?.Replace("\r\n", string.Empty).Trim();

                switch (elementTitle)
                {
                    case "IMPORT TYPE":
                        summary.ImportType = elementValue;
                        break;
                    case "COUNTRY OF ORIGIN":
                        summary.CountryOfOrigin = elementValue;
                        break;
                    case "COUNTRY FROM WHERE CONSIGNED":
                        summary.ContryFromWhereConsigned = elementValue;
                        break;
                    case "CONSIGNMENT CONFORMS TO REGULATORY REQUIREMENTS":
                        summary.ConsignmentConformToRegulatoryRequirements = elementValue;
                        break;
                    case "CONSIGNMENT REFERENCE NUMBER":
                        summary.ConsignmentReferenceNumber = elementValue;
                        break;
                    case "MAIN REASON FOR IMPORTING THE CONSIGNMENT":
                        summary.MainReasonForImport = elementValue;
                        break;
                    case "PURPOSE IN THE INTERNAL MARKET":
                        summary.Purpose = elementValue;
                        break;
                    case "IMPORT RISK CATEGORY":
                        summary.RiskCategory = elementValue;
                        break;
                }
            }

            summary.CommodityCode = txtCommodityCode.Select(x =>x.Text.Trim()).ToList();
            summary.TypeOfCommodity = txtCommodityType.Select(x => x.Text.Trim()).ToList();
            summary.TypeOfCommodity1 = txtCommodityType1.Select(x => x.Text.Trim()).ToList();
            summary.Species = txtSpecies.Select(x => x.Text.Trim()).ToList();
            summary.NetWeight = txtNetWeight.Select(x => x.Text.Trim().Replace(" kg/units", "")).ToList();
            summary.NumberOfPackages = txtPackages.Select(x => x.Text.Trim()).ToList();
            summary.PackageType = txtPackageType.Select(x => x.Text.Trim()).ToList();
            summary.SubtotalNetWeight = txtSubtotalNetWeight.Text.Trim().Replace(" kg/units", "");
            summary.SubtotalPackages = txtSubtotalPackages.Text.Trim();
            summary.TotalNetWeight = txtTotalNetWeight.Text.Trim().Replace(" kg/units", "");
            summary.TotalPackages = txtTotalPackages.Text.Trim();
            summary.TotalGrossWeight = txtTotalGrossWeight.Text.Trim().Replace(" kg/units", "");
            summary.Temperature = txtTemperature.Text.Trim();

            summary.DocumentType = txtDocumentType.Select(e => e.Text).ToArray();
            summary.DocumentReference = txtDocumentReference.Select(e => e.Text).ToArray();
            summary.DateOfIssue = txtDateOfIssue.Select(e => e.Text).ToArray();
            summary.ApprovedEstablishmentName = txtApprovedEstablishmentName.Text.Trim();
            summary.ApprovedEstablishmentCountry = txtApprovedEstablishmentCountry.Text.Trim();
            summary.ApprovedEstablishmentType = txtApprovedEstablishmentType.Text.Trim();
            summary.ApprovedEstablishmentApprovalNum = txtApprovedEstablishmentApprovalNum.Text.Trim();

            summary.ConsignorDetails = txtConsignorDetails.Text.Trim();
            summary.ConsigneeDetails = txtConsigneeDetails.Text.Trim();
            summary.ImporterDetails = txtImporterDetails.Text.Trim();
            summary.PlaceOfDestination = txtplaceOfDestination.Text.Trim();

            return summary;
        }
    }
}