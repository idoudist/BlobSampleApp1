using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlobSampleApp1.Models;
using BlobSampleApp1.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace BlobSampleApp1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAzureFileService _azureFileService;

        public HomeController(ILogger<HomeController> logger, IAzureFileService azureFileService)
        {
            _logger = logger;
            _azureFileService = azureFileService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upload()
        {
            await _azureFileService.UploadAsync();
            return View();
        }

        #region container
        public IActionResult ContainerDetails()
        {
            return View();
        }

        public async Task<IActionResult> UpdateContainer(ContainerCreateViewModel container)
        {
            await _azureFileService.CreateContainer(container.Name);
            return View("Index");
        }

        public async Task<IActionResult> ContainersList()
        {
            List<ContainerInfoViewModel> containers = await _azureFileService.ContainerList();
            return View("ContainerList", containers);
        }
        #endregion

        #region files

        public async Task<IActionResult> FileDetails()
        {
            UploadFileViewModel file = new UploadFileViewModel();
            file.ContainerSelectList = await _azureFileService.ContainerSelectList();
            return View(file);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(UploadFileViewModel file)
        {
            if (file.InputFile.Length > 0)
            {
                var filePath = Path.GetTempFileName();

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.InputFile.CopyToAsync(stream);
                }
                file.FilePath = filePath;
                await _azureFileService.UploadFileAsync(file);
            }
            
            return View("Index");
        }

        #endregion


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
