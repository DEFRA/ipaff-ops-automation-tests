using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;

namespace Defra.UI.Tests.Tools.PDFProcessor.Extractors
{
    /// <summary>
    /// Extracts checkbox states from PDF using AcroForms and Vector Path Analysis (visual fallback)
    /// </summary>
    public class CheckboxExtractor
    {
        /// <summary>
        /// Extracts checkbox field values from a PDF page using AcroForms or Visual Path Analysis
        /// </summary>
        public Dictionary<string, string> ExtractCheckboxes(Page page, PdfDocument document)
        {
            var checkboxes = new Dictionary<string, string>();

            try
            {
                if (document.TryGetForm(out var form) && form != null && form.Fields != null)
                {
                    foreach (var field in form.Fields)
                    {
                        var fieldTypeName = field.GetType().Name;
                        if (fieldTypeName.Contains("Checkbox", StringComparison.OrdinalIgnoreCase) ||
                            fieldTypeName.Contains("Button", StringComparison.OrdinalIgnoreCase))
                        {
                            var fieldName = field.Information?.MappingName ?? field.ToString();
                            var fieldString = field.ToString();
                            var isChecked = fieldString.Contains("true", StringComparison.OrdinalIgnoreCase) ||
                                          fieldString.Contains("yes", StringComparison.OrdinalIgnoreCase) ||
                                          fieldString.Contains("checked", StringComparison.OrdinalIgnoreCase);

                            checkboxes[fieldName] = isChecked.ToString().ToLower();
                        }
                    }
                }
            }
            catch { }

            try
            {
                ExtractFromPaths(page, checkboxes);
            }
            catch (Exception)
            {
            }

            return checkboxes;
        }

        /// <summary>
        /// Returns the set of (tx, ty) positions where a tick-mark XObject is placed on this page.
        /// Each checked checkbox has an image XObject (e.g. /Im2) placed at the box's origin.
        /// </summary>
        private static HashSet<(double tx, double ty)> GetTickMarkPositions(Page page)
        {
            var positions = new HashSet<(double, double)>();
            var ops = page.Operations;
            for (int i = 1; i < ops.Count; i++)
            {
                if (ops[i].GetType().Name != "InvokeNamedXObject") continue;

                var nameProp = ops[i].GetType().GetProperty("Name");
                var xobjName = nameProp?.GetValue(ops[i])?.ToString() ?? "";
                // Tick marks are image XObjects (Im2, Im3, etc.) — not Im1 which is the page background
                if (!xobjName.StartsWith("/Im", StringComparison.OrdinalIgnoreCase) ||
                    xobjName == "/Im1") continue;

                if (ops[i - 1].GetType().Name != "ModifyCurrentTransformationMatrix") continue;

                var valProp = ops[i - 1].GetType().GetProperty("Value");
                var rawValue = valProp?.GetValue(ops[i - 1]);

                double tx, ty;
                if (rawValue is double[] dMatrix && dMatrix.Length >= 6)
                {
                    tx = dMatrix[4]; ty = dMatrix[5];
                }
                else if (rawValue is decimal[] mMatrix && mMatrix.Length >= 6)
                {
                    tx = (double)mMatrix[4]; ty = (double)mMatrix[5];
                }
                else
                {
                    continue;
                }

                // tx = matrix[4], ty = matrix[5]
                positions.Add((Math.Round(tx, 1), Math.Round(ty, 1)));
            }
            return positions;
        }

        private void ExtractFromPaths(Page page, Dictionary<string, string> checkboxes)
        {
            var targetLabels = new[]
            {
                "Ambient", "Chilled", "Frozen",
                "Conforming", "Non-conforming",
                "Not Done", "Satisfactory Following Official Intervention",
                "Seal Check Only", "Full Identity Check",
                "Random", "Suspicion", "Intensified Controls", "Pending",
                "Human consumption", "Human Consumption", "Feedingstuff", "Technical use", "Other",
                "Local competent authority", "Second entry point", "Arrival of consignment", "I.25. For re-entry",
                "For internal market", "For transfer to", "For re-export to", "For transhipment to", "Domestic use"
            };
            var words = page.GetWords().ToList();
            var paths = page.ExperimentalAccess.Paths.ToList();
            var tickPositions = GetTickMarkPositions(page);

            foreach (var label in targetLabels)
            {
                var labelBounds = FindLabelBounds(words, label);

                if (labelBounds == null && label == "Non-conforming")
                {
                    var fuzzy = words.FirstOrDefault(w => w.Text.Contains("Non-conforming", StringComparison.OrdinalIgnoreCase));
                    if (fuzzy != null) labelBounds = fuzzy.BoundingBox;
                }

                if (labelBounds == null) continue;

                checkboxes[label] = DetectCheckboxState(labelBounds.Value, paths, words, tickPositions);
            }

            // Scoped extraction for Sections II.3, II.4, II.5, II.7
            ExtractIi3ScopedCheckboxes(words, paths, tickPositions, checkboxes);
            ExtractIi4ScopedCheckboxes(words, paths, tickPositions, checkboxes);
            ExtractIi5ScopedCheckboxes(words, paths, tickPositions, checkboxes);
            ExtractIi7ScopedCheckboxes(words, paths, tickPositions, checkboxes);

            ExtractIi6ScopedCheckboxes(words, paths, tickPositions, checkboxes);
            ExtractIi16ScopedCheckboxes(words, paths, tickPositions, checkboxes);
            ExtractI16ScopedCheckboxes(words, paths, tickPositions, checkboxes);
            ExtractIii5ScopedCheckboxes(words, paths, tickPositions, checkboxes);
        }

        private void ExtractIi3ScopedCheckboxes(List<Word> words, List<UglyToad.PdfPig.Graphics.PdfPath> paths, HashSet<(double, double)> tickPositions, Dictionary<string, string> checkboxes)
        {
            var anchor = words.FirstOrDefault(w => w.Text.Equals("II.3", StringComparison.OrdinalIgnoreCase)) ?? 
                         words.FirstOrDefault(w => w.Text.Contains("II.3"));
            if (anchor == null) return;

            var labels = new[] { "Satisfactory", "Not Satisfactory", "Not Done", "Satisfactory Following Official Intervention" };
            ExtractSectionScopedCheckboxes("II.3", anchor, labels, words, paths, tickPositions, checkboxes);
        }

