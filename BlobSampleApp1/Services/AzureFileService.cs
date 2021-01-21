using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using BlobSampleApp1.Interfaces;
using BlobSampleApp1.Models;
using BlobSampleApp1.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlobSampleApp1.Services
{
    public class AzureFileService : IAzureFileService
    {
        #region services
        private readonly IMockupServices _mockupService;
        private readonly BlobServiceClient blobServiceClient;
        #endregion

        #region Fields
        private readonly string connectionString;
        #endregion

        #region Constructor
        public AzureFileService(IConfiguration Configuration, IMockupServices mockupService)
        {
            connectionString = Configuration["storageconnectionstring"];
            _mockupService = mockupService;
            blobServiceClient = new BlobServiceClient(connectionString);
        }
        #endregion

        #region mockup methods
        public async Task UploadAsync()
        {
            // Create a temporary Lorem Ipsum file on disk that we can upload
            string path = _mockupService.CreateTempFile();

            // Get a reference to a container named "sample-container" and then create it
            BlobContainerClient container = new BlobContainerClient(connectionString, _mockupService.Randomize(AppConstant.SAMPLE_CONTAINER_NAME));
            await container.CreateAsync();
            try
            {
                // Get a reference to a blob
                BlobClient blob = container.GetBlobClient(_mockupService.Randomize(AppConstant.SAMPLE_FILE_NAME));

                // Open the file and upload its data
                using (FileStream file = File.OpenRead(path))
                {
                    await blob.UploadAsync(file);
                }

                // Verify we uploaded some content
                BlobProperties properties = await blob.GetPropertiesAsync();
            }
            finally
            {
                // delete file and container after compleating operation
                await container.DeleteAsync();
            }
        }

        #endregion

        #region file methods
        public async Task UploadFileAsync(UploadFileViewModel fileToUpload)
        {
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(fileToUpload.SelectedContainer);
            try
            {
                // Get a reference to a blob
                BlobClient blob = container.GetBlobClient(fileToUpload.InputFile.FileName);

                // Open the file and upload its data
                using (FileStream file = File.OpenRead(fileToUpload.FilePath))
                {
                    await blob.UploadAsync(file);
                }

                // Verify we uploaded some content
                BlobProperties properties = await blob.GetPropertiesAsync();
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        public async Task<List<SelectListItem>> BlobSelectListByContainerAsync(string containerName)
        {
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);
            try
            {
                // List all the blobs
                List<SelectListItem> blobs = new List<SelectListItem>();
                await foreach (BlobItem blob in container.GetBlobsAsync())
                {
                    blobs.Add(new SelectListItem(blob.Name, blob.Name));
                }
                return blobs;
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        public async Task<List<BlobItem>> BlobListByContainerAsync(string containerName)
        {
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);
            try
            {
                // List all the blobs
                List<BlobItem> blobs = new List<BlobItem>();
                await foreach (BlobItem blob in container.GetBlobsAsync())
                {
                    blobs.Add(blob);
                }
                return blobs;
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        public async Task<List<FileResponseViewModel>> BlobListByContainersAsync(List<string> containers)
        {
            if (containers.Count == 0)
            {
                var resultSegment = blobServiceClient.GetBlobContainersAsync().AsPages();
                await foreach (Azure.Page<BlobContainerItem> containerPage in resultSegment)
                {
                    foreach (BlobContainerItem containerItem in containerPage.Values)
                    {
                        containers.Add(containerItem.Name);
                    }

                }
            }
            // List all the blobs
            List<FileResponseViewModel> blobs = new List<FileResponseViewModel>();
            foreach (string containerName in containers )
            {
                BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);
                await foreach (BlobItem blob in container.GetBlobsAsync())
                {
                    blobs.Add(new FileResponseViewModel(blob, containerName));
                }
            }
            return blobs;
        }

        public async Task DeleteFile(string fileName, string containerName)
        {
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);
            try
            {
                await container.DeleteBlobAsync(fileName);
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }
        #endregion

    }
}
