using UglyToad.PdfPig;
using UglyToad.PdfPig.AcroForms.Fields;

namespace Defra.UI.Tests.Tools.PDFProcessor.Extractors
{
    /// <summary>
    /// Extracts the AcroForm text field values from a Phytosanitary Certificate PDF, keyed by field name
    /// </summary>
    public class PhytoCertificateFieldExtractor
    {
        public Dictionary<string, string> ExtractTextFields(PdfDocument document)
        {
            var fields = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (!document.TryGetForm(out var form) || form?.Fields == null)
                return fields;

            foreach (var field in form.Fields)
            {
                if (field is not AcroTextField textField)
                    continue;

                var name = field.Information?.PartialName ?? field.Information?.MappingName;
                if (string.IsNullOrWhiteSpace(name))
                    continue;

                fields[name] = (textField.Value ?? string.Empty).Replace("\r\n", "\n").Replace('\r', '\n').Trim();
            }

            return fields;
        }
    }
}
