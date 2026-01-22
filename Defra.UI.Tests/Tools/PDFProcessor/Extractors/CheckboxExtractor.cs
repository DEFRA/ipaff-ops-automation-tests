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
                // 1. Try to extract from AcroForms if available
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

            // Visual fallback using vector paths
            try
            {
                ExtractFromPaths(page, checkboxes);
            }
            catch
            {
                // Vector path analysis failed, return what we have
            }

            return checkboxes;
        }

        private void ExtractFromPaths(Page page, Dictionary<string, string> checkboxes)
        {
            var targetLabels = new[] { "Ambient", "Chilled", "Frozen", "Conforming", "Non-conforming" };
            var words = page.GetWords().ToList();
            var paths = page.ExperimentalAccess.Paths.ToList();

            foreach (var label in targetLabels)
            {
                var labelWord = words.FirstOrDefault(w => w.Text.Equals(label, StringComparison.OrdinalIgnoreCase));

                if (labelWord == null && label == "Non-conforming")
                {
                    labelWord = words.FirstOrDefault(w => w.Text.Contains("Non-conforming", StringComparison.OrdinalIgnoreCase));
                }

                if (labelWord != null)
                {
                    var searchRect = new PdfRectangle(
                        labelWord.BoundingBox.Left - 10,
                        labelWord.BoundingBox.Bottom - 10,
                        labelWord.BoundingBox.Right + 70,
                        labelWord.BoundingBox.Top + 10);

                    var candidateBoxes = paths.Where(p =>
                    {
                        if (p.GetBoundingRectangle() is not PdfRectangle bounds) return false;

                        var c = bounds.Centroid;
                        if (c.X < searchRect.Left || c.X > searchRect.Right || c.Y < searchRect.Bottom || c.Y > searchRect.Top) return false;

                        var width = bounds.Width;
                        var height = bounds.Height;
                        return width > 4 && width < 40 && height > 4 && height < 40;
                    }).ToList();

                    if (candidateBoxes.Any())
                    {
                        var box = candidateBoxes.OrderByDescending(b => b.GetBoundingRectangle().Value.Left).First();
                        var boxBounds = box.GetBoundingRectangle().Value;

                        var marks = paths.Where(p =>
                        {
                            if (p == box) return false;
                            if (p.GetBoundingRectangle() is not PdfRectangle bounds) return false;

                            return boxBounds.Left <= bounds.Left &&
                                   boxBounds.Right >= bounds.Right &&
                                   boxBounds.Bottom <= bounds.Bottom &&
                                   boxBounds.Top >= bounds.Top;
                        }).ToList();

                        checkboxes[label] = marks.Any() ? "true" : "false";
                    }
                    else
                    {
                        checkboxes[label] = "false";
                    }
                }
            }
        }
    }
}
