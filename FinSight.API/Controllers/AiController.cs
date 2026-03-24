
using FinSight.API.Models;
using FinSight.API.Services;
using FinSightAI.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinSight.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 🔐 REQUIRE LOGIN
    public class AiController : ControllerBase
    {
        private readonly OpenAIService _openAIService;
        private readonly AzureSearchService _searchService;

        public AiController(OpenAIService openAIService, AzureSearchService searchService)
        {
            _openAIService = openAIService;
            _searchService = searchService;
        }

        // =========================================
        // 🔍 QUERY WITH REAL USER (JWT)
        // =========================================
        [HttpPost("query")]
        [Authorize]
        public async Task<IActionResult> Query([FromBody] QueryRequest request)
        {
            try
            {
                // 🔐 GET USER ID FROM JWT
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                if (request == null || string.IsNullOrWhiteSpace(request.Question))
                {
                    return BadRequest("Question is empty");
                }

                Console.WriteLine($"👤 USER: {userId}");
                Console.WriteLine($"🧠 QUESTION: {request.Question}");
                Console.WriteLine($"📄 FILE: {request.FileName}");

                // =========================================
                // 1. CREATE EMBEDDING
                // =========================================
                var embedding = await _openAIService.CreateEmbeddingAsync(request.Question);

                if (embedding == null || embedding.Length == 0)
                {
                    return Ok(new
                    {
                        answer = "Failed to generate embedding.",
                        sources = new List<string>()
                    });
                }

                // =========================================
                // 2. SEARCH USER DOCUMENTS ONLY (+ FILE FILTER)
                // =========================================
                var docs = await _searchService.HybridSearchAsync(
                    request.Question,
                    embedding,
                    userId,
                    request.FileName // 🔥 PASS FILE FILTER
                );

                if (docs == null || docs.Count == 0)
                {
                    return Ok(new
                    {
                        answer = "I don't know based on the provided documents.",
                        sources = new List<string>()
                    });
                }

                // =========================================
                // 3. CLEAN VALID DOCUMENTS
                // =========================================
                var validDocs = docs
                    .Where(d => !string.IsNullOrWhiteSpace(d.Content))
                    .ToList();

                if (validDocs.Count == 0)
                {
                    return Ok(new
                    {
                        answer = "No usable content found.",
                        sources = new List<string>()
                    });
                }

                // =========================================
                // 4. BUILD CONTEXT
                // =========================================
                var context = string.Join("\n\n---\n\n", validDocs.Select(d => d.Content));

                // =========================================
                // 5. PROMPT
                // =========================================
                var prompt = $@"
You are a financial AI assistant.

STRICT RULES:
- Answer ONLY using the provided context
- If answer is not found, say: 'I don't know based on the provided documents.'
- Be concise

CONTEXT:
{context}

QUESTION:
{request.Question}
";

                // =========================================
                // 6. ASK AI
                // =========================================
                var answer = await _openAIService.GetChatCompletionAsync(prompt);

                // =========================================
                // 7. RETURN
                // =========================================
                return Ok(new
                {
                    answer,
                    sources = validDocs.Select(d => d.FileName).Distinct()
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("🔥 QUERY ERROR:");
                Console.WriteLine(ex.Message);

                return StatusCode(500, new
                {
                    message = "Query failed",
                    error = ex.Message
                });
            }
        }
    }
}