        private void ExtractIi4ScopedCheckboxes(List<Word> words, List<UglyToad.PdfPig.Graphics.PdfPath> paths, HashSet<(double, double)> tickPositions, Dictionary<string, string> checkboxes)
        {
            var anchor = words.FirstOrDefault(w => w.Text.Equals("II.4", StringComparison.OrdinalIgnoreCase)) ?? 
                         words.FirstOrDefault(w => w.Text.Contains("II.4"));
            if (anchor == null) return;

            var labels = new[] { "Yes", "No", "Seal Check Only", "Full Identity Check", "Satisfactory", "Not Satisfactory" };
            ExtractSectionScopedCheckboxes("II.4", anchor, labels, words, paths, tickPositions, checkboxes);
        }

        private void ExtractIi5ScopedCheckboxes(List<Word> words, List<UglyToad.PdfPig.Graphics.PdfPath> paths, HashSet<(double, double)> tickPositions, Dictionary<string, string> checkboxes)
        {
            var anchor = words.FirstOrDefault(w => w.Text.Equals("II.5", StringComparison.OrdinalIgnoreCase)) ??
                         words.FirstOrDefault(w => w.Text.Contains("II.5"));
            if (anchor == null) return;

            var labels = new[] { "Yes", "No", "Satisfactory", "Not Satisfactory" };
            ExtractSectionScopedCheckboxes("II.5", anchor, labels, words, paths, tickPositions, checkboxes);
        }

        private void ExtractIi7ScopedCheckboxes(List<Word> words, List<UglyToad.PdfPig.Graphics.PdfPath> paths, HashSet<(double, double)> tickPositions, Dictionary<string, string> checkboxes)
        {
            var anchor = words.FirstOrDefault(w => w.Text.Equals("II.7", StringComparison.OrdinalIgnoreCase)) ??
                         words.FirstOrDefault(w => w.Text.Contains("II.7"));
            if (anchor == null) return;

            var labels = new[] { "Yes", "No", "Satisfactory", "Not Satisfactory" };
            ExtractSectionScopedCheckboxes("II.7", anchor, labels, words, paths, tickPositions, checkboxes);
        }

        private void ExtractSectionScopedCheckboxes(string prefix, Word anchor, string[] labels, List<Word> words, List<UglyToad.PdfPig.Graphics.PdfPath> paths, HashSet<(double, double)> tickPositions, Dictionary<string, string> checkboxes)
        {
            var anchorX = anchor.BoundingBox.Left;
            var anchorY = anchor.BoundingBox.Bottom;

            foreach (var label in labels)
            {
                var key = $"{prefix}::{label}";
                if (checkboxes.ContainsKey(key)) continue;

                var bounds = FindLabelBoundsNearestTo(words, label, anchorX, anchorY, maxDistance: 250, belowAnchorOnly: true);
                if (bounds == null) continue;
                checkboxes[key] = CheckTickNearBounds(bounds.Value, tickPositions);
            }
        }

        private static string CheckTickNearBounds(PdfRectangle labelBounds, HashSet<(double, double)> tickPositions)
        {
            var searchLeft   = labelBounds.Left  - 10;
            var searchRight  = labelBounds.Right + 30;
            var searchBottom = labelBounds.Bottom - 5;
            var searchTop    = labelBounds.Top    + 5;
            return tickPositions.Any(t =>
                t.Item1 >= searchLeft  && t.Item1 <= searchRight &&
                t.Item2 >= searchBottom && t.Item2 <= searchTop)
                ? "true" : "false";
        }

