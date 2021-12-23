using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public class DesignerJobListSearch
    {
        public Guid userId { get; set; }
        public Guid jobId { get; set; }
        public string text { get; set; }
        public int? skip { get; set; } = 0;
        public int? take { get; set; } = 10;
    }
}
