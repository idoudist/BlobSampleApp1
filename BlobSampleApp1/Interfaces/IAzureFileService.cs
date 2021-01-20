using Azure.Storage.Blobs.Models;
using BlobSampleApp1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlobSampleApp1.Interfaces
{
    public interface IAzureFileService
    {
        Task UploadAsync();
        Task<BlobContainerInfo> CreateContainer(string containerName);
        Task<List<ContainerInfoViewModel>> ContainerList();
        Task<List<SelectListItem>> ContainerSelectList();
        Task UploadFileAsync(UploadFileViewModel fileToUpload);
    }
}
