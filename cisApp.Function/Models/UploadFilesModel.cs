using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public class UploadFilesModel 
    { 
        public Guid? AttachFileId { get; set; }
        public int? AlbumId { get; set; }
        public string ElementId { get; set; }
        public int TypeId { get; set; }
        public string Description { get; set; }
        public List<FileAttachModel> files { get; set; }
    }
}