        private void ExtractIi6ScopedCheckboxes(List<Word> words, List<UglyToad.PdfPig.Graphics.PdfPath> paths, HashSet<(double, double)> tickPositions, Dictionary<string, string> checkboxes)
        {
            var labWord = words.FirstOrDefault(w => w.Text.Equals("Laboratory", StringComparison.OrdinalIgnoreCase));
            if (labWord == null) return;

            var anchorX = labWord.BoundingBox.Left;
            var anchorY = labWord.BoundingBox.Bottom;

            var rowTicks = tickPositions
                .Where(t => Math.Abs(t.Item2 - anchorY) <= 10 && t.Item1 > anchorX - 40)
                .OrderBy(t => t.Item1)
                .ToList();


            // Section-level Yes / No
            var sectionClaimedBoxes = new HashSet<PdfRectangle>();
            PdfRectangle? yesBounds = null;
            PdfRectangle? noBounds = null;
            foreach (var label in new[] { "Yes", "No" })
            {
                var bounds = words
                    .Where(w => w.Text.Equals(label, StringComparison.OrdinalIgnoreCase) &&
                                w.BoundingBox.Left > anchorX + 40 &&
                                Math.Abs(w.BoundingBox.Bottom - anchorY) <= 8)
                    .OrderBy(w => Math.Abs(w.BoundingBox.Left - anchorX))
                    .Select(w => (PdfRectangle?)w.BoundingBox)
                    .FirstOrDefault();

                bounds ??= FindLabelBoundsNearestTo(words, label, anchorX + 80, anchorY, maxDistance: 160, belowAnchorOnly: true);
                if (bounds == null) continue;

                if (label.Equals("Yes", StringComparison.OrdinalIgnoreCase)) yesBounds = bounds.Value;
                if (label.Equals("No", StringComparison.OrdinalIgnoreCase)) noBounds = bounds.Value;

                var state = DetectSquareCheckboxStateNearLabel(bounds.Value, paths, words, tickPositions, sectionClaimedBoxes);
                checkboxes[$"II.6::{label}"] = state;
            }

            if (checkboxes.TryGetValue("II.6::Yes", out var ii6YesState) &&
                checkboxes.TryGetValue("II.6::No", out var ii6NoState) &&
                ii6YesState == "true" && ii6NoState == "true")
            {
                if (yesBounds.HasValue && noBounds.HasValue)
                {
                    var yesTight = CheckTickNearBoundsTight(yesBounds.Value, tickPositions);
                    var noTight = CheckTickNearBoundsTight(noBounds.Value, tickPositions);
                    if (yesTight == "true" && noTight == "false")
                    {
                        checkboxes["II.6::No"] = "false";
                    }
                    else if (yesTight == "false" && noTight == "true")
                    {
                        checkboxes["II.6::Yes"] = "false";
                    }
                    else if (rowTicks.Count == 1)
                    {
                        checkboxes["II.6::Yes"] = "true";
                        checkboxes["II.6::No"] = "false";
                    }
                }
                else if (rowTicks.Count == 1)
                {
                    checkboxes["II.6::Yes"] = "true";
                    checkboxes["II.6::No"] = "false";
                }
            }

            // Find test-name dot-words below II.6 and above II.12.
            var ii12Anchor = words.FirstOrDefault(w => w.Text.StartsWith("II.12", StringComparison.OrdinalIgnoreCase));
            var lowerLimitY = ii12Anchor != null ? ii12Anchor.BoundingBox.Bottom + 3 : double.MinValue;

            var testDotWords = words
                .Where(w => w.Text.Length > 1 && w.Text[0] == '.' && char.IsUpper(w.Text[1]) &&
                            w.BoundingBox.Bottom < anchorY &&
                            w.BoundingBox.Bottom > lowerLimitY)
                .OrderByDescending(w => w.BoundingBox.Bottom)
                .ToList();

            // Each test row anchor: (y, testName). Populated either from dot-words or from the
            // "Test" column header line as a fallback for templates that omit the dot prefix.
            var testRows = new List<(double y, string name)>();

            foreach (var tw in testDotWords)
            {
                var sameLineForName = words
                    .Where(w => Math.Abs(w.BoundingBox.Bottom - tw.BoundingBox.Bottom) < 3)
                    .OrderBy(w => w.BoundingBox.Left)
                    .ToList();
                var dotIdxForName = sameLineForName.FindIndex(w => w.Text == tw.Text);
                var nameParts = new List<string>();
                if (dotIdxForName >= 0)
                {
                    for (int k = dotIdxForName; k < sameLineForName.Count; k++)
                    {
                        var txt = sameLineForName[k].Text;
                        if (txt.Length > 0 && (txt[0] == '.' || char.IsUpper(txt[0])))
                            nameParts.Add(txt);
                        else
                            break;
                    }
                }
                var nm = string.Join(" ", nameParts).Trim();
                if (!string.IsNullOrWhiteSpace(nm))
                    testRows.Add((tw.BoundingBox.Bottom, nm));
            }

            // Fallback: no dot-prefixed test rows — look for plain test names on the "Test" header line.
            // Layout: "Test" column header sits below II.6 anchor, with the test name to its right
            // on the same line (e.g. "Test  Anaplasma marginale").
            if (testRows.Count == 0)
            {
                var testHeader = words.FirstOrDefault(w =>
                    w.Text.Equals("Test", StringComparison.OrdinalIgnoreCase) &&
                    w.BoundingBox.Bottom < anchorY &&
                    w.BoundingBox.Bottom > lowerLimitY);

                if (testHeader != null)
                {
                    var nameWords = words
                        .Where(w => Math.Abs(w.BoundingBox.Bottom - testHeader.BoundingBox.Bottom) < 3 &&
                                    w.BoundingBox.Left > testHeader.BoundingBox.Right)
                        .OrderBy(w => w.BoundingBox.Left)
                        .ToList();

                    if (nameWords.Count > 0)
                    {
                        var nm = string.Join(" ", nameWords.Select(w => w.Text)).Trim();
                        if (!string.IsNullOrWhiteSpace(nm))
                            testRows.Add((testHeader.BoundingBox.Bottom, nm));
                    }
                }
            }

            if (testRows.Count == 0) return;

            var wordsBelow = words
                .Where(w => w.BoundingBox.Bottom < anchorY && w.BoundingBox.Bottom > lowerLimitY)
                .ToList();

            PdfRectangle? FindLabelInBand(string label, double bandTop, double bandBottom)
            {
                var parts = label.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                // The II.6 column labels (Random, Results, etc.) sit at the same Left as "II.6." (one column-stop
                // LEFT of the "Laboratory" word). Filtering by Left > anchorX + 15 (Laboratory's Left) would drop
                // those leftmost columns. Use anchorX - 20 instead to include them while still excluding noise
                // from the far left margin (II.5 column at x≈30).
                var lines = wordsBelow
                    .Where(w => w.BoundingBox.Bottom <= bandTop && w.BoundingBox.Bottom >= bandBottom && w.BoundingBox.Left > anchorX - 20)
                    .GroupBy(w => Math.Round(w.BoundingBox.Bottom, 0))
                    .Select(g => g.OrderBy(w => w.BoundingBox.Left).ToList())
                    .ToList();

                foreach (var line in lines)
                {
                    for (int i = 0; i <= line.Count - parts.Length; i++)
                    {
                        bool match = true;
                        for (int j = 0; j < parts.Length; j++)
                        {
                            if (!line[i + j].Text.Equals(parts[j], StringComparison.OrdinalIgnoreCase))
                            {
                                match = false;
                                break;
                            }
                        }

                        if (match)
                        {
                            var s = line[i].BoundingBox;
                            var e = line[i + parts.Length - 1].BoundingBox;
                            return new PdfRectangle(s.Left, s.Bottom, e.Right, e.Top);
                        }
                    }
                }

                return null;
            }

            PdfRectangle? FindStandaloneSatInBand(double bandTop, double bandBottom)
            {
                var lines = wordsBelow
                    .Where(w => w.BoundingBox.Bottom <= bandTop && w.BoundingBox.Bottom >= bandBottom && w.BoundingBox.Left > anchorX - 20)
                    .GroupBy(w => Math.Round(w.BoundingBox.Bottom, 0))
                    .Select(g => g.OrderBy(w => w.BoundingBox.Left).ToList())
                    .ToList();

                foreach (var line in lines)
                {
                    for (int i = 0; i < line.Count; i++)
                    {
                        if (!line[i].Text.Equals("Satisfactory", StringComparison.OrdinalIgnoreCase)) continue;
                        bool precededByNot = i > 0 && line[i - 1].Text.Equals("Not", StringComparison.OrdinalIgnoreCase);
                        if (!precededByNot) return line[i].BoundingBox;
                    }
                }

                return null;
            }

            for (int i = 0; i < testRows.Count; i++)
            {
                var bandTop = testRows[i].y + 2;
                var bandBottom = (i + 1 < testRows.Count)
                    ? testRows[i + 1].y + 2
                    : lowerLimitY;

                var testName = testRows[i].name;
                if (string.IsNullOrWhiteSpace(testName)) continue;

                var prefix = $"II.6::row{i}";
                checkboxes[$"{prefix}::TestName"] = testName;

                var rowStates = new Dictionary<string, string>
                {
                    ["Random"] = "false",
                    ["Suspicion"] = "false",
                    ["Emergency measures"] = "false",
                    ["Results"] = "false",
                    ["Pending"] = "false",
                    ["Satisfactory"] = "false",
                    ["Not Satisfactory"] = "false"
                };

                var randomBounds = FindLabelInBand("Random", bandTop, bandBottom);
                var suspicionBounds = FindLabelInBand("Suspicion", bandTop, bandBottom);
                // "Emergency measures" (legacy) or "Intensified controls" (current EU CHED) — same column slot.
                var emergencyBounds = FindLabelInBand("Emergency measures", bandTop, bandBottom)
                                      ?? FindLabelInBand("Intensified controls", bandTop, bandBottom)
                                      ?? FindLabelInBand("Intensified Controls", bandTop, bandBottom);
                var resultsBounds = FindLabelInBand("Results", bandTop, bandBottom);
                var pendingBounds = FindLabelInBand("Pending", bandTop, bandBottom);
                var satBounds = FindStandaloneSatInBand(bandTop, bandBottom);
                var notSatBounds = FindLabelInBand("Not Satisfactory", bandTop, bandBottom);

                // 1) Geometric checkbox detection from square boxes near labels.
                var rowClaimedBoxes = new HashSet<PdfRectangle>();

                if (randomBounds.HasValue)
                    rowStates["Random"] = DetectSquareCheckboxStateNearLabel(randomBounds.Value, paths, words, tickPositions, rowClaimedBoxes);
                if (suspicionBounds.HasValue)
                    rowStates["Suspicion"] = DetectSquareCheckboxStateNearLabel(suspicionBounds.Value, paths, words, tickPositions, rowClaimedBoxes);
                if (emergencyBounds.HasValue)
                    rowStates["Emergency measures"] = DetectSquareCheckboxStateNearLabel(emergencyBounds.Value, paths, words, tickPositions, rowClaimedBoxes);
                if (resultsBounds.HasValue)
                    rowStates["Results"] = DetectSquareCheckboxStateNearLabel(resultsBounds.Value, paths, words, tickPositions, rowClaimedBoxes);
                if (pendingBounds.HasValue)
                    rowStates["Pending"] = DetectSquareCheckboxStateNearLabel(pendingBounds.Value, paths, words, tickPositions, rowClaimedBoxes);
                if (satBounds.HasValue)
                    rowStates["Satisfactory"] = DetectSquareCheckboxStateNearLabel(satBounds.Value, paths, words, tickPositions, rowClaimedBoxes);
                if (notSatBounds.HasValue)
                    rowStates["Not Satisfactory"] = DetectSquareCheckboxStateNearLabel(notSatBounds.Value, paths, words, tickPositions, rowClaimedBoxes);

                // 2) Fallback: per-line nearest-tick mapping when no box path is exposed for this template.
                var row1Columns = new List<(string key, PdfRectangle rect)>();
                if (randomBounds.HasValue) row1Columns.Add(("Random", randomBounds.Value));
                if (suspicionBounds.HasValue) row1Columns.Add(("Suspicion", suspicionBounds.Value));
                if (emergencyBounds.HasValue) row1Columns.Add(("Emergency measures", emergencyBounds.Value));

                var row2Columns = new List<(string key, PdfRectangle rect)>();
                if (resultsBounds.HasValue) row2Columns.Add(("Results", resultsBounds.Value));
                if (pendingBounds.HasValue) row2Columns.Add(("Pending", pendingBounds.Value));
                if (satBounds.HasValue) row2Columns.Add(("Satisfactory", satBounds.Value));
                if (notSatBounds.HasValue) row2Columns.Add(("Not Satisfactory", notSatBounds.Value));

                void MarkNearestForLine(List<(string key, PdfRectangle rect)> columns)
                {
                    if (columns.Count == 0) return;
                    var lineY = columns.Average(c => (c.rect.Bottom + c.rect.Top) / 2.0);
                    var lineTicks = tickPositions
                        .Where(t => t.Item2 >= lineY - 8 && t.Item2 <= lineY + 8 && t.Item1 > anchorX + 20)
                        .ToList();

                    var remaining = new List<(string key, PdfRectangle rect)>(columns);
                    foreach (var t in lineTicks)
                    {
                        if (remaining.Count == 0) break;
                        var nearest = remaining
                            .OrderBy(c => Math.Abs(t.Item1 - ((c.rect.Left + c.rect.Right) / 2.0)))
                            .First();
                        rowStates[nearest.key] = "true";
                        remaining.Remove(nearest);
                    }
                }

                bool row1AllFalse = rowStates["Random"] != "true" &&
                                    rowStates["Suspicion"] != "true" &&
                                    rowStates["Emergency measures"] != "true";
                if (row1AllFalse)
                {
                    MarkNearestForLine(row1Columns);
                }

                bool row2AllFalse = rowStates["Results"] != "true" &&
                                    rowStates["Pending"] != "true" &&
                                    rowStates["Satisfactory"] != "true" &&
                                    rowStates["Not Satisfactory"] != "true";
                if (row2AllFalse)
                {
                    MarkNearestForLine(row2Columns);
                }

                // 3) If both result states are on, keep the geometrically closer one.
                if (rowStates["Satisfactory"] == "true" && rowStates["Not Satisfactory"] == "true" &&
                    satBounds.HasValue && notSatBounds.HasValue)
                {
                    var row2LineY = row2Columns.Average(c => (c.rect.Bottom + c.rect.Top) / 2.0);
                    var row2Ticks = tickPositions
                        .Where(t => t.Item2 >= row2LineY - 8 && t.Item2 <= row2LineY + 8 && t.Item1 > anchorX + 20)
                        .ToList();

                    if (row2Ticks.Count > 0)
                    {
                        var satCenter = (satBounds.Value.Left + satBounds.Value.Right) / 2.0;
                        var notSatCenter = (notSatBounds.Value.Left + notSatBounds.Value.Right) / 2.0;
                        var satMin = row2Ticks.Min(t => Math.Abs(t.Item1 - satCenter));
                        var notSatMin = row2Ticks.Min(t => Math.Abs(t.Item1 - notSatCenter));

                        if (satMin <= notSatMin)
                            rowStates["Not Satisfactory"] = "false";
                        else
                            rowStates["Satisfactory"] = "false";
                    }
                }

                // 4) Sampling-row disambiguation (Random / Suspicion / Emergency measures).
                // These columns are mutually exclusive: a single tick lies in the gap between two adjacent
                // labels, and the existing per-label detection can attribute it to BOTH the label on its
                // left (via tight-fallback) and the label on its right (via geometric search). Resolve by
                // the template's convention: the checkbox sits to the RIGHT of its label, so the tick
                // belongs to the column whose Right edge is the smallest positive distance to the LEFT of
                // the tick's X. Only the loser is demoted — nothing is promoted.
                var samplingColumns = new List<(string key, PdfRectangle rect)>();
                if (randomBounds.HasValue) samplingColumns.Add(("Random", randomBounds.Value));
                if (suspicionBounds.HasValue) samplingColumns.Add(("Suspicion", suspicionBounds.Value));
                if (emergencyBounds.HasValue) samplingColumns.Add(("Emergency measures", emergencyBounds.Value));

                var trueSamplingColumns = samplingColumns.Where(c => rowStates[c.key] == "true").ToList();
                if (trueSamplingColumns.Count > 1)
                {
                    var samplingLineY = samplingColumns.Average(c => (c.rect.Bottom + c.rect.Top) / 2.0);
                    var samplingTicks = tickPositions
                        .Where(t => Math.Abs(t.Item2 - samplingLineY) <= 6)
                        .OrderBy(t => t.Item1)
                        .ToList();

                    foreach (var t in samplingTicks)
                    {
                        (string key, PdfRectangle rect)? winner = null;
                        double bestGap = double.MaxValue;
                        foreach (var c in trueSamplingColumns)
                        {
                            var gap = t.Item1 - c.rect.Right;
                            if (gap < 0 || gap > 20) continue;
                            if (gap < bestGap) { bestGap = gap; winner = c; }
                        }

                        if (winner.HasValue)
                        {
                            foreach (var c in trueSamplingColumns)
                            {
                                if (!c.key.Equals(winner.Value.key, StringComparison.Ordinal))
                                    rowStates[c.key] = "false";
                            }
                        }
                    }
                }

                checkboxes[$"{prefix}::Random"] = rowStates["Random"];
                checkboxes[$"{prefix}::Suspicion"] = rowStates["Suspicion"];
                checkboxes[$"{prefix}::EmergencyMeasures"] = rowStates["Emergency measures"];
                checkboxes[$"{prefix}::Results"] = rowStates["Results"];
                checkboxes[$"{prefix}::Pending"] = rowStates["Pending"];
                checkboxes[$"{prefix}::Satisfactory"] = rowStates["Satisfactory"];
                checkboxes[$"{prefix}::NotSatisfactory"] = rowStates["Not Satisfactory"];
            }
        }

