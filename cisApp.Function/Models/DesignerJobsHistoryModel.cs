using cisApp.library;
using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public class DesignerJobsHistoryModel
    {
        public bool IsCusFavorite { get; set; }
        public Guid JobId { get; set; }
        public string JobNo { get; set; }
        public string JobDescription { get; set; }
        public string JobTypeName { get; set; }
        public decimal JobAreaSize { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateStr
        {
            get
            {
                return CreatedDate.ToStringFormat();
            }
            set
            {
                CreatedDate = value.ToDateTimeFormat();
            }
        }
        public string WinText { get; set; }
        public Guid UserId { get; set; }
        public decimal Rate { get; set; }
        public string Fullname { get; set; }
        public Guid PicAttachFileID { get; set; }
        public string PicFileName { get; set; }
        public string UrlPath
        {
            get
            {
                return "~/Uploads" + "/" + this.PicAttachFileID + "/" + this.PicFileName;
            }
        }
        public string ImgCoverUrl { get; set; }
    }
}
