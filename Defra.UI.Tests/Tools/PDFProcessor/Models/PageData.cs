namespace PdfExtraction.Models
{
    /// <summary>
    /// Represents data extracted from a single PDF page
    /// </summary>
    public class PageData
    {
        public int PageNumber { get; set; }
        public Dictionary<string, object> Sections { get; set; }

        public PageData()
        {
            Sections = new Dictionary<string, object>();
        }
    }
}
