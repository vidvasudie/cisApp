using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public class WinnerModel
    {
        public int Rownum { get; set; }
        public int WinCount { get; set; }
        public Guid UserId { get; set; }
        public string Fullname { get; set; }
        public Guid AttachFileId { get; set; }
        public string AttachFileName { get; set; }
        public string UrlPath
        {
            get
            {
                return "~/Uploads" + "/" + this.AttachFileId + "/" + this.AttachFileName;
            }
        }
    }
}
