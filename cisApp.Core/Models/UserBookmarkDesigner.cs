using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Core
{
    public partial class UserBookmarkDesigner
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        /// <summary>
        /// รหัสผู้ใข้งานของนักออกแบบ
        /// </summary>
        public Guid? UserDesignerId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
