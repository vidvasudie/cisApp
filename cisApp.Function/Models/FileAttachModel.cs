using System;
using System.Collections.Generic;
using System.Text;
using cisApp.Core;

namespace cisApp.Function
{
    public class FileAttachModel : AttachFile
    {
        public Guid gId { get; set; }
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Description { get; set; }
        public bool IsView { get; set; } = false;
        public string FileBase64 { get; set; }
        public int NextImg { get; set; }
        public int NextImgSelected { get; set; }
        public int col { get; set; }
    }
}
