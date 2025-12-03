using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class ReviewYourNotificationSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IReviewYourNotificationPage? reviewPage => _objectContainer.IsRegistered<IReviewYourNotificationPage>() ? _objectContainer.Resolve<IReviewYourNotificationPage>() : null;
        private ISummaryPage? summaryPage => _objectContainer.IsRegistered<ISummaryPage>() ? _objectContainer.Resolve<ISummaryPage>() : null;

        public ReviewYourNotificationSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Review your notification page should be displayed")]
        public void ThenTheReviewYourNotificationPageShouldBeDisplayed()
        {
            Assert.True(reviewPage?.IsPageLoaded(), "Review your notification page not loaded");
        }

        [Then("the user verifies all the data displayed in review page")]
        public void ThenTheUserVerifiesAllTheDataDisplayedInReviewPage()
        {
            VerifyAboutTheConsignment();
            VerifyDescriptionOfTheGoods();
            VerifyDocuments();
            VerifyTraders();
        }

        private void VerifyAboutTheConsignment()
        {
            var summary = summaryPage?.GetSummaryDetails();
            var pageName = "Review your notification";

            var importType = _scenarioContext.Get<string>("ImportType");
            var countryOfOrigin = _scenarioContext.Get<string>("CountryOfOrigin");
            var contryFromWhereConsigned = _scenarioContext.Get<string>("ContryFromWhereConsigned");
            var consignmentConformToRegulatoryRequirements = _scenarioContext.Get<string>("ConsignmentConformToRegulatoryRequirements");
            var consignmentRefNum = _scenarioContext.Get<string>("ConsignmentReferenceNumber");
            var mainReasonForImport = _scenarioContext.Get<string>("MainReasonForImport");
            var purpose = _scenarioContext.Get<string>("Purpose");
            var riskCategory = _scenarioContext.Get<string>("RiskCategory");

            Assert.AreEqual(importType, summary?.ImportType, $"Import type is not matching in {pageName} page!");
            Assert.AreEqual(countryOfOrigin, summary?.CountryOfOrigin, $"Country of origin is not matching in {pageName} page!");
            Assert.AreEqual(contryFromWhereConsigned, summary?.ContryFromWhereConsigned, $"Country from where consigned is not matching in {pageName} page!");
            Assert.AreEqual(consignmentConformToRegulatoryRequirements, summary?.ConsignmentConformToRegulatoryRequirements, $"Consignment confirmation to regulatory requirements is not matching in {pageName} page!");
            Assert.AreEqual(consignmentRefNum, summary?.ConsignmentReferenceNumber, $"Consignment Reference Number is not matching in {pageName} page!");
            Assert.IsTrue(summary?.MainReasonForImport.ToUpper().Contains(mainReasonForImport.ToUpper()), $"Main reason for import is not matching in {pageName} page!");
            Assert.AreEqual(purpose.Replace(" ", "").ToUpper(), summary?.Purpose.Replace(" ", "").ToUpper(), $"Purpose is not matching in {pageName} page!");
            Assert.AreEqual(riskCategory.ToUpper(), summary?.RiskCategory.ToUpper(), $"Risk category is not matching in {pageName} page!");
        }

        private void VerifyDescriptionOfTheGoods()
        {
            var summary = summaryPage?.GetSummaryDetails();
            var pageName = "Review your notification";

            var commodityCode = _scenarioContext.Get<string>("CommodityCode");
            var typeOfCommodity = _scenarioContext.Get<string>("TypeOfCommodity");
            var species = _scenarioContext.Get<string>("Species");
            var netWeight = _scenarioContext.Get<string>("NetWeight");
            var packages = _scenarioContext.Get<string>("NumberOfPackages");
            var packageType = _scenarioContext.Get<string>("PackageType");
            var subtotalNetWeight = _scenarioContext.Get<string>("SubtotalNetWeight");
            var subtotalPackages = _scenarioContext.Get<string>("SubtotalPackages");
            var totalNetWeight = _scenarioContext.Get<string>("TotalNetWeight");
            var totalPackages = _scenarioContext.Get<string>("TotalPackages");
            var totalGrossWeight = _scenarioContext.Get<string>("TotalGrossWeight");
            var temperature = _scenarioContext.Get<string>("Temperature");

            Assert.AreEqual(commodityCode, summary?.CommodityCode, $"Commodity Code is not matching in {pageName} page!");
            Assert.AreEqual(typeOfCommodity, summary?.TypeOfCommodity, $"Type Of Commodity is not matching in {pageName} page!");
            Assert.AreEqual(species, summary?.Species, $"Species is not matching in {pageName} page!");
            Assert.AreEqual(netWeight, summary?.NetWeight, $"NetWeight is not matching in {pageName} page!");
            Assert.AreEqual(packages, summary?.NumberOfPackages, $"Number Of Packages is not matching in {pageName} page!");
            Assert.AreEqual(packageType, summary?.PackageType, $"Package Type is not matching in {pageName} page!");
            Assert.AreEqual(subtotalNetWeight, summary?.SubtotalNetWeight, $"Subtotal NetWeight is not matching in {pageName} page!");
            Assert.AreEqual(subtotalPackages, summary?.SubtotalPackages, $"Subtotal Packages is not matching in {pageName} page!");
            Assert.AreEqual(totalNetWeight, summary?.TotalNetWeight, $"Total NetWeight is not matching in {pageName} page!");
            Assert.AreEqual(totalPackages, summary?.TotalPackages, $"Total Packages is not matching in {pageName} page!");
            Assert.AreEqual(totalGrossWeight, summary?.TotalGrossWeight, $"Total Gross Weight is not matching in {pageName} page!");
            Assert.AreEqual(temperature, summary?.Temperature, $"Temperature is not matching in {pageName} page!");
        }

        private void VerifyDocuments()
        {
            var summary = summaryPage?.GetSummaryDetails();
            var pageName = "Review your notification";

            var documentType = _scenarioContext.Get<string>("DocumentType");
            var documentReference = _scenarioContext.Get<string>("DocumentReference");
            var dateOfIssue = _scenarioContext.Get<string>("DateOfIssue");
            var approvedEstablishmentName = _scenarioContext.Get<string>("ApprovedEstablishmentName");
            var approvedEstablishmentCountry = _scenarioContext.Get<string>("ApprovedEstablishmentCountry");
            var approvedEstablishmentType = _scenarioContext.Get<string>("ApprovedEstablishmentType");
            var approvedEstablishmentApprovalNum = _scenarioContext.Get<string>("ApprovedEstablishmentApprovalNum");

            Assert.AreEqual(documentType, summary?.DocumentType, $"Document Type is not matching in {pageName} page!");
            Assert.AreEqual(documentReference, summary?.DocumentReference, $"Document Reference is not matching in {pageName} page!");
            Assert.AreEqual(dateOfIssue, summary?.DateOfIssue, $"Date Of Issue is not matching in {pageName} page!");
            Assert.AreEqual(approvedEstablishmentName, summary?.ApprovedEstablishmentName, $"Approved Establishment Name is not matching in {pageName} page!");
            Assert.AreEqual(approvedEstablishmentCountry, summary?.ApprovedEstablishmentCountry, $"Approved Establishment Country is not matching in {pageName} page!");
            Assert.AreEqual(approvedEstablishmentType, summary?.ApprovedEstablishmentType, $"Approved Establishment Type is not matching in {pageName} page!");
            Assert.AreEqual(approvedEstablishmentApprovalNum, summary?.ApprovedEstablishmentApprovalNum, $"Approved Establishment Approval Number is not matching in {pageName} page!");
        }

        private void VerifyTraders()
        {
            var summary = summaryPage?.GetSummaryDetails();
            var pageName = "Review your notification";

            var consignorDetails = _scenarioContext.Get<string>("ConsignorDetails");
            var consigneeDetails = _scenarioContext.Get<string>("ConsigneeDetails");
            var importerDetails = _scenarioContext.Get<string>("ImporterDetails");
            var placeOfDestination = _scenarioContext.Get<string>("PlaceOfDestination");

            var actualConsignorDetails = summary?.ConsignorDetails.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var expectedConsignorDetails = consignorDetails.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var actualConsigneeDetails = summary?.ConsigneeDetails.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var expectedConsigneeDetails = consigneeDetails.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var actualImporterDetails = summary?.ImporterDetails.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var expectedImporterDetails = importerDetails.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var actualPlaceOfDestination = summary?.PlaceOfDestination.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");
            var expectedPlaceOfDestination = placeOfDestination.ToUpper().Replace(", ", ",").ReplaceLineEndings("\n");

            Assert.IsTrue(actualConsignorDetails.Contains(expectedConsignorDetails), $"Consignor Details is not matching in {pageName} page!");
            Assert.IsTrue(actualConsigneeDetails.Contains(expectedConsigneeDetails), $"Consignee Details is not matching in {pageName} page!");
            Assert.IsTrue(actualImporterDetails.Contains(expectedImporterDetails), $"Importer Details is not matching in {pageName} page!");
            Assert.IsTrue(actualPlaceOfDestination.Contains(expectedPlaceOfDestination), $"Place Of Destination is not matching in {pageName} page!");
        }
    }
}