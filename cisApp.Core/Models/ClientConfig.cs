using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Core
{
    public partial class ClientConfig
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
