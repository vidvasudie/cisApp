using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Core
{
    public partial class TmUserType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
