using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBobProject.Models;
using System.Net.Http.Headers;

namespace AzureBobProject.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobContainerClient;
        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobContainerClient = blobServiceClient;
        }
        public async Task<bool> CreateBlob(string name, IFormFile file, string containerName, BlobModel blobModel)
        {
            BlobContainerClient blobContainerClient = _blobContainerClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(name);

            var httpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            IDictionary<string, string> metaData = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(blobModel.Title))
            {
                metaData.Add("title", blobModel.Title);
            }
            if (!string.IsNullOrEmpty(blobModel.Comment))
            {
                metaData.Add("comment", blobModel.Comment);
            }

            var result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders, metaData);

            if (result != null)
            {
                return true;
            }

            return false;

        }

        public async Task<bool> DeleteBolb(string name, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobContainerClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(name);
            return await blobClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllBlobs(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobContainerClient.GetBlobContainerClient(containerName);
            var allBlobs = blobContainerClient.GetBlobsAsync();
            List<string> blobs = new List<string>();

            await foreach (BlobItem item in allBlobs)
            {
                blobs.Add(item.Name);
            }

            return blobs;
        }

        public async Task<List<BlobModel>> GetAllBlobsWithUri(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobContainerClient.GetBlobContainerClient(containerName);
            var allBlobs = blobContainerClient.GetBlobsAsync();
            List<BlobModel> blobs = new List<BlobModel>();

            await foreach (BlobItem item in allBlobs)
            {
                var blobClient = blobContainerClient.GetBlobClient(item.Name);

                BlobModel blobModel = new BlobModel()
                {
                    Uri = blobClient.Uri.AbsoluteUri
                };

                var properties = await blobClient.GetPropertiesAsync();

                if (properties.Value.Metadata.ContainsKey("title"))
                {
                    blobModel.Title = properties.Value.Metadata["title"];
                }

                if (properties.Value.Metadata.ContainsKey("comment"))
                {
                    blobModel.Title = properties.Value.Metadata["comment"];
                }

                blobs.Add(blobModel);
            }

            return blobs;
        }

        public async Task<string> GetBlob(string name, string containerName)
        {

            BlobContainerClient blobContainerClient = _blobContainerClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(name);

            if (blobClient != null)
            {
                return blobClient.Uri.AbsoluteUri;
            }

            return string.Empty;
        }
    }
}
