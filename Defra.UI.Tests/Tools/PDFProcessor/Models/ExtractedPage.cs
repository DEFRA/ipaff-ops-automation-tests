using Defra.UI.Tests.Tools.PDFProcessor.Models;

namespace PdfExtraction.Models
{
    public class ExtractedPage
    {
        public int PageNumber { get; set; }
        public Dictionary<string, ChedSection> Sections { get; set; }
    }
}