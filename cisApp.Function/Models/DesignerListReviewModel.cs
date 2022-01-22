using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public class DesignerListReviewModel
    {
        public Guid DesignerUserId { get; set; }
        public string DesignerFullname { get; set; }
        public Guid AttachFileId { get; set; }
        public string FileName { get; set; }
        public string UrlPath
        {
            get
            {
                return "~/Uploads" + "/" + this.AttachFileId + "/" + this.FileName;
            }
        }
        public Guid UserId { get; set; }
        public string Fullname { get; set; }
        public string PositionName { get; set; }
        public decimal Rate { get; set; }
        public decimal RateAll { get; set; }
        public string Message { get; set; }

    }
}
