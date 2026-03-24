using UglyToad.PdfPig;

namespace FinSight.API.Services
{
    public class DocumentTextExtractor
    {
        public string ExtractPdfText(string filePath)
        {
            var text = "";

            using (var document = PdfDocument.Open(filePath))
            {
                foreach (var page in document.GetPages())
                {
                    text += page.Text;
                }
            }

            return text;
        }
    }
}