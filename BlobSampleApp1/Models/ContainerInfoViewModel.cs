using Azure.Storage.Blobs.Models;

namespace BlobSampleApp1.Models
{
    public class ContainerInfoViewModel
    {
        public string Name { get; set; }
        public BlobContainerProperties Properties { get; set; }
    }
}
