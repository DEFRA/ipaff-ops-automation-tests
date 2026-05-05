using Defra.UI.Tests.Tools.PDFProcessor.Extractors;
using Defra.UI.Tests.Tools.PDFProcessor.Models;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace Defra.UI.Tests.Tools.PDFProcessor
{
    public partial class PdfToJsonConverter
    {
        private readonly CheckboxExtractor _checkboxExtractor;
        private readonly FormExtractor _formExtractor;

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
            var ii23Value = ExtractIi23CustomsDocumentReference(lines);
            if (!string.IsNullOrWhiteSpace(ii23Value))
            {
                formFields["II.23.CustomsDocumentReference"] = ii23Value;
            }
            var ii21Fields = ExtractIi21CertifyingOfficerFields(lines);
            if (ii21Fields.TryGetValue("Name", out var ii21Name) && !string.IsNullOrWhiteSpace(ii21Name))
            {
                formFields["II.21.Name"] = ii21Name;
            }
            if (ii21Fields.TryGetValue("Date of signature", out var ii21Date) && !string.IsNullOrWhiteSpace(ii21Date))
            {
                formFields["II.21.DateOfSignature"] = ii21Date;
            }
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
                if (partIndex == -1) partIndex = lineText.IndexOf("PART IV", StringComparison.OrdinalIgnoreCase);

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

            var sectionStartRegex = new Regex(@"^((?:I{1,3})\.\d+(?:\.\d*)?\.?|IV\.)");
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

                else if (wordText.Equals("Fiche", StringComparison.OrdinalIgnoreCase) && i + 2 < lineWords.Count &&
                         lineWords[i + 1].Text.Equals("for", StringComparison.OrdinalIgnoreCase) &&
                         lineWords[i + 2].Text.Equals("sampling", StringComparison.OrdinalIgnoreCase))
                {
                    isSectionStart = true;
                    detectedHeader = "IV. Fiche for sampling";
                    i += 2;
                }
                else if (wordText.Equals("References", StringComparison.OrdinalIgnoreCase) &&
                         (i == 0 || !lineWords[i - 1].Text.Equals("IV.", StringComparison.OrdinalIgnoreCase)))
                {
                    isSectionStart = true;
                    detectedHeader = "IV. References";
                }
                else if (wordText.Equals("Identification", StringComparison.OrdinalIgnoreCase) && i + 3 < lineWords.Count &&
                         lineWords[i + 1].Text.Equals("of", StringComparison.OrdinalIgnoreCase) &&
                         lineWords[i + 2].Text.Equals("the", StringComparison.OrdinalIgnoreCase) &&
                         lineWords[i + 3].Text.Equals("sample", StringComparison.OrdinalIgnoreCase))
                {
                    isSectionStart = true;
                    detectedHeader = "IV. Identification of the sample";
                    i += 3;
                }
                else if (wordText.Equals("Requested", StringComparison.OrdinalIgnoreCase) && i + 1 < lineWords.Count &&
                         lineWords[i + 1].Text.Equals("analysis", StringComparison.OrdinalIgnoreCase))
                {
                    isSectionStart = true;
                    detectedHeader = "IV. Requested analysis";
                    i += 1;
                }
                else if (wordText.Equals("Results", StringComparison.OrdinalIgnoreCase) &&
                         (i == 0 || !lineWords[i - 1].Text.Contains("Sample", StringComparison.OrdinalIgnoreCase)))
                {
                    isSectionStart = true;
                    detectedHeader = "IV. Results";
                }

                if (isSectionStart)
                {
                    // If we were building a previous section, save it
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
            // e.g. "I.4..." header, and content starts with "/Control Unit"
            while (contentLines.Count > 0 && contentLines[0].Trim().StartsWith("/"))
            {
                sectionHeader += " " + contentLines[0].Trim();
                contentLines.RemoveAt(0);
            }

            sectionHeader = sectionHeader.Trim();

            // Cleanup header common artifacts
            // e.g. "I.11. Country of Origin ISO Code" -> "I.11. Country of Origin"
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
            if (sectionHeader.Contains("II.23", StringComparison.OrdinalIgnoreCase))
            {
                var ii23Match = Regex.Match(
                    sectionHeader,
                    @"II\.23\.?\s*Customs\s+Document\s+Reference\s*(?<val>.+)$",
                    RegexOptions.IgnoreCase);
                if (ii23Match.Success)
                {
                    var v = ii23Match.Groups["val"].Value.Trim();
                    if (!string.IsNullOrWhiteSpace(v))
                    {
                        contentLines.Insert(0, v);
                    }
                }
                sectionHeader = "II.23 Customs Document Reference";
            }
            if (sectionHeader.StartsWith("I.25", StringComparison.OrdinalIgnoreCase))
            {
                sectionHeader = "I.25 For-reentry";
            }

            if (sectionHeader.Contains("II.25", StringComparison.OrdinalIgnoreCase) && sectionHeader.Contains("BCP Reference Number", StringComparison.OrdinalIgnoreCase))
            {
                var ii25Match = Regex.Match(
                    sectionHeader,
                    @"II\.25\.?\s*BCP\s+Reference\s+Number\s*(?<val>.+)$",
                    RegexOptions.IgnoreCase);
                if (ii25Match.Success)
                {
                    var v = ii25Match.Groups["val"].Value.Trim();
                    if (!string.IsNullOrWhiteSpace(v))
                    {
                        contentLines.Insert(0, v);
                    }
                }
                sectionHeader = "II.25 BCP Reference Number";
            }

            // Special handling for I.31 to support array of objects for multi-row values
            if (sectionHeader.Contains("I.31", StringComparison.OrdinalIgnoreCase) &&
                sectionHeader.Contains("Description of the goods", StringComparison.OrdinalIgnoreCase))
            {
                // Check for "Descriptions followed by Table" pattern (Grouped layout)
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
                        // Check if line starts with commodity code (approx 4-10 digits)
                        if (Regex.IsMatch(line.Trim(), @"^\d{4,10}\b"))
                        {
                            dataRows.Add(line.Trim());
                        }
                        else if (dataRows.Any())
                        {
                            // Append continuation lines to the last row
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
                    // Treat as single chunk if no numbering found
                    currentChunk = new List<string>(contentLines);
                }

                if (currentChunk.Any())
                {
                    var chunkData = new Dictionary<string, object>();
                    ParseContent(chunkData, sectionHeader, currentChunk, formFields, checkboxes);
                    var cleanChunk = new Dictionary<string, object>();
                    foreach (var kvp in chunkData) cleanChunk[SanitizePropertyName(kvp.Key)] = kvp.Value;
                    items.Add(cleanChunk);
                }

                sections[SanitizePropertyName(sectionHeader)] = items;
                return;
            }

            var sectionData = new Dictionary<string, object>();

            // Parse content to find key-value pairs
            ParseContent(sectionData, sectionHeader, contentLines, formFields, checkboxes);

            // Sanitize keys for valid C# property names
            var cleanSectionData = new Dictionary<string, object>();
            foreach (var kvp in sectionData)
            {
                cleanSectionData[SanitizePropertyName(kvp.Key)] = kvp.Value;
            }

            var cleanHeader = SanitizePropertyName(sectionHeader);

            if (cleanSectionData.Any())
            {
                sections[cleanHeader] = cleanSectionData;
            }
            else
            {
                sections[cleanHeader] = new Dictionary<string, object>();
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

            // 2. Dynamic Keyword Extraction
            // We use a comprehensive list of potential keys found in the document
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
                "Stamp", "Unit number",
                "TRACES unit No", "Exit BCP", "3rd country", "Country of destination", "Exit authority", "Code",
                "Name of the inspector", "Phone/email", "Phone/Email", "Certificate reference number", "Countries of origin",
                "EPPO Code", "Harmful organism/name", "Inspector conclusion", "Motivation",
                "Sample date", "Sample time", "Sample batch Nr.", "Sample use by date", "Sample type",
                "Number of samples", "Conservation of samples", "Laboratory test", "Laboratory test method",
                "Released date", "Sample/R results", "Laboratory conclusion", "Phone Email"
            }.OrderByDescending(k => k.Length).ToArray();

            foreach (var keyword in knownKeywords)
            {
                // Local Reference sections - should only have "id"
                if (sectionName.Contains("Local Reference", StringComparison.OrdinalIgnoreCase))
                {
                    if (keyword == "Identification")
                    {
                        continue;
                    }
                }
                // I.8 Operator responsible for the consignment - only allow specific fields
                if (sectionName.Contains("I.8", StringComparison.OrdinalIgnoreCase) &&
                    sectionName.Contains("Operator responsible", StringComparison.OrdinalIgnoreCase))
                {
                    var allowedI8Fields = new[] {
                        "Name", "Address", "Organisation", "Country", "ISO Code"
                    };

                    if (!allowedI8Fields.Contains(keyword))
                    {
                        continue; // Skip generic keywords not specific to I.8
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
                    // Search for keyword in full text
                    // We use IndexOf. Note: Keywords might be substrings of others, so we processed longest first?
                    // Ideally knownKeywords should be sorted by length descending to match "Date of issue" before "Date".

                    // Case insensitive search
                    int idx = fullText.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);
                    if (idx != -1)
                    {
                        // Found key. Value is everything until the next known keyword.
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

            // 3. Flag/Boolean Extraction
            // Look for specific toggle words that indicate state
            var booleanFlags = new[] {
                "Ambient", "Chilled", "Frozen",
                "Conforming", "Non-conforming",
                "Human Consumption", "Animal Feeedingstuff", "Technical use", "Other",
                "EU Standard", "National Requirements",
                "Satisfactory", "Not Satisfactory", "Not Done",
                "Seal Check Only", "Full Identity Check",
                "Random", "Suspicion", "Intensified Controls", "Pending",
                "Validation", "Acceptable", "Refused"
            };

            foreach (var flag in booleanFlags)
            {
                // If the flag word appears in text, we might want to capture it.
                // In your sample output, these are explicitly set as "true"/"false" if present/absent
                // OR listed in a "value" field.

                if (sectionName.Contains("I.16", StringComparison.OrdinalIgnoreCase) ||
                    sectionName.Contains("I.18", StringComparison.OrdinalIgnoreCase) ||
                    sectionName.Contains("I.19", StringComparison.OrdinalIgnoreCase) ||
                    sectionName.Contains("II.3", StringComparison.OrdinalIgnoreCase) ||
                    sectionName.Contains("II.4", StringComparison.OrdinalIgnoreCase) ||
                    sectionName.Contains("II.5", StringComparison.OrdinalIgnoreCase) ||
                    sectionName.Contains("II.6", StringComparison.OrdinalIgnoreCase) ||
                    sectionName.Contains("II.11", StringComparison.OrdinalIgnoreCase) ||
                    sectionName.Contains("II.12", StringComparison.OrdinalIgnoreCase) ||
                    sectionName.Contains("III.5", StringComparison.OrdinalIgnoreCase))
                {
                    // Skip heuristic boolean flags for I.16 and I.19, rely on specific checkbox/OCR extraction
                }
                else if (fullText.Contains(flag, StringComparison.OrdinalIgnoreCase))
                {
                    keyValuePairs[flag] = "true";
                }
            }

            // Group-aware boolean logic (Semi-hardcoded but generic concept of 'Choice Groups')
            var choiceGroups = new List<string[]> {
                new[] { "Ambient", "Chilled", "Frozen" },
                new[] { "Conforming", "Non-conforming" },
                new[] { "Satisfactory", "Not Satisfactory" },
                new[] { "Yes", "No" }
            };

            foreach (var group in choiceGroups)
            {
                // If any item in the group is present, we might want to explictly set specific true/false for all
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
                            sectionName.Contains("II.11", StringComparison.OrdinalIgnoreCase) ||
                            sectionName.Contains("II.12", StringComparison.OrdinalIgnoreCase) ||
                            sectionName.Contains("III.5", StringComparison.OrdinalIgnoreCase) ||
                            sectionName.Contains("I.35", StringComparison.OrdinalIgnoreCase))
                        {
                            continue; // Skip I.16, I.19, and II.3 for heuristic choice groups
                        }

                        if (fullText.Contains(item, StringComparison.OrdinalIgnoreCase))
                            keyValuePairs[item] = "true";
                        else
                            keyValuePairs[item] = "false";
                    }
                }
            }

            // Enum-like Value Extraction
            // Some keys are defined by the presence of a specific value from a set
            var enumDefinitions = new Dictionary<string, string[]> {
                { "Mode", new[] { "Road vehicle", "Aeroplane", "Vessel", "Railway", "Flight" } },
                { "Type", new[] { "Veterinary health certificate", "Cold Stores", "Processing Plant", "Slaughterhouse" } } // Helps I.9, I.15
            };

            foreach (var def in enumDefinitions)
            {
                // I.13 Means of transport - skip enum extraction (Mode) to prevent duplicates/capitalization issues
                if (sectionName.Contains("I.13", StringComparison.OrdinalIgnoreCase) &&
                    sectionName.Contains("Means of transport", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                foreach (var val in def.Value)
                {
                    if (fullText.Contains(val, StringComparison.OrdinalIgnoreCase))
                    {
                        // Found a value for this key
                        // We prefer this over extracted empty value
                        if (!keyValuePairs.ContainsKey(def.Key) || (keyValuePairs[def.Key] is string sEnum && string.IsNullOrWhiteSpace(sEnum)))
                        {
                            keyValuePairs[def.Key] = val.ToUpper(); // Standardization to Upper as per reference for Mode? Reference has "ROAD VEHICLE"
                            if (def.Key == "Type") keyValuePairs[def.Key] = val; // Type keeps casing
                        }
                    }
                }
            }

            // Regex Heuristics for specific IDs/Codes
            var regexFillers = new Dictionary<string, string> {
                { "International transport document", @"\b(DOC\w*\d+\w*)\b" }, // e.g. DOC1234. Require at least one digit to avoid matching "document"
                { "Identification", @"\b(\d{5,})\b" }, // e.g. 123456
                { "Approval Number", @"\b(\d+\.\d+\.\d+)\b" }, // e.g. 15.141.001
                { "ISO Code", @"\b([A-Z]{2}(?:-[A-Z]{3})?)\b" }, // e.g. FR, GB, GB-ENG
                { "Commodity", @"Commodity.*?(\b\d{6,10}\b)" }, // Anchored to Commodity, boundary
                { "Net Weight", @"(\b\d[\d,.]*\s*(?:Kg|Tonnes)\b|\b\d[\d,.]*(?=\s+\d+\s*(?:Box|Case|Package|Carton|Bag|Pallet|Unit)s?\b))" }, // Find number with weight unit or number before package count
                { "Package Count", @"(\b\d+\s*(?:Box|Case|Package|Carton|Bag|Pallet|Unit)s?\b)" }, // Find number with package unit
                { "Establishment of Origin", @"(?-i)([A-Z][A-Z0-9\s,.-]+?\s+\([A-Z]{2}\))" }, // Match "CHARRADE MARCEL ETS (FR)" - uppercase company name with country code
                { "Country of Origin", @"Country\s+of\s+Origin.*(\b(?-i)[A-Z][a-z][a-zA-Z\s]*\([A-Z]{2}\))" }, // Match "France (FR)". ProperCase + Code. Greedy prefix to skip "trophies".
                { "Product Type", @"(?-i)\b([A-Z][a-z]+\s+[a-z]+)\b(?=\s+[A-Z][A-Z\s]+\s+\([A-Z]{2}\))" }, // Match "Game trophies" before establishment name
                { "Country", @"Country\s+(\b(?-i)[A-Z][a-zA-Z\s]+?)(?=\s+ISO|\s+Code|\d|$)" }, // Specific Country regex. Case sensitive.
                { "Species", @"(?:\d{6,10})\s+((?-i)(?!Extinct)[A-Z][a-z]+)\b" }, // "97052200 Cervidae". Looks for digits then proper case word. Exclude "Extinct".
                { "Date of signature", @"\b(\d{1,2}\s+[A-Za-z]+\s+\d{4}\s+\d{2}:\d{2}:\d{2}\s+[\+\-]\d{4}\s+[A-Za-z]{3})\b" } // Timestamp
            };

            foreach (var filler in regexFillers)
            {
                // I.13 Means of transport - skip regex extraction to prevent duplicates
                if (sectionName.Contains("I.13", StringComparison.OrdinalIgnoreCase) &&
                    sectionName.Contains("Means of transport", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Skip generic extractions for specific sections that should only have limited fields

                // CHED Reference sections - should only have "id"
                if (sectionName.Contains("CHED Reference", StringComparison.OrdinalIgnoreCase))
                {
                    // Skip ISO Code and Identification as these are part of the CHED ID itself
                    if (filler.Key == "ISO Code" || filler.Key == "Identification")
                    {
                        continue;
                    }
                }

                // Local Reference sections - should only have "id"
                if (sectionName.Contains("Local Reference", StringComparison.OrdinalIgnoreCase))
                {
                    // Skip Identification as it gets promoted to "id" later
                    if (filler.Key == "Identification")
                    {
                        continue;
                    }
                }

                // I.8 Operator responsible for the consignment - only allow specific fields
                if (sectionName.Contains("I.8", StringComparison.OrdinalIgnoreCase) &&
                    sectionName.Contains("Operator responsible", StringComparison.OrdinalIgnoreCase))
                {
                    var allowedI8RegexFields = new[] {
                        "Name", "Address", "Organisation", "Country", "ISO Code"
                    };

                    if (!allowedI8RegexFields.Contains(filler.Key))
                    {
                        continue; // Skip regex extraction for fields not specific to I.8
                    }
                }

                // I.9 Accompanying documents - only allow specific fields
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
                        continue; // Skip regex extraction for fields not specific to I.9
                    }
                }

                // I.10 Prior notification - only allow Date/Time
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

                // If value exists but contains the pattern along with other text (noisy extraction), prefer the clean regex match
                if (!shouldTryRegex && keyValuePairs[filler.Key] is string currentVal && !string.IsNullOrWhiteSpace(currentVal))
                {
                    var checkMatch = Regex.Match(currentVal, filler.Value, RegexOptions.IgnoreCase);

                    // If it matches, but the match is significantly shorter than the current value (e.g. valid ID vs ID + Name), overwrite
                    if (checkMatch.Success && checkMatch.Value.Length < currentVal.Length * 0.9)
                    {
                        shouldTryRegex = true;
                    }
                    // If it doesn't match at all (completely wrong value), overwrite
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

            // 4. Populate Section Data
            foreach (var kvp in keyValuePairs)
            {
                // Section-specific filtering when adding to sectionData
                bool shouldSkipField = false;

                // I.8 Operator responsible - only allow specific fields
                if (sectionName.Contains("I.8", StringComparison.OrdinalIgnoreCase) &&
                    sectionName.Contains("Operator responsible", StringComparison.OrdinalIgnoreCase))
                {
                    var allowedI8Fields = new[] { "Name", "Address", "Organisation", "Country", "ISO Code" };
                    if (!allowedI8Fields.Contains(kvp.Key))
                    {
                        shouldSkipField = true;
                    }
                }

                // I.9 Accompanying documents - only allow specific fields
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

                // I.10 Prior notification - only allow specific fields
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

                if (sectionName.Contains("II.11", StringComparison.OrdinalIgnoreCase))
                {
                    var allowedIi11 = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                    {
                        "3rd country", "Exit BCP", "TRACES unit No", "ISO Code", "Country",
                        "Human consumption", "Human Consumption", "Validation", "Acceptable", "Refused",
                        "value"
                    };
                    if (!allowedIi11.Contains(kvp.Key))
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
            // I.8 Operator responsible: Fix truncated Address and Country values
            if (sectionName.Contains("I.8", StringComparison.OrdinalIgnoreCase) &&
                sectionName.Contains("Operator responsible", StringComparison.OrdinalIgnoreCase))
            {
                // Extract complete Address (including postal code like "CV2 4FT")
                if (sectionData.ContainsKey("Address"))
                {
                    // Pattern: Address text followed by "Country" keyword
                    // Extract everything between "Address" and "Country"
                    var addressMatch = Regex.Match(fullText, @"Address\s+(.+?)\s+Country", RegexOptions.IgnoreCase);
                    if (addressMatch.Success)
                    {
                        sectionData["Address"] = addressMatch.Groups[1].Value.Trim();
                    }
                }

                // Extract complete Country (including full name like "United Kingdom of Great Britain and Northern Ireland")
                if (sectionData.ContainsKey("Country"))
                {
                    // Case 1: Interleaved text "Country United Kingdom of ISO Code GB Great Britain and Northern Ireland"
                    var interleavedMatch = Regex.Match(fullText, @"Country\s+(.+?)\s+ISO\s+Code\s+\w+\s+(.+)$", RegexOptions.IgnoreCase);
                    bool handled = false;

                    if (interleavedMatch.Success)
                    {
                        var part1 = interleavedMatch.Groups[1].Value.Trim();
                        var part2 = interleavedMatch.Groups[2].Value.Trim();

                        string combined = $"{part1} {part2}".Trim();

                        // Verify length improvement
                        if (combined.Length > sectionData["Country"].ToString()?.Length)
                        {
                            sectionData["Country"] = combined;
                            handled = true;
                        }
                    }

                    if (!handled)
                    {
                        // Case 2: Normal extraction (everything before ISO Code)
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




            // I.13 Means of transport: Custom Regex Extraction for columnar layout
            if (sectionName.Contains("I.13", StringComparison.OrdinalIgnoreCase) &&
                sectionName.Contains("Means of transport", StringComparison.OrdinalIgnoreCase))
            {
                // Remove the headers to isolate values
                var headers = "Mode International transport document Identification";
                var cleanText = fullText;

                // Use case insensitive replace to remove headers from the text
                var regexHeaders = new Regex(Regex.Escape(headers), RegexOptions.IgnoreCase);
                if (regexHeaders.IsMatch(cleanText))
                {
                    cleanText = regexHeaders.Replace(cleanText, "").Trim();
                }
                else
                {
                    // Maybe headers are split? Remove individual keywords
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
                    // Fallback: try capturing just ID at end?
                    match = Regex.Match(cleanText, @"(?<id>\w+)$");
                    if (match.Success)
                    {
                        sectionData["identification"] = match.Groups["id"].Value.Trim();
                        // The rest is possibly Mode + Doc. Hard to split without more info.
                        // Maybe Doc is second to last?
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
                        // Assume entire text is mode if no ID found? Unlikely.
                        sectionData["mode"] = cleanText;
                    }
                }
            }

            // Generic Name/Description Rescue: If 'Name' or 'Product Type' is present but empty, try to use residual text.
            // This handles cases like I.15 where Name is the text remaining after extracting Approval Number and Type.
            var keysToRescue = new[] { "Name", "Product Type", "Name of Signatory" };
            foreach (var rescueKey in keysToRescue)
            {
                if (sectionData.ContainsKey(rescueKey) && string.IsNullOrWhiteSpace(sectionData[rescueKey].ToString()))
                {
                    var residualText = RemoveKnownAttributes(fullText, sectionData).Trim();

                    if (!string.IsNullOrWhiteSpace(residualText))
                    {
                        // formatting cleanup
                        if (residualText.EndsWith(",")) residualText = residualText.TrimEnd(',').Trim();
                        // Heuristic: If text is overly long, it might be garbage. But names can be long.
                        if (residualText.Length < 200 && residualText.Length > 2)
                        {
                            sectionData[rescueKey] = residualText;
                        }
                    }
                }
            }

            // Helper local function to remove known attributes from text
            string RemoveKnownAttributes(string text, Dictionary<string, object> currentSectionData)
            {
                var residual = text;

                // Remove all extracted values
                foreach (var val in currentSectionData.Values)
                {
                    if (val is string s && !string.IsNullOrWhiteSpace(s))
                    {
                        // Replace exactly? Or case insensitive?
                        // Values extracted are usually substrings of fullText.
                        residual = residual.Replace(s, " ");
                    }
                }

                // Remove all known keywords
                foreach (var kw in knownKeywords)
                {
                    // Case insensitive remove
                    residual = Regex.Replace(residual, Regex.Escape(kw), " ", RegexOptions.IgnoreCase);
                }

                // Remove boolean flags
                foreach (var f in booleanFlags)
                {
                    residual = Regex.Replace(residual, Regex.Escape(f), " ", RegexOptions.IgnoreCase);
                }

                // Cleanup residual
                residual = Regex.Replace(residual, @"\s+", " ").Trim();
                residual = residual.TrimStart(':', '-', ',', '.').Trim();

                return residual;
            }

            // 5. Fallback: Full Text as 'value' or 'id'
            // If we extracted very little structured data, or if there's leftover text that is important.
            // The sample output puts the full text in 'value' often.

            // Skip adding 'value' for sections that should only have specific fields
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
            // I.11 Country of Origin & I.14 Country of dispatch: Cleanup "value" to remove ISO Code
            bool isCountrySection = (sectionName.Contains("I.11", StringComparison.OrdinalIgnoreCase) && sectionName.Contains("Country of Origin", StringComparison.OrdinalIgnoreCase)) ||
                                    (sectionName.Contains("I.14", StringComparison.OrdinalIgnoreCase) && sectionName.Contains("Country of dispatch", StringComparison.OrdinalIgnoreCase));

            if (isCountrySection && sectionData.ContainsKey("value") && sectionData.ContainsKey("ISO Code"))
            {
                var val = sectionData["value"]?.ToString() ?? "";
                var iso = sectionData["ISO Code"]?.ToString() ?? "";

                if (!string.IsNullOrEmpty(iso) && val.EndsWith(iso))
                {
                    // Remove ISO code from value (e.g. "France FR" -> "France")
                    val = val.Substring(0, val.Length - iso.Length).Trim();
                    sectionData["value"] = val;
                }
            }

            // 6. Generic ID extraction
            // If the section header suggests an ID (like "Reference", "Code"), verify we extracted a value.
            // If "value" looks like an ID, map it to "id" key if required by schema preference.
            if (sectionName.Contains("Reference") || sectionName.Contains("Code"))
            {
                if (sectionData.ContainsKey("value") && !sectionData.ContainsKey("id"))
                {
                    var val = sectionData["value"].ToString();
                    // Simple heuristic for ID: alphanumeric, no spaces, length > 3
                    if (val.Length > 3 && !val.Contains(" ") && Regex.IsMatch(val, "^[A-Za-z0-9.-]+$"))
                    {
                        sectionData["id"] = val;
                        // We keep 'value' as well usually, or remove it? Sample output shows "id": "..." and no value for I.2.
                        // Let's remove 'value' if we promoted it to 'id'
                        sectionData.Remove("value");
                    }
                }
            }

            // 7. Boolean Section Defaults
            // Some sections imply "true" just by existence if no other data is found (checkbox behavior at section level)
            var booleanSections = new[] { "For internal market", "Domestic use" };
            foreach (var boolSec in booleanSections)
            {
                if (sectionName.Contains(boolSec, StringComparison.OrdinalIgnoreCase))
                {
                    // If we have no significant data, defaulting to true
                    if (!sectionData.Any() || (sectionData.Count == 1 && sectionData.ContainsKey("value") && string.IsNullOrWhiteSpace(sectionData["value"].ToString())))
                    {
                        sectionData["value"] = "true";
                    }
                }
            }

            // I.19 Conformity of the goods: Specific Cleanup
            if (sectionName.Contains("I.19", StringComparison.OrdinalIgnoreCase))
            {
                // Remove keys that shouldn't be here
                var keysToRemove = new[] { "Yes", "No", "value" };
                foreach (var key in keysToRemove)
                {
                    if (sectionData.ContainsKey(key))
                    {
                        sectionData.Remove(key);
                    }
                }
            }

            // I.27 Means of transport after BCP/storage: Specific Cleanup
            if (sectionName.Contains("I.27", StringComparison.OrdinalIgnoreCase))
            {
                // Remove value
                if (sectionData.ContainsKey("value")) sectionData.Remove("value");

                // Ensure keys exist (empty if missing)
                var requiredKeys = new[] { "Mode", "International transport document", "Identification" };
                foreach (var key in requiredKeys)
                {
                    if (!sectionData.ContainsKey(key))
                    {
                        sectionData[key] = "";
                    }
                }
            }

            // I.28 Transporter: Specific Cleanup
            if (sectionName.Contains("I.28", StringComparison.OrdinalIgnoreCase))
            {
                if (sectionData.ContainsKey("value")) sectionData.Remove("value");

                // Map "Approval Number" to "Approval number" if present
                if (sectionData.ContainsKey("Approval Number"))
                {
                    sectionData["Approval number"] = sectionData["Approval Number"];
                    sectionData.Remove("Approval Number");
                }

                // Clean garbage values from headers being picked up
                var garbageMap = new Dictionary<string, string> {
                    { "Name", "Code" },         // "Name" often picks up "Code"
                    { "Address", "Country" },   // "Address" often picks up "Country"
                    { "Country", "ISO" },       // "Country" often picks up "ISO"
                    { "ISO Code", "Approval" }  // "ISO Code" often picks up "Approval"
                };

                foreach (var kvp in garbageMap)
                {
                    if (sectionData.ContainsKey(kvp.Key) && sectionData[kvp.Key].ToString().Contains(kvp.Value))
                    {
                        sectionData[kvp.Key] = "";
                    }
                }

                // Ensure keys exist (empty if missing)
                var requiredKeys = new[] { "Name", "Address", "Country", "ISO Code", "Approval number" };
                foreach (var key in requiredKeys)
                {
                    if (!sectionData.ContainsKey(key))
                    {
                        sectionData[key] = "";
                    }
                }
            }

            // I.15 Establishment of Origin: Specific Cleanup
            if (sectionName.Contains("I.15", StringComparison.OrdinalIgnoreCase))
            {
                var cleanText = Regex.Replace(fullText, @"^(Name\s+Type\s+Approval\s+Number|Name\s+Approval\s+Number)", "", RegexOptions.IgnoreCase).Trim();

                var knownTypes = new[] { "ABP Transport", "Cold Stores", "Processing Plant", "Slaughterhouse" };
                string foundType = "";
                foreach (var t in knownTypes)
                {
                    if (cleanText.IndexOf(t, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        foundType = t;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(foundType))
                {
                    var regex = new Regex($@"^(?<name>.+?)\s+{Regex.Escape(foundType)}\s+(?<approval>\S+)$", RegexOptions.IgnoreCase);
                    var match = regex.Match(cleanText);
                    if (match.Success)
                    {
                        sectionData["Name"] = match.Groups["name"].Value.Trim();
                        sectionData["Type"] = foundType;
                        sectionData["Approval Number"] = match.Groups["approval"].Value.Trim();
                    }
                }
                else
                {
                    var match = Regex.Match(cleanText, @"^(?<name>.+?)\s+(?<approval>\w{5,})$");
                    if (match.Success)
                    {
                        // Fallback if no specific type matched
                        sectionData["Name"] = match.Groups["name"].Value.Trim();
                        sectionData["Approval Number"] = match.Groups["approval"].Value.Trim();
                    }
                }

                if (sectionData.ContainsKey("Identification"))
                {
                    sectionData.Remove("Identification");
                }

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

            // I.31 Description of the goods: Specific Cleanup
            if (sectionName.Contains("I.31", StringComparison.OrdinalIgnoreCase))
            {
                // Remove keys that shouldn't be here
                var keysToRemove = new[] { "Type", "Yes", "No", "Identification", "ISO Code" };
                foreach (var key in keysToRemove)
                {
                    if (sectionData.ContainsKey(key))
                    {
                        sectionData.Remove(key);
                    }
                }
            }

            // I.25 For re-entry: Specific Cleanup
            if (sectionName.Contains("I.25", StringComparison.OrdinalIgnoreCase))
            {
                if (sectionData.ContainsKey("I.25. For re-entry"))
                {
                    sectionData["value"] = sectionData["I.25. For re-entry"];
                }
                var keysToRemove = sectionData.Keys.Where(k => !k.Equals("value", StringComparison.OrdinalIgnoreCase)).ToList();
                foreach (var k in keysToRemove) sectionData.Remove(k);
            }

            // I.35 Declaration: Specific Cleanup and Signatory Extraction
            if (sectionName.Contains("I.35", StringComparison.OrdinalIgnoreCase) && sectionData.ContainsKey("value"))
            {
                var val = sectionData["value"].ToString();

                // 1. Extract Date and Name from value
                // Look for timestamp: 16 December 2025 13:03:41 +0000 GMT
                var timestampRegex = new Regex(@"\d{1,2}\s+[A-Za-z]+\s+\d{4}\s+\d{2}:\d{2}:\d{2}\s+[\+\-]\d{4}\s+[A-Za-z]{3}");
                var match = timestampRegex.Match(val);
                if (match.Success)
                {
                    sectionData["Date of signature"] = match.Value;

                    // Name is typically AFTER the timestamp in the declaration text
                    // "Date ... GMT IPaff Automation"
                    var namePart = val.Substring(match.Index + match.Length).Trim();
                    if (!string.IsNullOrWhiteSpace(namePart))
                    {
                        sectionData["Name of Signatory"] = namePart;
                    }
                }

                // 2. Ensure "Signature" is present (empty)
                if (!sectionData.ContainsKey("Signature"))
                {
                    sectionData["Signature"] = "";
                }
            }

            // II.3 Documentary Check: Specific Cleanup
            if (sectionName.Contains("II.3", StringComparison.OrdinalIgnoreCase))
            {
                var keysToRemove = new[] { "value", "Yes", "No", "ISO Code" };
                foreach (var k in keysToRemove)
                {
                    if (sectionData.ContainsKey(k)) sectionData.Remove(k);
                }

                var requiredKeys = new[] { "Satisfactory", "Not Satisfactory", "Not Done", "Satisfactory Following Official Intervention" };
                foreach (var k in requiredKeys)
                {
                    if (checkboxes.TryGetValue($"II.3::{k}", out var v))
                        sectionData[k] = v;
                    else if (!sectionData.ContainsKey(k))
                        sectionData[k] = "false";
                }

                sectionData["EU Standard"] = "";
            }

            // II.4 Identity Check: Specific Cleanup
            if (sectionName.Contains("II.4", StringComparison.OrdinalIgnoreCase))
            {
                var keysToRemove = new[] { "value", "Identity Check", "ISO Code" };
                foreach (var k in keysToRemove)
                {
                    if (sectionData.ContainsKey(k)) sectionData.Remove(k);
                }

                var requiredKeys = new[] { "Yes", "No", "Seal Check Only", "Full Identity Check", "Satisfactory", "Not Satisfactory" };
                foreach (var k in requiredKeys)
                {
                    if (checkboxes.TryGetValue($"II.4::{k}", out var v))
                        sectionData[k] = v;
                    else if (!sectionData.ContainsKey(k))
                        sectionData[k] = "false";
                }
            }

            // II.5 Physical Check: Specific Cleanup
            if (sectionName.Contains("II.5", StringComparison.OrdinalIgnoreCase))
            {
                var keysToRemove = new[] { "value", "ISO Code" };
                foreach (var k in keysToRemove)
                {
                    if (sectionData.ContainsKey(k)) sectionData.Remove(k);
                }

                var requiredKeys = new[] { "Yes", "No", "Satisfactory", "Not Satisfactory" };
                foreach (var k in requiredKeys)
                {
                    if (checkboxes.TryGetValue($"II.5::{k}", out var v))
                        sectionData[k] = v;
                    else if (!sectionData.ContainsKey(k))
                        sectionData[k] = "false";
                }
            }

            // II.6 Laboratory tests: Specific Cleanup
            if (sectionName.Contains("II.6", StringComparison.OrdinalIgnoreCase))
            {
                var keysToRemove = new[] { "value", "ISO Code" };
                foreach (var k in keysToRemove)
                {
                    if (sectionData.ContainsKey(k)) sectionData.Remove(k);
                }

                // Extract Test name: appears as "Test" label followed by the test name value
                var testMatch = Regex.Match(fullText, @"\bTest\b\s*(.*?)(?=\s*\bRandom\b|\s*\bSuspicion\b|\s*\bIntensified\b|\s*\bPending\b|\s*\bSatisfactory\b|\s*\bYes\b|\s*\bNo\b|\s*\bResults\b|$)", RegexOptions.IgnoreCase);
                if (testMatch.Success)
                {
                    var testValue = testMatch.Groups[1].Value.Trim();
                    // Clean up if it's just a label
                    var labels = new[] { "Random", "Suspicion", "Intensified Controls", "Results", "Pending", "Satisfactory", "Not Satisfactory", "Yes", "No" };
                    if (labels.Any(l => testValue.Equals(l, StringComparison.OrdinalIgnoreCase)))
                    {
                        testValue = "";
                    }

                    if (!string.IsNullOrWhiteSpace(testValue))
                        sectionData["Test"] = testValue;
                }

                var ii6Map = new Dictionary<string, string>
                {
                    { "II.6::Yes", "Yes" },
                    { "II.6::No", "No" },
                    { "II.6::Satisfactory", "Satisfactory" },
                    { "II.6::Not Satisfactory", "Not Satisfactory" },
                    { "II.6::Random", "Random" },
                    { "II.6::Suspicion", "Suspicion" },
                    { "II.6::Intensified Controls", "Intensified Controls" },
                    { "II.6::Pending", "Pending" }
                };

                foreach (var scoped in ii6Map)
                {
                    if (checkboxes.TryGetValue(scoped.Key, out var scopedValue))
                    {
                        sectionData[scoped.Value] = scopedValue;
                    }
                }

                var requiredFalseKeys = new[] { "Yes", "No", "Satisfactory", "Not Satisfactory", "Random", "Suspicion", "Intensified Controls", "Pending" };
                foreach (var k in requiredFalseKeys)
                {
                    if (!sectionData.ContainsKey(k)) sectionData[k] = "false";
                }

                if (!sectionData.ContainsKey("Test"))
                {
                    sectionData["Test"] = "";
                }
            }

            // II.11 Acceptable for transit: strip leaked II.x keys; structured fields come from keywords + allowlist
            if (sectionName.Contains("II.11", StringComparison.OrdinalIgnoreCase))
            {
                var ii11Noise = new[]
                {
                    "Yes", "No", "Satisfactory", "Not Satisfactory", "Not Done",
                    "Random", "Suspicion", "Intensified Controls", "Pending",
                    "Seal Check Only", "Full Identity Check", "Satisfactory Following Official Intervention"
                };
                foreach (var k in ii11Noise)
                {
                    if (sectionData.ContainsKey(k)) sectionData.Remove(k);
                }
            }

            // II.12 Acceptable for INTERNAL market: Specific Cleanup
            if (sectionName.Contains("II.12", StringComparison.OrdinalIgnoreCase))
            {
                sectionData.Clear();
                if (!string.IsNullOrWhiteSpace(fullText))
                {
                    sectionData["value"] = fullText.Trim();
                }
            }

            // II.17 Reason for Refusal: Specific Cleanup
            if (sectionName.Contains("II.17", StringComparison.OrdinalIgnoreCase))
            {
                var keysToRemove = sectionData.Keys.Where(k => !k.Equals("value", StringComparison.OrdinalIgnoreCase)).ToList();
                foreach (var k in keysToRemove) sectionData.Remove(k);
            }

            // II.20 Identification of BCP: Specific Cleanup
            if (sectionName.Contains("II.20", StringComparison.OrdinalIgnoreCase))
            {
                if (sectionData.ContainsKey("value"))
                {
                    var val = sectionData["value"].ToString();
                    // Manually extract BCP if keyword extraction didn't catch it
                    if (!sectionData.ContainsKey("BCP") && val.Contains("BCP"))
                    {
                        // Let's rely on regex or known format.
                        // Assuming "BCP" keyword is followed by the BCP name.
                        var match = Regex.Match(val, @"BCP\s+(.*?)(?=\s+(Stamp|Unit number)|$)");
                        if (match.Success)
                        {
                            sectionData["BCP"] = match.Groups[1].Value.Trim();
                        }
                        else
                        {
                            //no action
                        }
                    }

                    sectionData.Remove("value");
                }
            }

            // II.16 Not Acceptable: Specific Cleanup
            if (sectionName.Contains("II.16", StringComparison.OrdinalIgnoreCase) || sectionName.Contains("II16", StringComparison.OrdinalIgnoreCase))
            {
                sectionData["IsChecked"] = "false";
                sectionData["Text"] = "";
                sectionData["DateTime"] = "";

                // Use detected checkbox states instead of hardcoding
                var ii16Options = new[] { "Re-dispatching", "Destruction", "Transformation", "Re-entry" };
                foreach (var option in ii16Options)
                {
                    if (checkboxes.TryGetValue($"II.16::{option}", out var cbState) &&
                        cbState.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        sectionData["IsChecked"] = "true";
                        sectionData["Text"] = option;
                        break;
                    }
                }

                // Extract Date/Time from the full text
                if (lines != null && lines.Count > 0)
                {
                    var dtMatch = Regex.Match(fullText, @"(?:Date/time|Date\s+and\s+time)\s+(?<dt>\d{1,2}\s+[A-Za-z]+\s+\d{4}\s+\d{2}:\d{2}:\d{2}\s+[\+\-]\d{4}\s+[A-Za-z]{3})", RegexOptions.IgnoreCase);
                    if (dtMatch.Success)
                    {
                        sectionData["DateTime"] = dtMatch.Groups["dt"].Value.Trim();
                    }
                    else
                    {
                        // Try matching anywhere in the content lines
                        foreach (var line in lines)
                        {
                            var m = Regex.Match(line, @"(?<dt>\d{1,2}\s+[A-Za-z]+\s+\d{4}\s+\d{2}:\d{2}:\d{2}\s+[\+\-]\d{4}\s+[A-Za-z]{3})");
                            if (m.Success)
                            {
                                sectionData["DateTime"] = m.Groups["dt"].Value.Trim();
                                break;
                            }
                        }
                    }
                }

                // Remove misparsed noise keys
                var keysToKeep = new[] { "IsChecked", "Text", "DateTime", "value" };
                var keysToRemove = sectionData.Keys.Where(k => !keysToKeep.Contains(k)).ToList();
                foreach (var k in keysToRemove) sectionData.Remove(k);
            }

            // II.21 Certifying Officer: Specific Cleanup
            if (sectionName.Contains("II.21", StringComparison.OrdinalIgnoreCase))
            {
                var keysToRemove = new[] { "value", "ISO Code", "Official Inspector", "Full name Signature" };
                foreach (var k in keysToRemove)
                {
                    if (sectionData.ContainsKey(k)) sectionData.Remove(k);
                }

                // Extract structured fields from the free text row when keyword parsing misses them.
                var fullNameMatch = Regex.Match(
                    fullText,
                    @"Full\s+name\s+(?<name>.+?)\s+Signature",
                    RegexOptions.IgnoreCase);
                if (fullNameMatch.Success)
                {
                    var parsedName = fullNameMatch.Groups["name"].Value.Trim();
                    if (!string.IsNullOrWhiteSpace(parsedName))
                    {
                        sectionData["Name"] = parsedName;
                    }
                }

                var dateMatch = Regex.Match(
                    fullText,
                    @"Date\s+of\s+signature\s+(?<date>\d{1,2}\s+[A-Za-z]+\s+\d{4}\s+\d{2}:\d{2}:\d{2}\s+[\+\-]\d{4}\s+[A-Za-z]{3})",
                    RegexOptions.IgnoreCase);
                if (dateMatch.Success)
                {
                    var parsedDate = dateMatch.Groups["date"].Value.Trim();
                    if (!string.IsNullOrWhiteSpace(parsedDate))
                    {
                        sectionData["Date of signature"] = parsedDate;
                    }
                }

                // If parser captured "Full name", normalize to schema key "Name".
                if (sectionData.TryGetValue("Full name", out var fullNameObj))
                {
                    var fullName = fullNameObj?.ToString()?.Trim() ?? "";
                    if (!string.IsNullOrWhiteSpace(fullName) &&
                        (!sectionData.ContainsKey("Name") || string.IsNullOrWhiteSpace(sectionData["Name"]?.ToString())))
                    {
                        sectionData["Name"] = fullName;
                    }
                    sectionData.Remove("Full name");
                }

                if ((!sectionData.ContainsKey("Name") || string.IsNullOrWhiteSpace(sectionData["Name"]?.ToString())) &&
                    formFields.TryGetValue("II.21.Name", out var ii21NameFromPage))
                {
                    sectionData["Name"] = ii21NameFromPage;
                }

                if ((!sectionData.ContainsKey("Date of signature") || string.IsNullOrWhiteSpace(sectionData["Date of signature"]?.ToString())) &&
                    formFields.TryGetValue("II.21.DateOfSignature", out var ii21DateFromPage))
                {
                    sectionData["Date of signature"] = ii21DateFromPage;
                }

                var requiredKeys = new[] { "Name", "Date of signature", "Signature" };
                foreach (var k in requiredKeys)
                {
                    if (!sectionData.ContainsKey(k)) sectionData[k] = "";
                }
            }

            // II.22 Inspection fees: Specific Cleanup
            if (sectionName.Contains("II.22", StringComparison.OrdinalIgnoreCase))
            {
                sectionData.Clear();
            }

            // II.23 Customs Document Reference: Specific Cleanup
            if (sectionName.Contains("II.23", StringComparison.OrdinalIgnoreCase))
            {
                sectionData.Clear();

                // Value is often merged into the section header line on this template.
                var fromHeader = Regex.Match(
                    sectionName,
                    @"II\.23\.?\s*Customs\s+Document\s+Reference\s*(?<val>.+)$",
                    RegexOptions.IgnoreCase);
                var headerValue = fromHeader.Success ? fromHeader.Groups["val"].Value.Trim() : "";

                // Fallback to content text if header doesn't carry the value.
                if (string.IsNullOrWhiteSpace(headerValue))
                {
                    var fromText = Regex.Match(
                        fullText,
                        @"Customs\s+Document\s+Reference\s*(?<val>.+)$",
                        RegexOptions.IgnoreCase);
                    headerValue = fromText.Success ? fromText.Groups["val"].Value.Trim() : "";
                }

                if (string.IsNullOrWhiteSpace(headerValue) &&
                    formFields.TryGetValue("II.23.CustomsDocumentReference", out var ii23FromPage))
                {
                    headerValue = ii23FromPage?.Trim() ?? "";
                }

                sectionData["value"] = headerValue;
            }

            // II.25 BCP Reference Number: Specific Cleanup
            if (sectionName.Contains("II.25", StringComparison.OrdinalIgnoreCase) && sectionName.Contains("BCP Reference Number", StringComparison.OrdinalIgnoreCase))
            {
                sectionData.Clear();

                var headerValue = "";
                if (!string.IsNullOrWhiteSpace(fullText))
                {
                    headerValue = fullText.Trim();
                }

                if (string.IsNullOrWhiteSpace(headerValue) &&
                    formFields.TryGetValue("II.25.BCPReferenceNumber", out var ii25FromPage))
                {
                    headerValue = ii25FromPage?.Trim() ?? "";
                }

                sectionData["value"] = headerValue;
            }

            // III.4 Details on re-dispatching: Specific Cleanup
            if (sectionName.Contains("III.4", StringComparison.OrdinalIgnoreCase))
            {
                // Country and ISO Code
                var countryMatch = Regex.Match(fullText, @"Country\s+of\s+destination\s*(.*?)(?=\s*ISO\s+Code|$)", RegexOptions.IgnoreCase);
                if (countryMatch.Success)
                    sectionData["Country of destination"] = countryMatch.Groups[1].Value.Trim();

                var isoMatch = Regex.Match(fullText, @"ISO\s+Code\s*(.*?)(?=\s*Exit\s+authority|$)", RegexOptions.IgnoreCase);
                if (isoMatch.Success)
                    sectionData["ISO Code"] = isoMatch.Groups[1].Value.Trim();

                // Exit Authority and Code
                var exitMatch = Regex.Match(fullText, @"Exit\s+authority\s*(.*?)(?=\s*Code|$)", RegexOptions.IgnoreCase);
                if (exitMatch.Success)
                    sectionData["Exit authority"] = exitMatch.Groups[1].Value.Trim();

                var codeMatch = Regex.Match(fullText, @"\bCode\s*(.*?)(?=\s*Means\s+of\s+transport|$|\s*Mode)", RegexOptions.IgnoreCase);
                if (codeMatch.Success)
                    sectionData["Code"] = codeMatch.Groups[1].Value.Trim();

                // Means of Transport row (Mode, Document, Identification)
                var transportMatch = Regex.Match(fullText, @"Identification\s*(.*?)(?=\s*Date\s+and\s+time|$)", RegexOptions.IgnoreCase);
                if (transportMatch.Success)
                {
                    var transportVals = transportMatch.Groups[1].Value.Trim();
                    // Only try to parse if there's actual content (not just matching whitespace/next labels)
                    if (!string.IsNullOrWhiteSpace(transportVals) && !transportVals.Equals("Date and time", StringComparison.OrdinalIgnoreCase))
                    {
                        var modeMatch = Regex.Match(transportVals, @"^(?<mode>.+?)\s+(?<doc>\w+)\s+(?<id>\w+)$");
                        if (modeMatch.Success)
                        {
                            sectionData["Mode"] = modeMatch.Groups["mode"].Value.Trim();
                            sectionData["International transport document"] = modeMatch.Groups["doc"].Value.Trim();
                            sectionData["Identification"] = modeMatch.Groups["id"].Value.Trim();
                        }
                        else
                        {
                            var modeMatch2 = Regex.Match(transportVals, @"^(?<mode>\S+)\s+(?<id>\S+)$");
                            if (modeMatch2.Success)
                            {
                                sectionData["Mode"] = modeMatch2.Groups["mode"].Value.Trim();
                                sectionData["Identification"] = modeMatch2.Groups["id"].Value.Trim();
                            }
                        }
                    }
                }

                // Keep only the expected keys; remove noise keys
                var expectedKeys = new[] { "Country of destination", "ISO Code", "Exit authority", "Code", "Mode", "International transport document", "Identification" };
                var keysToRemove = sectionData.Keys
                    .Where(k => !expectedKeys.Contains(k, StringComparer.OrdinalIgnoreCase))
                    .ToList();
                foreach (var k in keysToRemove) sectionData.Remove(k);

                // Default any missing keys to ""
                foreach (var k in expectedKeys)
                {
                    if (!sectionData.ContainsKey(k))
                        sectionData[k] = "";
                }

                // If values are actually labels (e.g. from an empty form), clear them
                // This handles cases where lookahead might have caught too much or labels were extracted as values
                var allLabels = new[] {
                    "Country of destination", "ISO Code", "Exit authority", "Code",
                    "Means of transport", "Mode", "International transport document",
                    "Identification", "Date and time"
                };

                foreach (var k in expectedKeys)
                {
                    var val = sectionData[k]?.ToString() ?? "";
                    if (string.IsNullOrWhiteSpace(val)) continue;

                    foreach (var label in allLabels)
                    {
                        if (val.Equals(label, StringComparison.OrdinalIgnoreCase) ||
                            val.StartsWith(label + " ", StringComparison.OrdinalIgnoreCase) ||
                            val.Contains(" " + label + " ", StringComparison.OrdinalIgnoreCase))
                        {
                            sectionData[k] = "";
                            break;
                        }
                    }
                }

                // Normalize Mode value (e.g. "AIRPLANE" -> "AIRPLANE", keep as-is)
                if (sectionData.TryGetValue("Mode", out var modeObj))
                {
                    var modeStr = modeObj?.ToString()?.Trim() ?? "";
                    sectionData["Mode"] = modeStr.ToUpperInvariant();
                }
            }

            // III.5 Follow up: Specific Cleanup
            if (sectionName.Contains("III.5", StringComparison.OrdinalIgnoreCase))
            {
                var keysToRemove = new[] { "value", "Yes", "No", "Satisfactory", "Not Satisfactory", "ISO Code" };
                foreach (var k in keysToRemove) if (sectionData.ContainsKey(k)) sectionData.Remove(k);

                var falseKeys = new[] { "Local competent authority", "Second entry point", "Arrival of consignment" };
                foreach (var k in falseKeys) if (!sectionData.ContainsKey(k)) sectionData[k] = "false";

                var emptyKeys = new[] { "Control at destination", "Compliance of the consignment" };
                foreach (var k in emptyKeys) if (!sectionData.ContainsKey(k)) sectionData[k] = "";

                // Normalize III.5 output shape (exclusive Yes/No; blank if both true or neither true):
                // - Arrival: scoped III.5::Arrival::Yes / ::No when anchors exist; else single checkbox fallback.
                // - Compliance: scoped III.5::Yes / III.5::No.
                var hasArrivalScoped = checkboxes.ContainsKey("III.5::Arrival::Yes") ||
                                       checkboxes.ContainsKey("III.5::Arrival::No");
                string arrivalOutcome;
                if (hasArrivalScoped)
                {
                    var arrY = checkboxes.TryGetValue("III.5::Arrival::Yes", out var av) &&
                               av.Equals("true", StringComparison.OrdinalIgnoreCase);
                    var arrN = checkboxes.TryGetValue("III.5::Arrival::No", out var an) &&
                               an.Equals("true", StringComparison.OrdinalIgnoreCase);
                    if (arrY && arrN)
                    {
                        arrivalOutcome = "";
                    }
                    else if (arrY)
                    {
                        arrivalOutcome = "Yes";
                    }
                    else if (arrN)
                    {
                        arrivalOutcome = "No";
                    }
                    else
                    {
                        arrivalOutcome = "";
                    }
                }
                else if (sectionData.TryGetValue("Arrival of consignment", out var arrivalObj))
                {
                    var arrivalRaw = arrivalObj?.ToString()?.Trim() ?? "";
                    arrivalOutcome = arrivalRaw.Equals("true", StringComparison.OrdinalIgnoreCase) ? "Yes" : "";
                }
                else
                {
                    arrivalOutcome = "";
                }

                sectionData["Arrival of consignment"] = arrivalOutcome;

                var yesSelected = checkboxes.TryGetValue("III.5::Yes", out var yesVal) &&
                                  yesVal.Equals("true", StringComparison.OrdinalIgnoreCase);
                var noSelected = checkboxes.TryGetValue("III.5::No", out var noVal) &&
                                 noVal.Equals("true", StringComparison.OrdinalIgnoreCase);

                string complianceOutcome;
                if (yesSelected && noSelected)
                {
                    complianceOutcome = "";
                }
                else if (yesSelected)
                {
                    complianceOutcome = "Yes";
                }
                else if (noSelected)
                {
                    complianceOutcome = "No";
                }
                else
                {
                    complianceOutcome = "";
                }

                sectionData["Compliance of the consignment"] = complianceOutcome;
            }

            // IV. Fiche for sampling: Specific Cleanup
            if (sectionName.Equals("IV. Fiche for sampling", StringComparison.OrdinalIgnoreCase))
            {
                var val = sectionData.ContainsKey("value") ? sectionData["value"]?.ToString() ?? "" : "";
                sectionData.Clear();
                sectionData["value"] = val;
            }


            // IV. References: Specific Cleanup
            if (sectionName.Equals("IV. References", StringComparison.OrdinalIgnoreCase))
            {
                var keysToKeep = new[] { "Name", "Name of the inspector", "Phone/email", "Certificate reference number" };

                // Robust Phone/Email extraction
                // The text often looks like: "Phone/Email +44 /email@address.com 1179 381171 Certificate reference number"
                var matchPhoneEmail = Regex.Match(fullText, @"Phone/Email\s+(?<val>.*?)(?=\s*Certificate\s+reference\s+number|$)", RegexOptions.IgnoreCase);
                if (matchPhoneEmail.Success)
                {
                    var rawVal = matchPhoneEmail.Groups["val"].Value.Trim();
                    // Extract email
                    var emailMatch = Regex.Match(rawVal, @"\S+@\S+");
                    var email = emailMatch.Success ? emailMatch.Value.TrimStart('/') : "";
                    // Extract phone parts (things that aren't the email)
                    var phoneParts = Regex.Replace(rawVal, Regex.Escape(emailMatch.Success ? emailMatch.Value : ""), "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var phone = string.Join(" ", phoneParts).Trim().TrimEnd('/');

                    if (!string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(email))
                        sectionData["Phone/email"] = $"{phone}/{email}";
                    else if (!string.IsNullOrEmpty(phone))
                        sectionData["Phone/email"] = phone;
                    else if (!string.IsNullOrEmpty(email))
                        sectionData["Phone/email"] = email;
                }

                // Because keys contain spaces, let's use the raw form and they'll be sanitized to NameOfTheInspector etc.
                var keysToRemove = sectionData.Keys.Where(k => !keysToKeep.Contains(k, StringComparer.OrdinalIgnoreCase)).ToList();
                foreach (var k in keysToRemove) sectionData.Remove(k);
            }

            // IV. Identification of the sample: Specific Cleanup
            if (sectionName.Equals("IV. Identification of the sample", StringComparison.OrdinalIgnoreCase))
            {
                var keysToKeep = new[] { "Establishments of origin", "Countries of origin", "Country of dispatch", "Place of destination", "Commodity", "EPPO Code" };

                // Specific extraction for PlaceOfDestination to prevent bleeding into Commodity
                var matchPlace = Regex.Match(fullText, @"Place of destination\s+(?<val>.*?)(?=\s+\d+\.\s*\d+)", RegexOptions.IgnoreCase);
                if (matchPlace.Success) sectionData["Place of destination"] = matchPlace.Groups["val"].Value.Trim();

                // Custom extraction for Commodity:
                // Captures the sequence starting with digit pattern (e.g. 1. 51 ...) until Eppo Code or next section
                var matchCommodity = Regex.Match(fullText, @"(?<val>\d+\.\s*\d+\s+.*?)(?=\s*Eppo\s+Code|\s*$)", RegexOptions.IgnoreCase);
                if (matchCommodity.Success)
                {
                    string comm = matchCommodity.Groups["val"].Value.Trim();
                    // Just removing the 'Commodity' literal injected by parser if any
                    comm = Regex.Replace(comm, @"\bCommodity\s*", "", RegexOptions.IgnoreCase).Trim();
                    sectionData["Commodity"] = comm;
                }

                var keysToRemove = sectionData.Keys.Where(k => !keysToKeep.Contains(k, StringComparer.OrdinalIgnoreCase)).ToList();
                foreach (var k in keysToRemove) sectionData.Remove(k);
            }

            // IV. Requested analysis: Specific Cleanup
            if (sectionName.Equals("IV. Requested analysis", StringComparison.OrdinalIgnoreCase))
            {
                var keysToKeep = new[] { "Harmful organism/name", "Inspector conclusion", "Motivation" };

                // Extracted name often drops the prefix or is missed by keyword extraction
                var matchHarmful = Regex.Match(fullText, @"Harmful organism name\s+(?<val>.*?)(?=\s*Inspector\s+conclusion|\s*Motivation|$)", RegexOptions.IgnoreCase);
                if (matchHarmful.Success && (!sectionData.ContainsKey("Harmful organism/name") || string.IsNullOrWhiteSpace(sectionData["Harmful organism/name"]?.ToString())))
                {
                    sectionData["Harmful organism/name"] = matchHarmful.Groups["val"].Value.Trim();
                }

                var keysToRemove = sectionData.Keys.Where(k => !keysToKeep.Contains(k, StringComparer.OrdinalIgnoreCase)).ToList();
                foreach (var k in keysToRemove) sectionData.Remove(k);
            }

            // IV. Results: Specific Cleanup
            if (sectionName.Equals("IV. Results", StringComparison.OrdinalIgnoreCase))
            {
                var keysToKeep = new[] { "Name", "Address", "Identification", "Phone/email", "Sample date", "Sample time", "Sample batch Nr.", "Sample use by date", "Sample type", "Number of samples", "Conservation of samples", "Laboratory test", "Laboratory test method", "Released date", "Sample Results", "Laboratory conclusion" };

                // Fix Identification: If it's currently a number, re-extract from text to handle non-numeric values like lab names.
                if (sectionData.TryGetValue("Identification", out var identObj))
                {
                    var identStr = identObj?.ToString() ?? "";
                    if (Regex.IsMatch(identStr, @"^\d+$"))
                    {
                        var matchIdent = Regex.Match(fullText, @"Identification\s+(?<val>.*?)(?=\s+Phone/Email|$)", RegexOptions.IgnoreCase);
                        if (matchIdent.Success && !string.IsNullOrWhiteSpace(matchIdent.Groups["val"].Value))
                        {
                            sectionData["Identification"] = matchIdent.Groups["val"].Value.Trim();
                        }
                    }
                }

                // Map "Sample results" to "Sample Results" (schema requirement)
                if (sectionData.TryGetValue("Sample results", out var srObj))
                {
                    sectionData["Sample Results"] = srObj;
                    sectionData.Remove("Sample results");
                }
                else if (!sectionData.ContainsKey("Sample Results"))
                {
                    sectionData["Sample Results"] = "";
                }

                // If "Released date" captured "Sample results", clear it.
                if (sectionData.TryGetValue("Released date", out var rdObj))
                {
                    var rdStr = rdObj?.ToString() ?? "";
                    if (rdStr.Contains("Sample results", StringComparison.OrdinalIgnoreCase))
                    {
                        sectionData["Released date"] = "";
                    }
                }

                var keysToRemove = sectionData.Keys.Where(k => !keysToKeep.Contains(k, StringComparer.OrdinalIgnoreCase)).ToList();
                foreach (var k in keysToRemove) sectionData.Remove(k);
            }

            // III.6 Official Inspector: Specific Cleanup
            if (sectionName.Contains("III.6", StringComparison.OrdinalIgnoreCase))
            {
                sectionData.Clear();
                var keysToDefault = new[] { "Full name", "Date of signature", "Signature" };
                foreach (var k in keysToDefault) sectionData[k] = "";
            }
        }

        /// <summary>
        /// Attempts to extract key-value pairs from text lines
        /// </summary>
        private Dictionary<string, string> ExtractKeyValuePairs(List<string> lines)
        {
            var pairs = new Dictionary<string, string>();
            var commonKeys = new[] {
                "Country and place of issue", "Name", "Address", "Country", "ISO Code",
                "Time", "Date", "Approval Number", "Type", "Organisation",
                "Document reference", "Date of issue", "Date", "Time", "Approval Number", "Means of transport", "Identification", "International transport document",
                "Name of Signatory", "Total Gross Weight", "Total Net Weight", "Total number of packages", "Commercial documentary references", "Commodity", "Date of signature", "Signature", "Full name", "Species", "Net Weight", "Package Count", "Product Type", "Establishment of Origin", "Country of Origin",
                "Commercial documentary references", "Commodity", "Date of signature", "Signature", "Full name", "Species", "Net Weight", "Package Count", "Product Type", "Establishment of Origin", "Country of Origin",
                "Name of the inspector", "Phone/email", "Phone/Email", "Certificate reference number", "Countries of origin",
                "EPPO Code", "Harmful organism/name", "Inspector conclusion", "Motivation",
                "Sample date", "Sample time", "Sample batch Nr.", "Sample use by date", "Sample type",
                "Number of samples", "Conservation of samples", "Laboratory test", "Laboratory test method",
                "Released date", "Sample results", "Sample/R results", "Laboratory conclusion", "Phone Email"
            }.OrderByDescending(k => k.Length).ToArray();

            foreach (var line in lines)
            {
                // check if line matching "Key: Value" pattern
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

                // Check for "Key Value" without colon for specific known keys
                // Handle multiple keys in one line "Country England ISO Code GB-ENG"
                string remainingLine = line.Trim();

                // Repeatedly try to find keys at the start of the remaining string
                // or if not at start, split the line if multiple keys exist

                // Simpler approach: Locate all known keys in the line
                var foundKeyIndices = new List<(string Key, int Index)>();
                foreach (var key in commonKeys)
                {
                    // Check if key exists in line
                    // We need word boundary check really, but containment is a start
                    // We prioritize keys that appear earlier
                    int idx = line.IndexOf(key, StringComparison.OrdinalIgnoreCase);
                    while (idx != -1)
                    {
                        // Verify semantic boundary? (e.g. not substring of another word)
                        // For now accept it
                        foundKeyIndices.Add((key, idx));
                        idx = line.IndexOf(key, idx + 1, StringComparison.OrdinalIgnoreCase);
                    }
                }

                if (foundKeyIndices.Any())
                {
                    // Sort by occurrence in line
                    foundKeyIndices = foundKeyIndices.OrderBy(x => x.Index).ToList();

                    var filteredIndices = new List<(string Key, int Index)>();
                    for (int i = 0; i < foundKeyIndices.Count; i++)
                    {
                        var current = foundKeyIndices[i];
                        bool skip = false;
                        // Check against other entries
                        foreach (var other in foundKeyIndices)
                        {
                            if (current == other) continue;
                            // If current is inside other (same start, shorter length)
                            if (other.Index == current.Index && other.Key.Length > current.Key.Length)
                            {
                                skip = true;
                                break;
                            }
                            // If current is inside other (later start, ends before or at other end)
                            if (other.Index < current.Index && (other.Index + other.Key.Length) >= (current.Index + current.Key.Length))
                            {
                                skip = true;
                                break;
                            }
                        }
                        if (!skip) filteredIndices.Add(current);
                    }
                    foundKeyIndices = filteredIndices.OrderBy(x => x.Index).ToList();

                    // Now extract values between keys
                    for (int i = 0; i < foundKeyIndices.Count; i++)
                    {
                        var keyItem = foundKeyIndices[i];
                        int valueStart = keyItem.Index + keyItem.Key.Length;
                        int valueEnd = (i + 1 < foundKeyIndices.Count) ? foundKeyIndices[i + 1].Index : line.Length;

                        if (valueEnd > valueStart)
                        {
                            var val = line.Substring(valueStart, valueEnd - valueStart).Trim();
                            // remove leading colon/hyphen if present
                            if (val.StartsWith(":") || val.StartsWith("-")) val = val.Substring(1).Trim();

                            if (!string.IsNullOrEmpty(val))
                            {
                                pairs[keyItem.Key] = val; // Normalize key casing to our common key?
                                                          // Ideally use the casing from commonKeys list, but keyItem.Key is from that list
                            }
                        }
                    }
                }
            }

            return pairs;
        }

        /// <summary>
        /// Checks if a field is relevant to a section based on naming patterns
        /// </summary>
        private bool IsFieldRelevantToSection(string fieldName, string sectionName)
        {
            if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(sectionName))
                return false;

            if (sectionName.Contains("I.25", StringComparison.OrdinalIgnoreCase))
            {
                return fieldName.Equals("I.25. For re-entry", StringComparison.OrdinalIgnoreCase);
            }

            // I.16 keys: restrict ONLY to I.16
            var i16Keys = new[] { "Ambient", "Chilled", "Frozen" };
            if (i16Keys.Contains(fieldName, StringComparer.OrdinalIgnoreCase))
                return sectionName.Contains("I.16", StringComparison.OrdinalIgnoreCase);

            // I.19 keys: restrict ONLY to I.19
            var i19Keys = new[] { "Conforming", "Non-conforming" };
            if (i19Keys.Contains(fieldName, StringComparer.OrdinalIgnoreCase))
                return sectionName.Contains("I.19", StringComparison.OrdinalIgnoreCase);

            // Part II/III checkbox keys: allow into any II.x or III.x section
            if (sectionName.Contains("II.3", StringComparison.OrdinalIgnoreCase))
            {
                var keys = new[] { "Satisfactory", "Not Satisfactory", "Not Done", "Satisfactory Following Official Intervention", "EU Standard" };
                return keys.Contains(fieldName, StringComparer.OrdinalIgnoreCase);
            }

            if (sectionName.Contains("II.4", StringComparison.OrdinalIgnoreCase))
            {
                var keys = new[] { "Yes", "No", "Seal Check Only", "Full Identity Check", "Satisfactory", "Not Satisfactory" };
                return keys.Contains(fieldName, StringComparer.OrdinalIgnoreCase);
            }

            if (sectionName.Contains("II.5", StringComparison.OrdinalIgnoreCase))
            {
                var keys = new[] { "Yes", "No", "Satisfactory", "Not Satisfactory" };
                return keys.Contains(fieldName, StringComparer.OrdinalIgnoreCase);
            }

            // II.6 relies strictly on prefixed checkboxes (II.6::*) via ii6Map later in SaveSection.
            // Returning false here prevents global checkboxes (like 'Satisfactory' from II.5) from bleeding into II.6.

            if (sectionName.Contains("II.11", StringComparison.OrdinalIgnoreCase))
            {
                var keys = new[] { "Human consumption", "Human Consumption", "Validation", "Acceptable", "Refused" };
                return keys.Contains(fieldName, StringComparer.OrdinalIgnoreCase);
            }

            var iii5OnlyKeys = new[] { "Local competent authority", "Second entry point", "Arrival of consignment" };
            if (iii5OnlyKeys.Contains(fieldName, StringComparer.OrdinalIgnoreCase))
            {
                return sectionName.Contains("III.5", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        /// <summary>
        /// Groups words into lines based on vertical position
        /// </summary>
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

        private string ExtractIi23CustomsDocumentReference(List<List<Word>> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                var lineWords = lines[i];
                var lineText = string.Join(" ", lineWords.Select(w => w.Text)).Trim();
                if (!lineText.Contains("II.23", StringComparison.OrdinalIgnoreCase) ||
                    !lineText.Contains("Customs Document Reference", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var m = Regex.Match(
                    lineText,
                    @"II\.23\.?\s*Customs\s+Document\s+Reference\s*(?<val>.+)$",
                    RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    var value = m.Groups["val"].Value.Trim();
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        return value;
                    }
                }

                // On this template the value can be on the next visual line.
                for (int j = i + 1; j < lines.Count; j++)
                {
                    var nextText = string.Join(" ", lines[j].Select(w => w.Text)).Trim();
                    if (string.IsNullOrWhiteSpace(nextText))
                    {
                        continue;
                    }

                    // Stop if another section starts before we find a candidate value.
                    if (Regex.IsMatch(nextText, @"^(II|III)\.\d+\.?", RegexOptions.IgnoreCase))
                    {
                        break;
                    }

                    // Prefer short code-like values (e.g., IUUNA).
                    if (Regex.IsMatch(nextText, @"^[A-Z0-9-]{3,20}$"))
                    {
                        return nextText;
                    }
                }
            }

            return string.Empty;
        }

        private Dictionary<string, string> ExtractIi21CertifyingOfficerFields(List<List<Word>> lines)
        {
            var result = new Dictionary<string, string>();
            var startIndex = -1;

            for (int i = 0; i < lines.Count; i++)
            {
                var lineText = string.Join(" ", lines[i].Select(w => w.Text)).Trim();
                if (lineText.Contains("II.21", StringComparison.OrdinalIgnoreCase) &&
                    lineText.Contains("Certifying officer", StringComparison.OrdinalIgnoreCase))
                {
                    startIndex = i;
                    break;
                }
            }

            if (startIndex == -1)
            {
                return result;
            }

            var block = new StringBuilder();
            for (int i = startIndex + 1; i < lines.Count; i++)
            {
                var lineText = string.Join(" ", lines[i].Select(w => w.Text)).Trim();
                if (string.IsNullOrWhiteSpace(lineText))
                {
                    continue;
                }

                if (lineText.Contains("II.22", StringComparison.OrdinalIgnoreCase) ||
                    lineText.Contains("II.23", StringComparison.OrdinalIgnoreCase) ||
                    Regex.IsMatch(lineText, @"^(III)\.\d+\.?", RegexOptions.IgnoreCase))
                {
                    break;
                }

                block.Append(lineText).Append(' ');
            }

            var fullText = block.ToString().Trim();
            if (!string.IsNullOrWhiteSpace(fullText))
            {
                var nameMatch = Regex.Match(
                    fullText,
                    @"Full\s+name\s+(?<name>.+?)\s+Signature",
                    RegexOptions.IgnoreCase);
                if (nameMatch.Success)
                {
                    var name = nameMatch.Groups["name"].Value.Trim();
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        result["Name"] = name;
                    }
                }

                var dateMatch = Regex.Match(
                    fullText,
                    @"(?<date>\d{1,2}\s+[A-Za-z]+\s+\d{4}\s+\d{2}:\d{2}:\d{2}\s+[\+\-]\d{4}\s+[A-Za-z]{3})",
                    RegexOptions.IgnoreCase);
                if (dateMatch.Success)
                {
                    var date = dateMatch.Groups["date"].Value.Trim();
                    if (!string.IsNullOrWhiteSpace(date))
                    {
                        result["Date of signature"] = date;
                    }
                }
            }

            // Fallback: scan entire page lines in case section-column grouping excluded II.21 row text.
            if (!result.ContainsKey("Name") || !result.ContainsKey("Date of signature"))
            {
                var allText = string.Join(" ", lines.Select(l => string.Join(" ", l.Select(w => w.Text).ToList()))).Trim();
                if (!result.ContainsKey("Name"))
                {
                    var nameGlobalMatch = Regex.Match(
                        allText,
                        @"Full\s+name\s+(?<name>.+?)\s+Signature",
                        RegexOptions.IgnoreCase);
                    if (nameGlobalMatch.Success)
                    {
                        var name = nameGlobalMatch.Groups["name"].Value.Trim();
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            result["Name"] = name;
                        }
                    }
                }

                if (!result.ContainsKey("Date of signature"))
                {
                    var dateGlobalMatch = Regex.Match(
                        allText,
                        @"(?<date>\d{1,2}\s+[A-Za-z]+\s+\d{4}\s+\d{2}:\d{2}:\d{2}\s+[\+\-]\d{4}\s+[A-Za-z]{3})",
                        RegexOptions.IgnoreCase);
                    if (dateGlobalMatch.Success)
                    {
                        var date = dateGlobalMatch.Groups["date"].Value.Trim();
                        if (!string.IsNullOrWhiteSpace(date))
                        {
                            result["Date of signature"] = date;
                        }
                    }
                }
            }

            return result;
        }

        private bool IsMainSectionHeader(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            text = text.Trim().ToUpper();

            return text.StartsWith("PART I: DESCRIPTION") ||
                   text.StartsWith("PART II: CONTROLS") ||
                   text.StartsWith("PART III: FOLLOW UP") ||
                   text.StartsWith("PART IV");
        }

        private string SanitizePropertyName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return name;
            if (name == "I.25 For-reentry") return "I25For-reentry";
            if (name == "IV. Requested analysis") return "IVRequested analysis";
            if (name == "IV. Fiche for sampling") return "IVFicheForSampling";
            if (name == "IV. References") return "IVReferences";
            if (name == "IV. Identification of the sample") return "IVIdentificationOfTheSample";
            if (name == "IV. Results") return "IVResults";
            if (name == "value" || name == "Value") return "value";

            // Split by non-alphanumeric characters to handle spaces, dots, slashes, etc.
            var parts = Regex.Split(name, @"[^a-zA-Z0-9]+");
            var sb = new StringBuilder();

            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part)) continue;
                // Roman-style section numbers use only I/i (e.g. II.11, III.5) — keep II / III, not Ii / Iii.
                if (part.All(c => c == 'i' || c == 'I'))
                {
                    sb.Append(part.ToUpperInvariant());
                    continue;
                }

                var lower = part.ToLower();
                sb.Append(char.ToUpper(lower[0]) + lower.Substring(1));
            }

            // Ensure identifier doesn't start with a digit
            if (sb.Length > 0 && char.IsDigit(sb[0]))
            {
                sb.Insert(0, "_");
            }

            return sb.ToString();
        }
    }
}