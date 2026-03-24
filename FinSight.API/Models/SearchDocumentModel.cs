using System.Text.Json.Serialization;

public class SearchDocumentModel
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    [JsonPropertyName("fileName")]
    public string FileName { get; set; } = string.Empty;

    [JsonPropertyName("embedding")]
    public float[]? Embedding { get; set; }

    // ✅ ADD THIS
    [JsonPropertyName("userId")]
    public string? UserId { get; set; }

    // ✅ OPTIONAL (since you added it in Azure)
    [JsonPropertyName("chunkId")]
    public string? ChunkId { get; set; }
}