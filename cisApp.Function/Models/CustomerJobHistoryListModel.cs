using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public class CustomerJobHistoryListModel
    {
        public Guid JobID { get; set; }
        public string JobType { get; set; }
        public decimal JobAreaSize { get; set; }
        public string JobDescription { get; set; }
        public string JobStatusDesc { get; set; }
        public Guid? AttachFileID { get; set; }
        public string FileName { get; set; }
        public string Fullname { get; set; }
        public string UrlPath
        {
            get
            {
                return "~/Uploads" + "/" + this.AttachFileID + "/" + this.FileName;
            }
        }
    }
}
