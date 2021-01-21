using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlobSampleApp1.Interfaces;
using Azure.Storage.Blobs.Models;
using System.Threading.Tasks;
using BlobSampleApp1.Utils;
using BlobSampleApp1Tests.Config;
using Azure.Storage.Blobs;
using System.IO;

namespace BlobSampleApp1.Services.Tests
{
    [TestClass()]
    public class ContainerServiceTests : UnitTestManager
    {
        

        [TestMethod()]
        public async Task CloneContainerTest()
        {
            //destination container name 
            string destinationContainerName = _mockupService.Randomize(AppConstant.SAMPLE_CONTAINER_NAME);
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


                //test operation
                var response = await _containerService.CloneContainer(container.Name, destinationContainerName);

                Assert.IsTrue(response != null);

            }
            finally
            {
                // delete file and container after compleating operation
                await container.DeleteAsync();
                //// delete destination container
                await _containerService.DeleteContainerAsync(destinationContainerName);
            }

        }
    }
}