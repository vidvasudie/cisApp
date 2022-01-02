using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.SignalR.Models
{
    public class UserAndConnectionId
    {
        public Guid UserId { get; set; }
        public string ConnectionId { get; set; }
    }
}
