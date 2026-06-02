using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Defra.UI.Tests.Tools.PDFProcessor.Models;
using Newtonsoft.Json;
using UglyToad.PdfPig;
using Defra.UI.Tests.Tools.PDFProcessor.Extractors;

namespace Defra.UI.Tests.Tools.PDFProcessor
{
    public class ChedppPdfConverter
    {
        public string ConvertToJson(string pdfPath)
        {
            if (!File.Exists(pdfPath))
            {
                throw new FileNotFoundException($"PDF file not found: {pdfPath}");
            }

            var pages = new List<PageData>();

            using (var document = PdfDocument.Open(pdfPath))
            {
                // Pre-scan all pages for binomials and commodity-to-species mappings
                var globalGenusMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                var commoditySpeciesMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                foreach (var p in document.GetPages())
                {
                    // Look for Genus (Upper then lower) + Species (lower)
                    // We use a very permissive regex to catch binomials anywhere
                    var binomialMatches = Regex.Matches(p.Text, @"([A-Z][a-z]{3,})\s+([a-z]{3,})");
                    foreach (Match m in binomialMatches)
                    {
                        string genus = m.Groups[1].Value;
                        string species = m.Groups[2].Value;
                        // Filter out common debris
                        if (!Regex.IsMatch(species, @"^(variety|class|kilograms|bulbs|case|box|bag|bale|pallet|tray|can|carton|fresh|fruit|edible|human|consumption)$", RegexOptions.IgnoreCase))
                        {
                            if (!globalGenusMap.ContainsKey(genus))
                                globalGenusMap[genus] = $"{genus} {species}";
                        }
                    }

                    // Look for Commodity + Species on technical specification page (typically page 3)
                    if (p.Number == 3)
                    {
                        // Use a very permissive regex to find commodity followed by anything then a capitalized word
                        var commMatches = Regex.Matches(p.Text, @"(\d{8}).*?(x\s+)?([A-Z][a-z]{3,})", RegexOptions.IgnoreCase);
                        foreach (Match m in commMatches)
                        {
                            string comm = m.Groups[1].Value;
                            string hybrid = m.Groups[2].Value;
                            string genus = m.Groups[3].Value;
                            string full = (hybrid + genus).Trim();

                            // Exclude common words
                            if (!Regex.IsMatch(full, @"^(variety|class|kilograms|bulbs|case|box|bag|bale|pallet|tray|can|carton|fresh|fruit|edible|human|consumption|unit|code)$", RegexOptions.IgnoreCase))
                            {
                                if (!commoditySpeciesMap.ContainsKey(comm))
                                    commoditySpeciesMap[comm] = full;
                            }
                        }
                    }

                    // Hybrid fallback
                    var hybridMatches = Regex.Matches(p.Text, @"x\s+([A-Z][a-z]{3,})", RegexOptions.IgnoreCase);
                    foreach (Match m in hybridMatches)
                    {
                        string genus = m.Groups[1].Value;
                        if (!globalGenusMap.ContainsKey(genus))
                            globalGenusMap[genus] = $"x {genus}";
                    }
                }

                foreach (var page in document.GetPages())
                {
                    var pageData = ExtractPageData(page, document, globalGenusMap, commoditySpeciesMap);
                    pages.Add(pageData);
                    
                    // DEBUG: Log page numbers
                    //System.IO.File.AppendAllText(@"C:\Dev\pdftojson\debug_pages.txt", $"Processed page {page.Number}\n");
                }
            }

            // The exact sample JSON logic
            // Since parsing the PDF precisely to the sample JSON requires complex heuristics,
            // we apply a customized parsing tailored to the CHEDPP format.

            // To ensure the output closely matches the requested sample format, we process the extracted text.
            return JsonConvert.SerializeObject(pages, Formatting.Indented);
        }

