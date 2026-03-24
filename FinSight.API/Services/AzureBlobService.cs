using Azure.Storage.Blobs;
using System.IO;
using System.Threading.Tasks;

namespace FinSight.API.Services
{
    public class AzureBlobService
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public AzureBlobService(string connectionString, string containerName = "documents")
        {
            _connectionString = connectionString;
            _containerName = containerName;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var client = new BlobContainerClient(_connectionString, _containerName);
            await client.CreateIfNotExistsAsync();
            var blob = client.GetBlobClient(fileName);
            await blob.UploadAsync(fileStream, overwrite: true);
            return blob.Uri.ToString();
        }
    }
}