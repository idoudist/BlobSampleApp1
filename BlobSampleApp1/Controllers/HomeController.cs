using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlobSampleApp1.Models;
using BlobSampleApp1.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BlobSampleApp1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAzureFileService _azureFileService;
        private readonly IContainerService _containerService;

        public HomeController(ILogger<HomeController> logger, IAzureFileService azureFileService, IContainerService containerService)
        {
            _logger = logger;
            _azureFileService = azureFileService;
            _containerService = containerService;
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

        #region files

        public async Task<IActionResult> FileDetails()
        {
            UploadFileViewModel file = new UploadFileViewModel();
            file.ContainerSelectList = await _containerService.ContainerSelectList();
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

        public async Task<IActionResult> FilesListWrapper()
        {
            FilesOverviewViewModel overviewModel = new FilesOverviewViewModel();
            overviewModel.Containers = await _containerService.ContainerSelectList();
            overviewModel.Files = await _azureFileService.BlobListByContainersAsync(overviewModel.Containers.Select(x => x.Value).ToList());
            return View(overviewModel);
        }


        [HttpPost]
        public async Task<IActionResult> FilesList(FilesFilterViewModel filter)
        {
            List<FileResponseViewModel> fileResponse = await _azureFileService.BlobListByContainersAsync(filter.Containers.ToList());
            return View(fileResponse);
        }

        #endregion


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