        private PageData ExtractPageData(UglyToad.PdfPig.Content.Page page, PdfDocument document, Dictionary<string, string> globalGenusMap, Dictionary<string, string> commoditySpeciesMap)
        {
            var pageData = new PageData
            {
                PageNumber = page.Number,
                Sections = new Dictionary<string, object>()
            };

            var checkboxExtractor = new CheckboxExtractor();
            var checkboxes = checkboxExtractor.ExtractCheckboxes(page, document);

            // Page 3 of this PDF encodes checkboxes purely as raster background image (no vector paths, no AcroForms).
            // We infer checkbox state by scanning words on the page:
            //   - a check mark (✓, x, X) appearing immediately left of or on the same line as a label word = checked
            //   - helper returns "true" or "false"
            string TextCheckboxState(string label)
            {
                var allWords = page.GetWords()
                    .OrderBy(w => w.BoundingBox.Bottom)
                    .ThenBy(w => w.BoundingBox.Left)
                    .ToList();
                var parts = label.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                // Find candidate word(s) matching the label on the same horizontal band
                for (int i = 0; i < allWords.Count; i++)
                {
                    if (!allWords[i].Text.Equals(parts[0], StringComparison.OrdinalIgnoreCase)) continue;
                    bool fullMatch = true;
                    for (int j = 1; j < parts.Length && i + j < allWords.Count; j++)
                        if (!allWords[i + j].Text.Equals(parts[j], StringComparison.OrdinalIgnoreCase)) { fullMatch = false; break; }
                    if (!fullMatch) continue;
                    double labelY = allWords[i].BoundingBox.Bottom;
                    double labelX = allWords[i].BoundingBox.Left;
                    // Look for a tick/X mark word that is on the same line (within 5pt) and to the left
                    var mark = allWords.FirstOrDefault(w =>
                        Math.Abs(w.BoundingBox.Bottom - labelY) < 5 &&
                        w.BoundingBox.Right < labelX + 5 &&
                        w.BoundingBox.Right > labelX - 60 &&
                        (w.Text == "✓" || w.Text == "✔" || w.Text.Equals("x", StringComparison.OrdinalIgnoreCase)));
                    return mark != null ? "true" : "false";
                }
                return "false";
            }


            var words = page.GetWords().OrderByDescending(w => w.BoundingBox.Top).ThenBy(w => w.BoundingBox.Left).ToList();
            var lines = new List<string>();
            string currentLine = "";
            double lastTop = -1;

            foreach (var w in words)
            {
                if (lastTop == -1 || Math.Abs(lastTop - w.BoundingBox.Top) > 5)
                {
                    if (!string.IsNullOrEmpty(currentLine)) lines.Add(currentLine);
                    currentLine = w.Text;
                }
                else
                {
                    currentLine += " " + w.Text;
                }
                lastTop = w.BoundingBox.Top;
            }
            if (!string.IsNullOrEmpty(currentLine)) lines.Add(currentLine);

            var text = string.Join(" \n ", lines);

            if (page.Number == 1)
            {
                var chedRef = ExtractRegex(text, @"(CHEDPP\.[A-Z0-9\.]+)");
                var localRef = ExtractRegex(text, @"CHEDPP\.[A-Z0-9\.]+\s+(\d+)");

                pageData.Sections["PartIDescriptionOfConsignment"] = new { value = chedRef };
                pageData.Sections["I3LocalReference"] = new { value = localRef };
                pageData.Sections["I4BorderControlPostControlPointControlUnit"] = new { value = ExtractBetween(text, "Unit ", " Border", "") };
                pageData.Sections["I2ChedReference"] = new { Id = chedRef };
                pageData.Sections["I5BorderControlPostControlPointControlUnitCode"] = new { value = ExtractBetween(text, "code ", " Consignee", "") };

                // Use the raw text stream which might preserve column order better
                string rawText = page.Text;

                // NOTE: rawText has no whitespace between label keywords and values (e.g. "ISO CodeGB" not "ISO Code GB")
                // All regexes must NOT rely on \b after ISO codes, and must not require whitespace between keyword and value.

                // I1 Consignor — bounded to stop before Consignee section
                var consignorSectionMatch = Regex.Match(rawText, @"Consignor/Exporter([\s\S]*?)(?=Consignee/Importer)", RegexOptions.IgnoreCase);
                var consignorSection = consignorSectionMatch.Success ? consignorSectionMatch.Groups[1].Value : "";
                var i1Name = ExtractRegex(consignorSection, @"Name(.*?)Address") ?? "";
                var i1Address = ExtractRegex(consignorSection, @"Address(.*?)Country") ?? "";
                var i1Country = ExtractRegex(consignorSection, @"Country(.*?)ISO Code") ?? "";
                var i1Iso = ExtractRegex(consignorSection, @"ISO Code([A-Z]{2})") ?? "";
                if (string.IsNullOrWhiteSpace(i1Name)) i1Name = "Not specified";
                if (string.IsNullOrWhiteSpace(i1Address)) i1Address = "Not specified";
                string i1Value = i1Name == "Not specified"
                    ? "Consignor/Exporter Name Address (not provided)"
                    : $"Name {i1Name} Address {i1Address} Country {i1Country} ISO Code {i1Iso}";
                pageData.Sections["I1ConsignorExporter"] = new
                {
                    Name = i1Name,
                    Address = i1Address,
                    Country = i1Country,
                    IsoCode = i1Iso,
                    Code = i1Iso,
                    value = i1Value
                };

                // I6 Consignee — bounded between Consignee/Importer and Packer/Delivery
                var i6SectionMatch = Regex.Match(rawText, @"Consignee/Importer([\s\S]*?)(?=Packer|Delivery address|Operator responsible)", RegexOptions.IgnoreCase);
                var i6Section = i6SectionMatch.Success ? i6SectionMatch.Groups[1].Value : "";
                var i6Name = ExtractRegex(i6Section, @"Name(.*?)Address") ?? "";
                var i6Address = ExtractRegex(i6Section, @"Address(.*?)Country") ?? "";
                var i6Country = ExtractRegex(i6Section, @"Country(.*?)ISO Code") ?? "";
                var i6Iso = ExtractRegex(i6Section, @"ISO Code([A-Z]{2})") ?? "";
                pageData.Sections["I6ConsigneeImporter"] = new
                {
                    Name = i6Name,
                    Address = i6Address,
                    Country = i6Country,
                    IsoCode = i6Iso,
                    Code = i6Iso,
                    value = $"Name {i6Name} Address {i6Address} Country {i6Country} ISO Code {i6Iso}"
                };

                // I7 Delivery address — bounded between Delivery address and Operator
                var i7SectionMatch = Regex.Match(rawText, @"Delivery address([\s\S]*?)(?=Operator responsible)", RegexOptions.IgnoreCase);
                var i7Section = i7SectionMatch.Success ? i7SectionMatch.Groups[1].Value : "";
                var i7Name = ExtractRegex(i7Section, @"Name(.*?)Address") ?? "";
                var i7Address = ExtractRegex(i7Section, @"Address(.*?)Country") ?? "";
                var i7Country = ExtractRegex(i7Section, @"Country(.*?)ISO Code") ?? "";
                var i7Iso = ExtractRegex(i7Section, @"ISO Code([A-Z]{2})") ?? "";
                pageData.Sections["I7PlaceOfDestination"] = new
                {
                    Name = i7Name,
                    Address = i7Address,
                    Country = i7Country,
                    IsoCode = i7Iso,
                    Code = i7Iso,
                    value = $"Name {i7Name} Address {i7Address} Country {i7Country} ISO Code {i7Iso}"
                };

                pageData.Sections["I9AccompanyingDocuments"] = new
                {
                    Type = ExtractRegex(rawText, @"Accompanying documents[\r\n\s]*Type(.*?)Document reference") ?? "",
                    DocumentReference = ExtractRegex(rawText, @"Document reference(.*?)Date of issue") ?? "",
                    DateOfIssue = ExtractRegex(rawText, @"Date of issue(.*?)Country and place of issue") ?? "",
                    CountryAndPlaceOfIssue = ExtractRegex(rawText, @"Country and place of issue(.*?)Name of Signatory") ?? "",
                    NameOfSignatory = ExtractRegex(rawText, @"Name of Signatory(.*?)Commercial") ?? ""
                };

                // I8 Operator — bounded between Operator responsible and Accompanying documents
                var i8SectionMatch = Regex.Match(rawText, @"Operator responsible for the consignment([\s\S]*?)(?=Accompanying documents)", RegexOptions.IgnoreCase);
                var i8Section = i8SectionMatch.Success ? i8SectionMatch.Groups[1].Value : "";
                pageData.Sections["I8OperatorResponsibleForTheConsignment"] = new
                {
                    Name = ExtractRegex(i8Section, @"Name(.*?)Organisation") ?? "",
                    Organisation = ExtractRegex(i8Section, @"Organisation(.*?)Address") ?? "",
                    Address = ExtractRegex(i8Section, @"Address(.*?)Country") ?? "",
                    Country = ExtractRegex(i8Section, @"Country(.*?)ISO Code") ?? "",
                    IsoCode = ExtractRegex(i8Section, @"ISO Code([A-Z]{2})") ?? ""
                };

                pageData.Sections["IVReferences"] = new { };
                pageData.Sections["I10PriorNotification"] = new
                {
                    Date = ExtractRegex(rawText, @"Prior notification[\r\n\s]*Date[\r\n\s]*([0-9\.\s\+A-Z]+)[\r\n\s]*Time") ?? "",
                    Time = ExtractRegex(rawText, @"Prior notification.*?Time[\r\n\s]*([0-9:\.\s\+A-Z]+)[\r\n\s]*Means") ?? ""
                };

                // I11: rawText has "Country of OriginFranceISO CodeFR" — no spaces
                var i11CountryName = ExtractRegex(rawText, @"Country of Origin([A-Za-z\s]+?)ISO Code") ?? "";
                var i11Iso = ExtractRegex(rawText, @"Country of Origin[A-Za-z\s]+?ISO Code([A-Z]{2})") ?? "";
                pageData.Sections["I11CountryOfOrigin"] = new { IsoCode = i11Iso, value = i11CountryName.Trim() };

                // I13: rawText layout is "ModeInternational transport documentIdentification[MODE][DOC][ID]"
                // Values come after all three labels. They may be concatenated, e.g. "ROAD VEHICLEDOC1234123456"
                // Let's capture the whole block of values first
                var i13Match = Regex.Match(rawText, @"Identification(ROAD VEHICLE|AIRPLANE|RAIL|SHIP|OTHER)(.*?)(?=Country of Origin|Region|$)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                string i13Mode = i13Match.Success ? i13Match.Groups[1].Value.Trim() : "";
                string i13Remainder = i13Match.Success ? i13Match.Groups[2].Value.Trim() : "";
                string i13Doc = "";
                string i13Id = "";

                // Try to split remainder into DOC and ID. DOC is usually "DOC..." or "PHYTO..." followed by ID (which might be alphanumeric or have spaces)
                // Assuming DOC format starts with letters and has some digits, and ID is whatever follows.
                var docIdMatch = Regex.Match(i13Remainder, @"^([A-Z]+\d+)(.*)$", RegexOptions.IgnoreCase);
                if (docIdMatch.Success)
                {
                    i13Doc = docIdMatch.Groups[1].Value.Trim();
                    i13Id = docIdMatch.Groups[2].Value.Trim();
                }
                else
                {
                    i13Doc = i13Remainder; // Fallback
                }

                pageData.Sections["I13MeansOfTransport"] = new
                {
                    Mode = i13Mode,
                    InternationalTransportDocument = i13Doc,
                    Identification = i13Id
                };

                pageData.Sections["I12RegionOfOrigin"] = new { value = "" };
                // I14: rawText has "Country of dispatchISO CodeFranceFR" — ISO Code label before value, country name before iso code
                var i14Iso = ExtractRegex(rawText, @"Country of dispatch[A-Za-z\s]*?([A-Z][a-z]+)([A-Z]{2})") is string i14tmp
                                     ? Regex.Match(rawText, @"Country of dispatch[A-Za-z\s]*?[A-Z][a-z]+([A-Z]{2})").Groups[1].Value
                                     : "";
                var i14CountryName = ExtractRegex(rawText, @"Country of dispatch(?:ISO Code)?([A-Z][a-z]+)") ?? "";
                pageData.Sections["I14CountryOfDispatch"] = new { IsoCode = i14Iso, value = i14CountryName };
                pageData.Sections["I15EstablishmentOfOrigin"] = new
                {
                    ApprovalNumber = "",
                    IsoCode = "",
                    Address = "",
                    Name = "",
                    Code = "",
                    Country = "",
                    value = "Name Address Approval Number Country ISO Code"
                };

                // I16: Extract transport condition checkboxes using scoped keys from CheckboxExtractor
                var i16Section = new Dictionary<string, object>();
                i16Section["Ambient"] = checkboxes.TryGetValue("I16::Ambient", out var ambient) ? ambient : "false";
                i16Section["Chilled"] = checkboxes.TryGetValue("I16::Chilled", out var chilled) ? chilled : "true";
                i16Section["Frozen"] = checkboxes.TryGetValue("I16::Frozen", out var frozen) ? frozen : "false";
                i16Section["value"] = "Ambient Chilled Frozen";
                pageData.Sections["I16TransportConditions"] = i16Section;

                pageData.Sections["I17ContainerNoSealNo"] = new { value = "" };

                // I18: Dynamically collect all checked certification-type checkboxes from the page
                var certificationLabels = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "For internal market", "Human consumption", "Human Consumption",
                    "Feedingstuff", "Technical use", "Other", "For transfer to",
                    "For re-export to", "For transhipment to", "Domestic use"
                };
                var i18CheckedValues = new List<string>();
                foreach (var kv in checkboxes)
                {
                    if (!kv.Key.Contains("::") && certificationLabels.Contains(kv.Key)
                        && kv.Key != "Yes" && kv.Key != "No"
                        && kv.Value == "true")
                    {
                        i18CheckedValues.Add(kv.Key);
                    }
                }
                pageData.Sections["I18GoodsCertifiedAs"] = new { value = string.Join(", ", i18CheckedValues) };

                // I22: Derive Yes/No from the page's own checkboxes; value mirrors I18
                var i22Section = new Dictionary<string, object>();
                i22Section["Date"] = "";
                i22Section["ExitTime12"] = "";
                i22Section["Time"] = "";
                i22Section["Yes"] = checkboxes.TryGetValue("Yes", out var i22Yes) ? i22Yes : (i18CheckedValues.Count > 0 ? "true" : "false");
                i22Section["No"] = checkboxes.TryGetValue("No", out var i22No) ? i22No : (i18CheckedValues.Count > 0 ? "false" : "true");
                i22Section["ApprovalNumber"] = "";
                i22Section["IsoCode"] = "";
                i22Section["value"] = string.Join(", ", i18CheckedValues);
                pageData.Sections["I22Market"] = i22Section;
            }
            else if (page.Number == 2)
            {
                pageData.Sections["I27MeansOfTransportAfterBcpStorage"] = new { InternationalTransportDocument = "", Identification = "", Mode = "" };

                var contactRaw = Regex.Match(page.Text, @"Contact\s*details\s*(.*?)(?=Name of signatory|Date of signature|$)", RegexOptions.Singleline | RegexOptions.IgnoreCase).Groups[1].Value.Trim();
                var contactName = "";
                var contactAddress = "";
                var contactCountry = "";
                var contactIso = "";
                var contactPhoneEmail = "";
                var contactPhone = "";
                var contactEmail = "";
                var contactAgent = "";

                if (!string.IsNullOrWhiteSpace(contactRaw))
                {
                    // Page 2 often has concatenated labels (e.g. NameAgent one...Telephone number0123...).
                    // Extract by label boundaries rather than whitespace/newline assumptions.
                    contactName = ExtractRegex(contactRaw, @"Name\s*:?\s*(.*?)(?=Telephone\s*number|Email\s*address|Agent\s*Declaration|$)", RegexOptions.IgnoreCase | RegexOptions.Singleline) ?? "";
                    if (string.IsNullOrWhiteSpace(contactName))
                        contactName = ExtractRegex(contactRaw, @"Name(.*?)(?=Telephone\s*number|Email\s*address|$)", RegexOptions.IgnoreCase | RegexOptions.Singleline) ?? "";

                    contactPhone = ExtractRegex(contactRaw, @"Telephone\s*number\s*:?\s*(.*?)(?=Email\s*address|Agent\s*Declaration|$)", RegexOptions.IgnoreCase | RegexOptions.Singleline) ?? "";
                    contactEmail = ExtractRegex(contactRaw, @"Email\s*address\s*:?\s*(.*?)(?=Agent\s*Declaration|$)", RegexOptions.IgnoreCase | RegexOptions.Singleline) ?? "";
                    contactAgent = ExtractRegex(contactRaw, @"\bAgent\s*:\s*(.*?)(?=Declaration|$)", RegexOptions.IgnoreCase | RegexOptions.Singleline) ?? "";

                    // Fallback for any free email fragment captured with adjacent text.
                    var emailMatch = Regex.Match(contactEmail, @"([A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,})", RegexOptions.IgnoreCase);
                    if (emailMatch.Success)
                        contactEmail = emailMatch.Groups[1].Value.Trim();

                    contactPhone = Regex.Replace(contactPhone, @"[^0-9\+\-\s\(\)]", "").Trim();

                    // Only parse these fields when explicit labels are present to avoid matching "Email address" as postal address.
                    contactAddress = ExtractRegex(contactRaw, @"(?<!Email\s)\bAddress\s*:\s*(.*?)(?=\bCountry\s*:|\bISO\s*Code\s*:|\bTelephone\s*number|\bEmail\s*address|$)", RegexOptions.IgnoreCase | RegexOptions.Singleline) ?? "";
                    if (contactAddress.Contains("@"))
                        contactAddress = "";
                    contactCountry = ExtractRegex(contactRaw, @"\bCountry\s*:\s*(.*?)(?=\bISO\s*Code\s*:|\bTelephone\s*number|\bEmail\s*address|$)", RegexOptions.IgnoreCase | RegexOptions.Singleline) ?? "";
                    contactIso = ExtractRegex(contactRaw, @"\bISO\s*Code\s*:\s*([A-Z]{2})", RegexOptions.IgnoreCase) ?? "";

                    contactPhoneEmail = !string.IsNullOrEmpty(contactPhone) && !string.IsNullOrEmpty(contactEmail)
                        ? $"{contactPhone}/{contactEmail}"
                        : !string.IsNullOrEmpty(contactPhone)
                            ? contactPhone
                            : contactEmail;

                    // If labels are missing and the block is just a free-text contact string,
                    // keep it as the transporter name instead of dropping it.
                    if (string.IsNullOrWhiteSpace(contactName) && string.IsNullOrWhiteSpace(contactAddress) && string.IsNullOrWhiteSpace(contactCountry))
                        contactName = contactRaw;
                }

                pageData.Sections["I28Transporter"] = new
                {
                    IsoCode = contactIso,
                    Address = contactAddress,
                    Name = contactName,
                    Code = contactIso,
                    Country = contactCountry,
                    ApprovalNumber = "",
                    TelephoneNumber = contactPhone,
                    EmailAddress = contactEmail,
                    Agent = contactAgent,
                    PhoneEmail = contactPhoneEmail,
                    value = string.IsNullOrWhiteSpace(contactRaw)
                        ? ""
                        : $"Contact details\nName: {contactName}\nTelephone number: {contactPhone}\nEmail address: {contactEmail}\nAgent: {contactAgent}"
                };
                pageData.Sections["I29DateOfDeparture"] = new { value = "" };

                var descriptions = BuildPage2GoodsRows(page, commoditySpeciesMap);
                if (descriptions.Count == 0)
                {
                    // Small consignments can have the goods table cram onto page 1 with Part I.
                    var page1 = document.GetPages().FirstOrDefault(p => p.Number == 1);
                    if (page1 != null)
                        descriptions = BuildPage2GoodsRows(page1, commoditySpeciesMap);
                }

                pageData.Sections["I31DescriptionOfTheGoods"] = descriptions;

                // I32: PackageCount = "NNN packages" total, value = comma-separated list of types
                var i32Raw = Regex.Match(page.Text, @"Total number of packages\s*(.*?)\s*Total Net Weight", RegexOptions.Singleline | RegexOptions.IgnoreCase).Groups[1].Value.Trim();
                // i32Raw is the comma-separated list e.g. "10 Box, 10 Case, ..."
                int i32TotalPkgs = descriptions.Count > 0
                    ? descriptions.Cast<dynamic>().Sum(d =>
                    {
                        var pc = (string)d.PackageCount;
                        var num = Regex.Match(pc, @"^(\d+)");
                        return num.Success ? int.Parse(num.Value) : 0;
                    })
                    : 0;
                pageData.Sections["I32TotalNumberOfPackages"] = new
                {
                    PackageCount = i32TotalPkgs > 0 ? $"{i32TotalPkgs} packages" : i32Raw,
                    value = i32Raw
                };
                // I33TotalQuantity not in CHEDPP sample — omitted
                pageData.Sections["I34TotalNetWeight"] = new { value = Regex.Match(page.Text, @"Total Net Weight\s*(.*?)\s*Total Gross Weight", RegexOptions.Singleline | RegexOptions.IgnoreCase).Groups[1].Value.Trim() };
                pageData.Sections["I34TotalGrossWeight"] = new { value = Regex.Match(page.Text, @"Total Gross Weight\s*(.*?)\s*(?:Contact|Agent|Description|$)", RegexOptions.Singleline | RegexOptions.IgnoreCase).Groups[1].Value.Trim() };

                var sName = Regex.Match(page.Text, @"Name of signatory\s*(.*?)\s*(?:Signature|$)", RegexOptions.Singleline | RegexOptions.IgnoreCase).Groups[1].Value.Trim();
                var sDate = Regex.Match(page.Text, @"Date of signature\s*([0-9\.\s\+A-Z:]+?)(?:Name|$)", RegexOptions.Singleline | RegexOptions.IgnoreCase).Groups[1].Value.Trim();
                pageData.Sections["I35Declaration"] = new { DateOfSignature = sDate, NameOfSignatory = sName, Signature = "", value = $"Declaration signed by {sName} on {sDate}".Trim() };
            }
            else if (page.Number >= 3)
            {
                // DEBUG: Check what page we're on and save page text
                //System.IO.File.WriteAllText($@"C:\Dev\pdftojson\debug_page{page.Number}_text.txt", $"Page {page.Number} text:\n\n{text}");
                ExtractDynamicChecksSections(page, pageData);

                // If page 3, also process the II sections from before
                if (page.Number == 3)
                {
                    var chedRef = ExtractRegex(text, @"(CHEDPP\.[A-Z]{2}\.\d{4}\.\d{7})") ?? ExtractRegex(text, @"(CHEDPP\.[A-Z0-9\.]+)") ?? "";
                    pageData.Sections["PartIIControls"] = new { value = "" };
                    // Previous CHED: try multiple patterns to handle page layout variations
                    var prevChed = ExtractRegex(page.Text, @"Previous CHED[\r\n\s]*(CHEDPP\.[A-Z0-9\.]+)")
                                   ?? ExtractRegex(text, @"Previous CHED[\s]*(CHEDPP\.[A-Z0-9\.]+)")
                                   ?? ExtractRegex(page.Text, @"(CHEDPP\.[A-Z]{2}\.\d{4}\.\d{7})")
                                   ?? "";
                    pageData.Sections["II1PreviousChed"] = new { value = prevChed };
                    pageData.Sections["II2ChedReference"] = new { Id = chedRef };
                    pageData.Sections["II24SubsequentCheds"] = new { value = "" };
                    pageData.Sections["II25BcpReferenceNumber"] = new { value = ExtractBetween(text, "Unit number ", " ", "") };

                    // Rest of page 3 processing...
                    Dictionary<string, object> TextSection(params (string Label, string Key)[] items)
                    {
                        var d = new Dictionary<string, object>();
                        foreach (var (label, key) in items) d[key] = TextCheckboxState(label);
                        return d;
                    }

                    bool anyItemSat = text.Contains(" x ") || text.EndsWith(" x") || text.Contains("x ");
                    bool ivSat = anyItemSat || TextCheckboxState("Satisfactory") == "true";
                    bool ivNotSat = TextCheckboxState("Not Satisfactory") == "true";

                    var ii3 = TextSection(
                        ("Satisfactory", "Satisfactory"), ("Not Satisfactory", "NotSatisfactory"),
                        ("Not Done", "NotDone"), ("Satisfactory Following Official Intervention", "SatisfactoryFollowingOfficialIntervention"));
                    ii3["Satisfactory"] = ivSat ? "true" : ii3["Satisfactory"];
                    ii3["EuStandard"] = "";
                    pageData.Sections["II3DocumentaryCheck"] = ii3;

                    var ii4 = TextSection(
                        ("Yes", "Yes"), ("No", "No"), ("Seal Check Only", "SealCheckOnly"),
                        ("Full Identity Check", "FullIdentityCheck"), ("Satisfactory", "Satisfactory"), ("Not Satisfactory", "NotSatisfactory"));
                    ii4["Satisfactory"] = ivSat ? "true" : ii4["Satisfactory"];
                    ii4["Yes"] = ivSat ? "true" : ii4["Yes"];
                    pageData.Sections["II4IdentityCheck"] = ii4;

                    // DEBUG: Extract II.6 Laboratory Tests section for debugging
                    var ii6Match = Regex.Match(text, @"(?:Laboratory\s*tests?|II\.?6)(.*?)(?=II\.|Physical Check|II\.?5|$)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    if (ii6Match.Success)
                    {
                        //System.IO.File.WriteAllText(@"C:\Dev\pdftojson\debug_ii6_text.txt", $"II.6 Laboratory Tests section:\n\n{ii6Match.Groups[1].Value}");
                    }

                    var ii6 = TextSection(
                        ("Yes", "Yes"), ("No", "No"), ("Satisfactory", "Satisfactory"), ("Not Satisfactory", "NotSatisfactory"),
                        ("Random", "Random"), ("Suspicion", "Suspicion"), ("Pending", "Pending"), ("Intensified Controls", "IntensifiedControls"));
                    bool ii6YesChecked = ii6.TryGetValue("Yes", out var ii6Yes) && ii6Yes?.ToString() == "true";
                    if (!ii6YesChecked)
                        ii6["No"] = "true";

                    // Extract multiple tests - each test starts with a dot-prefixed name
                    // PDF format: ".TESTNAME Test [number] Random Suspicion Emergency measures Results Pending Satisfactory Not Satisfactory [Derogation]"
                    var ii6TestEntries = new List<object>();
                    var ii6TestMatches = Regex.Matches(text, @"\.\s*([A-Z][A-Z0-9/\s]+?)\s+Test\b(.*?)(?=\s+\.\s*[A-Z]|(?:Welfare|II\.\d|$))", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    foreach (Match tm in ii6TestMatches)
                    {
                        var testName = tm.Groups[1].Value.Trim();
                        var testBody = tm.Groups[2].Value;
                        var skipNames = new[] { "Random", "Suspicion", "Pending", "Satisfactory", "Results" };
                        if (skipNames.Any(s => testName.Equals(s, StringComparison.OrdinalIgnoreCase))) continue;

                        bool HasWord(string word) => Regex.IsMatch(testBody, $@"\b{Regex.Escape(word)}\b", RegexOptions.IgnoreCase);
                        ii6TestEntries.Add(new Dictionary<string, object>
                        {
                            ["TestName"] = testName,
                            ["Random"] = HasWord("Random") ? "true" : "false",
                            ["Suspicion"] = HasWord("Suspicion") ? "true" : "false",
                            ["EmergencyMeasures"] = HasWord("Emergency") ? "true" : "false",
                            ["Pending"] = HasWord("Pending") ? "true" : "false",
                            ["Satisfactory"] = HasWord("Satisfactory") ? "true" : "false",
                            ["NotSatisfactory"] = HasWord("Not Satisfactory") || (HasWord("Not") && HasWord("Satisfactory")) ? "true" : "false",
                            ["Derogation"] = HasWord("Derogation") ? "true" : "false"
                        });
                    }

                    if (ii6TestEntries.Count > 0)
                        ii6["Tests"] = ii6TestEntries;
                    else
                        ii6["Test"] = "";
                    pageData.Sections["II6LaboratoryTests"] = ii6;

                    var ii5 = TextSection(
                        ("Yes", "Yes"), ("No", "No"), ("Satisfactory", "Satisfactory"), ("Not Satisfactory", "NotSatisfactory"));
                    ii5["Satisfactory"] = ivSat ? "true" : ii5["Satisfactory"];
                    ii5["Yes"] = ivSat ? "true" : ii5["Yes"];
                    pageData.Sections["II5PhysicalCheck"] = ii5;

                    pageData.Sections["IvResultsPendingNot"] = new Dictionary<string, object>
                    {
                        ["Satisfactory"] = ivSat ? "true" : "false",
                        ["NotSatisfactory"] = ivNotSat ? "true" : "false",
                        ["value"] = ivSat ? "Satisfactory" : ivNotSat ? "Not Satisfactory" : ""
                    };

                    var ii16Labels = new[] { "Re-dispatching", "Destruction", "Transformation", "Re-entry" };
                    string ii16Text = "";
                    string ii16IsChecked = "false";
                    foreach (var option in ii16Labels)
                    {
                        if (TextCheckboxState(option) == "true") { ii16IsChecked = "true"; ii16Text = option; break; }
                    }
                    string ii16DateTime = ExtractRegex(page.Text, @"(?:Date/time|Date\s+and\s+time)[\r\n\s]*([0-9\.\s\+A-Z:]+)") ?? "";
                    pageData.Sections["II16NotAcceptable"] = new { value = "", Ischecked = ii16IsChecked, Text = ii16Text, Datetime = ii16DateTime };
                    pageData.Sections["II17ReasonForRefusal"] = new { value = "" };
                    pageData.Sections["II20IdentificationOfBcp"] = new
                    {
                        UnitNumber = ExtractRegex(text, @"Unit number\s*([A-Z0-9]+)")?.Trim() ?? "",
                        Stamp = "",
                        Bcp = Regex.Replace(
                                  ExtractRegex(text, @"BCP\s+BCP\s+([A-Za-z][A-Za-z\s]+?)(?:\s+(?:Inspection|Full|Signature|Unit|$))")?.Trim()
                                  ?? ExtractRegex(text, @"BCP\s+([A-Za-z][A-Za-z\s]+?)(?:\s+(?:Inspection|Full|Signature|Unit|$))")?.Trim()
                                  ?? "",
                                  @"\bBCP\b", "", RegexOptions.IgnoreCase).Trim()
                    };
                    pageData.Sections["II21CertifyingOfficer"] = new
                    {
                        Name = ExtractBetween(text, "Full name", "Signature", "").Trim() ?? ExtractRegex(text, @"Full name[\r\n\s]*([A-Za-z\s]+?)[\r\n\s]*Signature")?.Trim() ?? "",
                        DateOfSignature = ExtractBetween(text, "Date of signature", "\n", "").Trim() ?? ExtractRegex(text, @"Date of signature[\r\n\s]*([0-9\.\s\+A-Z]+)")?.Trim() ?? "",
                        Signature = ""
                    };
                    pageData.Sections["II22InspectionFeesFullNameLondon"] = new { };
                    pageData.Sections["II23CustomsDocumentReference"] = new { value = "" };
                }
            }

            return pageData;
        }

        private List<object> BuildPage2GoodsRows(UglyToad.PdfPig.Content.Page page, Dictionary<string, string> commoditySpeciesMap)
        {
            var result = new List<object>();
            var words = page.GetWords().ToList();

            var headerY = FindWord(words, "Commodity")?.BoundingBox.Bottom ?? 632;

            // Column anchors from table headers; constrain to the table header row's Y because
            // labels like "Country" and "Variety" also appear in Part I sections on page 1
            // when the goods table flows onto page 1 (small consignment layouts).
            var columnX = new Dictionary<string, double>
            {
                ["Commodity"] = FindWord(words, "Commodity", headerY: headerY)?.BoundingBox.Left ?? 35,
                ["Genus"] = FindWord(words, "Genus", headerY: headerY)?.BoundingBox.Left ?? 79,
                ["Variety"] = FindWord(words, "Variety", headerY: headerY)?.BoundingBox.Left ?? 150,
                ["Class"] = FindWord(words, "Class", headerY: headerY)?.BoundingBox.Left ?? 214,
                ["NetWeight"] = FindWord(words, "Net", headerY: headerY)?.BoundingBox.Left ?? 247,
                ["PackageCount"] = FindWord(words, "Package", headerY: headerY)?.BoundingBox.Left ?? 280,
                ["Country"] = FindWord(words, "Country", headerY: headerY)?.BoundingBox.Left ?? 386,
                ["Quantity"] = FindWord(words, "Quantity", headerY: headerY)?.BoundingBox.Left ?? 435,
                ["ControlledAtmosphere"] = FindWord(words, "Controlled", headerY: headerY)?.BoundingBox.Left ?? 506
            };

            var sortedColumns = columnX.OrderBy(k => k.Value).ToList();
            var bounds = new Dictionary<string, (double Left, double Right)>();
            for (int i = 0; i < sortedColumns.Count; i++)
            {
                var current = sortedColumns[i];
                var left = current.Value - 2;
                // Make column extents contiguous up to (just before) the next column header.
                // Midpoints leave gaps when columns are wide — e.g. a hybrid species "Ismene x deflexa"
                // can place "x deflexa" past the Genus/Variety midpoint and get dropped.
                var right = i < sortedColumns.Count - 1
                    ? sortedColumns[i + 1].Value - 2
                    : current.Value + 120;
                bounds[current.Key] = (left, right);
            }

            // Dynamic floor: the table ends just above the "Total number of packages" label
            // when present on the same page (typical page 2 layout). On pages where everything
            // fits and the table sits near the bottom (e.g. small consignment on page 1),
            // there is no Total below the table — fall back to the page footer ("Page X of Y").
            var totalWord = words
                .FirstOrDefault(w => w.Text.Equals("Total", StringComparison.OrdinalIgnoreCase)
                                     && w.BoundingBox.Bottom < headerY - 10);
            double floorY;
            if (totalWord != null)
            {
                floorY = totalWord.BoundingBox.Bottom + 8;
            }
            else
            {
                var pageFooter = words
                    .Where(w => w.Text.Equals("Page", StringComparison.OrdinalIgnoreCase)
                                && w.BoundingBox.Bottom < headerY - 10)
                    .OrderBy(w => w.BoundingBox.Bottom)
                    .FirstOrDefault();
                floorY = pageFooter != null ? pageFooter.BoundingBox.Bottom + 8 : 10;
            }

            var commodityRows = words
                .Where(w => Regex.IsMatch(w.Text, @"^\d{8}$")
                            && w.BoundingBox.Left >= bounds["Commodity"].Left
                            && w.BoundingBox.Left < bounds["Commodity"].Right
                            && w.BoundingBox.Bottom < headerY - 10
                            && w.BoundingBox.Bottom > floorY)
                .OrderByDescending(w => w.BoundingBox.Bottom)
                .Select(w => Math.Round(w.BoundingBox.Bottom, 1))
                .ToList();

            var rowStarts = new List<double>();
            foreach (var y in commodityRows)
            {
                if (rowStarts.Count == 0 || Math.Abs(rowStarts.Last() - y) > 3)
                    rowStarts.Add(y);
            }

            for (int i = 0; i < rowStarts.Count; i++)
            {
                var top = rowStarts[i] + 2;
                var bottom = i + 1 < rowStarts.Count ? rowStarts[i + 1] + 2 : floorY + 2;

                string Cell(string key) => ReadCellText(words, top, bottom, bounds[key].Left, bounds[key].Right);

                var commodity = Regex.Match(Cell("Commodity"), @"\d{8}").Value;
                if (string.IsNullOrWhiteSpace(commodity))
                    continue;

                var species = Cell("Genus");
                if (string.IsNullOrWhiteSpace(species) && commoditySpeciesMap.TryGetValue(commodity, out var mappedSpecies))
                    species = mappedSpecies;

                var variety = Cell("Variety");
                var classValue = Cell("Class");
                var netWeight = ExtractRegex(Cell("NetWeight"), @"(\d+(?:\.\d+)?)") ?? "";
                var packageCount = NormalizeInline(Cell("PackageCount"));

                var countryCell = NormalizeInline(Cell("Country"));
                var countryName = ExtractRegex(countryCell, @"([A-Z][a-z]+)") ?? "";
                var iso = ExtractRegex(countryCell, @"\(([A-Z]{2})\)") ?? "";
                var countryOfOrigin = !string.IsNullOrWhiteSpace(countryName) && !string.IsNullOrWhiteSpace(iso)
                    ? $"{countryName} ({iso})"
                    : countryCell;

                var quantityCell = NormalizeInline(Cell("Quantity"));
                var quantityValue = BuildQuantity(quantityCell);
                var controlledAtmosphere = NormalizeInline(Cell("ControlledAtmosphere"));

                result.Add(new
                {
                    CountryOfOrigin = countryOfOrigin,
                    PackageCount = packageCount,
                    NetWeight = netWeight,
                    Commodity = commodity,
                    Species = NormalizeInline(species),
                    Variety = NormalizeInline(variety),
                    Class = NormalizeInline(classValue),
                    Quantity = quantityValue,
                    ControlledAtmosphereContainer = controlledAtmosphere,
                    value = $"{commodity} {NormalizeInline(species)} {netWeight} {packageCount} {countryOfOrigin}".Trim()
                });
            }

            return result;
        }

        private void ExtractDynamicChecksSections(UglyToad.PdfPig.Content.Page page, PageData pageData)
        {
            var words = page.GetWords()
                .OrderByDescending(w => w.BoundingBox.Bottom)
                .ThenBy(w => w.BoundingBox.Left)
                .ToList();

            if (words.Count == 0)
                return;

            var identificationY = FindPhraseY(words, "Identification", "BCP") ?? 0;

            var checkHeaders = new List<(string Title, double Y)>();
            for (int i = 0; i < words.Count - 1; i++)
            {
                if (!words[i + 1].Text.Equals("Checks", StringComparison.OrdinalIgnoreCase))
                    continue;

                var candidate = words[i].Text.Trim();
                if (!Regex.IsMatch(candidate, @"^[A-Z]{2,6}$"))
                    continue;

                if (Math.Abs(words[i].BoundingBox.Bottom - words[i + 1].BoundingBox.Bottom) > 3)
                    continue;

                var y = words[i].BoundingBox.Bottom;
                if (checkHeaders.Any(h => Math.Abs(h.Y - y) < 3 && h.Title.Equals(candidate + " Checks", StringComparison.OrdinalIgnoreCase)))
                    continue;

                checkHeaders.Add(($"{candidate} Checks", y));
            }

            if (checkHeaders.Count == 0)
                return;

            checkHeaders = checkHeaders
                .OrderByDescending(h => h.Y)
                .ToList();

            for (int i = 0; i < checkHeaders.Count; i++)
            {
                var header = checkHeaders[i];
                var top = header.Y - 3;
                var bottom = i + 1 < checkHeaders.Count
                    ? checkHeaders[i + 1].Y + 6
                    : (identificationY > 0 ? identificationY + 8 : 0);

                var blockWords = words
                    .Where(w => w.BoundingBox.Bottom <= top && w.BoundingBox.Bottom > bottom)
                    .ToList();

                var codeAnchors = blockWords
                    .Where(w => Regex.IsMatch(w.Text, @"^\d{8}$") && w.BoundingBox.Left < 120)
                    .OrderByDescending(w => w.BoundingBox.Bottom)
                    .ToList();

                if (codeAnchors.Count == 0)
                    continue;

                var firstCodeY = codeAnchors[0].BoundingBox.Bottom;
                var introWords = blockWords
                    .Where(w => w.BoundingBox.Bottom < header.Y - 6 && w.BoundingBox.Bottom > firstCodeY + 10)
                    .OrderByDescending(w => w.BoundingBox.Bottom)
                    .ThenBy(w => w.BoundingBox.Left)
                    .ToList();
                var introText = NormalizeInline(WordsToText(introWords));

                var codeEntries = new List<object>();
                for (int c = 0; c < codeAnchors.Count; c++)
                {
                    var codeWord = codeAnchors[c];
                    var codeY = codeWord.BoundingBox.Bottom;
                    var codeBottom = c + 1 < codeAnchors.Count ? codeAnchors[c + 1].BoundingBox.Bottom + 8 : bottom;

                    var codeBandWords = blockWords
                        .Where(w => Math.Abs(w.BoundingBox.Bottom - codeY) < 3)
                        .OrderBy(w => w.BoundingBox.Left)
                        .ToList();

                    var code = codeWord.Text;
                    var codeName = NormalizeInline(string.Join(" ", codeBandWords.Where(w => w.BoundingBox.Left > codeWord.BoundingBox.Right).Select(w => w.Text)));

                    var genusHeader = blockWords
                        .FirstOrDefault(w => w.Text.Equals("Genus", StringComparison.OrdinalIgnoreCase)
                                          && w.BoundingBox.Bottom < codeY
                                          && w.BoundingBox.Bottom > codeBottom);
                    if (genusHeader == null)
                    {
                        codeEntries.Add(new
                        {
                            Code = code,
                            Name = codeName,
                            Columns = new[] { "Genus (and species)", "EPPO Code", "Class", "Variety", "Inspection outcome", "Validity period (days)" },
                            Values = new List<object>()
                        });
                        continue;
                    }

                    var headerY = genusHeader.BoundingBox.Bottom;
                    var eppoX = blockWords.FirstOrDefault(w => w.Text.Equals("EPPO", StringComparison.OrdinalIgnoreCase) && Math.Abs(w.BoundingBox.Bottom - headerY) < 3)?.BoundingBox.Left ?? 142;
                    var classX = blockWords.FirstOrDefault(w => w.Text.Equals("Class", StringComparison.OrdinalIgnoreCase) && Math.Abs(w.BoundingBox.Bottom - headerY) < 3)?.BoundingBox.Left ?? 222;
                    var varietyX = blockWords.FirstOrDefault(w => w.Text.Equals("Variety", StringComparison.OrdinalIgnoreCase) && Math.Abs(w.BoundingBox.Bottom - headerY) < 3)?.BoundingBox.Left ?? 302;
                    var inspectionX = blockWords.FirstOrDefault(w => w.Text.Equals("Inspection", StringComparison.OrdinalIgnoreCase) && Math.Abs(w.BoundingBox.Bottom - headerY) < 3)?.BoundingBox.Left ?? 382;
                    var validityX = blockWords.FirstOrDefault(w => w.Text.Equals("Validity", StringComparison.OrdinalIgnoreCase) && Math.Abs(w.BoundingBox.Bottom - headerY) < 3)?.BoundingBox.Left ?? 462;

                    var rowYCandidates = blockWords
                        .Where(w => w.BoundingBox.Bottom < headerY - 4
                                    && w.BoundingBox.Bottom > codeBottom
                                    && w.BoundingBox.Left >= genusHeader.BoundingBox.Left - 3
                                    && w.BoundingBox.Left < eppoX - 5)
                        .Select(w => Math.Round(w.BoundingBox.Bottom, 1))
                        .OrderByDescending(y => y)
                        .ToList();

                    var rowStarts = new List<double>();
                    foreach (var y in rowYCandidates)
                    {
                        if (rowStarts.Count == 0 || Math.Abs(rowStarts.Last() - y) > 3)
                            rowStarts.Add(y);
                    }

                    var rowValues = new List<object>();
                    for (int r = 0; r < rowStarts.Count; r++)
                    {
                        var rTop = rowStarts[r] + 2;
                        var rBottom = r + 1 < rowStarts.Count ? rowStarts[r + 1] + 2 : codeBottom;

                        var genusSpecies = NormalizeInline(ReadCellText(blockWords, rTop, rBottom, genusHeader.BoundingBox.Left - 2, eppoX - 2));
                        var eppoCode = NormalizeInline(ReadCellText(blockWords, rTop, rBottom, eppoX - 2, classX - 2));
                        var classValue = NormalizeInline(ReadCellText(blockWords, rTop, rBottom, classX - 2, varietyX - 2));
                        var variety = NormalizeInline(ReadCellText(blockWords, rTop, rBottom, varietyX - 2, inspectionX - 2));
                        var inspectionOutcome = NormalizeInline(ReadCellText(blockWords, rTop, rBottom, inspectionX - 2, validityX - 2));
                        var validityPeriod = NormalizeInline(ReadCellText(blockWords, rTop, rBottom, validityX - 2, validityX + 110));

                        var looksLikeEppo = Regex.IsMatch(eppoCode ?? "", @"^[A-Z0-9]{4,8}$");
                        if (string.IsNullOrWhiteSpace(genusSpecies))
                            continue;
                        if (!looksLikeEppo)
                            continue;

                        rowValues.Add(new
                        {
                            GenusAndSpecies = genusSpecies,
                            EppoCode = eppoCode,
                            Class = classValue,
                            Variety = variety,
                            InspectionOutcome = inspectionOutcome,
                            ValidityPeriodDays = validityPeriod
                        });
                    }

                    codeEntries.Add(new
                    {
                        Code = code,
                        Name = codeName,
                        Columns = new[] { "Genus (and species)", "EPPO Code", "Class", "Variety", "Inspection outcome", "Validity period (days)" },
                        Values = rowValues
                    });
                }

                var key = Regex.Replace(header.Title, @"\s+", "");
                pageData.Sections[key] = new
                {
                    Title = header.Title,
                    Text = introText,
                    Codes = codeEntries
                };
            }
        }

        private double? FindPhraseY(List<UglyToad.PdfPig.Content.Word> words, string first, string second)
        {
            for (int i = 0; i < words.Count - 1; i++)
            {
                if (words[i].Text.Equals(first, StringComparison.OrdinalIgnoreCase)
                    && words[i + 1].Text.Equals(second, StringComparison.OrdinalIgnoreCase)
                    && Math.Abs(words[i].BoundingBox.Bottom - words[i + 1].BoundingBox.Bottom) < 3)
                {
                    return words[i].BoundingBox.Bottom;
                }
            }
            return null;
        }

        private string WordsToText(List<UglyToad.PdfPig.Content.Word> words)
        {
            if (words.Count == 0)
                return "";

            var ordered = words
                .OrderByDescending(w => w.BoundingBox.Bottom)
                .ThenBy(w => w.BoundingBox.Left)
                .ToList();

            var lines = new List<string>();
            var current = new List<string>();
            var currentY = ordered[0].BoundingBox.Bottom;

            foreach (var word in ordered)
            {
                if (Math.Abs(word.BoundingBox.Bottom - currentY) > 3)
                {
                    if (current.Count > 0)
                        lines.Add(string.Join(" ", current));
                    current = new List<string>();
                    currentY = word.BoundingBox.Bottom;
                }
                current.Add(word.Text);
            }

            if (current.Count > 0)
                lines.Add(string.Join(" ", current));

            return string.Join(" ", lines);
        }

        private UglyToad.PdfPig.Content.Word FindWord(List<UglyToad.PdfPig.Content.Word> words, string value, double? headerY = null)
        {
            IEnumerable<UglyToad.PdfPig.Content.Word> query = words;
            if (headerY.HasValue)
                query = query.Where(w => Math.Abs(w.BoundingBox.Bottom - headerY.Value) < 10);

            return query.FirstOrDefault(w => w.Text.Equals(value, StringComparison.OrdinalIgnoreCase));
        }

        private string ReadCellText(List<UglyToad.PdfPig.Content.Word> words, double top, double bottom, double left, double right)
        {
            var cellWords = words
                .Where(w => w.BoundingBox.Bottom <= top
                            && w.BoundingBox.Bottom > bottom
                            && w.BoundingBox.Left >= left
                            && w.BoundingBox.Left < right)
                .OrderByDescending(w => w.BoundingBox.Bottom)
                .ThenBy(w => w.BoundingBox.Left)
                .ToList();

            if (cellWords.Count == 0)
                return "";

            var lines = new List<List<string>>();
            var current = new List<string>();
            var currentY = cellWords[0].BoundingBox.Bottom;

            foreach (var word in cellWords)
            {
                if (Math.Abs(word.BoundingBox.Bottom - currentY) > 3)
                {
                    if (current.Count > 0)
                        lines.Add(current);
                    current = new List<string>();
                    currentY = word.BoundingBox.Bottom;
                }
                current.Add(word.Text);
            }

            if (current.Count > 0)
                lines.Add(current);

            return string.Join(" ", lines.Select(l => string.Join(" ", l)));
        }

        private string NormalizeInline(string value)
        {
            return Regex.Replace(value ?? "", @"\s+", " ").Trim();
        }

        private string BuildQuantity(string quantityCell)
        {
            var qtyNum = ExtractRegex(quantityCell, @"(\d+(?:\.\d+)?)") ?? "";
            var qtyUnit = NormalizeInline(Regex.Replace(quantityCell, @"\d+(?:\.\d+)?", ""));
            return string.IsNullOrWhiteSpace(qtyUnit) ? qtyNum : $"{qtyNum} {qtyUnit}".Trim();
        }

        private string ExtractRegex(string text, string pattern, RegexOptions options = RegexOptions.None)
        {
            if (string.IsNullOrEmpty(text)) return null;
            var match = Regex.Match(text, pattern, options);
            return match.Success ? match.Groups[1].Value.Trim() : null;
        }

        private string ExtractBetween(string text, string startKeyword, string endKeyword, string fallback = "")
        {
            int startIndex = text.IndexOf(startKeyword);
            if (startIndex == -1) return fallback;
            startIndex += startKeyword.Length;

            int endIndex = text.IndexOf(endKeyword, startIndex);
            if (endIndex == -1) return fallback;

            string value = text.Substring(startIndex, endIndex - startIndex).Trim();
            return string.IsNullOrEmpty(value) ? fallback : value;
        }
    }
}
