using BlobSampleApp1.Interfaces;
using BlobSampleApp1.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlobSampleApp1.IocContainer
{
    public static class IocServices
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.RegisterBlobServices();
        }

        private static void RegisterBlobServices(this IServiceCollection services)
        {
            services.AddTransient<IAzureFileService, AzureFileService>();
            services.AddTransient<IMockupServices, MockupServices>();
            services.AddTransient<IContainerService, ContainerService>();
        }
    }
}
