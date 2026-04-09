namespace Defra.UI.Tests.Tools.PDFProcessor.Models
{
    public class ChedRootObject : List<ChedPageObject> { }

    public class ChedPageObject
    {
        public int PageNumber { get; set; }
        public ChedSectionsMap Sections { get; set; }
    }
}