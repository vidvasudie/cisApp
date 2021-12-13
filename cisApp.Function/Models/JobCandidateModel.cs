using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using cisApp.Core;
using cisApp.library;

namespace cisApp.Function
{
    public class JobCandidateModel : JobsCandidate
    {
        public string UserFullName { get; set; }
        public int UserLikes { get; set; }
        public decimal PriceRate { get; set; }
        public decimal UserRate { get; set; }
        public int UserMessageCount { get; set; }
        public string CaStatusDesc { get; set; }
        public int RegisterDays
        {
            get
            {
                return DateTime.Now.Subtract(CreatedDate.Value).Days; //return days pass after register
            } 
        }
        public string CreatedDateStr
        {
            get
            {
                return CreatedDate.ToStringFormat(DateTimeUtil.DateTimeFormat.FULL);
            }
        }
        public int UserRegAccept { get; set; }
        public int UserRegTotal { get; set; }
        public Guid AttachFileId { get; set; }
        public string AttachFileName { get; set; }
        public string UrlPath
        {
            get
            {
                return "~/Uploads" + "/" + this.AttachFileId + "/" + this.AttachFileName;
            }
        }
        [NotMapped]
        public List<JobCandidateModel> userCandidates { get; set; }
    }
}
