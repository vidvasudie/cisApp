using System;
using System.Collections.Generic;
using System.Text;
using cisApp.Core;
using cisApp.library;

namespace cisApp.Function
{
    public class JobModel : Jobs
    {
        public bool IsDraft { get; set; } = false;
        public string UserFullName { get; set; }
        public string UserTypeDesc { get; set; }
        public string UserEmail { get; set; }
        public string UserTel { get; set; }
         
        public string JobTypeDesc { get; set; }
        public string PayStatusDesc { get; set; }
        public string JobStatusDesc { get; set; } 
        public string JobBeginDateStr
        {
            get 
            { 
                return JobBeginDate.ToStringFormat(); 
            }
            set
            {
                JobBeginDate = value.ToDateTimeFormat();
            }
        } 
        public string JobEndDateStr 
        {
            get
            {
                return JobEndDate.ToStringFormat();
            }
            set
            {
                JobEndDate = value.ToDateTimeFormat();
            }
        }
        public Guid JobsExImgId { get; set; } 
        public int? JobsExTypeId { get; set; }
        public string JobsExTypeDesc { get; set; }
        public List<FileAttachModel> files { get; set; }
        public bool IsApi { get; set; } = false;
    }
}
