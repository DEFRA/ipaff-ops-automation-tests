namespace Defra.UI.Tests.Tools.PDFProcessor.Models
{
    public partial class PdfToJsonConverter
    {
        private class WorkingSection
        {
            public string Header { get; set; } = "";
            public double StartX { get; set; }
            public List<string> Content { get; set; } = new List<string>();
        }
    }
}
