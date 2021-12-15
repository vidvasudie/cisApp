using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.API.Models
{
    public class UploadProfileModel
    {
        [Required]
        public Guid? UserId { get; set; }
        [Required]
        public Guid? AttachId { get; set; }
    }
}
