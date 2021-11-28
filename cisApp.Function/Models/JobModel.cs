using System;
using System.Collections.Generic;
using System.Text;
using cisApp.library;

namespace cisApp.Function
{
    public class JobModel
    {
        public Guid JobId { get; set; }
        /// <summary>
        /// customer
        /// </summary>
        public Guid? UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserTypeDesc { get; set; }
        public string UserEmail { get; set; }
        public string UserTel { get; set; }
        /// <summary>
        /// รหัสงาน
        /// </summary>
        public string JobNo { get; set; }
        /// <summary>
        /// designer
        /// </summary>
        public Guid? JobCaUserId { get; set; }
        /// <summary>
        /// รหัสประเภทใบงาน
        /// </summary>
        public int? JobTypeId { get; set; }
        public string JobTypeDesc { get; set; }
        /// <summary>
        /// ขอบเขตงาน
        /// </summary>
        public string JobDescription { get; set; }
        /// <summary>
        /// ขนาดพื้นที่ 
        /// </summary>
        public decimal? JobAreaSize { get; set; }
        /// <summary>
        /// ยอดรวม
        /// </summary>
        public decimal? JobPrice { get; set; }
        /// <summary>
        /// ราคา/ตรม
        /// </summary>
        public decimal? JobPricePerSqM { get; set; }
        /// <summary>
        /// สถานะปัจจุบัน
        /// </summary>
        public int? JobStatus { get; set; }
        public string JobStatusDesc { get; set; }
        public DateTime? JobBeginDate { get; set; } 
        public string JobBeginDateStr
        {
            get 
            { 
                return JobBeginDate.ToStringFormat(); 
            }
            set
            {
                JobBeginDate = value.ToDateTimeFormat();
            }
        }
        public DateTime? JobEndDate { get; set; }
        public string JobEndDateStr 
        {
            get
            {
                return JobEndDate.ToStringFormat();
            }
            set
            {
                JobEndDate = value.ToDateTimeFormat();
            }
        }
        public Guid JobsExImgId { get; set; } 
        public int? JobsExTypeId { get; set; }
        public string JobsExTypeDesc { get; set; }

    }
}
