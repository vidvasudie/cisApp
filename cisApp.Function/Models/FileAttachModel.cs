using System;
using System.Collections.Generic;
using System.Text;
using cisApp.Core;

namespace cisApp.Function
{
    public class FileAttachModel : AttachFile
    {
        public int JobExTypeId { get; set; }
        public string Description { get; set; }
        public string FileBase64 { get; set; }
    }
}
