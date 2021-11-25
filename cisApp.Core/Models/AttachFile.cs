using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace cisApp.Core
{
    public partial class AttachFile
    {
        public Guid AttachFileId { get; set; }
        /// <summary>
        /// รหัสอ้างอิงตัวไฟล์
        /// </summary>
        public Guid? RefId { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public int Size { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? DeletedBy { get; set; }

        [NotMapped]
        public string UrlPath
        {
            get
            {
                return "~/Uploads" + "/" + this.AttachFileId + "/" + this.FileName;
            }
        }
    }
}
