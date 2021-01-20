using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using BlobSampleApp1.Interfaces;
using BlobSampleApp1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlobSampleApp1.Services
{
    public class ContainerService : IContainerService
    {
        #region services
        private readonly IMockupServices _mockupService;
        private readonly BlobServiceClient blobServiceClient;
        #endregion

        #region Fields
        private readonly string connectionString;
        #endregion

        #region Constructor
        public ContainerService(IConfiguration Configuration, IMockupServices mockupService)
        {
            connectionString = Configuration["storageconnectionstring"];
            _mockupService = mockupService;
            blobServiceClient = new BlobServiceClient(connectionString);
        }
        #endregion

        #region Methods

        public async Task<BlobContainerInfo> CreateContainer(string containerName)
        {
            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            BlobContainerInfo containerInfo = await container.CreateAsync();
            return containerInfo;
        }

        public async Task<List<ContainerInfoViewModel>> ContainerList()
        {
            try
            {
                List<ContainerInfoViewModel> containers = new List<ContainerInfoViewModel>();
                var resultSegment = blobServiceClient.GetBlobContainersAsync().AsPages();
                await foreach (Azure.Page<BlobContainerItem> containerPage in resultSegment)
                {
                    foreach (BlobContainerItem containerItem in containerPage.Values)
                    {
                        ContainerInfoViewModel newContainer = new ContainerInfoViewModel()
                        {
                            Name = containerItem.Name,
                            Properties = containerItem.Properties
                        };
                        containers.Add(newContainer);
                    }

                }
                return containers;
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        public async Task<List<SelectListItem>> ContainerSelectList()
        {
            try
            {
                List<SelectListItem> containers = new List<SelectListItem>();
                var resultSegment = blobServiceClient.GetBlobContainersAsync().AsPages();
                await foreach (Azure.Page<BlobContainerItem> containerPage in resultSegment)
                {
                    foreach (BlobContainerItem containerItem in containerPage.Values)
                    {
                        containers.Add(new SelectListItem(containerItem.Name, containerItem.Name));
                    }

                }
                return containers;
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        public async Task DeleteContainerAsync(string containerName)
        {
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);

            try
            {
                // Delete the specified container and handle the exception.
                await container.DeleteAsync();
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine("HTTP error code {0}: {1}",
                                    e.Status, e.ErrorCode);
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        public async Task<BlobContainerInfo> CloneContainer(string sourceContainerName , string targetContainerName)
        {
            try
            {
                // create target container
                BlobContainerClient targetContainerClient = new BlobContainerClient(connectionString, targetContainerName);
                BlobContainerInfo targetContainer = await targetContainerClient.CreateAsync();
                // get source container
                BlobContainerClient sourceContainerClient = blobServiceClient.GetBlobContainerClient(sourceContainerName);
                // blobs from source container
                await foreach (BlobItem blob in sourceContainerClient.GetBlobsAsync())
                {
                    // create a blob client for the source blob
                    BlobClient sourceBlob = sourceContainerClient.GetBlobClient(blob.Name);
                    // Ensure that the source blob exists.
                    if (await sourceBlob.ExistsAsync())
                    {
                        // Lease the source blob for the copy operation 
                        // to prevent another client from modifying it.
                        BlobLeaseClient lease = sourceBlob.GetBlobLeaseClient();

                        // Specifying -1 for the lease interval creates an infinite lease.
                        await lease.AcquireAsync(TimeSpan.FromSeconds(-1));

                        // Get a BlobClient representing the destination blob with a unique name.
                        BlobClient destBlob = targetContainerClient.GetBlobClient(sourceBlob.Name);

                        // Start the copy operation.
                        await destBlob.StartCopyFromUriAsync(sourceBlob.Uri);

                        // Update the source blob's properties.
                        BlobProperties sourceProperties = await sourceBlob.GetPropertiesAsync();

                        if (sourceProperties.LeaseState == LeaseState.Leased)
                        {
                            // Break the lease on the source blob.
                            await lease.BreakAsync();
                        }
                    }
                }

                
                return targetContainer;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                throw;
            }

        }
        #endregion
    }
}
