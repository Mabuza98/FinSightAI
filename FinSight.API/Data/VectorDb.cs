using System.Collections.Generic;
using FinSight.API.Models;

namespace FinSight.API.Data
{
    public static class VectorDb
    {
        // Stores all uploaded documents
        public static List<SearchDocumentModel> Documents { get; } = new List<SearchDocumentModel>();
    }
}