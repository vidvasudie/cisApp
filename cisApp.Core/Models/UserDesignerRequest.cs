using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace cisApp.Core
{
    public partial class UserDesignerRequest
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public Guid? UserId { get; set; }
        public string PersonalId { get; set; }
        public int? BankId { get; set; }
        public string AccountNumber { get; set; }
        public int? AccountType { get; set; }
        public string Address { get; set; }
        public int? SubDistrictId { get; set; }
        public int? DistrictId { get; set; }
        public int? ProvinceId { get; set; }
        public string PostCode { get; set; }
        public int? Status { get; set; }
        public string Remark { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public string provinceName { get; set; }
        [NotMapped]
        public string districtName { get; set; }
        [NotMapped]
        public string subDistrictName { get; set; }
        [NotMapped]
        public string accountTypeName { get; set; }
        [NotMapped]
        public string bankName { get; set; }
    }
}
