using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.API.Models
{
    public class ChatMessageAPIModel
    {
        [Required]
        public Guid? SenderId { get; set; }
        [Required]
        public Guid? RecieverId { get; set; }
        public List<Guid> Imgs { get; set; }
        public string Message { get; set; }
        public string Ip { get; set; }
    }
}
