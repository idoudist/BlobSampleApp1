using Azure.Storage.Blobs.Models;
using BlobSampleApp1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlobSampleApp1.Interfaces
{
    public interface IContainerService
    {
        Task<BlobContainerInfo> CreateContainer(string containerName, PublicAccessType publicAccessType = PublicAccessType.None);
        Task<List<ContainerInfoViewModel>> ContainerList();
        Task<List<SelectListItem>> ContainerSelectList();
        Task DeleteContainerAsync(string containerName);
        Task<BlobContainerInfo> CloneContainer(string sourceContainerName, string targetContainerName);
        Task ChangeContainerPermission(string containerName, PublicAccessType publicAccessType = PublicAccessType.None);
    }
}
