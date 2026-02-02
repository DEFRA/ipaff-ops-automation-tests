using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools.PDFProcessor;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    /// <summary>
    /// Step definitions for Notification Certificate (PDF) validation
    /// </summary>
    [Binding]
    public class NotificationCertificateSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private INotificationCertificatePage? certificatePage =>
            _objectContainer.IsRegistered<INotificationCertificatePage>()
                ? _objectContainer.Resolve<INotificationCertificatePage>()
                : null;

        public NotificationCertificateSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [When("the user checks that the data in the certificate matches the data entered into the notification")]
        public void WhenTheUserChecksThatTheDataInTheCertificateMatchesTheDataEnteredIntoTheNotification()
        {
            var chedReference = _scenarioContext.Get<string>("CHEDReference");

            // Verify we're on the certificate page
            Assert.True(certificatePage?.IsPageLoaded(),
                "Certificate page is not loaded. Ensure the PDF certificate tab is active.");

            // Verify the URL contains the correct CHED reference
            Assert.True(certificatePage?.VerifyChedReferenceInUrl(chedReference),
                $"Certificate URL does not contain the expected CHED reference: {chedReference}");

            // Extract PDF content
            var extractionSuccess = certificatePage?.ExtractPdfContent() ?? false;
            Assert.True(extractionSuccess, "Failed to extract PDF content from certificate");

            // Validate PDF content against notification data
            var validationResult = certificatePage?.ValidatePdfContent();

            if (validationResult != null)
            {
                // Log the validation result for debugging
                Console.WriteLine(validationResult.ToString());

                // Store validation result in ScenarioContext
                _scenarioContext["PdfValidationResult"] = validationResult;

                // Assert validation passed (no errors)
                Assert.True(validationResult.IsValid,
                    $"PDF validation failed with errors:\n{string.Join("\n", validationResult.Errors)}");

                // Log any warnings
                if (validationResult.Warnings.Any())
                {
                    Console.WriteLine($"[PDF VALIDATION] Warnings:\n{string.Join("\n", validationResult.Warnings)}");
                }
            }
        }

        [Then("the PDF certificate should be displayed")]
        public void ThenThePdfCertificateShouldBeDisplayed()
        {
            Assert.True(certificatePage?.IsPageLoaded(),
                "PDF Certificate page is not loaded");
        }

        [Then("the PDF certificate should contain the CHED reference")]
        public void ThenThePdfCertificateShouldContainTheChedReference()
        {
            var expectedReference = _scenarioContext.Get<string>("CHEDReference");

            // Ensure PDF content is extracted
            if (!_scenarioContext.ContainsKey("ExtractedPdfContent"))
            {
                Assert.True(certificatePage?.ExtractPdfContent(),
                    "Failed to extract PDF content");
            }

            var pdfChedRef = certificatePage?.GetChedReference();

            Assert.IsNotNull(pdfChedRef, "Could not find CHED Reference in PDF");
            Assert.True(pdfChedRef!.Contains(expectedReference),
                $"PDF CHED Reference '{pdfChedRef}' does not contain expected reference '{expectedReference}'");

            Console.WriteLine($"[PDF VALIDATION] ✓ CHED Reference verified: {pdfChedRef}");
        }

        [Then("the PDF certificate should contain the CHED reference {string}")]
        public void ThenThePdfCertificateShouldContainTheChedReferenceValue(string expectedReference)
        {
            // Ensure PDF content is extracted
            if (!_scenarioContext.ContainsKey("ExtractedPdfContent"))
            {
                Assert.True(certificatePage?.ExtractPdfContent(),
                    "Failed to extract PDF content");
            }

            var pdfChedRef = certificatePage?.GetChedReference();

            Assert.IsNotNull(pdfChedRef, "Could not find CHED Reference in PDF");
            Assert.True(pdfChedRef!.Contains(expectedReference),
                $"PDF CHED Reference '{pdfChedRef}' does not contain expected reference '{expectedReference}'");

            Console.WriteLine($"[PDF VALIDATION] ✓ CHED Reference verified: {pdfChedRef}");
        }

        [Then("the PDF certificate should contain the country of origin {string}")]
        public void ThenThePdfCertificateShouldContainTheCountryOfOrigin(string expectedCountry)
        {
            // Ensure PDF content is extracted
            if (!_scenarioContext.ContainsKey("ExtractedPdfContent"))
            {
                Assert.True(certificatePage?.ExtractPdfContent(),
                    "Failed to extract PDF content");
            }

            var pdfCountry = certificatePage?.GetCountryOfOrigin();

            Assert.IsNotNull(pdfCountry, "Could not find Country of Origin in PDF");
            Assert.True(pdfCountry!.Contains(expectedCountry, StringComparison.OrdinalIgnoreCase),
                $"PDF Country of Origin '{pdfCountry}' does not contain expected country '{expectedCountry}'");

            Console.WriteLine($"[PDF VALIDATION] ✓ Country of Origin verified: {pdfCountry}");
        }

        [Then("the PDF certificate should contain the country of origin from the notification")]
        public void ThenThePdfCertificateShouldContainTheCountryOfOriginFromTheNotification()
        {
            var expectedCountry = _scenarioContext.Get<string>("CountryOfOrigin");

            // Ensure PDF content is extracted
            if (!_scenarioContext.ContainsKey("ExtractedPdfContent"))
            {
                Assert.True(certificatePage?.ExtractPdfContent(),
                    "Failed to extract PDF content");
            }

            var pdfCountry = certificatePage?.GetCountryOfOrigin();

            Assert.IsNotNull(pdfCountry, "Could not find Country of Origin in PDF");
            Assert.True(pdfCountry!.Contains(expectedCountry, StringComparison.OrdinalIgnoreCase),
                $"PDF Country of Origin '{pdfCountry}' does not contain expected country '{expectedCountry}'");

            Console.WriteLine($"[PDF VALIDATION] ✓ Country of Origin verified: {pdfCountry}");
        }

        [Then("the PDF certificate should contain the commodity code {string}")]
        public void ThenThePdfCertificateShouldContainTheCommodityCode(string expectedCode)
        {
            // Ensure PDF content is extracted
            if (!_scenarioContext.ContainsKey("ExtractedPdfContent"))
            {
                Assert.True(certificatePage?.ExtractPdfContent(),
                    "Failed to extract PDF content");
            }

            var pdfCommodity = certificatePage?.GetCommodityCode();

            Assert.IsNotNull(pdfCommodity, "Could not find Commodity Code in PDF");
            Assert.True(pdfCommodity!.Contains(expectedCode),
                $"PDF Commodity Code '{pdfCommodity}' does not contain expected code '{expectedCode}'");

            Console.WriteLine($"[PDF VALIDATION] ✓ Commodity Code verified: {pdfCommodity}");
        }

        [Then("the PDF certificate should contain the BCP or port of entry {string}")]
        public void ThenThePdfCertificateShouldContainTheBcpOrPortOfEntry(string expectedBcp)
        {
            // Ensure PDF content is extracted
            if (!_scenarioContext.ContainsKey("ExtractedPdfContent"))
            {
                Assert.True(certificatePage?.ExtractPdfContent(),
                    "Failed to extract PDF content");
            }

            var pdfBcp = certificatePage?.GetBcpOrPortOfEntry();

            Assert.IsNotNull(pdfBcp, "Could not find BCP/Port of Entry in PDF");

            // BCP names may be formatted differently, check for partial match
            var bcpKeywords = expectedBcp.Split(' ', '-').Where(w => w.Length > 3).ToArray();
            var matchCount = bcpKeywords.Count(k => pdfBcp!.Contains(k, StringComparison.OrdinalIgnoreCase));

            Assert.True(matchCount >= bcpKeywords.Length / 2,
                $"PDF BCP/Port of Entry '{pdfBcp}' does not match expected '{expectedBcp}'");

            Console.WriteLine($"[PDF VALIDATION] ✓ BCP/Port of Entry verified: {pdfBcp}");
        }

        [Then("the PDF validation result should have no errors")]
        public void ThenThePdfValidationResultShouldHaveNoErrors()
        {
            if (_scenarioContext.TryGetValue("PdfValidationResult", out PdfValidationResult? validationResult))
            {
                Assert.True(validationResult?.IsValid ?? false,
                    $"PDF validation has errors:\n{string.Join("\n", validationResult?.Errors ?? new List<string>())}");
            }
            else
            {
                Assert.Fail("No PDF validation result found. Ensure PDF validation was performed first.");
            }
        }

        [Then("the PDF validation result should contain validated field {string}")]
        public void ThenThePdfValidationResultShouldContainValidatedField(string fieldName)
        {
            if (_scenarioContext.TryGetValue("PdfValidationResult", out PdfValidationResult? validationResult))
            {
                Assert.True(validationResult?.ValidatedFields.ContainsKey(fieldName) ?? false,
                    $"PDF validation result does not contain validated field '{fieldName}'. " +
                    $"Validated fields: {string.Join(", ", validationResult?.ValidatedFields.Keys ?? Enumerable.Empty<string>())}");
            }
            else
            {
                Assert.Fail("No PDF validation result found. Ensure PDF validation was performed first.");
            }
        }

        [Then("the PDF certificate data should match the notification data")]
        public void ThenThePdfCertificateDataShouldMatchTheNotificationData()
        {
            var result = certificatePage?.VerifyPdfDataMatchesNotification() ?? false;
            Assert.True(result, "PDF certificate data does not match the notification data");
        }

        [When("the user extracts the PDF certificate content")]
        public void WhenTheUserExtractsThePdfCertificateContent()
        {
            Assert.True(certificatePage?.ExtractPdfContent(),
                "Failed to extract PDF certificate content");
        }

        [Then("the PDF content should be extracted successfully")]
        public void ThenThePdfContentShouldBeExtractedSuccessfully()
        {
            Assert.True(_scenarioContext.ContainsKey("ExtractedPdfContent"),
                "PDF content was not extracted or stored in ScenarioContext");
        }

        [Then("the PDF certificate should contain the consignor {string}")]
        public void ThenThePdfCertificateShouldContainTheConsignor(string expectedConsignor)
        {
            // Ensure PDF content is extracted
            if (!_scenarioContext.ContainsKey("ExtractedPdfContent"))
            {
                Assert.True(certificatePage?.ExtractPdfContent(),
                    "Failed to extract PDF content");
            }

            var pdfConsignor = certificatePage?.GetConsignorName();

            Assert.IsNotNull(pdfConsignor, "Could not find Consignor in PDF");
            Assert.True(pdfConsignor!.Contains(expectedConsignor, StringComparison.OrdinalIgnoreCase),
                $"PDF Consignor '{pdfConsignor}' does not contain expected '{expectedConsignor}'");

            Console.WriteLine($"[PDF VALIDATION] ✓ Consignor verified: {pdfConsignor}");
        }

        [Then("the PDF certificate should contain the consignee {string}")]
        public void ThenThePdfCertificateShouldContainTheConsignee(string expectedConsignee)
        {
            // Ensure PDF content is extracted
            if (!_scenarioContext.ContainsKey("ExtractedPdfContent"))
            {
                Assert.True(certificatePage?.ExtractPdfContent(),
                    "Failed to extract PDF content");
            }

            var pdfConsignee = certificatePage?.GetConsigneeName();

            Assert.IsNotNull(pdfConsignee, "Could not find Consignee in PDF");
            Assert.True(pdfConsignee!.Contains(expectedConsignee, StringComparison.OrdinalIgnoreCase),
                $"PDF Consignee '{pdfConsignee}' does not contain expected '{expectedConsignee}'");

            Console.WriteLine($"[PDF VALIDATION] ✓ Consignee verified: {pdfConsignee}");
        }

        [Then("the PDF certificate should contain the means of transport {string}")]
        public void ThenThePdfCertificateShouldContainTheMeansOfTransport(string expectedTransport)
        {
            // Ensure PDF content is extracted
            if (!_scenarioContext.ContainsKey("ExtractedPdfContent"))
            {
                Assert.True(certificatePage?.ExtractPdfContent(),
                    "Failed to extract PDF content");
            }

            var pdfTransport = certificatePage?.GetMeansOfTransport();

            Assert.IsNotNull(pdfTransport, "Could not find Means of Transport in PDF");
            Assert.True(pdfTransport!.Contains(expectedTransport, StringComparison.OrdinalIgnoreCase),
                $"PDF Means of Transport '{pdfTransport}' does not contain expected '{expectedTransport}'");

            Console.WriteLine($"[PDF VALIDATION] ✓ Means of Transport verified: {pdfTransport}");
        }
    }
}