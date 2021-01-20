using Azure.Storage.Blobs.Models;

namespace BlobSampleApp1.Models
{
    public class FileResponseViewModel
    {
        public BlobItem File { get; set; }

        public string ContainerName { get; set; }

        public FileResponseViewModel ()
        {
        }
        public FileResponseViewModel(BlobItem file, string containerName)
        {
            File = file;
            ContainerName = containerName;
        }
    }
}
