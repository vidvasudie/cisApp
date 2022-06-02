using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public class CandidateSelectModel
    {
        public Guid JobId { get; set; }
        public Guid CaUserId { get; set; }
        public string ip { get; set; }
        public Guid UserId { get; set; }
        public int CaStatusId { get; set; }
    }
}
