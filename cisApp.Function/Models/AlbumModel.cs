using System;
using System.Collections.Generic;
using System.Text;
using cisApp.Core;
using cisApp.library;

namespace cisApp.Function
{
    public class AlbumModel : Album
    {
        public Guid CaUserId { get; set; }
        public int CaStatusId { get; set; }
        public string CreatedDateStr
        {
            get
            {
                return CreatedDate.ToStringFormat(DateTimeUtil.DateTimeFormat.ABBR);
            }
        }
        public string UserFullName { get; set; }
        public Guid ImgId { get; set; }
        public Guid AttachFileId { get; set; }
        public string AttachFileName { get; set; }
        public string UrlPath
        {
            get
            {
                return this.AttachFileId != Guid.Empty ? "~/Uploads" + "/" + this.AttachFileId + "/" + this.AttachFileName : null;
            }
        }
        public int CandidateSelected { get; set; }
        
        public List<FileAttachModel> files { get; set; }
        public List<Guid> apiFiles { get; set; }

    }
}
