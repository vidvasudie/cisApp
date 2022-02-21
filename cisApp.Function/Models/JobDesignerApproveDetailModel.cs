using cisApp.library;
using System;
using System.Collections.Generic;
using System.Text;
using static cisApp.library.DateTimeUtil;

namespace cisApp.Function
{
    public class JobDesignerApproveDetailModel
    {
        public Guid JobId { get; set; }
        public Guid JobCaUserId { get; set; }
        public Guid PicAttachFileID { get; set; }
        public string PicFileName { get; set; }
        public string PicUrlPath
        {
            get
            {
                return "~/Uploads" + "/" + this.PicAttachFileID + "/" + this.PicFileName;
            }
        }
        public DateTime? LastLogin { get; set; }
        public string LastLoginStr
        {
            get
            {
                return LastLogin.ToStringFormat(DateTimeFormat.FULL);
            }
            set
            {
                LastLogin = value.ToDateTimeFormat();
            }
        }
        public string Fullname { get; set; }
        public string Tel { get; set; }
        public bool IsCanEdit { get; set; }
        public bool IsCusFavorite { get; set; }
        /// <summary>
        /// true if job_status = 5 ขอไฟล์แบบติดตั้ง แสดงข้อความ  ยืนยันผลงาน
        /// false if job_status = ส่งแนบไฟล์ แสดงข้อความ  ขอไฟล์แบบติดตั้ง
        /// </summary>
        public bool IsConfirmApprove { get; set; } 
        public string AlbumName { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Url { get; set; }
        public string AlbumType { get; set; }
        public string AlbumTypeDesc { get; set; }
        public Guid ImgAttachFileID { get; set; }
        public string ImgFileName { get; set; }
        public string ImgUrlPath
        {
            get
            {
                return "~/Uploads" + "/" + this.ImgAttachFileID + "/" + this.ImgFileName;
            }
        }
    } 
}
