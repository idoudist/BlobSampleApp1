using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BlobSampleApp1.Models
{
    public class FilesOverviewViewModel
    {
        public IList<SelectListItem> containers { get; set; }

        public List<BlobItem> files { get; set; }

        public FilesOverviewViewModel()
        {
            containers = new List<SelectListItem>();
        }
    }
}
