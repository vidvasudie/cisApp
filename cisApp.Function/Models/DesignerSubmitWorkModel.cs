using cisApp.library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public class DesignerSubmitWorkModel
    {
        public Guid JobId { get; set; }
        public Guid? CaUserId { get; set; }
        public Guid? AttachFileId { get; set; }//id designer image
        public string FileName { get; set;}
        public string Fullname { get; set; }
        public DateTime? LastLogin { get; set; }
        
        public DateTime? UpdatedDate { get; set; }
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
        public string CaUrlPath
        {
            get
            {
                if (this.AttachFileId == null || this.AttachFileId == Guid.Empty)
                {
                    return null;
                }
                else
                {
                    return "~/Uploads" + "/" + this.AttachFileId + "/" + this.FileName;
                }                
            }
        }
       
        public List<DesignerSubmitWorkModel> works { get; set; }
        public string DetailJson { get; set; }
        public List<AlbumImageJsonModel> workImages 
        {
            get 
            {  
                return String.IsNullOrEmpty(this.DetailJson) ? null : JsonConvert.DeserializeObject<List<AlbumImageJsonModel>>(this.DetailJson); 
            }  
        }
        
    }
}
