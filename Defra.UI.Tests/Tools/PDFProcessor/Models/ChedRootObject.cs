using System.Collections.Generic;
using Newtonsoft.Json;

namespace PdfExtraction.Models
{
    public class ChedRootObject : List<ChedPageObject> { }

    public class ChedPageObject
    {
        public int PageNumber { get; set; }
        public ChedSectionsMap Sections { get; set; }
    }
}