        private void ExtractIi16ScopedCheckboxes(List<Word> words, List<UglyToad.PdfPig.Graphics.PdfPath> paths, HashSet<(double, double)> tickPositions, Dictionary<string, string> checkboxes)
        {
            var claimedBoxes = new HashSet<PdfRectangle>();

            // Group words into lines by Y position
            var lineGroups = words
                .GroupBy(w => Math.Round(w.BoundingBox.Bottom, 0))
                .Select(g => g.OrderBy(w => w.BoundingBox.Left).ToList())
                .ToList();

            // II.16 option labels — checkbox is on the same line but in a column far to the right
            var ii16Labels = new[] { "Re-dispatching", "Destruction", "Transformation", "Re-entry" };

            foreach (var label in ii16Labels)
            {
                foreach (var line in lineGroups)
                {
                    var labelWord = line.FirstOrDefault(w => w.Text.Contains(label, StringComparison.OrdinalIgnoreCase));
                    if (labelWord == null) continue;

                    // Build a wide bounding box spanning from the label to the far-right checkbox column
                    // and vertically expanded to cover the checkbox which may sit slightly above the text baseline
                    var wideBounds = new PdfRectangle(
                        labelWord.BoundingBox.Left,
                        labelWord.BoundingBox.Bottom,
                        labelWord.BoundingBox.Right + 350,
                        labelWord.BoundingBox.Top + 20);

                    var state = DetectCheckboxState(wideBounds, paths, words, tickPositions, claimedBoxes);
                    checkboxes[$"II.16::{label}"] = state;
                    break;
                }
            }
        }

