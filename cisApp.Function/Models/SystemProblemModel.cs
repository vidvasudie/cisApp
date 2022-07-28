using cisApp.Core;
using System;
using System.Collections.Generic;
using System.Text;
using static cisApp.library.DateTimeUtil;

namespace cisApp.Function
{
    public class SystemProblemModel : UserHelp
    {
        public string OfficerName { get; set; }
        public string StatusMsg { get; set; }
        public string CreatedDateStr
        {
            get
            {
                return CreatedDate.ToStringFormat(DateTimeFormat.FULL);
            }
            set
            {
                CreatedDate = value.ToDateTimeFormat();
            }
        }
        public string UpdatedDateStr
        {
            get
            {
                DateTime? dt = UpdatedDate;
                return dt.ToStringFormat(DateTimeFormat.FULL);
            }
        }
    }
}
