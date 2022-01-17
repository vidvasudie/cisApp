using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Core
{
    public partial class Faq
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int Qorder { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
