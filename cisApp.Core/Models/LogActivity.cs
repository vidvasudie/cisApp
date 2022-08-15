using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Core
{
    public partial class LogActivity
    {
        public int ActivityId { get; set; }
        public string Ip { get; set; }
        /// <summary>
        /// รหัสผู้ใช้งาน
        /// </summary>
        public Guid? RefUserId { get; set; }
        /// <summary>
        /// ชื่อผู้ใช้งาน
        /// </summary>
        public string RefFullname { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Note { get; set; }
        public string Url { get; set; }
        public string ExceptionNote { get; set; }
        public string RequestData { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
