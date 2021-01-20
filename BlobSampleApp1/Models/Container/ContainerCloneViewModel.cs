using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BlobSampleApp1.Models.Container
{
    public class ContainerCloneViewModel
    {
        public string SourceContainer { get; set; }
        public string TargetContainer { get; set; }
        public IList<SelectListItem> Containers { get; set; }

        public ContainerCloneViewModel()
        {
            Containers = new List<SelectListItem>();
        }
    }
}
