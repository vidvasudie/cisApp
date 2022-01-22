using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public class DesignerProfileModel
    {
        public Guid DesignerUserId { get; set; }
        public string Fullname { get; set; }
        public Guid AttachFileID { get; set; }
        public string FileName { get; set; }
        public string UrlPath
        {
            get
            {
                return "~/Uploads" + "/" + this.AttachFileID + "/" + this.FileName;
            }
        }
        public decimal RateAll { get; set; }
        public int RateCount { get; set; }
        public int ContestWinTotal { get; set; }
        public int AreaSQMMax { get; set; }
        public int AreaSQMUsed { get; set; }
        public int AreaSQMRemain { get; set; }
        public decimal AreaSQMRate { get; set; }
        public string PositionName { get; set; }
        public string Caption { get; set; }
        public bool IsFavorite { get; set; }
    }
}
