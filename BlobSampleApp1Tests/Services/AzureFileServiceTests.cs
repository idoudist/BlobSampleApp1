using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlobSampleApp1.Services;
using System;
using System.Collections.Generic;
using System.Text;
using BlobSampleApp1Tests.Config;
using Azure.Storage.Blobs;
using BlobSampleApp1.Utils;
using System.Threading.Tasks;
using System.IO;

namespace BlobSampleApp1.Services.Tests
{
    [TestClass()]
    public class AzureFileServiceTests : UnitTestManager
    {
        [TestMethod()]
        public async Task DeleteFileTest()
        {
            // Create a temporary Lorem Ipsum file on disk that we can upload
            string path = _mockupService.CreateTempFile();

            // Get a reference to a container named "sample-container" and then create it
            string containerName = _mockupService.Randomize(AppConstant.SAMPLE_CONTAINER_NAME);
            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            await container.CreateAsync();
            try
            {
                // Get a reference to a blob
                string blobName = _mockupService.Randomize(AppConstant.SAMPLE_FILE_NAME);
                BlobClient blob = container.GetBlobClient(blobName);

                // Open the file and upload its data
                using (FileStream file = File.OpenRead(path))
                {
                    await blob.UploadAsync(file);
                }
                
                await _azureFileService.DeleteFile(blobName, containerName);
                Assert.IsTrue(true);
            }
            finally
            {
                // delete file and container after compleating operation
                await container.DeleteAsync();
            }
            
        }
    }
}