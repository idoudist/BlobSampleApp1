using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlobSampleApp1.Interfaces;
using Azure.Storage.Blobs.Models;
using System.Threading.Tasks;
using BlobSampleApp1.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace BlobSampleApp1.Services.Tests
{
    [TestClass()]
    public class ContainerServiceTests
    {
        private readonly IContainerService _containerService;
        private readonly IMockupServices _mockupService;
        private IConfiguration _config;

        public ContainerServiceTests()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(Configuration);

            _mockupService = new MockupServices();
            _containerService = new ContainerService(_config, _mockupService);
        }

        public IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    var provider = new PhysicalFileProvider("/Config");
                    var builder = new ConfigurationBuilder().AddJsonFile(provider, $"testsettings.json", optional: false , false);
                    _config = builder.Build();
                }

                return _config;
            }
        }

        public ContainerServiceTests(IContainerService containerService, IMockupServices mockupService)
        {
            _containerService = containerService;
            _mockupService = mockupService;
        }

        [TestMethod()]
        public async Task CloneContainerTest()
        {
            var response = await _containerService.CloneContainer("fadi", _mockupService.Randomize(AppConstant.SAMPLE_CONTAINER_NAME));
            Assert.IsTrue(response != null);
        }
    }
}