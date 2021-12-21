using cisApp.library;
using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public class CustomerJobListModel
    {
        public Guid JobID { get; set; }
        public string JobType { get; set; }
        public decimal JobAreaSize { get; set; }
        public int JobStatus { get; set; }
        public string JobStatusDesc { get; set; }
        public DateTime? DueDate { get; set; }
        public string DueDateStr
        {
            get
            {
                return DueDate.ToStringFormat();
            }
        }
        public int CandidateCount { get; set; }
        public int CandidateWorkSubmitCount { get; set; }
    }
}
