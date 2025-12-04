using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Contracts;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using DocumentFormat.OpenXml.Drawing.Diagrams;
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
        private IWebElement txtCommodityCode => _driver.WaitForElement(By.XPath("//*[normalize-space()='Commodity code']//following-sibling::td[1]"));
        private IWebElement txtCommodityType => _driver.WaitForElement(By.XPath("//*[normalize-space()='Type of commodity']//following-sibling::td[1]"));
        private IWebElement txtSpecies => _driver.WaitForElement(By.XPath("//*[normalize-space()='Species']/../../following-sibling::tbody//td[1]"));
        private IWebElement txtNetWeight => _driver.WaitForElement(By.XPath("//*[@class='govuk-table__cell govuk-table__cell--numeric consignment-net-weight']"));
        private IWebElement txtPackages => _driver.WaitForElement(By.XPath("//*[@class='govuk-table__cell govuk-table__cell--numeric consignment-net-weight']//following-sibling::td[1]"));
        private IWebElement txtPackageType => _driver.WaitForElement(By.XPath("//*[@class='govuk-table__cell govuk-table__cell--numeric consignment-net-weight']//following-sibling::td[2]"));
        private IWebElement txtSubtotalNetWeight => _driver.WaitForElement(By.XPath("//*[normalize-space()='Subtotal']//following-sibling::td[1]"));
        private IWebElement txtSubtotalPackages => _driver.WaitForElement(By.XPath("//*[normalize-space()='Subtotal']//following-sibling::td[2]"));
        private IWebElement txtTotalNetWeight => _driver.WaitForElement(By.XPath("//*[normalize-space()='Total net weight']//following-sibling::td"));
        private IWebElement txtTotalPackages => _driver.WaitForElement(By.XPath("//*[normalize-space()='Total packages']//following-sibling::td"));
        private IWebElement txtTotalGrossWeight => _driver.WaitForElement(By.XPath("//*[normalize-space()='Total gross weight']//following-sibling::td[1]"));
        private IWebElement txtTemperature => _driver.WaitForElement(By.XPath("//*[normalize-space()='Temperature']//following-sibling::dd"));
        private IWebElement txtDocumentType => _driver.WaitForElement(By.Id("veterinary-document-type-1"));
        private IWebElement txtDocumentReference => _driver.WaitForElement(By.Id("veterinary-document-reference-1"));
        private IWebElement txtDateOfIssue => _driver.WaitForElement(By.Id("veterinary-document-issue-date-1"));
        private IWebElement txtApprovedEstablishmentName => _driver.WaitForElement(By.XPath("//*[@id='establishments-row-1']/td[1]"));
        private IWebElement txtApprovedEstablishmentCountry => _driver.WaitForElement(By.XPath("//*[@id='establishments-row-1']/td[2]"));
        private IWebElement txtApprovedEstablishmentType => _driver.WaitForElement(By.XPath("//*[@id='establishments-row-1']/td[3]"));
        private IWebElement txtApprovedEstablishmentApprovalNum => _driver.WaitForElement(By.XPath("//*[@id='establishments-row-1']/td[4]"));
        private IWebElement txtConsignorDetails => _driver.WaitForElement(By.Id("consignor"));
        private IWebElement txtConsigneeDetails => _driver.WaitForElement(By.Id("consignee"));
        private IWebElement txtImporterDetails => _driver.WaitForElement(By.Id("importer"));
        private IWebElement txtplaceOfDestination => _driver.WaitForElement(By.Id("final-destination"));

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

            summary.CommodityCode = txtCommodityCode.Text.Trim();
            summary.TypeOfCommodity = txtCommodityType.Text.Trim();
            summary.Species = txtSpecies.Text.Trim();
            summary.NetWeight = txtNetWeight.Text.Trim().Replace(" kg/units", "");
            summary.NumberOfPackages = txtPackages.Text.Trim();
            summary.PackageType = txtPackageType.Text.Trim();
            summary.SubtotalNetWeight = txtSubtotalNetWeight.Text.Trim().Replace(" kg/units", "");
            summary.SubtotalPackages = txtSubtotalPackages.Text.Trim();
            summary.TotalNetWeight = txtTotalNetWeight.Text.Trim().Replace(" kg/units", "");
            summary.TotalPackages = txtTotalPackages.Text.Trim();
            summary.TotalGrossWeight = txtTotalGrossWeight.Text.Trim().Replace(" kg/units", "");
            summary.Temperature = txtTemperature.Text.Trim();

            summary.DocumentType = txtDocumentType.Text.Trim();
            summary.DocumentReference = txtDocumentReference.Text.Trim();
            summary.DateOfIssue = txtDateOfIssue.Text.Trim();
            summary.ApprovedEstablishmentName = txtApprovedEstablishmentName.Text.Trim();
            summary.ApprovedEstablishmentCountry = txtApprovedEstablishmentCountry.Text.Trim();
            summary.ApprovedEstablishmentType = txtApprovedEstablishmentType.Text.Trim();
            summary.ApprovedEstablishmentApprovalNum = txtApprovedEstablishmentApprovalNum.Text.Trim();

            summary.ConsignorDetails = txtConsignorDetails.Text.Trim();
            summary.ConsigneeDetails = txtConsigneeDetails.Text.Trim();
            summary.ImporterDetails = txtImporterDetails.Text.Trim();
            summary.PlaceOfDestination = txtplaceOfDestination.Text.Trim();


            /*//Inspector Pages
            summary.BCPRefNum = txtBCPRefNum.Text.Trim();
            summary.DocumentaryChk = txtDocumentaryChk.Text.Trim();
            summary.IdentityChkType = txtIdentityChkType.Text.Trim();
            summary.IdentityChkDecision = txtIdentityChkDecision.Text.Trim();
            summary.PhysicalChkDecision = txtPhysicalChkDecision.Text.Trim();
*/

            return summary;
        }
    }
}