        private void ExtractI16ScopedCheckboxes(List<Word> words, List<UglyToad.PdfPig.Graphics.PdfPath> paths, HashSet<(double, double)> tickPositions, Dictionary<string, string> checkboxes)
        {
            // I.16 Transport conditions: Ambient / Chilled / Frozen.
            // Use section-anchored label matching and detect the square checkbox nearest each label.
            var i16Anchor = words.FirstOrDefault(w => w.Text.Equals("I.16", StringComparison.OrdinalIgnoreCase))
                         ?? words.FirstOrDefault(w => w.Text.StartsWith("I.16", StringComparison.OrdinalIgnoreCase));
            if (i16Anchor == null)
            {
                return;
            }

            var anchorX = i16Anchor.BoundingBox.Left;
            var anchorY = i16Anchor.BoundingBox.Bottom;
            var labels = new[] { "Ambient", "Chilled", "Frozen" };
            var claimedBoxes = new HashSet<PdfRectangle>();
            var matchedLabelBounds = new List<(string label, PdfRectangle bounds)>();

            string DetectI16LabelState(PdfRectangle labelBounds)
            {
                // I.16 checkboxes are immediately RIGHT of each label on this template.
                var searchRect = new PdfRectangle(
                    labelBounds.Right + 2,
                    labelBounds.Bottom - 8,
                    labelBounds.Right + 24,
                    labelBounds.Top + 8);

                var box = paths
                    .Where(p =>
                    {
                        if (p.GetBoundingRectangle() is not PdfRectangle b) return false;

                        foreach (var claimed in claimedBoxes)
                        {
                            if ((Math.Abs(b.Centroid.X - claimed.Centroid.X) < 6 && Math.Abs(b.Centroid.Y - claimed.Centroid.Y) < 6) ||
                                Intersects(b, claimed))
                                return false;
                        }

                        var c = b.Centroid;
                        if (c.X < searchRect.Left || c.X > searchRect.Right ||
                            c.Y < searchRect.Bottom || c.Y > searchRect.Top) return false;

                        // For I.16, boxes can be either solid rectangles or thin line segments forming a box
                        // Accept either: (1) solid box 6-20px, or (2) thin lines (0.5-2px width/height)
                        var ratio = b.Width / b.Height;
                        var isSolidBox = b.Width >= 6 && b.Width <= 20 &&
                                        b.Height >= 6 && b.Height <= 20 &&
                                        ratio >= 0.6 && ratio <= 1.6;
                        var isThinLine = (b.Width < 2 && b.Height >= 8 && b.Height <= 20) ||  // vertical line
                                        (b.Height < 2 && b.Width >= 8 && b.Width <= 20);      // horizontal line
                        return isSolidBox || isThinLine;
                    })
                    .OrderBy(p =>
                    {
                        var b = p.GetBoundingRectangle()!.Value;
                        var dx = b.Centroid.X - labelBounds.Right;
                        var dy = b.Centroid.Y - (labelBounds.Bottom + labelBounds.Top) / 2.0;
                        return dx * dx + dy * dy;
                    })
                    .FirstOrDefault();

                if (box == null)
                {
                    return "false";
                }

                var boxBounds = box.GetBoundingRectangle()!.Value;
                claimedBoxes.Add(boxBounds);

                var checkedByTick = tickPositions.Any(t =>
                    Math.Abs(t.Item1 - boxBounds.Left) <= 1.5 &&
                    Math.Abs(t.Item2 - boxBounds.Bottom) <= 1.5);
                return checkedByTick ? "true" : "false";
            }

            foreach (var label in labels)
            {
                // maxDistance must cover the rightmost column (Frozen) on a full-width row; the
                // I.16 anchor sits in the left margin while "Frozen" sits near the right edge ~380px away.
                var bounds = FindLabelBoundsNearestTo(words, label, anchorX, anchorY, maxDistance: 500, belowAnchorOnly: true);
                var key = $"I16::{label}";

                if (bounds == null)
                {
                    checkboxes[key] = "false";
                    continue;
                }

                matchedLabelBounds.Add((label, bounds.Value));
                var state = DetectI16LabelState(bounds.Value);
                checkboxes[key] = state;
            }

            // In this template these three are mutually exclusive. If multiple are true,
            // keep the one whose label column is geometrically closest to a tick on the same line.
            var trueLabels = labels.Where(l => checkboxes.TryGetValue($"I16::{l}", out var v) && v == "true").ToList();
            if (trueLabels.Count > 1)
            {
                var labelMap = matchedLabelBounds.ToDictionary(x => x.label, x => x.bounds);
                var bestLabel = trueLabels[0];
                var bestScore = double.MaxValue;

                foreach (var label in trueLabels)
                {
                    if (!labelMap.TryGetValue(label, out var b)) continue;
                    var centerY = (b.Bottom + b.Top) / 2.0;
                    var centerX = b.Left - 12; // expected checkbox center is slightly left of label text
                    var lineTicks = tickPositions.Where(t => Math.Abs(t.Item2 - centerY) <= 8).ToList();
                    var score = lineTicks.Count > 0
                        ? lineTicks.Min(t => Math.Abs(t.Item1 - centerX))
                        : tickPositions.Min(t => Math.Abs(t.Item1 - centerX) + Math.Abs(t.Item2 - centerY));
                    if (score < bestScore)
                    {
                        bestScore = score;
                        bestLabel = label;
                    }
                }

                foreach (var label in trueLabels)
                {
                    checkboxes[$"I16::{label}"] = label == bestLabel ? "true" : "false";
                }
            }
        }

