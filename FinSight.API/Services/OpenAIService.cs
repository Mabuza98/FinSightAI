using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace FinSightAI.API.Services
{
    public class OpenAIService
    {
        private readonly AzureOpenAIClient _client;
        private readonly string _chatDeploymentName;
        private readonly string _embeddingDeploymentName;

        public OpenAIService(string endpoint, string apiKey, string chatDeployment, string embeddingDeployment)
        {
            if (string.IsNullOrWhiteSpace(endpoint) ||
                string.IsNullOrWhiteSpace(apiKey) ||
                string.IsNullOrWhiteSpace(chatDeployment) ||
                string.IsNullOrWhiteSpace(embeddingDeployment))
            {
                throw new InvalidOperationException("OpenAI config missing");
            }

            _client = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            _chatDeploymentName = chatDeployment;
            _embeddingDeploymentName = embeddingDeployment;
        }

        // =========================================
        // ✅ NORMAL CHAT (🔥 THIS FIXES YOUR ERROR)
        // =========================================
        public async Task<string> GetChatCompletionAsync(string prompt)
        {
            try
            {
                var chatClient = _client.GetChatClient(_chatDeploymentName);

                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage("You are a financial analyst AI."),
                    new UserChatMessage(prompt)
                };

                var response = await chatClient.CompleteChatAsync(messages);

                return response.Value.Content.FirstOrDefault()?.Text ?? "No response from AI.";
            }
            catch (Exception ex)
            {
                Console.WriteLine("🔥 OPENAI ERROR:");
                Console.WriteLine(ex.Message);

                return "AI failed: " + ex.Message;
            }
        }

        // =========================================
        // 🚀 STREAMING CHAT
        // =========================================
        public async IAsyncEnumerable<string> GetChatCompletionStreamAsync(string prompt)
        {
            var chatClient = _client.GetChatClient(_chatDeploymentName);

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage("You are a financial analyst AI."),
                new UserChatMessage(prompt)
            };

            var response = chatClient.CompleteChatStreamingAsync(messages);

            await foreach (var update in response)
            {
                foreach (var content in update.ContentUpdate)
                {
                    if (!string.IsNullOrEmpty(content.Text))
                        yield return content.Text;
                }
            }
        }

        // =========================================
        // ✅ EMBEDDINGS
        // =========================================
        public async Task<float[]> CreateEmbeddingAsync(string text)
        {
            try
            {
                var embeddingClient = _client.GetEmbeddingClient(_embeddingDeploymentName);

                var response = await embeddingClient.GenerateEmbeddingAsync(text);

                return response.Value.ToFloats().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine("🔥 EMBEDDING ERROR:");
                Console.WriteLine(ex.Message);

                return Array.Empty<float>();
            }
        }
    }
}