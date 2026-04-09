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
                "Human consumption", "Human Consumption",
                "Local competent authority", "Second entry point", "Arrival of consignment", "I.25. For re-entry"
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

            // Scoped extraction for Sections II.3, II.4, II.5
            ExtractIi3ScopedCheckboxes(words, paths, tickPositions, checkboxes);
            ExtractIi4ScopedCheckboxes(words, paths, tickPositions, checkboxes);
            ExtractIi5ScopedCheckboxes(words, paths, tickPositions, checkboxes);

            ExtractIi6ScopedCheckboxes(words, paths, tickPositions, checkboxes);
            ExtractIi16ScopedCheckboxes(words, paths, tickPositions, checkboxes);
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

        private void ExtractSectionScopedCheckboxes(string prefix, Word anchor, string[] labels, List<Word> words, List<UglyToad.PdfPig.Graphics.PdfPath> paths, HashSet<(double, double)> tickPositions, Dictionary<string, string> checkboxes)
        {
            var anchorX = anchor.BoundingBox.Left;
            var anchorY = anchor.BoundingBox.Bottom;
            var claimedBoxes = new HashSet<PdfRectangle>();

            foreach (var label in labels)
            {
                var bounds = FindLabelBoundsNearestTo(words, label, anchorX, anchorY, maxDistance: 250, belowAnchorOnly: true);
                if (bounds == null) continue;
                checkboxes[$"{prefix}::{label}"] = DetectCheckboxState(bounds.Value, paths, words, tickPositions, claimedBoxes);
            }
        }

        private void ExtractIi6ScopedCheckboxes(List<Word> words, List<UglyToad.PdfPig.Graphics.PdfPath> paths, HashSet<(double, double)> tickPositions, Dictionary<string, string> checkboxes)
        {
            var labWord = words.FirstOrDefault(w => w.Text.Equals("Laboratory", StringComparison.OrdinalIgnoreCase));
            if (labWord == null) return;

            var anchorX = labWord.BoundingBox.Left;
            var anchorY = labWord.BoundingBox.Bottom;
            var ii6Labels = new[]
            {
                "Yes", "No", "Satisfactory", "Not Satisfactory",
                "Random", "Suspicion", "Intensified Controls", "Pending"
            };

            var claimedBoxes = new HashSet<PdfRectangle>();
            foreach (var label in ii6Labels)
            {
                var bounds = FindLabelBoundsNearestTo(words, label, anchorX, anchorY, maxDistance: 200, belowAnchorOnly: true);
                if (bounds == null) continue;
                checkboxes[$"II.6::{label}"] = DetectCheckboxState(bounds.Value, paths, words, tickPositions, claimedBoxes);
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

        private static string DetectCheckboxState(PdfRectangle labelBounds, List<UglyToad.PdfPig.Graphics.PdfPath> paths, List<Word> words, HashSet<(double, double)> tickPositions, HashSet<PdfRectangle> claimedBoxes = null)
        {
            var searchRect = new PdfRectangle(
                labelBounds.Left - 25,
                labelBounds.Bottom - 5,
                labelBounds.Right + 25,
                labelBounds.Top + 5);

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

            if (box == null) return "false";

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

            // Fallback: look for a mark text character (X, checkmark) inside the box
            var boxExpanded = new PdfRectangle(
                boxBounds.Left - 2, boxBounds.Bottom - 2,
                boxBounds.Right + 2, boxBounds.Top + 2);
            var markAsText = words.Any(w =>
            {
                var t = w.Text.Trim();
                return (t == "X" || t == "x" || t == "✓" || t == "✔") &&
                       Intersects(boxExpanded, w.BoundingBox);
            });

            return markAsText ? "true" : "false";
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