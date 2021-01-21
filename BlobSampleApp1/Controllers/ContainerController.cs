using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlobSampleApp1.Interfaces;
using BlobSampleApp1.Models;
using BlobSampleApp1.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlobSampleApp1.Controllers
{
    public class ContainerController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContainerService _containerService;
        private readonly IMockupServices _mockupService;

        public ContainerController(ILogger<HomeController> logger, IContainerService containerService, IMockupServices mockupService)
        {
            _logger = logger;
            _containerService = containerService;
            _mockupService = mockupService;
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
            await _containerService.CloneContainer("fadi", _mockupService.Randomize(AppConstant.SAMPLE_CONTAINER_NAME));
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
