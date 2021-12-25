using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.API.Models
{
    public class SubmitworkModel
    {
        [Required]
        public Guid? JobId { get; set; }
        [Required]
        public Guid? UserId { get; set; }
        public string Category { get; set; }
        public string Tags { get; set; }
        public string AlbumName { get; set; }
        [Required]
        public string Url { get; set; }
        /// <summary>
        /// แบ่งเป็นเป็น 1=ประกวด,2=ส่งงานงวดที่1,3=ส่งงานงวดที่2,4=ส่งงานงวดที่3
        /// </summary>
        public string AlbumType { get; set; }
        public List<Guid> imgs { get; set; }
    }
}
