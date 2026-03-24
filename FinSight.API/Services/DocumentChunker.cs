using System;
using System.Collections.Generic;

namespace FinSightAI.API.Services
{
    public class DocumentChunker
    {
        public List<string> ChunkDocument(string text, int chunkSize = 500, int overlap = 50)
        {
            var chunks = new List<string>();

            if (string.IsNullOrWhiteSpace(text))
                return chunks;

            int start = 0;

            while (start < text.Length)
            {
                int length = Math.Min(chunkSize, text.Length - start);
                string chunk = text.Substring(start, length);

                chunks.Add(chunk);

                start += chunkSize - overlap;
            }

            return chunks;
        }
    }
}