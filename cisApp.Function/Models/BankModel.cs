using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp
{
    public class BankModel
    {
        [Required]
        public int BankId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public int AccountTypeId { get; set; }

    }
}