        private void ExtractIii5ScopedCheckboxes(List<Word> words, List<UglyToad.PdfPig.Graphics.PdfPath> paths, HashSet<(double, double)> tickPositions, Dictionary<string, string> checkboxes)
        {
            var claimedBoxes = new HashSet<PdfRectangle>();

            // Group words into lines by Y position
            var lineGroups = words
                .GroupBy(w => Math.Round(w.BoundingBox.Bottom, 0))
                .Select(g => g.OrderBy(w => w.BoundingBox.Left).ToList())
                .ToList();

            // Find "Yes" and "No" on the line that contains "Compliance of the consignment"
            foreach (var line in lineGroups)
            {
                var lineText = string.Join(" ", line.Select(w => w.Text));

                if (lineText.Contains("Compliance", StringComparison.OrdinalIgnoreCase) &&
                    lineText.Contains("consignment", StringComparison.OrdinalIgnoreCase))
                {

                    foreach (var target in new[] { "Yes", "No" })
                    {
                        var labelWord = line.FirstOrDefault(w => w.Text.Equals(target, StringComparison.OrdinalIgnoreCase));
                        if (labelWord == null) continue;

                        var state = DetectCheckboxState(labelWord.BoundingBox, paths, words, tickPositions, claimedBoxes);

                        checkboxes[$"III.5::{target}"] = state;
                    }
                }

                if (lineText.Contains("Arrival", StringComparison.OrdinalIgnoreCase) &&
                    lineText.Contains("consignment", StringComparison.OrdinalIgnoreCase))
                {

                    foreach (var target in new[] { "Yes", "No" })
                    {
                        var labelWord = line.FirstOrDefault(w => w.Text.Equals(target, StringComparison.OrdinalIgnoreCase));
                        if (labelWord == null) continue;

                        var state = DetectCheckboxState(labelWord.BoundingBox, paths, words, tickPositions, claimedBoxes);

                        checkboxes[$"III.5::Arrival::{target}"] = state;
                    }
                }
            }
        }

