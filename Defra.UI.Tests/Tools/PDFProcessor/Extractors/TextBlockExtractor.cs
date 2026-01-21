using UglyToad.PdfPig.Content;

namespace Defra.UI.Tests.Tools.PDFProcessor.Extractors
{
    public class TextBlockExtractor
    {
        public Dictionary<string, object> ExtractTextBlocks(Page page)
        {
            var textBlocks = new Dictionary<string, object>();

            try
            {
                var words = page.GetWords().OrderBy(w => w.BoundingBox.Bottom).ThenBy(w => w.BoundingBox.Left).ToList();

                if (!words.Any())
                    return textBlocks;

                var lines = GroupWordsIntoLines(words);

                // Extract section headers and content
                string currentSection = "";
                var sectionContent = new List<string>();

                foreach (var line in lines)
                {
                    var lineText = string.Join(" ", line.Select(w => w.Text)).Trim();

                    if (IsSectionHeader(lineText))
                    {
                        // Save previous section if exists
                        if (!string.IsNullOrEmpty(currentSection) && sectionContent.Any())
                        {
                            textBlocks[currentSection] = string.Join(" ", sectionContent);
                        }

                        currentSection = lineText;
                        sectionContent = new List<string>();
                    }
                    else if (!string.IsNullOrEmpty(currentSection))
                    {
                        sectionContent.Add(lineText);
                    }
                }

                if (!string.IsNullOrEmpty(currentSection) && sectionContent.Any())
                {
                    textBlocks[currentSection] = string.Join(" ", sectionContent);
                }
            }
            catch
            {
                // If extraction fails, return empty dictionary
            }

            return textBlocks;
        }

        private List<List<Word>> GroupWordsIntoLines(List<Word> words)
        {
            var lines = new List<List<Word>>();
            var currentLine = new List<Word>();
            double lastBottom = 0;
            double lineThreshold = 5;

            foreach (var word in words)
            {
                if (currentLine.Any() && Math.Abs(word.BoundingBox.Bottom - lastBottom) > lineThreshold)
                {
                    lines.Add(currentLine.OrderBy(w => w.BoundingBox.Left).ToList());
                    currentLine = new List<Word>();
                }

                currentLine.Add(word);
                lastBottom = word.BoundingBox.Bottom;
            }

            if (currentLine.Any())
            {
                lines.Add(currentLine.OrderBy(w => w.BoundingBox.Left).ToList());
            }

            return lines;
        }

        private bool IsSectionHeader(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            text = text.Trim();

            // Roman numeral patterns at the start
            return text.StartsWith("I.") ||
                   text.StartsWith("II.") ||
                   text.StartsWith("III.") ||
                   text.StartsWith("IV.") ||
                   text.StartsWith("PART I") ||
                   text.StartsWith("PART II") ||
                   text.StartsWith("PART III");
        }

        public Dictionary<string, string> ExtractLabelValuePairs(Page page)
        {
            var pairs = new Dictionary<string, string>();

            try
            {
                var words = page.GetWords().OrderBy(w => w.BoundingBox.Bottom).ThenBy(w => w.BoundingBox.Left).ToList();
                var lines = GroupWordsIntoLines(words);

                foreach (var line in lines)
                {
                    var lineText = string.Join(" ", line.Select(w => w.Text)).Trim();

                    if (lineText.Contains(":"))
                    {
                        var parts = lineText.Split(new[] { ':' }, 2);
                        if (parts.Length == 2)
                        {
                            var label = parts[0].Trim();
                            var value = parts[1].Trim();
                            pairs[label] = value;
                        }
                    }
                }
            }
            catch
            {
                // If extraction fails, return empty dictionary
            }

            return pairs;
        }
    }
}
