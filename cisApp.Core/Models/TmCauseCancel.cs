using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Core
{
    public partial class TmCauseCancel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
