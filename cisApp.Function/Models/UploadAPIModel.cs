using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.Function
{
    public class UploadAPIModel
    { 
        public string FileBase64 { get; set; } 
        public string FileName { get; set; } 
        public int Size { get; set; } 
        public Guid UserId { get; set; }
    }
}
