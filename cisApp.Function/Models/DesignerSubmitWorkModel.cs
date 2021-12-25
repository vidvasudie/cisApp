using cisApp.library;
using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public class DesignerSubmitWorkModel
    {
        public Guid JobId { get; set; }
        public Guid? CaUserId { get; set; }
        public Guid? AttachFileId { get; set; }//id designer image
        public string FileName { get; set;}
        public string Fullname { get; set; }
        public DateTime? LastLogin { get; set; }
        public string AlbumName { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateStr
        {
            get
            {
                return UpdatedDate.ToStringFormat();
            }
            set
            {
                UpdatedDate = value.ToDateTimeFormat();
            }
        }
        public Guid? AlbumAttachFileID { get; set; }//id work image
        public string AlbumFileName { get; set; }
        public string CaUrlPath
        {
            get
            {
                return "~/Uploads" + "/" + this.AttachFileId + "/" + this.FileName;
            }
        }
        public string WorkUrlPath
        {
            get
            {
                return "~/Uploads" + "/" + this.AlbumAttachFileID + "/" + this.AlbumFileName;
            }
        }
        public List<DesignerSubmitWorkModel> works { get; set; }
    }
}
