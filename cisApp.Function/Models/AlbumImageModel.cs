using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public class AlbumImageModel
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

        public Guid? ImgId { get; set; }
        public int AlbumId { get; set; }
        public Guid? UserId { get; set; }

        public Guid JobID { get; set; }
        public string Category { get; set; }
        public string Tags { get; set; }
        public string AlbumName { get; set; }

        public string UrlPath
        {
            get
            {
                return "Uploads" + "/" + this.AttachFileId + "/" + this.FileName;
            }
        }

        public string DesignerFullName { get; set; }
        public Guid? DesignerUserId { get; set; }

        public string FullUrlPath { get; set; }
        public int? LikeCount { get; set; }
        public bool IsLiked { get; set; }
        public Guid? AlbumRefId { get; set; }

        public List<AlbumThumbnail> Thumbnails { get; set; }

        public int NextImg { get; set; }

    }

    public class AlbumThumbnail
    {
        public Guid AttachId { get; set; }
        public string FileName { get; set; }
        public string UrlPath { get; set; }
        public string FullUrlPath { get; set; }
    }
}
