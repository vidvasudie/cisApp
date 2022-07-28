using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Core
{
    public partial class UserHelp
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        /// <summary>
        /// สถานะ 1=แจ้งปัญหา, 2=ตอบกลับ 
        /// </summary>
        public int Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        /// <summary>
        /// ตอบกลับปัญหา
        /// </summary>
        public string Remark { get; set; }
    }
}
