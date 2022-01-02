using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Core
{
    public partial class JobDesignerReview
    {
        public int? Id { get; set; }
        public Guid? JobId { get; set; }
        /// <summary>
        /// รหัสผู้ใช้งาน
        /// </summary>
        public Guid? UserId { get; set; }
        public decimal? Rate { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        /// <summary>
        /// รหัสนักออกแบบ
        /// </summary>
        public Guid? DesignerUserId { get; set; }
    }
}