        private static string DetectCheckboxState(PdfRectangle labelBounds, List<UglyToad.PdfPig.Graphics.PdfPath> paths, List<Word> words, HashSet<(double, double)> tickPositions, HashSet<PdfRectangle>? claimedBoxes = null)
        {
            var searchRect = new PdfRectangle(
                labelBounds.Left - 50,
                labelBounds.Bottom - 15,
                labelBounds.Right + 300,
                labelBounds.Top + 15);

            // Find the checkbox box path near this label
            var box = paths
                .Where(p =>
                {
                    if (p.GetBoundingRectangle() is not PdfRectangle b) return false;
                    
                    if (claimedBoxes != null)
                    {
                        foreach (var claimed in claimedBoxes)
                        {
                            if (Math.Abs(b.Centroid.X - claimed.Centroid.X) < 2 && Math.Abs(b.Centroid.Y - claimed.Centroid.Y) < 2)
                                return false;
                        }
                    }

                    var c = b.Centroid;
                    if (c.X < searchRect.Left || c.X > searchRect.Right ||
                        c.Y < searchRect.Bottom || c.Y > searchRect.Top) return false;
                    var ratio = b.Width / b.Height;
                    return b.Width >= 6 && b.Width <= 20 &&
                           b.Height >= 6 && b.Height <= 20 &&
                           ratio >= 0.6 && ratio <= 1.6;
                })
                .OrderBy(p =>
                {
                    var b = p.GetBoundingRectangle()!.Value;
                    var dx = b.Centroid.X - (labelBounds.Left + labelBounds.Right) / 2.0;
                    var dy = b.Centroid.Y - (labelBounds.Bottom + labelBounds.Top) / 2.0;
                    return dx * dx + dy * dy;
                })
                .FirstOrDefault();

            if (box == null) 
            {
                return "false";
            }

            var boxBounds = box.GetBoundingRectangle()!.Value;
            if (claimedBoxes != null)
            {
                claimedBoxes.Add(boxBounds);
            }

            // Primary: check if a tick-mark XObject was placed at this box's origin
            var txRounded = Math.Round(boxBounds.Left, 1);
            var tyRounded = Math.Round(boxBounds.Bottom, 1);
            if (tickPositions.Contains((txRounded, tyRounded)))
                return "true";

            // Fallback 1: look for a mark text character (X, checkmark) inside the box
            var boxExpanded = new PdfRectangle(
                boxBounds.Left - 2, boxBounds.Bottom - 2,
                boxBounds.Right + 2, boxBounds.Top + 2);
            var markAsText = words.Any(w =>
            {
                var t = w.Text.Trim();
                return (t == "X" || t == "x" || t == "✓" || t == "✔") &&
                       Intersects(boxExpanded, w.BoundingBox);
            });
            if (markAsText) return "true";

            // Fallback 2: look for vector paths (marks) inside the box
            var marksInside = paths.Any(p =>
            {
                if (p.GetBoundingRectangle() is not PdfRectangle pb) return false;
                // A mark is usually smaller than the box and inside it
                return pb.Width < boxBounds.Width && pb.Height < boxBounds.Height &&
                       Intersects(boxBounds, pb);
            });

            return marksInside ? "true" : "false";
        }

