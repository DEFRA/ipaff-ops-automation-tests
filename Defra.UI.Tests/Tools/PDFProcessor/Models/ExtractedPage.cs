namespace PdfExtraction.Models
{
    public class ExtractedPage
    {
        public int PageNumber { get; set; }
        public Dictionary<string, ChedSection> Sections { get; set; }
    }
}