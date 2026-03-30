namespace Defra.UI.Tests.Tools.PDFProcessor.Models
{
    public class ExtractedPage
    {
        public int PageNumber { get; set; }

        // Sections are dynamic, but keys are now sanitized (e.g., "I1ConsignorExporter")
        // You can deserialize into this Dictionary, or map to a specific class if you flatten the structure.
        public Dictionary<string, ChedSection> Sections { get; set; }
    }
}