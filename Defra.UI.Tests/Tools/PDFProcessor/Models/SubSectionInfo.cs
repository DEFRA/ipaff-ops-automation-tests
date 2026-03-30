namespace PdfExtraction
{
    public partial class PdfToJsonConverter
    {
        private class SubSectionInfo
        {
            public string Header { get; set; }
            public string Content { get; set; }
            public double StartX { get; set; }
        }
    }
}
