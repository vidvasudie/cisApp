using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.API.Models
{
    public class PaymentModel
    {
        [Required]
        public Guid? JobId { get; set; }
        [Required]
        public Guid? UserId { get; set; }
        public int? JobPaymentId { get; set; }
        public Guid? AttachId { get; set; }
        public string Ip { get; set; }
    }
}
