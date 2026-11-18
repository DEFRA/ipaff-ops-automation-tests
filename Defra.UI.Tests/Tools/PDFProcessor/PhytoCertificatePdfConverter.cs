using Defra.UI.Tests.Tools.PDFProcessor.Extractors;
using Defra.UI.Tests.Tools.PDFProcessor.Models;
using Newtonsoft.Json;
using UglyToad.PdfPig;

namespace Defra.UI.Tests.Tools.PDFProcessor
{
    /// <summary>
    /// Converts a Phytosanitary Certificate PDF into a <see cref="PhytoCertificate"/> model
    /// </summary>
    public class PhytoCertificatePdfConverter
    {
        private readonly PhytoCertificateFieldExtractor _fieldExtractor = new();

        public PhytoCertificate Convert(string pdfPath)
        {
            if (!File.Exists(pdfPath))
                throw new FileNotFoundException($"PDF file not found: {pdfPath}");

            using var document = PdfDocument.Open(pdfPath);
            var fields = _fieldExtractor.ExtractTextFields(document);

            string? Get(string key) => fields.TryGetValue(key, out var value) ? value : null;

            return new PhytoCertificate
            {
                ExporterDetails = Get("ExporterDetails"),
                CertificateNumber = Get("CertificateSerialNumberAuto"),
                ConsigneeName = Get("ConsigneeName"),
                ConsigneeAddress = Get("ConsigneeAddress"),
                DestinationCountry = Get("DestinationCountry"),
                PlaceOfOrigin = Get("PlaceOfOrigin"),
                TransportMode = Get("TransportMode"),
                PointOfEntry = Get("PointOfEntry"),
                CommodityDetails = Get("CommodityDetails"),
                QuantityDeclared = Get("QuantityDetails"),
                AdditionalDeclaration = Get("AdditionalDeclaration"),
                Treatment = Get("Treatment"),
                Chemical = Get("Chemical"),
                DurationAndTemperature = Get("Duration"),
                Concentration = Get("Concentration"),
                TreatmentDate = Get("TreatmentDate"),
                AdditionalInformation = Get("AdditionalInformation"),
                PlaceOfIssue = Get("PlaceOfIssue"),
                DateOfIssue = Get("DateOfIssue"),
                NameOfAuthorisedOfficer = Get("NameOfAuthorisedOfficer"),
                RawFields = fields
            };
        }

        public string ConvertToJson(string pdfPath) =>
            JsonConvert.SerializeObject(Convert(pdfPath), Formatting.Indented);
    }
}
