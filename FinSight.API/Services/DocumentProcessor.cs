using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinSightAI.API.Services
{
    public class DocumentProcessor
    {
        private readonly EmbeddingService _embeddingService;

        public DocumentProcessor(IConfiguration configuration)
        {
            // Load configuration values
            var endpoint = configuration["AzureOpenAI:Endpoint"];
            var apiKey = configuration["AzureOpenAI:Key"];
            var deploymentName = configuration["AzureOpenAI:EmbeddingDeployment"];

            // Validate
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new InvalidOperationException("AzureOpenAI:Endpoint is missing in configuration!");
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("AzureOpenAI:Key is missing in configuration!");
            if (string.IsNullOrWhiteSpace(deploymentName))
                throw new InvalidOperationException("AzureOpenAI:EmbeddingDeployment is missing in configuration!");

            // Initialize service safely
            _embeddingService = new EmbeddingService(endpoint, apiKey, deploymentName);
        }

        public async Task<List<(string Chunk, IReadOnlyList<float> Embedding)>> ProcessDocumentChunksAsync(List<string> documentChunks)
        {
            var results = new List<(string, IReadOnlyList<float>)>();

            foreach (var chunk in documentChunks)
            {
                var embedding = await _embeddingService.CreateEmbeddingAsync(chunk);
                results.Add((chunk, embedding));
            }

            return results;
        }
    }
}