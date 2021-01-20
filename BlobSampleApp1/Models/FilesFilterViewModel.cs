using System.Collections.Generic;

namespace BlobSampleApp1.Models
{
    public class FilesFilterViewModel
    {
        public IEnumerable<string> Containers { get; set; }

        public FilesFilterViewModel ()
        {
            Containers = new List<string>();
        }
    }
}
