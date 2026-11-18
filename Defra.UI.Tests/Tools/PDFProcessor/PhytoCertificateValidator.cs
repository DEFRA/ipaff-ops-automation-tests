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

        public static List<string> Validate(PhytoCertificate certificate, IDictionary<string, object?> expected, string? applicationReference)
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
            Check("Box 1 (Exporter name)", exporterName != null && Normalise(exporterName) == Normalise(actualExporterName), actualExporterName,
                $"Box 1 (Exporter name): expected '{exporterName}', actual '{actualExporterName}'", mismatches);

            var exporterAddress = Expected("ExporterAddress");
            Check("Box 1 (Exporter address)", exporterAddress != null && NormaliseSeparators(exporterAddress) == NormaliseSeparators(actualExporterAddress), actualExporterAddress,
                $"Box 1 (Exporter address): expected '{exporterAddress}', actual '{actualExporterAddress}'", mismatches);

            // 2 Phytosanitary Certificate - UK/GB/E&W/<YEAR>/<Application Reference>
            if (!string.IsNullOrWhiteSpace(applicationReference))
            {
                var match = Regex.Match(certificate.CertificateNumber ?? string.Empty,
                    $@"^UK/GB/E&W/(?<year>\d{{4}})/{Regex.Escape(applicationReference)}$");
                if (!match.Success)
                    mismatches.Add($"Box 2 (Certificate number): expected 'UK/GB/E&W/<YEAR>/{applicationReference}', actual '{certificate.CertificateNumber}'");
                else if (DateTime.TryParseExact(certificate.DateOfIssue, "d MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var issued)
                         && issued.Year.ToString() != match.Groups["year"].Value)
                    mismatches.Add($"Box 2 (Certificate number): year '{match.Groups["year"].Value}' does not match date of issue '{certificate.DateOfIssue}'");
                else
                    LogMatch("Box 2 (Certificate number)", certificate.CertificateNumber);
            }
            else
                LogSkipped("Box 2 (Certificate number)");

            // 3 Declared name and address of consignee
            CompareEquals("Box 3 (Consignee name)", Expected("ExporterConsigneeName"), certificate.ConsigneeName, mismatches);
            var consigneeAddress = Expected("ExporterConsigneeAddress");
            if (consigneeAddress == null)
                LogSkipped("Box 3 (Consignee address)");
            else
                Check("Box 3 (Consignee address)", NormaliseAddress(consigneeAddress) == NormaliseAddress(certificate.ConsigneeAddress), certificate.ConsigneeAddress,
                    $"Box 3 (Consignee address): expected '{consigneeAddress}', actual '{certificate.ConsigneeAddress}'", mismatches);

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
                Check("Box 6 (Means of conveyance)", actual.Length > 0 && (expectedMode.Contains(actual) || actual.Contains(expectedMode)), certificate.TransportMode,
                    $"Box 6 (Means of conveyance): expected '{transportMode}', actual '{certificate.TransportMode}'", mismatches);
            }
            else
                LogSkipped("Box 6 (Means of conveyance)");

            // 7 Declared point of entry
            var pointOfEntry = Expected("ExporterPointOfEntry");
            if (pointOfEntry != null)
                Check("Box 7 (Point of entry)", Normalise(certificate.PointOfEntry).Contains(Normalise(pointOfEntry)), certificate.PointOfEntry,
                    $"Box 7 (Point of entry): expected to contain '{pointOfEntry}', actual '{certificate.PointOfEntry}'", mismatches);
            else
                LogSkipped("Box 7 (Point of entry)");

            // 8 Commodity details
            ValidateCommodityDetails(certificate, Expected, mismatches);

            // 9 Quantity declared - in kilograms to 3 decimal places
            var (declaredQuantity, declaredUnit) = IsPotatoesCommodity(Expected)
                ? (Expected("PotatoQuantity"), Expected("PotatoUnit"))
                : IsPlantsCommodity(Expected)
                ? (GetPlantsQuantity(Expected), Expected("PlantUnit"))
                : (Expected("SeedQuantity"), Expected("SeedUnit"));
            var quantityKg = GetQuantityInKilograms(declaredQuantity, declaredUnit);
            if (quantityKg != null)
            {
                var expectedQuantity = quantityKg.Value.ToString("0.000", CultureInfo.InvariantCulture);
                Check("Box 9 (Quantity declared)", Regex.IsMatch(Normalise(certificate.QuantityDeclared), $@"^{Regex.Escape(expectedQuantity)}( KILOGRAMS)?$"), certificate.QuantityDeclared,
                    $"Box 9 (Quantity declared): expected '{expectedQuantity} KILOGRAMS', actual '{certificate.QuantityDeclared}'", mismatches);
            }
            else
                LogSkipped("Box 9 (Quantity declared)");

            // 11 Additional declaration - the selected declaration description when one was added, otherwise X's
            var declarationDescription = Expected("SelectedDeclarationDescription");
            if (declarationDescription != null)
                ValidateAdditionalDeclaration(certificate.AdditionalDeclaration, declarationDescription, mismatches);
            else
                ValidateFilledWithX("Box 11 (Additional declaration)", certificate.AdditionalDeclaration, mismatches);

            // 12 - 17 fields filled with X's
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
            {
                LogSkipped("Box 8 (Commodity details)");
                return;
            }

            if (IsPotatoesCommodity(expected))
            {
                ValidatePotatoesCommodityDetails(certificate, expected, mismatches);
                return;
            }

            if (IsPlantsCommodity(expected))
            {
                ValidatePlantsCommodityDetails(certificate, commodity, expected, mismatches);
                return;
            }

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
            var eppoDisplay = eppoCode != null ? $" ({eppoCode})" : string.Empty;
            var expectedDisplay = $"{commodity}{eppoDisplay}, {JoinNonEmpty(", ", parts)}".ToUpperInvariant();
            Check("Box 8 (Commodity details)", Regex.IsMatch(actual, pattern), certificate.CommodityDetails,
                $"Box 8 (Commodity details): expected '{expectedDisplay}', actual '{certificate.CommodityDetails}'", mismatches);
        }

        /// <summary>
        /// Plants and fresh produce Box 8 format: '&lt;name&gt;, &lt;origin&gt;, &lt;commodity type&gt;, &lt;packages&gt;, &lt;packaging&gt;, &lt;quantity&gt; &lt;units&gt;'.
        /// For draw down applications the quantity is the drawn down amount rather than the quantity originally entered.
        /// </summary>
        private static void ValidatePlantsCommodityDetails(PhytoCertificate certificate, string commodity, Func<string, string?> expected, List<string> mismatches)
        {
            var plantsQuantity = GetPlantsQuantity(expected);
            var quantity = decimal.TryParse(plantsQuantity, NumberStyles.Number, CultureInfo.InvariantCulture, out var q)
                ? q.ToString("0.000", CultureInfo.InvariantCulture)
                : plantsQuantity;

            var parts = new[]
            {
                commodity,
                expected("PlantCountriesOfOrigin"),
                expected("PlantCommodityType"),
                expected("PlantNumberOfPackages"),
                expected("PlantPackaging"),
                JoinNonEmpty(" ", quantity, expected("PlantUnit"))
            };

            var expectedDetails = JoinNonEmpty(", ", parts);
            var pattern = $@"^(\d+\)\s*)?{Regex.Escape(NormaliseSeparators(expectedDetails))}$";

            if (!Regex.IsMatch(NormaliseSeparators(certificate.CommodityDetails), pattern))
                mismatches.Add($"Box 8 (Commodity details): expected '{expectedDetails.ToUpperInvariant()}', actual '{certificate.CommodityDetails}'");
            else
                LogMatch("Box 8 (Commodity details)", certificate.CommodityDetails);
        }

        public const string PotatoBotanicalName = "Solanum tuberosum";
        private const string AdditionalDeclarationPrefix = "AD1 - ";
        private const string AdditionalDeclarationSuffixPattern = @"SEE ATTACHED \d+ SHEET\(S\) FOR DETAILS";

        /// <summary>
        /// Potatoes Box 8 format: '&lt;genus and species&gt;, &lt;type&gt; POTATOES, &lt;origin&gt;, &lt;stock number&gt;, &lt;variety&gt;, &lt;chemical&gt;,
        /// &lt;quantity&gt; &lt;units&gt;, &lt;packages&gt;, &lt;packaging&gt;, &lt;packing material&gt;' followed by 'CONTAINER NUMBER: &lt;container&gt;'
        /// </summary>
        private static void ValidatePotatoesCommodityDetails(PhytoCertificate certificate, Func<string, string?> expected, List<string> mismatches)
        {
            var potatoQuantity = expected("PotatoQuantity");
            var quantity = decimal.TryParse(potatoQuantity, NumberStyles.Number, CultureInfo.InvariantCulture, out var q)
                ? q.ToString("0.000", CultureInfo.InvariantCulture)
                : potatoQuantity;

            var parts = new[]
            {
                PotatoBotanicalName,
                expected("PotatoType") is { } type ? $"{type} Potatoes" : null,
                expected("PotatoCountriesOfOrigin"),
                expected("PotatoStockNumber"),
                expected("PotatoVariety"),
                expected("PotatoChemicalUsed"),
                JoinNonEmpty(" ", quantity, expected("PotatoUnit")),
                expected("PotatoNumberOfPackages"),
                expected("PotatoPackaging"),
                expected("PotatoPackingMaterial")
            };

            var expectedDetails = JoinNonEmpty(", ", parts);
            var container = expected("ExporterContainerNumber");
            if (container != null)
                expectedDetails += $" Container Number: {container}";

            var pattern = $@"^(\d+\)\s*)?{Regex.Escape(NormaliseSeparators(expectedDetails))}$";
            Check("Box 8 (Commodity details)", Regex.IsMatch(NormaliseSeparators(certificate.CommodityDetails), pattern), certificate.CommodityDetails,
                $"Box 8 (Commodity details): expected '{expectedDetails.ToUpperInvariant()}', actual '{certificate.CommodityDetails}'", mismatches);
        }

        /// <summary>
        /// Box 11 shows the declaration description prefixed with 'AD1 - ' and suffixed with 'SEE ATTACHED n SHEET(S) FOR DETAILS'.
        /// The text is truncated on the certificate, so the remaining text must be the start of the selected declaration description.
        /// </summary>
        private static void ValidateAdditionalDeclaration(string? actual, string expectedDescription, List<string> mismatches)
        {
            const string label = "Box 11 (Additional declaration)";
            var text = NormaliseDeclaration(actual);
            if (text.StartsWith(Normalise(AdditionalDeclarationPrefix), StringComparison.Ordinal))
                text = text[Normalise(AdditionalDeclarationPrefix).Length..].Trim();
            text = Regex.Replace(text, $"{AdditionalDeclarationSuffixPattern}$", string.Empty).Trim();

            Check(label, text.Length > 0 && NormaliseDeclaration(expectedDescription).StartsWith(text, StringComparison.Ordinal), actual,
                $"{label}: expected the start of '{expectedDescription.ToUpperInvariant()}', actual '{actual}'", mismatches);
        }

        // The certificate PDF encodes typographic apostrophes as C1 control characters (e.g. U+0090)
        private static string NormaliseDeclaration(string? value) =>
            Normalise(Regex.Replace(value ?? string.Empty, "[\u2018\u2019\u02BC`\u0080-\u009F]", "'"));

        private static bool IsPotatoesCommodity(Func<string, string?> expected) =>
            expected("PotatoType") != null;

        private static bool IsPlantsCommodity(Func<string, string?> expected) =>
            expected("PlantCommodityType") != null;

        private static string? GetPlantsQuantity(Func<string, string?> expected) =>
            expected("DrawDownQuantity") ?? expected("PlantQuantity");

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
            else
                Console.WriteLine($"{LogPrefix} \u2713 {label}: filled with X's");
        }

        private static void CompareEquals(string label, string? expected, string? actual, List<string> mismatches)
        {
            if (expected == null)
                LogSkipped(label);
            else
                Check(label, Normalise(expected) == Normalise(actual), actual, $"{label}: expected '{expected}', actual '{actual}'", mismatches);
        }

        private const string LogPrefix = "[PHYTO CERTIFICATE VALIDATION]";

        private static void Check(string label, bool isMatch, string? actual, string mismatchMessage, List<string> mismatches)
        {
            if (isMatch)
                LogMatch(label, actual);
            else
                mismatches.Add(mismatchMessage);
        }

        private static void LogMatch(string label, string? actual) =>
            Console.WriteLine($"{LogPrefix} \u2713 {label}: '{Regex.Replace(actual ?? string.Empty, @"\s*\r?\n\s*", " ").Trim()}' matches");

        private static void LogSkipped(string label) =>
            Console.WriteLine($"{LogPrefix} \u2298 {label}: Skipped (not in context)");

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
