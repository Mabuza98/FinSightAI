using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace FinSight.API.Services
{
    public class DocumentAnalysisService
    {
        private readonly DocumentAnalysisClient _client;

        public DocumentAnalysisService(string endpoint, string key)
        {
            _client = new DocumentAnalysisClient(new Uri(endpoint), new AzureKeyCredential(key));
        }

        public async Task<AnalyzeResult> AnalyzeDocumentAsync(Stream fileStream)
        {
            var operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-document", fileStream);
            return operation.Value;
        }
    }
}