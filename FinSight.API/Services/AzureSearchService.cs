using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using FinSight.API.Models;
using FinSight.API.Dtos;
using System.Text.Json;

namespace FinSight.API.Services
{
    public class AzureSearchService
    {
        private readonly SearchClient _searchClient;
        private readonly string _apiKey;
        private readonly string _endpoint;
        private readonly string _indexName;

        public AzureSearchService(string endpoint, string apiKey, string indexName)
        {
            _endpoint = endpoint;
            _apiKey = apiKey;
            _indexName = indexName;

            _searchClient = new SearchClient(
                new Uri(endpoint),
                indexName,
                new AzureKeyCredential(apiKey));
        }

        // =========================================
        // ✅ UPLOAD DOCUMENT
        // =========================================
        public async Task UploadDocumentAsync(SearchDocumentModel doc)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));

            await _searchClient.UploadDocumentsAsync(new[] { doc });
        }

        // =========================================
        // 🔥 HYBRID SEARCH (USER + FILE FILTER)
        // =========================================
        public async Task<List<SearchDocumentModel>> HybridSearchAsync(
            string query,
            float[] queryEmbedding,
            string userId,
            string? fileName // 🔥 NEW PARAM
        )
        {
            var results = new List<SearchDocumentModel>();

            if (string.IsNullOrWhiteSpace(query) ||
                queryEmbedding == null ||
                queryEmbedding.Length == 0)
                return results;

            var options = new SearchOptions
            {
                Size = 5
            };

            // =========================================
            // 🔐 FILTER LOGIC
            // =========================================
            if (!string.IsNullOrEmpty(fileName))
            {
                options.Filter = $"userId eq '{userId}' and fileName eq '{fileName}'";
            }
            else
            {
                options.Filter = $"userId eq '{userId}'";
            }

            // =========================================
            // 🔍 VECTOR SEARCH
            // =========================================
            options.VectorSearch = new VectorSearchOptions
            {
                Queries =
                {
                    new VectorizedQuery(queryEmbedding)
                    {
                        KNearestNeighborsCount = 5,
                        Fields = { "embedding" }
                    }
                }
            };

            var response = await _searchClient.SearchAsync<SearchDocumentModel>(query, options);

            await foreach (var result in response.Value.GetResultsAsync())
            {
                if (result.Document != null)
                    results.Add(result.Document);
            }

            return results;
        }

        // =========================================
        // 📄 GET DOCUMENTS (SAFE)
        // =========================================
        public async Task<List<SearchDocumentDto>> GetAllDocumentsAsync(string userId)
        {
            var results = new List<SearchDocumentDto>();

            var options = new SearchOptions
            {
                Size = 100,
                Filter = $"userId eq '{userId}'"
            };

            options.Select.Add("fileName");
            options.Select.Add("content");

            var response = await _searchClient.SearchAsync<SearchDocumentModel>("*", options);

            var fileNames = new HashSet<string>();

            await foreach (var result in response.Value.GetResultsAsync())
            {
                var doc = result.Document;

                if (doc != null && !string.IsNullOrWhiteSpace(doc.FileName))
                {
                    if (!fileNames.Contains(doc.FileName))
                    {
                        fileNames.Add(doc.FileName);

                        results.Add(new SearchDocumentDto
                        {
                            FileName = doc.FileName,
                            Content = doc.Content
                        });
                    }
                }
            }

            return results;
        }
    }
}