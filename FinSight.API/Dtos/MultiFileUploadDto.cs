using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace FinSight.API.Dtos
{
    public class MultiFileUploadDto
    {
        public required List<IFormFile> Files { get; set; }
    }
}