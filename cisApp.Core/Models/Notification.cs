using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Core
{
    public partial class Notification
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Msg { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
    }
}
