using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public class UploadFilesModel 
    {
        public string ElementId { get; set; }
        public int JobExTypeId { get; set; }
        public string Description { get; set; }
        public List<FileAttachModel> files { get; set; }
    }
}
