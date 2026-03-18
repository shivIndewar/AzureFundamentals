using AzureFunctionWeb.Models;
using AzureFunctionWev.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureFunctionWev.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;    
        private readonly BlobServiceClient _blobServiceClient;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, BlobServiceClient blobServiceClient)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory; 
            _blobServiceClient = blobServiceClient;
        }

        public IActionResult Index()
        {
            Salesrequest s =new Salesrequest();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Salesrequest salesrequest, IFormFile file)
        {
            // http://localhost:7298/api/OnSalesUploadWriteToQueue
            salesrequest.Id = Guid.NewGuid().ToString();
            using var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://shivazurefunc.azurewebsites.net/api/");

            using (var content = new StringContent(JsonConvert.SerializeObject(salesrequest), System.Text.Encoding.UTF8))
            {
                HttpResponseMessage response = await client.PostAsync("OnSalesUploadWriteToQueue", content);
                string returnValue = await response.Content.ReadAsStringAsync();
            }

            if (file != null)
            { 
                var fileNmae = salesrequest.Id + Path.GetExtension(file.FileName);
                BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient("functionsalesrep");
                BlobClient blobClient = blobContainerClient.GetBlobClient(fileNmae);

                var httpHeader = new BlobHttpHeaders()
                {
                    ContentType = file.ContentType,
                };

                await blobClient.UploadAsync(file.OpenReadStream(), httpHeader);
            }


            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
