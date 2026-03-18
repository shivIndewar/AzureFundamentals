using AzureBobProject.Models;

namespace AzureBobProject.Services
{
    public interface IBlobService
    {
        Task<List<string>> GetAllBlobs(string containerName);
        Task<List<BlobModel>> GetAllBlobsWithUri(string containerName);
        Task<string> GetBlob(string name, string containerName);
        Task<bool> CreateBlob(string name, IFormFile file, string containerName, BlobModel model);
        Task<bool> DeleteBolb(string name, string containerName);
    }
}
