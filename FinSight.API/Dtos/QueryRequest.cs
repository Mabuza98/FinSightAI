using Microsoft.AspNetCore.Http;

namespace FinSight.API.Models
{
    public class QueryRequest
    {
        public string Question { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;  // make optional
    }
}