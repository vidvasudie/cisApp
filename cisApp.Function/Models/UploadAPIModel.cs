using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.Function
{
    public class UploadAPIModel
    {
        [Required]
        public string FileBase64 { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public int Size { get; set; }
        [Required]
        public Guid UserId { get; set; }
    }
}
