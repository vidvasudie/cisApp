using System;
using System.Collections.Generic;
using System.Text;
using cisApp.Core;

namespace cisApp.Function.Models
{
    public class ChatMessageFileModel : ChatMessage
    {
        public List<FileUploadModel> FileList { get; set; }
    }

    public class FileUploadModel
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int FileSize { get; set; }
        public string Base64Str { get; set; }
    }
}
