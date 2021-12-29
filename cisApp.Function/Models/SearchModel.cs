using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public class SearchModel
    {
        public Guid? Id { get; set; }
        public Guid? gId { get; set; }
        public string text { get; set; }
        public int? currentPage { get; set; } = 1;
        public int? pageSize { get; set; } = 10;
        public int status { get; set; } = 0;
        public int mode { get; set; } = 0; 
        public int? type { get; set; }
        public string goBack { get; set; }
        public bool? active { get; set; }

        public int? jobType { get; set; }
        public int? PaymentStatus { get; set; }
        public Guid? JobId { get; set; }
        public int? JobStatus { get; set; }

        public Guid? SenderId { get; set; }
        public Guid? RecieverId { get; set; }
    }
}
