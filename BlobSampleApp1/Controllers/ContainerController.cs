using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlobSampleApp1.Interfaces;
using BlobSampleApp1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlobSampleApp1.Controllers
{
    public class ContainerController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContainerService _containerService;

        public ContainerController(ILogger<HomeController> logger, IContainerService containerService)
        {
            _logger = logger;
            _containerService = containerService;
        }
        public async Task<IActionResult> Index()
        {
            List<ContainerInfoViewModel> containers = await _containerService.ContainerList();
            return View(containers);
        }

        public IActionResult Details()
        {
            return View();
        }

        public async Task<IActionResult> Update(ContainerCreateViewModel container)
        {
            await _containerService.CreateContainer(container.Name);
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _containerService.DeleteContainerAsync(id);
            List<ContainerInfoViewModel> containers = await _containerService.ContainerList();
            return View("List", containers);
        }
    }
}
