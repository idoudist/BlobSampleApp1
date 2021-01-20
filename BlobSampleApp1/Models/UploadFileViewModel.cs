using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BlobSampleApp1.Models
{
    public class UploadFileViewModel
    {
        public List<SelectListItem> ContainerSelectList { get; set; }

        public string SelectedContainer { get; set; }

        public string FilePath { get; set; }
        public IFormFile InputFile { get; set; }
        public UploadFileViewModel()
        {
            ContainerSelectList = new List<SelectListItem>();
        }
    }
}
