
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBobProject.Services
{
    public class ContainerService : IContainerService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public ContainerService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;   
        }
        public async Task CreateContainer(string containerName)
        {
            BlobContainerClient blobContainer = _blobServiceClient.GetBlobContainerClient(containerName);
            await blobContainer.CreateIfNotExistsAsync(PublicAccessType.Blob);
        }

        public async Task DeleteContainer(string containerName)
        {
            BlobContainerClient blobContainer = _blobServiceClient.GetBlobContainerClient(containerName);
            await blobContainer.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllContainer()
        {
            List<string> continarlist = new List<string>();

            await foreach (BlobContainerItem item in _blobServiceClient.GetBlobContainersAsync())
            {
                continarlist.Add(item.Name); 
            }

            return continarlist;
        }

        public async Task<List<string>> GetAllContainerAndBlobs()
        {
            List<string> continarAndBlobList = new List<string>();
            continarAndBlobList.Add(" ** Storage Account Name ** "+ _blobServiceClient.AccountName);
            continarAndBlobList.Add("---------------------------------------------------");
            await foreach (BlobContainerItem item in _blobServiceClient.GetBlobContainersAsync())
            {
                continarAndBlobList.Add("** Conatainer  : " +item.Name);
                BlobContainerClient blobContainer = _blobServiceClient.GetBlobContainerClient(item.Name);

                
                await foreach (var blboItem in blobContainer.GetBlobsAsync())
                {
                    var blobClient = blobContainer.GetBlobClient(blboItem.Name);
                    var blobProperties = await blobClient.GetPropertiesAsync();
                    var blobItemToAdd = blboItem.Name;

                    if (blobProperties.Value.Metadata.ContainsKey("title"))
                    {
                        blobItemToAdd += "(" +blobProperties.Value.Metadata["title"]+")";
                    }

                    continarAndBlobList.Add("   * BlobItem  : " + blobItemToAdd);
                }
                continarAndBlobList.Add("---------------------------------------------------");
            }

            return continarAndBlobList;
        }
    }
}
