using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.API.Models
{
    public class PostClientIdModel
    {
        public Guid UserId { get; set; }
        public string ClientId { get; set; }
    }
}
