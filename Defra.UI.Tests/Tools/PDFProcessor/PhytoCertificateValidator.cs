using System.Globalization;
using System.Text.RegularExpressions;
using Defra.UI.Tests.Tools.PDFProcessor.Models;

namespace Defra.UI.Tests.Tools.PDFProcessor
{
    /// <summary>
    /// Validates a <see cref="PhytoCertificate"/> against the data entered for the application.
    /// Expected values are keyed using the scenario context keys populated by the Exporter Portal steps.
    /// </summary>
    public static class PhytoCertificateValidator
    {
        public const string PlaceOfOriginText = "SEE SECTION 8 FOR DETAILS ON COUNTRIES OF ORIGIN";

        public static List<string> Validate(PhytoCertificate certificate, IDictionary<string, object?> expected)
        {
            var mismatches = new List<string>();

            string? Expected(string key) =>
                expected.TryGetValue(key, out var value) && !string.IsNullOrWhiteSpace(value?.ToString())
                    ? value!.ToString()!.Trim()
                    : null;

            // 1 Name and address of exporter - name on the first line, address on the remaining lines
            var exporterLines = (certificate.ExporterDetails ?? string.Empty)
                .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var actualExporterName = exporterLines.FirstOrDefault();
            var actualExporterAddress = string.Join(", ", exporterLines.Skip(1));

            var exporterName = Expected("ExporterName");
            if (exporterName == null || Normalise(exporterName) != Normalise(actualExporterName))
                mismatches.Add($"Box 1 (Exporter name): expected '{exporterName}', actual '{actualExporterName}'");

            var exporterAddress = Expected("ExporterAddress");
            if (exporterAddress == null || NormaliseSeparators(exporterAddress) != NormaliseSeparators(actualExporterAddress))
                mismatches.Add($"Box 1 (Exporter address): expected '{exporterAddress}', actual '{actualExporterAddress}'");

            // 2 Phytosanitary Certificate - UK/GB/E&W/<YEAR>/<Application Reference>
            var applicationReference = Expected("Phytosanitary Certificate Application_APHAReference");
            if (applicationReference != null)
            {
                var match = Regex.Match(certificate.CertificateNumber ?? string.Empty,
                    $@"^UK/GB/E&W/(?<year>\d{{4}})/{Regex.Escape(applicationReference)}$");
                if (!match.Success)
                    mismatches.Add($"Box 2 (Certificate number): expected 'UK/GB/E&W/<YEAR>/{applicationReference}', actual '{certificate.CertificateNumber}'");
                else if (DateTime.TryParseExact(certificate.DateOfIssue, "d MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var issued)
                         && issued.Year.ToString() != match.Groups["year"].Value)
                    mismatches.Add($"Box 2 (Certificate number): year '{match.Groups["year"].Value}' does not match date of issue '{certificate.DateOfIssue}'");
            }

            // 3 Declared name and address of consignee
            CompareEquals("Box 3 (Consignee name)", Expected("ExporterConsigneeName"), certificate.ConsigneeName, mismatches);
            var consigneeAddress = Expected("ExporterConsigneeAddress");
            if (consigneeAddress != null && NormaliseAddress(consigneeAddress) != NormaliseAddress(certificate.ConsigneeAddress))
                mismatches.Add($"Box 3 (Consignee address): expected '{consigneeAddress}', actual '{certificate.ConsigneeAddress}'");

            // 4 Plant Protection Organisation of UNITED KINGDOM - destination country
            CompareEquals("Box 4 (Destination country)", Expected("ExporterDestinationCountry"), certificate.DestinationCountry, mismatches);

            // 5 Place of origin
            CompareEquals("Box 5 (Place of origin)", PlaceOfOriginText, certificate.PlaceOfOrigin, mismatches);

            // 6 Declared means of conveyance - e.g. 'Road or ferry' is displayed as 'ROAD'
            var transportMode = Expected("ExporterTransportMode");
            if (transportMode != null)
            {
                var actual = Normalise(certificate.TransportMode);
                var expectedMode = Normalise(transportMode);
                if (actual.Length == 0 || !(expectedMode.Contains(actual) || actual.Contains(expectedMode)))
                    mismatches.Add($"Box 6 (Means of conveyance): expected '{transportMode}', actual '{certificate.TransportMode}'");
            }

            // 7 Declared point of entry
            var pointOfEntry = Expected("ExporterPointOfEntry");
            if (pointOfEntry != null && !Normalise(certificate.PointOfEntry).Contains(Normalise(pointOfEntry)))
                mismatches.Add($"Box 7 (Point of entry): expected to contain '{pointOfEntry}', actual '{certificate.PointOfEntry}'");

            // 8 Commodity details
            ValidateCommodityDetails(certificate, Expected, mismatches);

            // 9 Quantity declared - in kilograms to 3 decimal places
            var quantityKg = GetQuantityInKilograms(Expected("SeedQuantity"), Expected("SeedUnit"));
            if (quantityKg != null)
            {
                var expectedQuantity = quantityKg.Value.ToString("0.000", CultureInfo.InvariantCulture);
                if (!Regex.IsMatch(Normalise(certificate.QuantityDeclared), $@"^{Regex.Escape(expectedQuantity)}( KILOGRAMS)?$"))
                    mismatches.Add($"Box 9 (Quantity declared): expected '{expectedQuantity} KILOGRAMS', actual '{certificate.QuantityDeclared}'");
            }

            // 11 - 17 fields filled with X's
            ValidateFilledWithX("Box 11 (Additional declaration)", certificate.AdditionalDeclaration, mismatches);
            ValidateFilledWithX("Box 12 (Treatment)", certificate.Treatment, mismatches);
            ValidateFilledWithX("Box 13 (Chemical)", certificate.Chemical, mismatches);
            ValidateFilledWithX("Box 14 (Duration and temperature)", certificate.DurationAndTemperature, mismatches);
            ValidateFilledWithX("Box 15 (Concentration)", certificate.Concentration, mismatches);
            ValidateFilledWithX("Box 16 (Date)", certificate.TreatmentDate, mismatches);
            ValidateFilledWithX("Box 17 (Additional information)", certificate.AdditionalInformation, mismatches);

            return mismatches;
        }

        /// <summary>
        /// Box 8 format: '&lt;name&gt; (&lt;EPPO code&gt;), &lt;variety&gt;, &lt;origin&gt;, &lt;quantity&gt;,&lt;units&gt;, &lt;packages&gt; &lt;packaging&gt;, lot: &lt;lot&gt;'
        /// </summary>
        private static void ValidateCommodityDetails(PhytoCertificate certificate, Func<string, string?> expected, List<string> mismatches)
        {
            var commodity = expected("Commodity");
            if (commodity == null)
                return;

            var quantity = decimal.TryParse(expected("SeedQuantity"), NumberStyles.Number, CultureInfo.InvariantCulture, out var q)
                ? q.ToString("0.000", CultureInfo.InvariantCulture)
                : expected("SeedQuantity");

            var parts = new[]
            {
                expected("SeedVariety"),
                expected("SeedCountriesOfOrigin"),
                quantity,
                expected("SeedUnit"),
                JoinNonEmpty(" ", expected("SeedNumberOfPackages"), expected("SeedPackaging")),
                expected("SeedLotReference") is { } lot ? $"LOT: {lot}" : null
            };

            var expectedRemainder = NormaliseSeparators(JoinNonEmpty(", ", parts));
            var eppoCode = expected("EppoCode");
            var eppoPart = eppoCode != null ? $" ({NormaliseSeparators(eppoCode)})" : string.Empty;
            var pattern = $@"^(\d+\)\s*)?{Regex.Escape(NormaliseSeparators(commodity) + eppoPart)} {Regex.Escape(expectedRemainder)}$";

            var actual = NormaliseSeparators(certificate.CommodityDetails);
            if (!Regex.IsMatch(actual, pattern))
            {
                var eppoDisplay = eppoCode != null ? $" ({eppoCode})" : string.Empty;
                var expectedDisplay = $"{commodity}{eppoDisplay}, {JoinNonEmpty(", ", parts)}".ToUpperInvariant();
                mismatches.Add($"Box 8 (Commodity details): expected '{expectedDisplay}', actual '{certificate.CommodityDetails}'");
            }
        }

        private static decimal? GetQuantityInKilograms(string? quantity, string? unit)
        {
            if (!decimal.TryParse(quantity, NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
                return null;

            return Normalise(unit) switch
            {
                "GRAMS" or "GRAM" or "G" => value / 1000m,
                "TONNES" or "TONNE" or "T" => value * 1000m,
                _ => value
            };
        }

        private static void ValidateFilledWithX(string label, string? actual, List<string> mismatches)
        {
            if (string.IsNullOrWhiteSpace(actual) || !Regex.IsMatch(actual, @"^[X\s]+$"))
                mismatches.Add($"{label}: expected a field filled with X's, actual '{actual}'");
        }

        private static void CompareEquals(string label, string? expected, string? actual, List<string> mismatches)
        {
            if (expected != null && Normalise(expected) != Normalise(actual))
                mismatches.Add($"{label}: expected '{expected}', actual '{actual}'");
        }

        private static string JoinNonEmpty(string separator, params string?[] values) =>
            string.Join(separator, values.Where(v => !string.IsNullOrWhiteSpace(v)));

        private static string Normalise(string? value) =>
            Regex.Replace(value ?? string.Empty, @"\s+", " ").Trim().ToUpperInvariant();

        private static string NormaliseSeparators(string? value) =>
            Regex.Replace(value ?? string.Empty, @"\s*,\s*|\s+", " ").Trim().ToUpperInvariant();

        private static string NormaliseAddress(string? value) =>
            string.Join(", ", (value ?? string.Empty)
                .Split(new[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(Normalise));
    }
}
