using Defra.UI.Tests.Tools.PDFProcessor.Extractors;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace Defra.UI.Tests.Tools.PDFProcessor.Models
{
    public partial class PdfToJsonConverter
    {
        private readonly CheckboxExtractor _checkboxExtractor;
        private readonly FormExtractor _formExtractor;

        private static readonly string[] KnownKeywords = new[] {
            "Country and place of issue", "Name", "Address", "ISO Code",
            "Organisation", "Type", "Document reference", "Date of issue", "Commercial documentary references",
            "Name of Signatory", "Date", "Time", "Approval Number",
            "Means of transport", "Identification", "International transport document", "Mode",
            "Total Gross Weight", "Total Net Weight", "Total number of packages", "Commodity",
            "Date of signature", "Signature", "Full name", "Net Weight", "Package Count",
            "Product Type", "Establishment of Origin", "Country of Origin", "Region of Origin",
            "Place of destination", "Consignor/Exporter", "Consignee/Importer", "Operator responsible for the consignment",
            "Accompanying documents", "Prior notification", "Country of dispatch", "Establishments of Origin",
            "Transport conditions", "Container No/Seal No", "Goods certified as", "Conformity of the goods",
            "For internal market", "Means of transport after BCP/storage", "Transporter", "Date of departure",
            "Description of the goods", "Declaration", "Previous CHED", "Subsequent CHEDs", "BCP Reference Number",
            "Documentary Check", "Identity Check", "Physical Check", "Laboratory tests", "Acceptable for internal market",
            "Identification of BCP", "Certifying officer", "Inspection fees", "Customs Document Reference",
            "Details on", "Follow up", "Official Inspector", "Local Reference", "Border Control Post/Control Point /Control Unit",
            "CHED Reference", "Border Control Post/Control Point /Control Unit code",
            "Stamp", "Unit number"
        }.OrderByDescending(k => k.Length).ToArray();

        private static readonly string[] BooleanFlags = new[] {
            "Ambient", "Chilled", "Frozen",
            "Conforming", "Non-conforming",
            "Human Consumption", "Animal Feeedingstuff", "Technical use", "Other",
            "EU Standard", "National Requirements",
            "Satisfactory", "Not Satisfactory", "Not Done",
            "Seal Check Only", "Full Identity Check",
            "Random", "Suspicion", "Intensified Controls", "Results Pending",
            "Validation", "Acceptable", "Refused"
        };

        public PdfToJsonConverter()
        {
            _checkboxExtractor = new CheckboxExtractor();
            _formExtractor = new FormExtractor();
        }

        public string ConvertToJson(string pdfPath)
        {
            if (!File.Exists(pdfPath))
            {
                throw new FileNotFoundException($"PDF file not found: {pdfPath}");
            }

            var pages = new List<PageData>();

            using (var document = PdfDocument.Open(pdfPath))
            {
                foreach (var page in document.GetPages())
                {
                    var pageData = ExtractPageData(page, document);
                    pages.Add(pageData);
                }
            }

            return JsonConvert.SerializeObject(pages, Formatting.Indented);
        }

        private PageData ExtractPageData(Page page, PdfDocument document)
        {
            var pageData = new PageData
            {
                PageNumber = page.Number
            };

            var words = page.GetWords().OrderBy(w => w.BoundingBox.Bottom).ThenBy(w => w.BoundingBox.Left).ToList();
            var formFields = _formExtractor.ExtractFormFields(page, document);
            var checkboxes = _checkboxExtractor.ExtractCheckboxes(page, document);

            pageData.Sections = ExtractSections(page, words, formFields, checkboxes);

            return pageData;
        }

        private Dictionary<string, object> ExtractSections(Page page, List<Word> words,
            Dictionary<string, string> formFields, Dictionary<string, string> checkboxes)
        {
            var sections = new Dictionary<string, object>();

            if (!words.Any())
                return sections;

            var sortedWords = words.OrderByDescending(w => w.BoundingBox.Top).ThenBy(w => w.BoundingBox.Left).ToList();
            var lines = GroupWordsIntoLines(sortedWords);
            var activeColumnSections = new List<WorkingSection>();

            for (int i = 0; i < lines.Count; i++)
            {
                var lineWords = lines[i];
                var lineText = string.Join(" ", lineWords.Select(w => w.Text)).Trim();

                if (string.IsNullOrWhiteSpace(lineText))
                    continue;

                if (lineText.Contains("GREAT BRITAIN") || (lineText.Contains("Page") && lineText.Contains("of") && lineText.Length < 20))
                    continue;

                var partIndex = lineText.IndexOf("PART I", StringComparison.OrdinalIgnoreCase);
                if (partIndex == -1) partIndex = lineText.IndexOf("PART II", StringComparison.OrdinalIgnoreCase);
                if (partIndex == -1) partIndex = lineText.IndexOf("PART III", StringComparison.OrdinalIgnoreCase);

                if (partIndex >= 0)
                {
                    var headerText = lineText.Substring(partIndex).Trim();

                    if (IsMainSectionHeader(headerText))
                    {
                        if (activeColumnSections.Any())
                        {
                            foreach (var section in activeColumnSections)
                            {
                                SaveSection(sections, section.Header, section.Content, formFields, checkboxes);
                            }
                            activeColumnSections.Clear();
                        }

                        activeColumnSections.Add(new WorkingSection { Header = headerText, StartX = 0 });
                        continue;
                    }
                }

                var subSectionsInLine = FindSubSectionsInLine(lineWords);

                if (subSectionsInLine.Count > 0)
                {
                    var sectionsToKeep = new List<WorkingSection>();
                    var newSections = new List<WorkingSection>();

                    foreach (var sub in subSectionsInLine)
                    {
                        var ws = new WorkingSection { Header = sub.Header, StartX = sub.StartX };
                        if (!string.IsNullOrWhiteSpace(sub.Content))
                        {
                            ws.Content.Add(sub.Content);
                        }
                        newSections.Add(ws);
                    }

                    bool isNewRow = newSections.Any(s => s.StartX < 100);
                    double minNewX = isNewRow ? newSections.Min(s => s.StartX) : 0;

                    foreach (var active in activeColumnSections)
                    {
                        bool isOverlapped = false;
                        foreach (var newSec in newSections)
                        {
                            if (Math.Abs(active.StartX - newSec.StartX) < 100)
                            {
                                isOverlapped = true;
                                break;
                            }
                        }

                        if (!isOverlapped && isNewRow && active.StartX > minNewX)
                        {
                            isOverlapped = true;
                        }

                        if (isOverlapped)
                        {
                            SaveSection(sections, active.Header, active.Content, formFields, checkboxes);
                        }
                        else
                        {
                            sectionsToKeep.Add(active);
                        }
                    }

                    activeColumnSections = sectionsToKeep;
                    activeColumnSections.AddRange(newSections);
                    activeColumnSections = activeColumnSections.OrderBy(s => s.StartX).ToList();
                }
                else if (activeColumnSections.Any())
                {
                    if (activeColumnSections.Count == 1)
                    {
                        activeColumnSections[0].Content.Add(lineText);
                    }
                    else
                    {
                        var sectionWords = new Dictionary<WorkingSection, List<string>>();
                        foreach (var s in activeColumnSections) sectionWords[s] = new List<string>();

                        foreach (var word in lineWords)
                        {
                            var targetSection = activeColumnSections.LastOrDefault(s => s.StartX <= word.BoundingBox.Left + 10);
                            if (targetSection == null) targetSection = activeColumnSections.First();
                            sectionWords[targetSection].Add(word.Text);
                        }

                        foreach (var s in activeColumnSections)
                        {
                            if (sectionWords[s].Any())
                            {
                                s.Content.Add(string.Join(" ", sectionWords[s]));
                            }
                        }
                    }
                }
            }

            if (activeColumnSections.Any())
            {
                foreach (var section in activeColumnSections)
                {
                    SaveSection(sections, section.Header, section.Content, formFields, checkboxes);
                }
            }

            return sections;
        }

        private List<SubSectionInfo> FindSubSectionsInLine(List<Word> lineWords)
        {
            var result = new List<SubSectionInfo>();
            var currentHeaderWords = new List<string>();
            var currentContentWords = new List<string>();
            double currentStartX = 0;

            var sectionStartRegex = new Regex(@"^((?:I{1,3}|IV)\.\d+(\.\d*)?\.?)");
            var splitHeaderPart1 = new Regex(@"^(I{1,3}|IV)\.?$");
            var splitHeaderPart2 = new Regex(@"^(\d+(\.\d*)?\.?)$");

            for (int i = 0; i < lineWords.Count; i++)
            {
                var word = lineWords[i];
                var wordText = word.Text;
                bool isSectionStart = false;
                string detectedHeader = "";

                var combinedMatch = sectionStartRegex.Match(wordText);
                if (combinedMatch.Success)
                {
                    isSectionStart = true;
                    var sectionPart = combinedMatch.Groups[1].Value;

                    if (wordText.Length > sectionPart.Length)
                    {
                        var suffix = wordText.Substring(sectionPart.Length);
                        detectedHeader = sectionPart + " " + suffix;
                    }
                    else
                    {
                        detectedHeader = wordText;
                    }

                    if (Regex.IsMatch(detectedHeader, @"^((?:I{1,3}|IV)\.\d+(?:\.\d+)?)(?![.\d])"))
                    {
                        detectedHeader = Regex.Replace(detectedHeader, @"^((?:I{1,3}|IV)\.\d+(?:\.\d+)?)", "$1.");
                    }
                }
                else if (i < lineWords.Count - 1 && splitHeaderPart1.IsMatch(wordText))
                {
                    var nextWord = lineWords[i + 1];
                    if (splitHeaderPart2.IsMatch(nextWord.Text))
                    {
                        isSectionStart = true;
                        detectedHeader = wordText + nextWord.Text;
                        if (!detectedHeader.Contains(".")) detectedHeader = wordText + "." + nextWord.Text;

                        i++;
                    }
                }

                if (isSectionStart)
                {
                    if (currentHeaderWords.Any())
                    {
                        result.Add(new SubSectionInfo
                        {
                            Header = string.Join(" ", currentHeaderWords),
                            Content = string.Join(" ", currentContentWords),
                            StartX = currentStartX
                        });
                        currentHeaderWords.Clear();
                        currentContentWords.Clear();
                    }

                    currentHeaderWords.Add(detectedHeader);
                    currentStartX = word.BoundingBox.Left;
                }
                else if (currentHeaderWords.Any())
                {
                    bool isPartIIorIII = currentHeaderWords.Any() && (currentHeaderWords[0].StartsWith("II.") || currentHeaderWords[0].StartsWith("III."));

                    bool isCheckboxLabel = wordText.Equals("Yes", StringComparison.OrdinalIgnoreCase)
                                        || (isPartIIorIII && wordText.Equals("No", StringComparison.OrdinalIgnoreCase))
                                        || wordText.Equals("Satisfactory", StringComparison.OrdinalIgnoreCase);

                    if (!isCheckboxLabel && currentHeaderWords.Count < 6 && Regex.IsMatch(wordText, @"^[A-Za-z/()]+$"))
                    {
                        currentHeaderWords.Add(wordText);
                    }
                    else
                    {
                        currentContentWords.Add(wordText);
                    }
                }
            }

            if (currentHeaderWords.Any())
            {
                result.Add(new SubSectionInfo
                {
                    Header = string.Join(" ", currentHeaderWords),
                    Content = string.Join(" ", currentContentWords),
                    StartX = currentStartX
                });
            }

            return result;
        }

        /// <summary>
        /// Saves a section with its extracted content
        /// </summary>
        private void SaveSection(Dictionary<string, object> sections, string sectionHeader,
            List<string> contentLines, Dictionary<string, string> formFields, Dictionary<string, string> checkboxes)
        {
            // Heuristic: Check if first line of content is a continuation of the header
            while (contentLines.Count > 0 && contentLines[0].Trim().StartsWith("/"))
            {
                sectionHeader += " " + contentLines[0].Trim();
                contentLines.RemoveAt(0);
            }

            // Cleanup header common artifacts
            if (sectionHeader.EndsWith(" ISO Code"))
            {
                sectionHeader = sectionHeader.Substring(0, sectionHeader.Length - 9).Trim();
            }
            if (sectionHeader.Contains("Full name Signature"))
            {
                sectionHeader = sectionHeader.Replace("Full name Signature", "").Trim();
            }
            if (sectionHeader.StartsWith("III.4", StringComparison.OrdinalIgnoreCase))
            {
                sectionHeader = "III.4 Details on re-dispatching";
            }
            if (sectionHeader.StartsWith("III.5", StringComparison.OrdinalIgnoreCase))
            {
                sectionHeader = "III.5 Follow up";
            }
            if (sectionHeader.StartsWith("III.6", StringComparison.OrdinalIgnoreCase))
            {
                sectionHeader = "III.6 Official Inspector";
            }

            // Special handling for I.31 to support array of objects for multi-row values
            if (sectionHeader.Contains("I.31", StringComparison.OrdinalIgnoreCase) &&
                sectionHeader.Contains("Description of the goods", StringComparison.OrdinalIgnoreCase))
            {
                var headerIndex = contentLines.FindIndex(l => l.Contains("Commodity", StringComparison.OrdinalIgnoreCase) && l.Contains("Net weight", StringComparison.OrdinalIgnoreCase));
                if (headerIndex > 0)
                {
                    var descLines = contentLines.Take(headerIndex).ToList();
                    var tableLines = contentLines.Skip(headerIndex + 1).ToList();

                    var descriptions = new List<string>();
                    var currentDesc = new StringBuilder();
                    foreach (var line in descLines)
                    {
                        if (Regex.IsMatch(line, @"^\d+\.\s"))
                        {
                            if (currentDesc.Length > 0) descriptions.Add(currentDesc.ToString().Trim());
                            currentDesc.Clear();
                        }
                        currentDesc.Append(line + " ");
                    }
                    if (currentDesc.Length > 0) descriptions.Add(currentDesc.ToString().Trim());

                    var dataRows = new List<string>();
                    foreach (var line in tableLines)
                    {
                        if (Regex.IsMatch(line.Trim(), @"^\d{4,10}\b"))
                        {
                            dataRows.Add(line.Trim());
                        }
                        else if (dataRows.Any())
                        {
                            dataRows[dataRows.Count - 1] += " " + line.Trim();
                        }
                    }

                    if (descriptions.Count > 0 && descriptions.Count == dataRows.Count)
                    {
                        var descItems = new List<Dictionary<string, object>>();
                        for (int i = 0; i < descriptions.Count; i++)
                        {
                            var combinedContent = new List<string> { descriptions[i], "Commodity Country of Origin Net weight Package count", dataRows[i] };
                            var chunkData = new Dictionary<string, object>();
                            ParseContent(chunkData, sectionHeader, combinedContent, formFields, checkboxes);

                            chunkData["Value"] = string.Join(" ", combinedContent);
                            var cleanChunk = new Dictionary<string, object>();
                            foreach (var kvp in chunkData) cleanChunk[SanitizePropertyName(kvp.Key)] = kvp.Value;
                            descItems.Add(cleanChunk);
                        }
                        sections[SanitizePropertyName(sectionHeader)] = descItems;
                        return;
                    }
                }

                var items = new List<Dictionary<string, object>>();
                var itemStartRegex = new Regex(@"^\d+\.\s");
                var currentChunk = new List<string>();
                bool hasNumbering = contentLines.Any(l => itemStartRegex.IsMatch(l));

                if (hasNumbering)
                {
                    foreach (var line in contentLines)
                    {
                        if (itemStartRegex.IsMatch(line))
                        {
                            if (currentChunk.Any())
                            {
                                var chunkData = new Dictionary<string, object>();
                                ParseContent(chunkData, sectionHeader, currentChunk, formFields, checkboxes);
                                var cleanChunk = new Dictionary<string, object>();
                                foreach (var kvp in chunkData) cleanChunk[SanitizePropertyName(kvp.Key)] = kvp.Value;
                                items.Add(cleanChunk);
                            }
                            currentChunk = new List<string>();
                        }
                        currentChunk.Add(line);
                    }
                }
                else
                {
                    currentChunk = new List<string>(contentLines);
                }

                if (currentChunk.Any())
                {
                    var chunkData = new Dictionary<string, object>();
                    ParseContent(chunkData, sectionHeader, currentChunk, formFields, checkboxes);

                    // Enhanced I.31 parsing for ImportType, Species, IncludesUnweanedAnimals
                    var fullText = string.Join(" ", currentChunk);
                    ExtractEnhancedCommodityFields(chunkData, fullText);

                    var cleanChunk = new Dictionary<string, object>();
                    foreach (var kvp in chunkData) cleanChunk[SanitizePropertyName(kvp.Key)] = kvp.Value;
                    items.Add(cleanChunk);
                }

                sections[SanitizePropertyName(sectionHeader)] = items;
                return;
            }

            // Special handling for I.9 Accompanying Documents - parse as table
            if (sectionHeader.Contains("I.9", StringComparison.OrdinalIgnoreCase) &&
                sectionHeader.Contains("Accompanying documents", StringComparison.OrdinalIgnoreCase))
            {
                var sectionData = new Dictionary<string, object>();
                var fullText = string.Join(" ", contentLines);

                // Parse documents table
                var documents = ParseAccompanyingDocuments(fullText);
                sectionData["Documents"] = documents;
                sectionData["Value"] = fullText;

                var cleanSectionData = new Dictionary<string, object>();
                foreach (var kvp in sectionData)
                {
                    cleanSectionData[SanitizePropertyName(kvp.Key)] = kvp.Value;
                }

                sections[SanitizePropertyName(sectionHeader)] = cleanSectionData;
                return;
            }

            var sectionDataGeneric = new Dictionary<string, object>();

            ParseContent(sectionDataGeneric, sectionHeader, contentLines, formFields, checkboxes);

            // Post-processing for I.28 Transporter - fix multi-line Address and Country
            if (sectionHeader.Contains("I.28", StringComparison.OrdinalIgnoreCase))
            {
                FixTransporterMultiLineFields(sectionDataGeneric, contentLines);
            }

            // Post-processing for I.1 Consignor - fix multi-line fields
            if (sectionHeader.Contains("I.1", StringComparison.OrdinalIgnoreCase) &&
                sectionHeader.Contains("Consignor", StringComparison.OrdinalIgnoreCase))
            {
                FixAddressMultiLineFields(sectionDataGeneric, contentLines, "I1");
            }

            // Post-processing for I.6 Consignee - fix multi-line fields
            if (sectionHeader.Contains("I.6", StringComparison.OrdinalIgnoreCase) &&
                sectionHeader.Contains("Consignee", StringComparison.OrdinalIgnoreCase))
            {
                FixAddressMultiLineFields(sectionDataGeneric, contentLines, "I6");
            }

            // Post-processing for I.7 Place of Destination - fix multi-line fields
            if (sectionHeader.Contains("I.7", StringComparison.OrdinalIgnoreCase) &&
                sectionHeader.Contains("Place of Destination", StringComparison.OrdinalIgnoreCase))
            {
                FixAddressMultiLineFields(sectionDataGeneric, contentLines, "I7");
            }

            var cleanSectionDataGeneric = new Dictionary<string, object>();
            foreach (var kvp in sectionDataGeneric)
            {
                cleanSectionDataGeneric[SanitizePropertyName(kvp.Key)] = kvp.Value;
            }

            var cleanHeader = SanitizePropertyName(sectionHeader);

            if (cleanSectionDataGeneric.Any())
            {
                sections[cleanHeader] = cleanSectionDataGeneric;
            }
            else
            {
                sections[cleanHeader] = new Dictionary<string, object>();
            }
        }

        /// <summary>
        /// Extracts enhanced commodity fields (ImportType, Species, IncludesUnweanedAnimals) from I.31 text
        /// </summary>
        private void ExtractEnhancedCommodityFields(Dictionary<string, object> chunkData, string fullText)
        {
            // Extract ImportType (e.g., "LIVE ANIMALS", "PRODUCTS")
            var importTypeMatch = Regex.Match(fullText, @"\d+\.\s+\d+\s+([A-Z\s]+?)(?=\s+\d{4})", RegexOptions.IgnoreCase);
            if (importTypeMatch.Success)
            {
                var importType = importTypeMatch.Groups[1].Value.Trim();
                // Clean up - remove numbers and extra spaces
                importType = Regex.Replace(importType, @"\s+", " ").Trim();
                if (!string.IsNullOrEmpty(importType))
                {
                    chunkData["ImportType"] = importType;
                }
            }

            // Extract Species from text (e.g., "Sus scrofa domesticus")
            var speciesMatch = Regex.Match(fullText, @"(?:0103|0101|0102|0104|0105|0106)\s+[A-Za-z\s]+?\s+([A-Z][a-z]+\s+[a-z]+(?:\s+[a-z]+)?)", RegexOptions.None);
            if (speciesMatch.Success)
            {
                chunkData["Species"] = speciesMatch.Groups[1].Value.Trim();
            }

            // Extract "Includes unweaned animals" - look for the text pattern
            var unweanedMatch = Regex.Match(fullText, @"Includes\s+unweaned\s+animals", RegexOptions.IgnoreCase);
            if (unweanedMatch.Success)
            {
                // Check if there's a Yes/No indicator or checkbox value nearby
                // For now, if the text mentions it, we note it exists
                // The actual value would come from checkbox detection
                if (!chunkData.ContainsKey("IncludesUnweanedAnimals"))
                {
                    // Default to checking if "No" appears after the text
                    var afterText = fullText.Substring(unweanedMatch.Index + unweanedMatch.Length);
                    chunkData["IncludesUnweanedAnimals"] = afterText.Contains("No") ? "false" : "true";
                }
            }

            // Extract Quantity/Number of animals
            var quantityMatch = Regex.Match(fullText, @"(\d+)\s+Units?\s+(\d+)", RegexOptions.IgnoreCase);
            if (quantityMatch.Success)
            {
                chunkData["Quantity"] = quantityMatch.Groups[1].Value;
                chunkData["NumberOfPackages"] = quantityMatch.Groups[2].Value;
            }
        }

        /// <summary>
        /// Parses the I.9 Accompanying Documents section into a structured list
        /// </summary>
        private List<Dictionary<string, string>> ParseAccompanyingDocuments(string fullText)
        {
            var documents = new List<Dictionary<string, string>>();

            // Pattern to match document entries:
            // "Veterinary health VHC12345 1 December certificate 2025"
            // "Commercial invoice INV12345 24 November 2025"

            // Split by known document types
            var documentTypes = new[] {
                "Veterinary health certificate",
                "Commercial invoice",
                "Import permit",
                "Phytosanitary certificate",
                "Air waybill",
                "Bill of lading",
                "Catch certificate",
                "Other"
            };

            // Try to find and parse each document
            var remainingText = fullText;

            foreach (var docType in documentTypes)
            {
                // Handle split document type names (e.g., "Veterinary health" ... "certificate")
                var typePattern = docType.Replace(" ", @"\s+");

                // Also try partial matches for split text
                var partialPatterns = new List<string> { typePattern };
                if (docType.Contains(" "))
                {
                    var words = docType.Split(' ');
                    for (int i = 1; i < words.Length; i++)
                    {
                        var partial = string.Join(@"\s+", words.Take(i)) + @"\s+\w+\s+" +
                                     string.Join(@"\s+", words.Skip(i));
                        partialPatterns.Add(partial);
                    }
                }

                foreach (var pattern in partialPatterns)
                {
                    // Pattern: DocType Reference Date
                    var docPattern = $@"({pattern})\s+([A-Z0-9]+)\s+(\d{{1,2}}\s+[A-Za-z]+\s+\d{{4}})";
                    var match = Regex.Match(remainingText, docPattern, RegexOptions.IgnoreCase);

                    if (match.Success)
                    {
                        documents.Add(new Dictionary<string, string>
                        {
                            { "Type", NormalizeDocumentType(match.Groups[1].Value) },
                            { "Reference", match.Groups[2].Value },
                            { "DateOfIssue", match.Groups[3].Value }
                        });
                        break;
                    }
                }
            }

            // If no structured parsing worked, try a more generic approach
            if (!documents.Any())
            {
                // Look for reference patterns followed by dates
                var genericPattern = @"([A-Z]{2,}[\w]*\d+)\s+(\d{1,2}\s+[A-Za-z]+\s+\d{4})";
                var matches = Regex.Matches(fullText, genericPattern);

                foreach (Match match in matches)
                {
                    documents.Add(new Dictionary<string, string>
                    {
                        { "Type", "Unknown" },
                        { "Reference", match.Groups[1].Value },
                        { "DateOfIssue", match.Groups[2].Value }
                    });
                }
            }

            return documents;
        }

        private string NormalizeDocumentType(string type)
        {
            // Remove extra whitespace and normalize
            type = Regex.Replace(type, @"\s+", " ").Trim();

            // Fix known split patterns
            if (type.Contains("Veterinary health") && type.Contains("certificate"))
            {
                return "Veterinary health certificate";
            }

            return type;
        }

        /// <summary>
        /// Fixes multi-line Address and Country fields for I.28 Transporter section
        /// </summary>
        private void FixTransporterMultiLineFields(Dictionary<string, object> sectionData, List<string> contentLines)
        {
            var fullText = string.Join(" ", contentLines);

            // Fix Address - capture everything between "Address" and "Country"
            var addressMatch = Regex.Match(fullText, @"Address\s+(.+?)\s+Country\s+", RegexOptions.IgnoreCase);
            if (addressMatch.Success)
            {
                var address = addressMatch.Groups[1].Value.Trim().TrimEnd(',');
                if (!string.IsNullOrEmpty(address))
                {
                    sectionData["Address"] = address;
                }
            }

            // Fix Country - capture everything between "Country" and "ISO Code"
            // Handle multi-line: "United Kingdom of" + "Great Britain and Northern Ireland"
            var countryMatch = Regex.Match(fullText, @"Country\s+(.+?)\s+ISO\s+Code\s+(\w{2,3})\s*(.*?)(?=\s+(?:Yes|No|Approval|Identification|$))",
                RegexOptions.IgnoreCase);

            if (countryMatch.Success)
            {
                var countryPart1 = countryMatch.Groups[1].Value.Trim();
                var isoCode = countryMatch.Groups[2].Value.Trim();
                var countryPart2 = countryMatch.Groups[3].Value.Trim();

                // Combine country parts
                var fullCountry = countryPart1;
                if (!string.IsNullOrEmpty(countryPart2))
                {
                    fullCountry = $"{countryPart1} {countryPart2}".Trim();
                }

                // Clean up the country name
                fullCountry = Regex.Replace(fullCountry, @"\s+", " ").Trim();

                if (!string.IsNullOrEmpty(fullCountry))
                {
                    sectionData["Country"] = fullCountry;
                }
                if (!string.IsNullOrEmpty(isoCode))
                {
                    sectionData["ISO Code"] = isoCode;
                }
            }
        }

        /// <summary>
        /// Fixes multi-line Address and Country fields for address sections (I.1, I.6, I.7)
        /// </summary>
        private void FixAddressMultiLineFields(Dictionary<string, object> sectionData, List<string> contentLines, string sectionPrefix)
        {
            var fullText = string.Join(" ", contentLines);

            // Fix Address - capture everything between "Address" and "Country"
            var addressMatch = Regex.Match(fullText, @"Address\s+(.+?)\s+Country\s+", RegexOptions.IgnoreCase);
            if (addressMatch.Success)
            {
                var address = addressMatch.Groups[1].Value.Trim().TrimEnd(',');
                if (!string.IsNullOrEmpty(address))
                {
                    sectionData["Address"] = address;
                }
            }

            // Fix Country - similar to transporter
            var countryMatch = Regex.Match(fullText, @"Country\s+(.+?)\s+ISO\s+Code\s+(\w{2,3}(?:-\w{3})?)",
                RegexOptions.IgnoreCase);

            if (countryMatch.Success)
            {
                var country = countryMatch.Groups[1].Value.Trim();
                var isoCode = countryMatch.Groups[2].Value.Trim();

                if (!string.IsNullOrEmpty(country))
                {
                    sectionData["Country"] = country;
                }
                if (!string.IsNullOrEmpty(isoCode))
                {
                    sectionData["ISO Code"] = isoCode;
                }
            }
        }

        private void ParseContent(Dictionary<string, object> sectionData, string sectionName, List<string> lines,
            Dictionary<string, string> formFields, Dictionary<string, string> checkboxes)
        {
            foreach (var checkbox in checkboxes.Where(c => IsFieldRelevantToSection(c.Key, sectionName)))
            {
                sectionData[checkbox.Key] = checkbox.Value;
            }

            var fullText = string.Join(" ", lines);
            var rawPairs = ExtractKeyValuePairs(lines);
            var keyValuePairs = rawPairs.ToDictionary(k => k.Key, v => (object)v.Value);

            var knownKeywords = new[] {
                "Country and place of issue", "Name", "Address", "ISO Code",
                "Organisation", "Type", "Document reference", "Date of issue", "Commercial documentary references",
                "Name of Signatory", "Date", "Time", "Approval Number",
                "Means of transport", "Identification", "International transport document", "Mode",
                "Total Gross Weight", "Total Net Weight", "Total number of packages", "Commodity",
                "Date of signature", "Signature", "Full name", "Net Weight", "Package Count",
                "Product Type", "Establishment of Origin", "Country of Origin", "Region of Origin",
                "Place of destination", "Consignor/Exporter", "Consignee/Importer", "Operator responsible for the consignment",
                "Accompanying documents", "Prior notification", "Country of dispatch", "Establishments of Origin",
                "Transport conditions", "Container No/Seal No", "Goods certified as", "Conformity of the goods",
                "For internal market", "Means of transport after BCP/storage", "Transporter", "Date of departure",
                "Description of the goods", "Declaration", "Previous CHED", "Subsequent CHEDs", "BCP Reference Number",
                "Documentary Check", "Identity Check", "Physical Check", "Laboratory tests", "Acceptable for internal market",
                "Identification of BCP", "Certifying officer", "Inspection fees", "Customs Document Reference",
                "Details on", "Follow up", "Official Inspector", "Local Reference", "Border Control Post/Control Point /Control Unit",
                "CHED Reference", "Border Control Post/Control Point /Control Unit code",
                "Stamp", "Unit number"
            }.OrderByDescending(k => k.Length).ToArray();

            foreach (var keyword in knownKeywords)
            {
                if (sectionName.Contains("Local Reference", StringComparison.OrdinalIgnoreCase))
                {
                    if (keyword == "Identification")
                    {
                        continue;
                    }
                }

                if (sectionName.Contains("I.8", StringComparison.OrdinalIgnoreCase) &&
                    sectionName.Contains("Operator responsible", StringComparison.OrdinalIgnoreCase))
                {
                    var allowedI8Fields = new[] {
                        "Name", "Address", "Organisation", "Country", "ISO Code"
                    };

                    if (!allowedI8Fields.Contains(keyword))
                    {
                        continue;
                    }
                }

                if (sectionName.Contains("I.9", StringComparison.OrdinalIgnoreCase) &&
                    sectionName.Contains("Accompanying documents", StringComparison.OrdinalIgnoreCase))
                {
                    var allowedI9Fields = new[] {
                        "Type", "Document reference", "Date of issue",
                        "Country and place of issue", "Name of Signatory",
                        "Commercial documentary references"
                    };

                    if (!allowedI9Fields.Contains(keyword))
                    {
                        continue;
                    }
                }

                if (sectionName.Contains("I.10", StringComparison.OrdinalIgnoreCase) &&
                    sectionName.Contains("Prior notification", StringComparison.OrdinalIgnoreCase))
                {
                    if (keyword != "Date" && keyword != "Time")
                    {
                        continue;
                    }
                }

                if (sectionName.Contains("I.13", StringComparison.OrdinalIgnoreCase) &&
                    sectionName.Contains("Means of transport", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (sectionName.Contains("I.34", StringComparison.OrdinalIgnoreCase))
                {
                    if (keyword == "ISO Code" || keyword == "Identification" || keyword == "Net Weight")
                    {
                        continue;
                    }
                }

                if (sectionName.Contains("I.35", StringComparison.OrdinalIgnoreCase))
                {
                    if (keyword == "Name" || keyword == "Date" || keyword == "ISO Code")
                    {
                        continue;
                    }
                }

                if (!keyValuePairs.ContainsKey(keyword))
                {
                    int idx = fullText.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);
                    if (idx != -1)
                    {
                        int startVal = idx + keyword.Length;
                        int endVal = fullText.Length;

                        foreach (var nextKw in knownKeywords)
                        {
                            if (nextKw.Equals(keyword, StringComparison.OrdinalIgnoreCase)) continue;

                            bool shouldSkipAsBoundary = false;

                            if (sectionName.Contains("I.8", StringComparison.OrdinalIgnoreCase) &&
                                sectionName.Contains("Operator responsible", StringComparison.OrdinalIgnoreCase))
                            {
                                if ((keyword == "Address" || keyword == "Country") && nextKw == "ISO Code")
                                {
                                    shouldSkipAsBoundary = true;
                                }
                            }

                            if (shouldSkipAsBoundary)
                            {
                                continue;
                            }

                            int nextIdx = fullText.IndexOf(nextKw, startVal, StringComparison.OrdinalIgnoreCase);
                            if (nextIdx != -1 && nextIdx < endVal)
                            {
                                endVal = nextIdx;
                            }
                        }

                        string val = fullText.Substring(startVal, endVal - startVal).Trim();
                        val = val.TrimStart(':', '-', ' ').Trim();
                        keyValuePairs[keyword] = val;
                    }
                }
            }

            var booleanFlags = new[] {
                "Ambient", "Chilled", "Frozen",
                "Conforming", "Non-conforming",
                "Human Consumption", "Animal Feeedingstuff", "Technical use", "Other",
                "EU Standard", "National Requirements",
                "Satisfactory", "Not Satisfactory", "Not Done",
                "Seal Check Only", "Full Identity Check",
                "Random", "Suspicion", "Intensified Controls", "Results Pending",
                "Validation", "Acceptable", "Refused"
            };

            foreach (var flag in booleanFlags)
            {
                if (sectionName.Contains("I.16", StringComparison.OrdinalIgnoreCase) ||
                    sectionName.Contains("I.18", StringComparison.OrdinalIgnoreCase) ||
                    sectionName.Contains("I.19", StringComparison.OrdinalIgnoreCase) ||
                    sectionName.Contains("II.3", StringComparison.OrdinalIgnoreCase) ||
                    sectionName.Contains("II.4", StringComparison.OrdinalIgnoreCase) ||
                    sectionName.Contains("II.5", StringComparison.OrdinalIgnoreCase) ||
                    sectionName.Contains("II.6", StringComparison.OrdinalIgnoreCase) ||
                    sectionName.Contains("II.12", StringComparison.OrdinalIgnoreCase) ||
                    sectionName.Contains("III.5", StringComparison.OrdinalIgnoreCase))
                {
                }
                else if (fullText.Contains(flag, StringComparison.OrdinalIgnoreCase))
                {
                    keyValuePairs[flag] = "true";
                }
            }

            var choiceGroups = new List<string[]> {
                new[] { "Ambient", "Chilled", "Frozen" },
                new[] { "Conforming", "Non-conforming" },
                new[] { "Satisfactory", "Not Satisfactory" },
                new[] { "Yes", "No" }
            };

            foreach (var group in choiceGroups)
            {
                bool groupActive = group.Any(g => fullText.Contains(g, StringComparison.OrdinalIgnoreCase));
                if (groupActive)
                {
                    foreach (var item in group)
                    {
                        if (sectionName.Contains("I.16", StringComparison.OrdinalIgnoreCase) ||
                            sectionName.Contains("I.19", StringComparison.OrdinalIgnoreCase) ||
                            sectionName.Contains("II.3", StringComparison.OrdinalIgnoreCase) ||
                            sectionName.Contains("II.4", StringComparison.OrdinalIgnoreCase) ||
                            sectionName.Contains("II.5", StringComparison.OrdinalIgnoreCase) ||
                            sectionName.Contains("II.6", StringComparison.OrdinalIgnoreCase) ||
                            sectionName.Contains("II.12", StringComparison.OrdinalIgnoreCase) ||
                            sectionName.Contains("III.5", StringComparison.OrdinalIgnoreCase) ||
                            sectionName.Contains("I.35", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        if (fullText.Contains(item, StringComparison.OrdinalIgnoreCase))
                            keyValuePairs[item] = "true";
                        else
                            keyValuePairs[item] = "false";
                    }
                }
            }

            var enumDefinitions = new Dictionary<string, string[]> {
                { "Mode", new[] { "Road vehicle", "Aeroplane", "Vessel", "Railway", "Flight" } },
                { "Type", new[] { "Veterinary health certificate", "Cold Stores", "Processing Plant", "Slaughterhouse" } }
            };

            foreach (var def in enumDefinitions)
            {
                if (sectionName.Contains("I.13", StringComparison.OrdinalIgnoreCase) &&
                    sectionName.Contains("Means of transport", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                foreach (var val in def.Value)
                {
                    if (fullText.Contains(val, StringComparison.OrdinalIgnoreCase))
                    {
                        if (!keyValuePairs.ContainsKey(def.Key) || (keyValuePairs[def.Key] is string sEnum && string.IsNullOrWhiteSpace(sEnum)))
                        {
                            keyValuePairs[def.Key] = val.ToUpper();
                            if (def.Key == "Type") keyValuePairs[def.Key] = val;
                        }
                    }
                }
            }

            var regexFillers = new Dictionary<string, string> {
                { "International transport document", @"\b(DOC\w*\d+\w*)\b" },
                { "Identification", @"\b(\d{5,})\b" },
                { "Approval Number", @"\b(\d+\.\d+\.\d+)\b" },
                { "ISO Code", @"\b([A-Z]{2}(?:-[A-Z]{3})?)\b" },
                { "Commodity", @"Commodity.*?(\b\d{6,10}\b)" },
                { "Net Weight", @"(\b\d[\d,.]*\s*(?:Kg|Tonnes)\b|\b\d[\d,.]*(?=\s+\d+\s*(?:Box|Case|Package|Carton|Bag|Pallet|Unit)s?\b))" },
                { "Package Count", @"(\b\d+\s*(?:Box|Case|Package|Carton|Bag|Pallet|Unit)s?\b)" },
                { "Establishment of Origin", @"(?-i)([A-Z][A-Z0-9\s,.-]+?\s+\([A-Z]{2}\))" },
                { "Country of Origin", @"Country\s+of\s+Origin.*(\b(?-i)[A-Z][a-z][a-zA-Z\s]*\([A-Z]{2}\))" },
                { "Product Type", @"(?-i)\b([A-Z][a-z]+\s+[a-z]+)\b(?=\s+[A-Z][A-Z\s]+\s+\([A-Z]{2}\))" },
                { "Country", @"Country\s+(\b(?-i)[A-Z][a-zA-Z\s]+?)(?=\s+ISO|\s+Code|\d|$)" },
                { "Item", @"(?:\d{6,10})\s+((?-i)(?!Extinct)[A-Z][a-z]+)\b" },
                { "Date of signature", @"\b(\d{1,2}\s+[A-Za-z]+\s+\d{4}\s+\d{2}:\d{2}:\d{2}\s+[\+\-]\d{4}\s+[A-Za-z]{3})\b" }
            };

            foreach (var filler in regexFillers)
            {
                if (sectionName.Contains("I.13", StringComparison.OrdinalIgnoreCase) &&
                    sectionName.Contains("Means of transport", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (sectionName.Contains("CHED Reference", StringComparison.OrdinalIgnoreCase))
                {
                    if (filler.Key == "ISO Code" || filler.Key == "Identification")
                    {
                        continue;
                    }
                }

                if (sectionName.Contains("Local Reference", StringComparison.OrdinalIgnoreCase))
                {
                    if (filler.Key == "Identification")
                    {
                        continue;
                    }
                }

                if (sectionName.Contains("I.8", StringComparison.OrdinalIgnoreCase) &&
                    sectionName.Contains("Operator responsible", StringComparison.OrdinalIgnoreCase))
                {
                    var allowedI8RegexFields = new[] {
                        "Name", "Address", "Organisation", "Country", "ISO Code"
                    };

                    if (!allowedI8RegexFields.Contains(filler.Key))
                    {
                        continue;
                    }
                }

                if (sectionName.Contains("I.9", StringComparison.OrdinalIgnoreCase) &&
                    sectionName.Contains("Accompanying documents", StringComparison.OrdinalIgnoreCase))
                {
                    var allowedI9RegexFields = new[] {
                        "Type", "Document reference", "Date of issue",
                        "Country and place of issue", "Name of Signatory",
                        "Commercial documentary references"
                    };

                    if (!allowedI9RegexFields.Contains(filler.Key))
                    {
                        continue;
                    }
                }

                if (sectionName.Contains("I.10", StringComparison.OrdinalIgnoreCase) &&
                    sectionName.Contains("Prior notification", StringComparison.OrdinalIgnoreCase))
                {
                    if (filler.Key != "Date" && filler.Key != "Time")
                    {
                        continue;
                    }
                }

                if (sectionName.Contains("I.34", StringComparison.OrdinalIgnoreCase))
                {
                    if (filler.Key == "ISO Code" || filler.Key == "Identification" || filler.Key == "Net Weight")
                    {
                        continue;
                    }
                }

                if (sectionName.Contains("I.35", StringComparison.OrdinalIgnoreCase))
                {
                    if (filler.Key == "ISO Code" || filler.Key == "Name" || filler.Key == "Date")
                    {
                        continue;
                    }
                }

                bool shouldTryRegex = !keyValuePairs.ContainsKey(filler.Key) || (keyValuePairs[filler.Key] is string sVal && string.IsNullOrWhiteSpace(sVal));

                if (!shouldTryRegex && keyValuePairs[filler.Key] is string currentVal && !string.IsNullOrWhiteSpace(currentVal))
                {
                    var checkMatch = Regex.Match(currentVal, filler.Value, RegexOptions.IgnoreCase);

                    if (checkMatch.Success && checkMatch.Value.Length < currentVal.Length * 0.9)
                    {
                        shouldTryRegex = true;
                    }
                    else if (!checkMatch.Success)
                    {
                        shouldTryRegex = true;
                    }
                }

                if (shouldTryRegex)
                {
                    var match = Regex.Match(fullText, filler.Value, RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        keyValuePairs[filler.Key] = match.Groups[1].Value;
                    }
                }
            }

            foreach (var kvp in keyValuePairs)
            {
                bool shouldSkipField = false;

                if (sectionName.Contains("I.8", StringComparison.OrdinalIgnoreCase) &&
                    sectionName.Contains("Operator responsible", StringComparison.OrdinalIgnoreCase))
                {
                    var allowedI8Fields = new[] { "Name", "Address", "Organisation", "Country", "ISO Code" };
                    if (!allowedI8Fields.Contains(kvp.Key))
                    {
                        shouldSkipField = true;
                    }
                }

                if (sectionName.Contains("I.9", StringComparison.OrdinalIgnoreCase) &&
                    sectionName.Contains("Accompanying documents", StringComparison.OrdinalIgnoreCase))
                {
                    var allowedI9Fields = new[] {
                        "Type", "Document reference", "Date of issue",
                        "Country and place of issue", "Name of Signatory",
                        "Commercial documentary references"
                    };
                    if (!allowedI9Fields.Contains(kvp.Key))
                    {
                        shouldSkipField = true;
                    }
                }

                if (sectionName.Contains("I.10", StringComparison.OrdinalIgnoreCase) &&
                    sectionName.Contains("Prior notification", StringComparison.OrdinalIgnoreCase))
                {
                    var allowedI10Fields = new[] { "Date", "Time" };
                    if (!allowedI10Fields.Contains(kvp.Key))
                    {
                        shouldSkipField = true;
                    }
                }

                if (sectionName.Contains("I.34", StringComparison.OrdinalIgnoreCase))
                {
                    var skippedI34Fields = new[] { "ISO Code", "Identification", "Net Weight" };
                    if (skippedI34Fields.Contains(kvp.Key))
                    {
                        shouldSkipField = true;
                    }
                }

                if (sectionName.Contains("I.35", StringComparison.OrdinalIgnoreCase))
                {
                    var skippedI35Fields = new[] { "Name", "Date", "Yes", "No", "ISO Code" };
                    if (skippedI35Fields.Contains(kvp.Key))
                    {
                        shouldSkipField = true;
                    }
                    else if ((kvp.Key.Length > 0 && char.IsDigit(kvp.Key[0])) ||
                             kvp.Key.StartsWith("Operator responsible") ||
                             kvp.Key.StartsWith("I, the undersigned"))
                    {
                        shouldSkipField = true;
                    }
                }

                if (!shouldSkipField)
                {
                    sectionData[kvp.Key] = kvp.Value;
                }
            }

            // Post-extraction corrections for specific sections
            if (sectionName.Contains("I.8", StringComparison.OrdinalIgnoreCase) &&
                sectionName.Contains("Operator responsible", StringComparison.OrdinalIgnoreCase))
            {
                if (sectionData.ContainsKey("Address"))
                {
                    var addressMatch = Regex.Match(fullText, @"Address\s+(.+?)\s+Country", RegexOptions.IgnoreCase);
                    if (addressMatch.Success)
                    {
                        sectionData["Address"] = addressMatch.Groups[1].Value.Trim();
                    }
                }

                if (sectionData.ContainsKey("Country"))
                {
                    var interleavedMatch = Regex.Match(fullText, @"Country\s+(.+?)\s+ISO\s+Code\s+\w+\s+(.+)$", RegexOptions.IgnoreCase);
                    bool handled = false;

                    if (interleavedMatch.Success)
                    {
                        var part1 = interleavedMatch.Groups[1].Value.Trim();
                        var part2 = interleavedMatch.Groups[2].Value.Trim();

                        string combined = $"{part1} {part2}".Trim();

                        if (combined.Length > sectionData["Country"].ToString()?.Length)
                        {
                            sectionData["Country"] = combined;
                            handled = true;
                        }
                    }

                    if (!handled)
                    {
                        var countryMatch = Regex.Match(fullText, @"Country\s+(.+?)(?=\s+ISO\s+Code)", RegexOptions.IgnoreCase);
                        if (countryMatch.Success)
                        {
                            var countryValue = countryMatch.Groups[1].Value.Trim();
                            var currentCountry = sectionData["Country"]?.ToString() ?? "";
                            if (!string.IsNullOrWhiteSpace(countryValue) && countryValue.Length > currentCountry.Length)
                            {
                                sectionData["Country"] = countryValue;
                            }
                        }
                    }
                }
            }

            if (sectionName.Contains("I.13", StringComparison.OrdinalIgnoreCase) &&
                sectionName.Contains("Means of transport", StringComparison.OrdinalIgnoreCase))
            {
                var headers = "Mode International transport document Identification";
                var cleanText = fullText;

                var regexHeaders = new Regex(Regex.Escape(headers), RegexOptions.IgnoreCase);
                if (regexHeaders.IsMatch(cleanText))
                {
                    cleanText = regexHeaders.Replace(cleanText, "").Trim();
                }
                else
                {
                    cleanText = cleanText.Replace("Mode", "", StringComparison.OrdinalIgnoreCase)
                                         .Replace("International transport document", "", StringComparison.OrdinalIgnoreCase)
                                         .Replace("Identification", "", StringComparison.OrdinalIgnoreCase)
                                         .Trim();
                }

                var match = Regex.Match(cleanText, @"^(?<mode>.+?)\s+(?<doc>\w+)\s+(?<id>\w+)$");
                if (match.Success)
                {
                    sectionData["mode"] = match.Groups["mode"].Value.Trim();
                    sectionData["International transport document"] = match.Groups["doc"].Value.Trim();
                    sectionData["identification"] = match.Groups["id"].Value.Trim();
                }
                else
                {
                    match = Regex.Match(cleanText, @"(?<id>\w+)$");
                    if (match.Success)
                    {
                        sectionData["identification"] = match.Groups["id"].Value.Trim();
                        var remaining = cleanText.Substring(0, match.Index).Trim();
                        var matchDoc = Regex.Match(remaining, @"(?<doc>\w+)$");
                        if (matchDoc.Success)
                        {
                            sectionData["International transport document"] = matchDoc.Groups["doc"].Value.Trim();
                            sectionData["mode"] = remaining.Substring(0, matchDoc.Index).Trim();
                        }
                        else
                        {
                            sectionData["mode"] = remaining;
                        }
                    }
                    else
                    {
                        sectionData["mode"] = cleanText;
                    }
                }
            }

            var keysToRescue = new[] { "Name", "Product Type", "Name of Signatory" };
            foreach (var rescueKey in keysToRescue)
            {
                if (sectionData.ContainsKey(rescueKey) && string.IsNullOrWhiteSpace(sectionData[rescueKey].ToString()))
                {
                    var residualText = RemoveKnownAttributes(fullText, sectionData, knownKeywords, booleanFlags).Trim();

                    if (!string.IsNullOrWhiteSpace(residualText))
                    {
                        if (residualText.EndsWith(",")) residualText = residualText.TrimEnd(',').Trim();
                        if (residualText.Length < 200 && residualText.Length > 2)
                        {
                            sectionData[rescueKey] = residualText;
                        }
                    }
                }
            }

            bool shouldSkipValue = (sectionName.Contains("I.8", StringComparison.OrdinalIgnoreCase) &&
                                   sectionName.Contains("Operator responsible", StringComparison.OrdinalIgnoreCase)) ||
                                   (sectionName.Contains("I.9", StringComparison.OrdinalIgnoreCase) &&
                                   sectionName.Contains("Accompanying documents", StringComparison.OrdinalIgnoreCase)) ||
                                   (sectionName.Contains("I.10", StringComparison.OrdinalIgnoreCase) &&
                                   sectionName.Contains("Prior notification", StringComparison.OrdinalIgnoreCase)) ||
                                   (sectionName.Contains("I.13", StringComparison.OrdinalIgnoreCase) &&
                                   sectionName.Contains("Means of transport", StringComparison.OrdinalIgnoreCase));

            if (!sectionData.ContainsKey("value") && !shouldSkipValue)
            {
                sectionData["value"] = fullText.Trim();
            }

            // Post-fallback cleanup for specific sections
            bool isCountrySection = (sectionName.Contains("I.11", StringComparison.OrdinalIgnoreCase) && sectionName.Contains("Country of Origin", StringComparison.OrdinalIgnoreCase)) ||
                                    (sectionName.Contains("I.14", StringComparison.OrdinalIgnoreCase) && sectionName.Contains("Country of dispatch", StringComparison.OrdinalIgnoreCase));

            if (isCountrySection && sectionData.ContainsKey("value") && sectionData.ContainsKey("ISO Code"))
            {
                var val = sectionData["value"]?.ToString() ?? "";
                var iso = sectionData["ISO Code"]?.ToString() ?? "";

                if (!string.IsNullOrEmpty(iso) && val.EndsWith(iso))
                {
                    val = val.Substring(0, val.Length - iso.Length).Trim();
                    sectionData["value"] = val;
                }
            }

            if (sectionName.Contains("Reference") || sectionName.Contains("Code"))
            {
                if (sectionData.ContainsKey("value") && !sectionData.ContainsKey("id"))
                {
                    var val = sectionData["value"].ToString();
                    if (val.Length > 3 && !val.Contains(" ") && Regex.IsMatch(val, "^[A-Za-z0-9.-]+$"))
                    {
                        sectionData["id"] = val;
                        sectionData.Remove("value");
                    }
                }
            }

            var booleanSections = new[] { "For internal market", "Domestic use" };
            foreach (var boolSec in booleanSections)
            {
                if (sectionName.Contains(boolSec, StringComparison.OrdinalIgnoreCase))
                {
                    if (!sectionData.Any() || (sectionData.Count == 1 && sectionData.ContainsKey("value") && string.IsNullOrWhiteSpace(sectionData["value"].ToString())))
                    {
                        sectionData["value"] = "true";
                    }
                }
            }

            if (sectionName.StartsWith("II.") || sectionName.StartsWith("III."))
            {
                var checkboxLabels = new[] {
                    "Satisfactory", "Not Satisfactory", "Yes", "No", "EU Standard",
                    "Random", "Suspicion", "Intensified Controls", "Results Pending", "Pending",
                    "Seal Check Only", "Full Identity Check", "Satisfactory Following Official Intervention",
                    "Not Done", "Human consumption", "Local competent authority", "Second entry point", "Arrival of consignment"
                };

                foreach (var label in checkboxLabels)
                {
                    if (fullText.Contains(label, StringComparison.OrdinalIgnoreCase))
                    {
                        if (!sectionData.ContainsKey(label))
                        {
                            sectionData[label] = "false";
                        }
                    }
                }
            }

            // Section-specific cleanup
            ApplySectionSpecificCleanup(sectionData, sectionName, fullText);
        }

        private void ApplySectionSpecificCleanup(Dictionary<string, object> sectionData, string sectionName, string fullText)
        {
            if (sectionName.Contains("I.19", StringComparison.OrdinalIgnoreCase))
            {
                var keysToRemove = new[] { "Yes", "No", "value" };
                foreach (var key in keysToRemove)
                {
                    if (sectionData.ContainsKey(key))
                    {
                        sectionData.Remove(key);
                    }
                }
            }

            if (sectionName.Contains("I.27", StringComparison.OrdinalIgnoreCase))
            {
                if (sectionData.ContainsKey("value")) sectionData.Remove("value");

                var requiredKeys = new[] { "Mode", "International transport document", "Identification" };
                foreach (var key in requiredKeys)
                {
                    if (!sectionData.ContainsKey(key))
                    {
                        sectionData[key] = "";
                    }
                }
            }

            if (sectionName.Contains("I.28", StringComparison.OrdinalIgnoreCase))
            {
                if (sectionData.ContainsKey("value")) sectionData.Remove("value");

                if (sectionData.ContainsKey("Approval Number"))
                {
                    sectionData["Approval number"] = sectionData["Approval Number"];
                    sectionData.Remove("Approval Number");
                }

                var garbageMap = new Dictionary<string, string> {
                    { "Name", "Code" },
                    { "Address", "Country" },
                    { "Country", "ISO" },
                    { "ISO Code", "Approval" }
                };

                foreach (var kvp in garbageMap)
                {
                    if (sectionData.ContainsKey(kvp.Key) && sectionData[kvp.Key].ToString().Contains(kvp.Value))
                    {
                        sectionData[kvp.Key] = "";
                    }
                }

                var requiredKeys = new[] { "Name", "Address", "Country", "ISO Code", "Approval number" };
                foreach (var key in requiredKeys)
                {
                    if (!sectionData.ContainsKey(key))
                    {
                        sectionData[key] = "";
                    }
                }
            }

            if (sectionName.Contains("I.15", StringComparison.OrdinalIgnoreCase))
            {
                var garbageMap = new Dictionary<string, string> {
                    { "Name", "Code" },
                    { "Address", "Country" },
                    { "Country", "ISO" },
                    { "Approval Number", "Country" }
                };

                foreach (var kvp in garbageMap)
                {
                    if (sectionData.ContainsKey(kvp.Key) && sectionData[kvp.Key].ToString().Contains(kvp.Value))
                    {
                        sectionData[kvp.Key] = "";
                    }
                }
            }

            if (sectionName.Contains("I.31", StringComparison.OrdinalIgnoreCase))
            {
                var keysToRemove = new[] { "Type", "Yes", "No", "Identification", "ISO Code" };
                foreach (var key in keysToRemove)
                {
                    if (sectionData.ContainsKey(key))
                    {
                        sectionData.Remove(key);
                    }
                }
            }

            if (sectionName.Contains("I.35", StringComparison.OrdinalIgnoreCase) && sectionData.ContainsKey("value"))
            {
                var val = sectionData["value"].ToString();

                var timestampRegex = new Regex(@"\d{1,2}\s+[A-Za-z]+\s+\d{4}\s+\d{2}:\d{2}:\d{2}\s+[\+\-]\d{4}\s+[A-Za-z]{3}");
                var match = timestampRegex.Match(val);
                if (match.Success)
                {
                    sectionData["Date of signature"] = match.Value;

                    var namePart = val.Substring(match.Index + match.Length).Trim();
                    if (!string.IsNullOrWhiteSpace(namePart))
                    {
                        sectionData["Name of Signatory"] = namePart;
                    }
                }

                if (!sectionData.ContainsKey("Signature"))
                {
                    sectionData["Signature"] = "";
                }
            }

            if (sectionName.Contains("II.3", StringComparison.OrdinalIgnoreCase))
            {
                var keysToRemove = new[] { "value", "Yes", "No", "ISO Code" };
                foreach (var k in keysToRemove)
                {
                    if (sectionData.ContainsKey(k)) sectionData.Remove(k);
                }

                var requiredFalseKeys = new[] { "Satisfactory", "Not Satisfactory", "Not Done", "Satisfactory Following Official Intervention" };
                foreach (var k in requiredFalseKeys)
                {
                    if (!sectionData.ContainsKey(k)) sectionData[k] = "false";
                }

                if (!sectionData.ContainsKey("EU Standard"))
                {
                    sectionData["EU Standard"] = "";
                }
            }

            if (sectionName.Contains("II.4", StringComparison.OrdinalIgnoreCase))
            {
                var keysToRemove = new[] { "value", "Identity Check", "ISO Code" };
                foreach (var k in keysToRemove)
                {
                    if (sectionData.ContainsKey(k)) sectionData.Remove(k);
                }

                var requiredFalseKeys = new[] { "Yes", "No", "Seal Check Only", "Full Identity Check", "Satisfactory", "Not Satisfactory" };
                foreach (var k in requiredFalseKeys)
                {
                    if (!sectionData.ContainsKey(k)) sectionData[k] = "false";
                }
            }

            if (sectionName.Contains("II.5", StringComparison.OrdinalIgnoreCase))
            {
                var keysToRemove = new[] { "value", "ISO Code" };
                foreach (var k in keysToRemove)
                {
                    if (sectionData.ContainsKey(k)) sectionData.Remove(k);
                }

                var requiredFalseKeys = new[] { "Yes", "No", "Satisfactory", "Not Satisfactory" };
                foreach (var k in requiredFalseKeys)
                {
                    if (!sectionData.ContainsKey(k)) sectionData[k] = "false";
                }
            }

            if (sectionName.Contains("II.6", StringComparison.OrdinalIgnoreCase))
            {
                var keysToRemove = new[] { "value", "ISO Code", "Results Pending" };
                foreach (var k in keysToRemove)
                {
                    if (sectionData.ContainsKey(k)) sectionData.Remove(k);
                }

                var requiredFalseKeys = new[] { "Yes", "No", "Satisfactory", "Not Satisfactory", "Random", "Suspicion", "Intensified Controls", "Pending" };
                foreach (var k in requiredFalseKeys)
                {
                    if (!sectionData.ContainsKey(k)) sectionData[k] = "false";
                }
            }

            if (sectionName.Contains("II.12", StringComparison.OrdinalIgnoreCase))
            {
                var keysToRemove = new[] { "value", "Human Consumption" };
                foreach (var k in keysToRemove)
                {
                    if (sectionData.ContainsKey(k)) sectionData.Remove(k);
                }
                if (!sectionData.ContainsKey("Human consumption"))
                {
                    sectionData["Human consumption"] = "false";
                }
            }

            if (sectionName.Contains("II.20", StringComparison.OrdinalIgnoreCase))
            {
                if (sectionData.ContainsKey("value"))
                {
                    var val = sectionData["value"].ToString();
                    if (!sectionData.ContainsKey("BCP") && val.Contains("BCP"))
                    {
                        var match = Regex.Match(val, @"BCP\s+(.*?)(?=\s+(Stamp|Unit number)|$)");
                        if (match.Success)
                        {
                            sectionData["BCP"] = match.Groups[1].Value.Trim();
                        }
                    }

                    sectionData.Remove("value");
                }
            }

            if (sectionName.Contains("II.21", StringComparison.OrdinalIgnoreCase))
            {
                var keysToRemove = new[] { "value", "ISO Code", "Official Inspector", "Full name Signature" };
                foreach (var k in keysToRemove)
                {
                    if (sectionData.ContainsKey(k)) sectionData.Remove(k);
                }
                var requiredKeys = new[] { "Name", "Date of signature", "Signature" };
                foreach (var k in requiredKeys)
                {
                    if (!sectionData.ContainsKey(k)) sectionData[k] = "";
                }
            }

            if (sectionName.Contains("II.22", StringComparison.OrdinalIgnoreCase))
            {
                sectionData.Clear();
            }

            if (sectionName.Contains("II.23", StringComparison.OrdinalIgnoreCase))
            {
                sectionData.Clear();
            }

            if (sectionName.Contains("III.4", StringComparison.OrdinalIgnoreCase))
            {
                sectionData.Clear();
                var keysToDefault = new[] { "Country of destination", "ISO Code", "Exit authority", "Code", "Mode", "International transport document", "Identification" };
                foreach (var k in keysToDefault) sectionData[k] = "";
            }

            if (sectionName.Contains("III.5", StringComparison.OrdinalIgnoreCase))
            {
                var keysToRemove = new[] { "value", "Yes", "No", "Satisfactory", "Not Satisfactory", "ISO Code" };
                foreach (var k in keysToRemove) if (sectionData.ContainsKey(k)) sectionData.Remove(k);

                var falseKeys = new[] { "Local competent authority", "Second entry point", "Arrival of consignment" };
                foreach (var k in falseKeys) if (!sectionData.ContainsKey(k)) sectionData[k] = "false";

                var emptyKeys = new[] { "Control at destination", "Compliance of the consignment" };
                foreach (var k in emptyKeys) if (!sectionData.ContainsKey(k)) sectionData[k] = "";
            }

            if (sectionName.Contains("III.6", StringComparison.OrdinalIgnoreCase))
            {
                sectionData.Clear();
                var keysToDefault = new[] { "Full name", "Date of signature", "Signature" };
                foreach (var k in keysToDefault) sectionData[k] = "";
            }
        }

        private string RemoveKnownAttributes(string text, Dictionary<string, object> currentSectionData, string[] knownKeywords, string[] booleanFlags)
        {
            var residual = text;

            foreach (var val in currentSectionData.Values)
            {
                if (val is string s && !string.IsNullOrWhiteSpace(s))
                {
                    residual = residual.Replace(s, " ");
                }
            }

            foreach (var kw in knownKeywords)
            {
                residual = Regex.Replace(residual, Regex.Escape(kw), " ", RegexOptions.IgnoreCase);
            }

            foreach (var f in booleanFlags)
            {
                residual = Regex.Replace(residual, Regex.Escape(f), " ", RegexOptions.IgnoreCase);
            }

            residual = Regex.Replace(residual, @"\s+", " ").Trim();
            residual = residual.TrimStart(':', '-', ',', '.').Trim();

            return residual;
        }

        private Dictionary<string, string> ExtractKeyValuePairs(List<string> lines)
        {
            var pairs = new Dictionary<string, string>();
            var commonKeys = new[] {
                "Country and place of issue", "Name", "Address", "Country", "ISO Code",
                "Time", "Date", "Approval Number", "Type", "Organisation",
                "Document reference", "Date of issue", "Date", "Time", "Approval Number", "Means of transport", "Identification", "International transport document",
                "Name of Signatory", "Total Gross Weight", "Total Net Weight", "Total number of packages", "Commercial documentary references", "Commodity", "Date of signature", "Signature", "Full name", "Item", "Net Weight", "Package Count", "Product Type", "Establishment of Origin", "Country of Origin",
                "Commercial documentary references", "Commodity", "Date of signature", "Signature", "Full name", "Item", "Net Weight", "Package Count", "Product Type", "Establishment of Origin", "Country of Origin"
            }.OrderByDescending(k => k.Length).ToArray();

            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"^([^:]+):\s*(.*)$");
                if (match.Success)
                {
                    var key = match.Groups[1].Value.Trim();
                    var value = match.Groups[2].Value.Trim();

                    if (!string.IsNullOrEmpty(key) && key.Length < 50)
                    {
                        pairs[key] = value;
                    }
                    continue;
                }

                string remainingLine = line.Trim();

                var foundKeyIndices = new List<(string Key, int Index)>();
                foreach (var key in commonKeys)
                {
                    int idx = line.IndexOf(key, StringComparison.OrdinalIgnoreCase);
                    while (idx != -1)
                    {
                        foundKeyIndices.Add((key, idx));
                        idx = line.IndexOf(key, idx + 1, StringComparison.OrdinalIgnoreCase);
                    }
                }

                if (foundKeyIndices.Any())
                {
                    foundKeyIndices = foundKeyIndices.OrderBy(x => x.Index).ToList();

                    var filteredIndices = new List<(string Key, int Index)>();
                    for (int i = 0; i < foundKeyIndices.Count; i++)
                    {
                        var current = foundKeyIndices[i];
                        bool skip = false;
                        foreach (var other in foundKeyIndices)
                        {
                            if (current == other) continue;
                            if (other.Index == current.Index && other.Key.Length > current.Key.Length)
                            {
                                skip = true;
                                break;
                            }
                            if (other.Index < current.Index && (other.Index + other.Key.Length) >= (current.Index + current.Key.Length))
                            {
                                skip = true;
                                break;
                            }
                        }
                        if (!skip) filteredIndices.Add(current);
                    }
                    foundKeyIndices = filteredIndices.OrderBy(x => x.Index).ToList();

                    for (int i = 0; i < foundKeyIndices.Count; i++)
                    {
                        var keyItem = foundKeyIndices[i];
                        int valueStart = keyItem.Index + keyItem.Key.Length;
                        int valueEnd = (i + 1 < foundKeyIndices.Count) ? foundKeyIndices[i + 1].Index : line.Length;

                        if (valueEnd > valueStart)
                        {
                            var val = line.Substring(valueStart, valueEnd - valueStart).Trim();
                            if (val.StartsWith(":") || val.StartsWith("-")) val = val.Substring(1).Trim();

                            if (!string.IsNullOrEmpty(val))
                            {
                                pairs[keyItem.Key] = val;
                            }
                        }
                    }
                }
            }

            return pairs;
        }

        private bool IsFieldRelevantToSection(string fieldName, string sectionName)
        {
            if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(sectionName))
                return false;

            var i16Keys = new[] { "Ambient", "Chilled", "Frozen" };
            if (i16Keys.Contains(fieldName, StringComparer.OrdinalIgnoreCase))
            {
                return sectionName.Contains("I.16", StringComparison.OrdinalIgnoreCase);
            }

            var i19Keys = new[] { "Conforming", "Non-conforming" };
            if (i19Keys.Contains(fieldName, StringComparer.OrdinalIgnoreCase))
            {
                return sectionName.Contains("I.19", StringComparison.OrdinalIgnoreCase);
            }

            var sectionMatch = Regex.Match(sectionName, @"^(I{1,3})\.\d+");
            if (sectionMatch.Success)
            {
                var romanNumeral = sectionMatch.Groups[1].Value;
                return fieldName.Contains(romanNumeral, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        private List<List<Word>> GroupWordsIntoLines(List<Word> words)
        {
            var lines = new List<List<Word>>();
            var currentLine = new List<Word>();
            double lastTop = double.MaxValue;
            double lineThreshold = 5;

            foreach (var word in words)
            {
                if (currentLine.Any() && Math.Abs(word.BoundingBox.Top - lastTop) > lineThreshold)
                {
                    lines.Add(currentLine.OrderBy(w => w.BoundingBox.Left).ToList());
                    currentLine = new List<Word>();
                }

                currentLine.Add(word);
                lastTop = word.BoundingBox.Top;
            }

            if (currentLine.Any())
            {
                lines.Add(currentLine.OrderBy(w => w.BoundingBox.Left).ToList());
            }

            return lines;
        }

        private bool IsMainSectionHeader(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            text = text.Trim().ToUpper();

            return text.StartsWith("PART I: DESCRIPTION") ||
                   text.StartsWith("PART II: CONTROLS") ||
                   text.StartsWith("PART III: FOLLOW UP");
        }

        private string SanitizePropertyName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return name;

            var parts = Regex.Split(name, @"[^a-zA-Z0-9]+");
            var sb = new StringBuilder();

            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part)) continue;
                var lower = part.ToLower();
                sb.Append(char.ToUpper(lower[0]) + lower.Substring(1));
            }

            if (sb.Length > 0 && char.IsDigit(sb[0]))
            {
                sb.Insert(0, "_");
            }

            return sb.ToString();
        }
    }
}