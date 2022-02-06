using System;
using System.Collections.Generic;
using System.Text;
using cisApp.library;

namespace cisApp.Function
{
    public class AlbumImageJsonModel
    {
        public string AlbumName { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<AlbumDetail> albf { get; set; }

        public string UpdatedDateStr
        {
            get
            {
                return UpdatedDate.ToStringFormat();
            }
            set
            {
                UpdatedDate = value.ToDateTimeFormat();
            }
        }
    } 
     

    public class AlbumDetail
    {
        public string AlbumAttachFileID { get; set; }
        public string AlbumFileName { get; set; }
        public string WorkUrlPath
        {
            get
            {
                return "~/Uploads" + "/" + this.AlbumAttachFileID + "/" + this.AlbumFileName;
            }
        }
    }

}
