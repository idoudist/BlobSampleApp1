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
        Task UploadFileAsync(UploadFileViewModel fileToUpload);
        Task<List<BlobItem>> BlobListByContainerAsync(string containerName);
        Task<List<SelectListItem>> BlobSelectListByContainerAsync(string containerName);
        Task<List<FileResponseViewModel>> BlobListByContainersAsync(List<string> containers);
        Task DeleteFile(string fileName, string containerName);
    }
}
