using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace Defra.UI.Tests.Tools.PDFProcessor.Extractors
{
    /// <summary>
    /// Extracts form field data using text analysis
    /// </summary>
    public class FormExtractor
    {
        public Dictionary<string, string> ExtractFormFields(Page page, PdfDocument document)
        {
            var formFields = new Dictionary<string, string>();

            try
            {
                if (document.TryGetForm(out var form) && form != null && form.Fields != null)
                {
                    foreach (var field in form.Fields)
                    {
                        var fieldName = field.Information?.MappingName ?? field.ToString();
                        var fieldTypeName = field.GetType().Name;

                        if (fieldTypeName.Contains("Checkbox", System.StringComparison.OrdinalIgnoreCase) ||
                            fieldTypeName.Contains("Button", System.StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        var fieldString = field.ToString();
                        if (!string.IsNullOrEmpty(fieldString) && fieldString != field.GetType().FullName)
                        {
                            formFields[fieldName] = fieldString;
                        }
                    }
                }
            }
            catch
            {
                // Form extraction failed, return empty dictionary
            }

            return formFields;
        }
    }
}
