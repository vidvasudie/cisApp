using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Core
{
    public partial class JobCadidateLock
    {
        /// <summary>
        /// ลำดับ
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// รหัสใบงาน
        /// </summary>
        public Guid JobId { get; set; }
        /// <summary>
        /// เวลาสิ้นสุดการจ่ายเงิน + 15นาที
        /// </summary>
        public DateTime ExpireDate { get; set; }
        /// <summary>
        /// สถานะรายการจ่ายเงิน 
        /// </summary>
        public bool? IsActive { get; set; }
    }
}
