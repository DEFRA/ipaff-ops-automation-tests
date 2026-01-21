using UglyToad.PdfPig.Content;

namespace Defra.UI.Tests.Tools.PDFProcessor.Extractors
{
    public class TableExtractor
    {
        private const double ColumnThreshold = 10.0;
        private const double RowThreshold = 5.0;

        public List<Dictionary<string, string>> ExtractTables(Page page)
        {
            var tables = new List<Dictionary<string, string>>();

            try
            {
                var words = page.GetWords().OrderBy(w => w.BoundingBox.Bottom).ThenBy(w => w.BoundingBox.Left).ToList();

                if (!words.Any())
                    return tables;

                // Detect table regions by analyzing text density and alignment
                var tableRegions = DetectTableRegions(words);

                foreach (var region in tableRegions)
                {
                    var tableData = ExtractTableFromRegion(region);
                    if (tableData != null && tableData.Any())
                    {
                        tables.AddRange(tableData);
                    }
                }
            }
            catch
            {
                // Extraction failed, return empty list
            }

            return tables;
        }

        private List<List<Word>> DetectTableRegions(List<Word> words)
        {
            var regions = new List<List<Word>>();
            var rows = new List<List<Word>>();
            var currentRow = new List<Word>();
            double lastBottom = 0;

            foreach (var word in words)
            {
                if (currentRow.Any() && Math.Abs(word.BoundingBox.Bottom - lastBottom) > RowThreshold)
                {
                    if (currentRow.Count > 1)
                    {
                        rows.Add(currentRow.OrderBy(w => w.BoundingBox.Left).ToList());
                    }
                    currentRow = new List<Word>();
                }

                currentRow.Add(word);
                lastBottom = word.BoundingBox.Bottom;
            }

            if (currentRow.Count > 1)
            {
                rows.Add(currentRow.OrderBy(w => w.BoundingBox.Left).ToList());
            }

            var currentRegion = new List<Word>();
            int consecutiveTableRows = 0;

            for (int i = 0; i < rows.Count - 1; i++)
            {
                if (HasSimilarColumnStructure(rows[i], rows[i + 1]))
                {
                    if (consecutiveTableRows == 0)
                    {
                        currentRegion.AddRange(rows[i]);
                    }
                    currentRegion.AddRange(rows[i + 1]);
                    consecutiveTableRows++;
                }
                else
                {
                    if (consecutiveTableRows >= 2)
                    {
                        regions.Add(new List<Word>(currentRegion));
                    }
                    currentRegion.Clear();
                    consecutiveTableRows = 0;
                }
            }

            if (consecutiveTableRows >= 2)
            {
                regions.Add(currentRegion);
            }

            return regions;
        }

        private bool HasSimilarColumnStructure(List<Word> row1, List<Word> row2)
        {
            if (Math.Abs(row1.Count - row2.Count) > 1)
                return false;

            for (int i = 0; i < Math.Min(row1.Count, row2.Count); i++)
            {
                if (Math.Abs(row1[i].BoundingBox.Left - row2[i].BoundingBox.Left) > ColumnThreshold)
                    return false;
            }

            return true;
        }

        private List<Dictionary<string, string>> ExtractTableFromRegion(List<Word> region)
        {
            var tableData = new List<Dictionary<string, string>>();

            if (!region.Any())
                return tableData;

            var rows = new List<List<Word>>();
            var currentRow = new List<Word>();
            double lastBottom = region[0].BoundingBox.Bottom;

            foreach (var word in region.OrderBy(w => w.BoundingBox.Bottom).ThenBy(w => w.BoundingBox.Left))
            {
                if (currentRow.Any() && Math.Abs(word.BoundingBox.Bottom - lastBottom) > RowThreshold)
                {
                    rows.Add(currentRow.OrderBy(w => w.BoundingBox.Left).ToList());
                    currentRow = new List<Word>();
                }

                currentRow.Add(word);
                lastBottom = word.BoundingBox.Bottom;
            }

            if (currentRow.Any())
            {
                rows.Add(currentRow.OrderBy(w => w.BoundingBox.Left).ToList());
            }

            if (rows.Count < 2)
                return tableData;

            var headers = rows[0].Select(w => w.Text).ToList();

            // Remaining rows as data
            for (int i = 1; i < rows.Count; i++)
            {
                var rowDict = new Dictionary<string, string>();
                var rowWords = rows[i];

                for (int j = 0; j < Math.Min(headers.Count, rowWords.Count); j++)
                {
                    rowDict[headers[j]] = rowWords[j].Text;
                }

                tableData.Add(rowDict);
            }

            return tableData;
        }
    }
}
