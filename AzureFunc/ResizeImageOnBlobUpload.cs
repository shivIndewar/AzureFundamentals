using System.IO;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace AzureFunc;

public class ResizeImageOnBlobUpload
{
    private readonly ILogger<ResizeImageOnBlobUpload> _logger;

    public ResizeImageOnBlobUpload(ILogger<ResizeImageOnBlobUpload> logger)
    {
        _logger = logger;
    }

    [Function(nameof(ResizeImageOnBlobUpload))]
    [BlobOutput("functionsalesrep-final/{name}", Connection = "AzureWebJobsStorage")]
    public async Task<byte[]> Run(
    [BlobTrigger("functionsalesrep/{name}", Connection = "AzureWebJobsStorage")] byte[] myBytes,
    string name)
    {
        using var memoryReader = new MemoryStream(myBytes);
        using var image = Image.Load(memoryReader);

        image.Mutate(s => s.Resize(100, 100));

        using var outputStream = new MemoryStream();
        image.SaveAsJpeg(outputStream);

        outputStream.Position = 0;

        _logger.LogInformation("Blob processed: {name}", name);

        return outputStream.ToArray();
    }

}