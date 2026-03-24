using Azure;
using Azure.AI.OpenAI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinSightAI.API.Services
{
    public class EmbeddingService
    {
        private readonly AzureOpenAIClient _client;
        private readonly string _deploymentName;

        public EmbeddingService(string endpoint, string apiKey, string deploymentName)
        {
            _client = new AzureOpenAIClient(
                new Uri(endpoint),
                new AzureKeyCredential(apiKey)
            );

            _deploymentName = deploymentName;
        }

        public async Task<IReadOnlyList<float>> CreateEmbeddingAsync(string text)
        {
            var embeddingClient = _client.GetEmbeddingClient(_deploymentName);

            var response = await embeddingClient.GenerateEmbeddingAsync(text);

            return response.Value.ToFloats().ToArray();
        }
    }
}