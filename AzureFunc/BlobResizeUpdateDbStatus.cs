using System.IO;
using System.Threading.Tasks;
using AzureFunc.Data;
using AzureFunc.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureFunc;

public class BlobResizeUpdateDbStatus
{
    private readonly ILogger<BlobResizeUpdateDbStatus> _logger;
    DBContext _dbContext;
    public BlobResizeUpdateDbStatus(ILogger<BlobResizeUpdateDbStatus> logger, DBContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [Function(nameof(BlobResizeUpdateDbStatus))]
    public async Task Run([BlobTrigger("functionsalesrep-final/{name}")] Byte[] myByte, string name)
    {
        var finleName = Path.GetFileNameWithoutExtension(name);
        Salesrequest salesrequest = _dbContext.salesrequest.FirstOrDefault(u => u.Id == finleName);
        if (salesrequest != null)
        {
            salesrequest.Status = "Image Processed";
            _dbContext.SaveChanges();
        }
        _logger.LogInformation("C# Blob update DB status has been completed");
    }
}