using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BlobSampleApp1.Models
{
    public class FilesOverviewViewModel
    {
        public IList<SelectListItem> Containers { get; set; }

        public List<FileResponseViewModel> Files { get; set; }

        public FilesOverviewViewModel()
        {
            Containers = new List<SelectListItem>();
            Files = new List<FileResponseViewModel>();
        }
    }
}
