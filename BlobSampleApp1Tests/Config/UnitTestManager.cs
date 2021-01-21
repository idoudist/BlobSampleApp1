using Azure.Storage.Blobs;
using BlobSampleApp1.Interfaces;
using BlobSampleApp1.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace BlobSampleApp1Tests.Config
{

    public class UnitTestManager
    {
        #region service
        protected readonly IContainerService _containerService;
        protected readonly IMockupServices _mockupService;
        protected IConfiguration _config;
        protected readonly BlobServiceClient blobServiceClient;
        #endregion

        #region Fields
        protected readonly string connectionString;
        #endregion

        public UnitTestManager()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(Configuration);

            _mockupService = new MockupServices();
            _containerService = new ContainerService(_config, _mockupService);
            connectionString = _config["storageconnectionstring"];
        }

        public IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    var provider = new PhysicalFileProvider("/Config");
                    var builder = new ConfigurationBuilder().AddJsonFile(provider, $"testsettings.json", optional: false, false);
                    _config = builder.Build();
                }

                return _config;
            }
        }
    }
}
