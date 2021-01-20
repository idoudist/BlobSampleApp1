using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlobSampleApp1.Interfaces;
using Azure.Storage.Blobs.Models;
using System.Threading.Tasks;

namespace BlobSampleApp1.Services.Tests
{
    [TestClass()]
    public class ContainerServiceTests
    {
        private readonly IContainerService _containerService;
        private readonly IMockupServices _mockupService;
        public ContainerServiceTests(IContainerService containerService, IMockupServices mockupService)
        {
            _containerService = containerService;
            _mockupService = mockupService;
        }

        [TestMethod()]
        public async Task CloneContainerTest()
        {
            string sourceContainer = "fadi";
            string targetContainer = _mockupService.Randomize("new_container");
            BlobContainerInfo response =  await _containerService.CloneContainer(sourceContainer, targetContainer);
            Assert.IsTrue(response != null);
        }
    }
}