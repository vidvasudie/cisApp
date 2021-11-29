using System;
using System.Collections.Generic;
using System.Text;
using cisApp.Core;

namespace cisApp.Function
{
    public class JobsExamImageModel : AttachFile
    {
        public int? JobsExTypeId { get; set; }
        public string JobsExTypeDesc { get; set; }
    }
}
