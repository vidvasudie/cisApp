using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Common
{
    public class common
    {
        public bool success { get; set; }
        public int? page { get; set; }
        public int? nextpage { get; set; }
        public int? rows { get; set; }
        public int? Totalsrows { get; set; }
        public string Message { get; set; }
        public string MessageEN { get; set; } 
        public Object Data { get; set; }
    }

}
