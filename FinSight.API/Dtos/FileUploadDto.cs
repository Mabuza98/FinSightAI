using Microsoft.AspNetCore.Http;

namespace FinSight.API.Dtos
{
    public class FileUploadDto
    {
        public required IFormFile File { get; set; }
    }
}