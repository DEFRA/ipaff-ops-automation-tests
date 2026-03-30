using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;

namespace PdfExtraction.Extractors
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
            catch
            {
                // AcroForm extraction failed, continue to visual fallback
            }

#if DEBUG
            Console.WriteLine($"[CheckboxExtractor] Page {page.Number}: AcroForm keys={checkboxes.Count}");
#endif

            try
            {
                ExtractFromPaths(page, checkboxes);
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"[CheckboxExtractor] Page {page.Number}: Path extraction failed: {ex.Message}");
#endif
            }

#if DEBUG
            Console.WriteLine($"[CheckboxExtractor] Page {page.Number}: Total keys={checkboxes.Count}");
#endif

            return checkboxes;
        }

        private void ExtractFromPaths(Page page, Dictionary<string, string> checkboxes)
        {
            var targetLabels = new[]
            {
                "Ambient", "Chilled", "Frozen",
                "Conforming", "Non-conforming",
                "Satisfactory", "Not Satisfactory", "Not Done", "Satisfactory Following Official Intervention",
                "Yes", "No", "Seal Check Only", "Full Identity Check",
                "Random", "Suspicion", "Intensified Controls", "Pending",
                "Human consumption", "Human Consumption",
                "Local competent authority", "Second entry point", "Arrival of consignment"
            };
            var words = page.GetWords().ToList();
            var paths = page.ExperimentalAccess.Paths.ToList();

            foreach (var label in targetLabels)
            {
                var labelBounds = FindLabelBounds(words, label);

                if (labelBounds == null && label == "Non-conforming")
                {
                    var fuzzy = words.FirstOrDefault(w => w.Text.Contains("Non-conforming", StringComparison.OrdinalIgnoreCase));
                    if (fuzzy != null)
                    {
                        labelBounds = fuzzy.BoundingBox;
                    }
                }

                if (labelBounds == null)
                {
                    continue;
                }

                checkboxes[label] = DetectCheckboxState(labelBounds.Value, paths, words);
            }

            ExtractIi6ScopedCheckboxes(words, paths, checkboxes);
            ExtractIii5ScopedCheckboxes(words, paths, checkboxes);
        }

        private void ExtractIi6ScopedCheckboxes(List<Word> words, List<UglyToad.PdfPig.Graphics.PdfPath> paths, Dictionary<string, string> checkboxes)
        {
            var labWord = words.FirstOrDefault(w => w.Text.Equals("Laboratory", StringComparison.OrdinalIgnoreCase));
            if (labWord == null)
            {
                return;
            }

            var anchorX = labWord.BoundingBox.Left;
            var anchorY = labWord.BoundingBox.Bottom;
            var ii6Labels = new[]
            {
                "Yes", "No", "Satisfactory", "Not Satisfactory",
                "Random", "Suspicion", "Intensified Controls", "Pending"
            };

            foreach (var label in ii6Labels)
            {
                var bounds = FindLabelBoundsNearestTo(words, label, anchorX, anchorY);
                if (bounds == null)
                {
                    continue;
                }

                checkboxes[$"II.6::{label}"] = DetectCheckboxState(bounds.Value, paths, words);
            }
        }

        private void ExtractIii5ScopedCheckboxes(List<Word> words, List<UglyToad.PdfPig.Graphics.PdfPath> paths, Dictionary<string, string> checkboxes)
        {
            var iii5Labels = new[] { "Yes", "No" };

            var compliance = FindLabelBounds(words, "Compliance of the consignment");
            if (compliance != null)
            {
                var cx = compliance.Value.Left;
                var cy = compliance.Value.Bottom;
                foreach (var label in iii5Labels)
                {
                    var bounds = FindLabelBoundsNearestTo(words, label, cx, cy);
                    if (bounds == null)
                    {
                        continue;
                    }

                    checkboxes[$"III.5::{label}"] = DetectCheckboxState(bounds.Value, paths, words);
                }
            }

            var arrival = FindLabelBounds(words, "Arrival of consignment");
            if (arrival != null)
            {
                var ax = arrival.Value.Left;
                var ay = arrival.Value.Bottom;
                foreach (var label in iii5Labels)
                {
                    var bounds = FindLabelBoundsNearestTo(words, label, ax, ay);
                    if (bounds == null)
                    {
                        continue;
                    }

                    checkboxes[$"III.5::Arrival::{label}"] = DetectCheckboxState(bounds.Value, paths, words);
                }
            }
        }

        private static string DetectCheckboxState(PdfRectangle labelBounds, List<UglyToad.PdfPig.Graphics.PdfPath> paths, List<Word> words)
        {
            var searchRect = new PdfRectangle(
                labelBounds.Left - 25,
                labelBounds.Bottom - 5,
                labelBounds.Right + 25,
                labelBounds.Top + 5);

            var candidateBoxes = paths.Where(p =>
            {
                if (p.GetBoundingRectangle() is not PdfRectangle bounds) return false;

                var c = bounds.Centroid;
                if (c.X < searchRect.Left || c.X > searchRect.Right || c.Y < searchRect.Bottom || c.Y > searchRect.Top) return false;

                var width = bounds.Width;
                var height = bounds.Height;
                var ratio = width / height;
                return width >= 6 && width <= 20 &&
                       height >= 6 && height <= 20 &&
                       ratio >= 0.6 && ratio <= 1.6;
            }).ToList();

            if (!candidateBoxes.Any())
            {
                return "false";
            }

            var labelCenterX = (labelBounds.Left + labelBounds.Right) / 2.0;
            var labelCenterY = (labelBounds.Bottom + labelBounds.Top) / 2.0;
            var box = candidateBoxes
                .OrderBy(b =>
                {
                    var rect = b.GetBoundingRectangle();
                    if (rect == null) return double.MaxValue;
                    var bb = rect.Value;
                    var dx = bb.Centroid.X - labelCenterX;
                    var dy = bb.Centroid.Y - labelCenterY;
                    return (dx * dx) + (dy * dy);
                })
                .First();
            var boxBounds = box.GetBoundingRectangle()!.Value;

            var marks = paths.Where(p =>
            {
                if (p == box) return false;
                if (p.GetBoundingRectangle() is not PdfRectangle bounds) return false;

                var overlaps = Intersects(boxBounds, bounds);
                if (!overlaps) return false;

                return bounds.Width <= (boxBounds.Width * 1.3) &&
                       bounds.Height <= (boxBounds.Height * 1.3) &&
                       bounds.Width >= 1.5 &&
                       bounds.Height >= 1.5;
            }).ToList();

            var boxExpanded = new PdfRectangle(
                boxBounds.Left - 2,
                boxBounds.Bottom - 2,
                boxBounds.Right + 2,
                boxBounds.Top + 2);

            var markAsText = words.Any(w =>
            {
                var t = w.Text.Trim();
                if (t != "X" && t != "x" && t != "✓" && t != "✔")
                {
                    return false;
                }

                return Intersects(boxExpanded, w.BoundingBox);
            });

            return (marks.Any() || markAsText) ? "true" : "false";
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

        private PdfRectangle? FindLabelBoundsNearestTo(List<Word> words, string label, double anchorX, double anchorY)
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

            return pool
                .OrderBy(r =>
                {
                    var cx = (r.Left + r.Right) / 2.0;
                    var cy = (r.Bottom + r.Top) / 2.0;
                    var dx = cx - anchorX;
                    var dy = cy - anchorY;
                    return (dx * dx) + (dy * dy);
                })
                .First();
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