        private static string DetectSquareCheckboxStateNearLabel(PdfRectangle labelBounds, List<UglyToad.PdfPig.Graphics.PdfPath> paths, List<Word> words, HashSet<(double, double)> tickPositions, HashSet<PdfRectangle>? claimedBoxes = null)
        {
            // For Yes/No labels, the square checkbox is typically just to the left of the label.
            var searchRect = new PdfRectangle(
                labelBounds.Left - 40,
                labelBounds.Bottom - 8,
                labelBounds.Left + 10,
                labelBounds.Top + 8);

            var box = paths
                .Where(p =>
                {
                    if (p.GetBoundingRectangle() is not PdfRectangle b) return false;

                    if (claimedBoxes != null)
                    {
                        foreach (var claimed in claimedBoxes)
                        {
                            if (Math.Abs(b.Centroid.X - claimed.Centroid.X) < 2 && Math.Abs(b.Centroid.Y - claimed.Centroid.Y) < 2)
                                return false;
                        }
                    }

                    var c = b.Centroid;
                    if (c.X < searchRect.Left || c.X > searchRect.Right ||
                        c.Y < searchRect.Bottom || c.Y > searchRect.Top) return false;

                    var ratio = b.Width / b.Height;
                    return b.Width >= 6 && b.Width <= 20 &&
                           b.Height >= 6 && b.Height <= 20 &&
                           ratio >= 0.6 && ratio <= 1.6;
                })
                .OrderBy(p =>
                {
                    var b = p.GetBoundingRectangle()!.Value;
                    var dx = b.Centroid.X - labelBounds.Left;
                    var dy = b.Centroid.Y - (labelBounds.Bottom + labelBounds.Top) / 2.0;
                    return dx * dx + dy * dy;
                })
                .FirstOrDefault();

            if (box == null)
            {
                // Some templates place tick XObjects but do not expose a clean square vector path.
                // In that case, use a tighter tick-position window to avoid Yes/No cross-attribution.
                return CheckTickNearBoundsTight(labelBounds, tickPositions);
            }

            var boxBounds = box.GetBoundingRectangle()!.Value;
            if (claimedBoxes != null)
            {
                claimedBoxes.Add(boxBounds);
            }

            var checkedByTick = tickPositions.Any(t =>
                Math.Abs(t.Item1 - boxBounds.Left) <= 1.5 &&
                Math.Abs(t.Item2 - boxBounds.Bottom) <= 1.5);
            if (checkedByTick)
                return "true";

            var boxExpanded = new PdfRectangle(
                boxBounds.Left - 2, boxBounds.Bottom - 2,
                boxBounds.Right + 2, boxBounds.Top + 2);
            var markAsText = words.Any(w =>
            {
                var t = w.Text.Trim();
                return (t == "X" || t == "x" || t == "✓" || t == "✔") &&
                       Intersects(boxExpanded, w.BoundingBox);
            });
            if (markAsText) return "true";

            var marksInside = paths.Any(p =>
            {
                if (p.GetBoundingRectangle() is not PdfRectangle pb) return false;
                return pb.Width < boxBounds.Width && pb.Height < boxBounds.Height &&
                       Intersects(boxBounds, pb);
            });

            return marksInside ? "true" : "false";
        }

        private static string CheckTickNearBoundsTight(PdfRectangle labelBounds, HashSet<(double, double)> tickPositions)
        {
            var searchLeft = labelBounds.Left - 6;
            var searchRight = labelBounds.Right + 12;
            var searchBottom = labelBounds.Bottom - 5;
            var searchTop = labelBounds.Top + 5;

            return tickPositions.Any(t =>
                t.Item1 >= searchLeft && t.Item1 <= searchRight &&
                t.Item2 >= searchBottom && t.Item2 <= searchTop)
                ? "true" : "false";
        }

        private PdfRectangle? FindLabelBounds(List<Word> words, string label)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                return null;
            }

            var parts = label.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
            {
                return words.FirstOrDefault(w => w.Text.Equals(label, StringComparison.OrdinalIgnoreCase))?.BoundingBox;
            }

            var lines = words
                .GroupBy(w => Math.Round(w.BoundingBox.Bottom, 0))
                .OrderByDescending(g => g.Key);

            foreach (var line in lines)
            {
                var lineWords = line.OrderBy(w => w.BoundingBox.Left).ToList();
                for (int i = 0; i <= lineWords.Count - parts.Length; i++)
                {
                    bool match = true;
                    for (int j = 0; j < parts.Length; j++)
                    {
                        if (!lineWords[i + j].Text.Equals(parts[j], StringComparison.OrdinalIgnoreCase))
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match)
                    {
                        var start = lineWords[i].BoundingBox;
                        var end = lineWords[i + parts.Length - 1].BoundingBox;
                        return new PdfRectangle(start.Left, start.Bottom, end.Right, end.Top);
                    }
                }
            }

            return null;
        }

        private PdfRectangle? FindLabelBoundsNearestTo(List<Word> words, string label, double anchorX, double anchorY, double maxDistance = double.MaxValue, bool belowAnchorOnly = false)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                return null;
            }

            var parts = label.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var candidates = new List<PdfRectangle>();
            var lines = words
                .GroupBy(w => Math.Round(w.BoundingBox.Bottom, 0))
                .OrderByDescending(g => g.Key);

            foreach (var line in lines)
            {
                var lineWords = line.OrderBy(w => w.BoundingBox.Left).ToList();
                for (int i = 0; i <= lineWords.Count - parts.Length; i++)
                {
                    bool match = true;
                    for (int j = 0; j < parts.Length; j++)
                    {
                        if (!lineWords[i + j].Text.Equals(parts[j], StringComparison.OrdinalIgnoreCase))
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match)
                    {
                        var start = lineWords[i].BoundingBox;
                        var end = lineWords[i + parts.Length - 1].BoundingBox;
                        candidates.Add(new PdfRectangle(start.Left, start.Bottom, end.Right, end.Top));
                    }
                }
            }

            if (!candidates.Any())
            {
                return null;
            }

            var rightSideCandidates = candidates
                .Where(r => ((r.Left + r.Right) / 2.0) >= (anchorX - 5))
                .ToList();
            var pool = rightSideCandidates.Any() ? rightSideCandidates : candidates;

            if (belowAnchorOnly)
            {
                // Must not be significantly above the anchor Y
                pool = pool.Where(r => ((r.Bottom + r.Top) / 2.0) <= anchorY + 5).ToList();
            }

            var closestMatch = pool
                .Select(r =>
                {
                    var cx = (r.Left + r.Right) / 2.0;
                    var cy = (r.Bottom + r.Top) / 2.0;
                    var dx = cx - anchorX;
                    var dy = cy - anchorY;
                    var dist = Math.Sqrt((dx * dx) + (dy * dy));
                    return new { Rect = r, Dist = dist };
                })
                .Where(x => x.Dist <= maxDistance)
                .OrderBy(x => x.Dist)
                .FirstOrDefault();

            return closestMatch?.Rect;
        }

        private static bool Intersects(PdfRectangle a, PdfRectangle b)
        {
            return a.Left < b.Right &&
                   a.Right > b.Left &&
                   a.Bottom < b.Top &&
                   a.Top > b.Bottom;
        }
    }
}