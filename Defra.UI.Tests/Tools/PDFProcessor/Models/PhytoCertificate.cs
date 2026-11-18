namespace Defra.UI.Tests.Tools.PDFProcessor.Models
{
    /// <summary>
    /// Represents the data held in the numbered boxes of a Phytosanitary Certificate PDF
    /// </summary>
    public class PhytoCertificate
    {
        // 1 Name and address of exporter
        public string? ExporterDetails { get; set; }

        // 2 Phytosanitary Certificate
        public string? CertificateNumber { get; set; }

        // 3 Declared name and address of consignee
        public string? ConsigneeName { get; set; }
        public string? ConsigneeAddress { get; set; }

        // 4 Plant Protection Organisation of UNITED KINGDOM - To Plant Protection Organisation(s) of
        public string? DestinationCountry { get; set; }

        // 5 Place of origin
        public string? PlaceOfOrigin { get; set; }

        // 6 Declared means of conveyance
        public string? TransportMode { get; set; }

        // 7 Declared point of entry
        public string? PointOfEntry { get; set; }

        // 8 Distinguishing marks: number and description: name of produce botanical name of plants
        public string? CommodityDetails { get; set; }

        // 9 Quantity declared
        public string? QuantityDeclared { get; set; }

        // 11 Additional declaration
        public string? AdditionalDeclaration { get; set; }

        // 12 Treatment
        public string? Treatment { get; set; }

        // 13 Chemical (active ingredient)
        public string? Chemical { get; set; }

        // 14 Duration and temperature
        public string? DurationAndTemperature { get; set; }

        // 15 Concentration
        public string? Concentration { get; set; }

        // 16 Date
        public string? TreatmentDate { get; set; }

        // 17 Additional information
        public string? AdditionalInformation { get; set; }

        public string? PlaceOfIssue { get; set; }
        public string? DateOfIssue { get; set; }
        public string? NameOfAuthorisedOfficer { get; set; }

        public Dictionary<string, string> RawFields { get; set; } = new();
    }
}
