using AzureBobProject.Models;
using AzureBobProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureBobProject.Controllers
{
    public class ContainerController : Controller
    {
        private readonly IContainerService _containerService;
        public ContainerController(IContainerService containerService)
        {
            _containerService = containerService;    
        }
        public async  Task<IActionResult> Index()
        {
            var containers = await _containerService.GetAllContainer();
            return View(containers);
        }

 
        public async Task<IActionResult> Create()
        {
            return View(new ContainerModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContainerModel container)
        {
            await _containerService.CreateContainer(container.Name);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string containerName="")
        {
            await _containerService.DeleteContainer(containerName);
            return RedirectToAction("Index");
        }

    }
}
