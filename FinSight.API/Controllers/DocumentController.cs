using FinSight.API.Dtos;
using FinSight.API.Models;
using FinSight.API.Services;
using FinSightAI.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using UglyToad.PdfPig;

namespace FinSight.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 🔐 REQUIRE LOGIN
    public class DocumentController : ControllerBase
    {
        private readonly AzureBlobService _blobService;
        private readonly AzureSearchService _searchService;
        private readonly OpenAIService _openAIService;
        private readonly ILogger<DocumentController> _logger;

        public DocumentController(
            AzureBlobService blobService,
            AzureSearchService searchService,
            OpenAIService openAIService,
            ILogger<DocumentController> logger)
        {
            _blobService = blobService;
            _searchService = searchService;
            _openAIService = openAIService;
            _logger = logger;
        }

        // =========================================
        // 🔥 IMPROVED CHUNKING WITH OVERLAP
        // =========================================
        private List<string> SplitIntoChunks(string text, int chunkSize = 1000, int overlap = 200)
        {
            var chunks = new List<string>();

            if (string.IsNullOrWhiteSpace(text))
                return chunks;

            int start = 0;

            while (start < text.Length)
            {
                int length = Math.Min(chunkSize, text.Length - start);
                var chunk = text.Substring(start, length);

                chunks.Add(chunk);
                start += (chunkSize - overlap);
            }

            return chunks;
        }

        // =========================================
        // 🔥 UPLOAD DOCUMENT
        // =========================================
        [HttpPost("upload-single")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadSingle(IFormFile file)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                var fileUrl = await _blobService.UploadFileAsync(
                    file.OpenReadStream(),
                    file.FileName
                );

                string extractedText = "";

                using (var stream = file.OpenReadStream())
                using (var pdf = PdfDocument.Open(stream))
                {
                    foreach (var page in pdf.GetPages())
                    {
                        if (!string.IsNullOrWhiteSpace(page.Text))
                            extractedText += page.Text + "\n\n";
                    }
                }

                if (string.IsNullOrWhiteSpace(extractedText))
                    extractedText = file.FileName;

                var chunks = SplitIntoChunks(extractedText);

                int successCount = 0;

                foreach (var chunk in chunks)
                {
                    var embedding = await _openAIService.CreateEmbeddingAsync(chunk);

                    if (embedding == null || embedding.Length == 0)
                        continue;

                    var document = new SearchDocumentModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        FileName = file.FileName,
                        Content = chunk,
                        ChunkId = Guid.NewGuid().ToString(),
                        Embedding = embedding,
                        UserId = userId
                    };

                    await _searchService.UploadDocumentAsync(document);
                    successCount++;
                }

                return Ok(new
                {
                    message = "File chunked + embedded successfully 🔥",
                    fileName = file.FileName,
                    url = fileUrl,
                    chunks = chunks.Count,
                    indexed = successCount
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Upload failed");

                return StatusCode(500, new
                {
                    message = "Upload failed",
                    error = ex.Message
                });
            }
        }

        // =========================================
        // 🔥 GET USER DOCUMENTS (OLD ENDPOINT)
        // =========================================
        [HttpGet]
        public async Task<IActionResult> GetDocuments()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var docs = await _searchService.GetAllDocumentsAsync(userId);

                var userDocs = docs
                    .Where(d => !string.IsNullOrEmpty(d.FileName))
                    .GroupBy(d => d.FileName)
                    .Select(g => new
                    {
                        fileName = g.Key
                    })
                    .ToList();

                return Ok(userDocs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to fetch documents",
                    error = ex.Message
                });
            }
        }

        // =========================================
        // 🔥 NEW: /api/Document/my-documents
        // =========================================
        [HttpGet("my-documents")]
        public async Task<IActionResult> GetMyDocuments()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var docs = await _searchService.GetAllDocumentsAsync(userId);

                var userDocs = docs
                    .Where(d => !string.IsNullOrEmpty(d.FileName))
                    .GroupBy(d => d.FileName)
                    .Select(g => new
                    {
                        fileName = g.Key
                    })
                    .ToList();

                return Ok(userDocs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to fetch documents",
                    error = ex.Message
                });
            }
        }
    }
}