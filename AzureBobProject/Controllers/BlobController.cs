using AzureBobProject.Models;
using AzureBobProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;

namespace AzureBobProject.Controllers
{
    public class BlobController : Controller
    {
        private readonly IBlobService _blobService;
        public BlobController(IBlobService blobService)
        {
            _blobService = blobService;  
        }

        [HttpGet]
        public async Task<IActionResult> Manage(string containerName)
        {
            var blobs = await _blobService.GetAllBlobs(containerName);
            return View(blobs);
        }


        public async Task<IActionResult> DeleteFile(string name, string containerName)
        {
            await _blobService.DeleteBolb(name,containerName);
            return RedirectToAction("Manage", new { containerName = containerName });
        }

        public async Task<IActionResult> AddFile(string containerName)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(string containerName, IFormFile file, BlobModel blobModel)
        {
            if (file == null || file.Length < 1) return View();
            //file name - xps_img2.png 
            //new name - xps_img2_GUIDHERE.png
            var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);
            var result = await _blobService.CreateBlob(fileName, file, containerName, blobModel);

            if (result)
                return RedirectToAction("Manage", new { containerName });


            return View();
        }


        public async Task<IActionResult> ViewFile(string name, string containerName)
        {
            return Redirect(await _blobService.GetBlob(name, containerName));
        }

    }
}
