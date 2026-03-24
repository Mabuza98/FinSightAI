using FinSight.API.Services;
using FinSightAI.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InsightsController : ControllerBase
{
    private readonly AzureSearchService _searchService;
    private readonly OpenAIService _openAI;

    public InsightsController(
        AzureSearchService searchService,
        OpenAIService openAI)
    {
        _searchService = searchService;
        _openAI = openAI;
    }

    [HttpGet]
    public async Task<IActionResult> GetInsights([FromQuery] string query = "financial performance risks growth opportunities insights")
    {
        try
        {
            // 1️⃣ Get userId from JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User ID not found in token" });

            // 2️⃣ Generate embedding for the query
            var queryEmbedding = await _openAI.CreateEmbeddingAsync(query);
            if (queryEmbedding == null || queryEmbedding.Length == 0)
                return Ok(new[]
                {
                new { title = "Embedding Error", description = "Failed to generate embedding", type = "Other" }
            });

            // 3️⃣ Vector search with user filter
            var documents = await _searchService.HybridSearchAsync(
                query,
                queryEmbedding,
                userId,
                null // ✅ IMPORTANT: Insights uses ALL documents
            );

            if (documents == null || !documents.Any())
            {
                return Ok(new[]
                {
        new
        {
            title = "No Data",
            description = "No relevant documents found",
            type = "Other"
        }
    });
            }

            // 4️⃣ Build context
            var contextBuilder = new StringBuilder();
            foreach (var doc in documents.Take(5))
            {
                if (!string.IsNullOrWhiteSpace(doc.Content))
                {
                    contextBuilder.AppendLine($"[Source: {doc.FileName}]");
                    contextBuilder.AppendLine(doc.Content);
                    contextBuilder.AppendLine("\n---\n");
                }
            }

            // 5️⃣ Prompt AI for structured JSON insights
            var prompt = $@"
You are a financial analyst AI.
Analyze the documents below and return insights in STRICT JSON format.
Each item must contain:
- title
- description
- type (Risk, Growth, Opportunity, Other)

Return ONLY JSON like:
[
  {{
    ""title"": ""..."",
    ""description"": ""..."",
    ""type"": ""...""
  }}
]

DOCUMENTS:
{contextBuilder}
";

            var aiResponse = await _openAI.GetChatCompletionAsync(prompt);
            if (string.IsNullOrWhiteSpace(aiResponse))
                throw new Exception("Empty AI response");

            // 6️⃣ Clean and parse JSON
            var cleaned = aiResponse.Replace("```json", "").Replace("```", "").Trim();
            try
            {
                var parsed = JsonSerializer.Deserialize<object>(cleaned);
                return Ok(parsed);
            }
            catch
            {
                return Ok(new[]
                {
                new { title = "Parsing Error", description = cleaned, type = "Other" }
            });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return Ok(new[]
            {
            new { title = "System Error", description = "AI failed to process request", type = "Other" },
            new { title = "Debug Info", description = ex.Message, type = "Other" }
        });
        }
    }
}