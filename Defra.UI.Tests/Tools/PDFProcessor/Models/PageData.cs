namespace Defra.UI.Tests.Tools.PDFProcessor.Models
{
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
