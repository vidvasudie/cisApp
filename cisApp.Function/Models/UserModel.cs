using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace cisApp.Function
{
    public class UserModel
    {
        public Guid? UserId { get; set; }

        [Display(Name = "ชื่อ")]
        [Required(ErrorMessage = "กรุณากรอกชื่อ")]
        public string Fname { get; set; }

        [Display(Name = "นามสกุล")]
        [Required(ErrorMessage = "กรุณากรอกนามสกุล")]
        public string Lname { get; set; }
        /// <summary>
        /// 1 = ผู้ใข้งาน,
        /// 2 = นักออกแบบ
        /// ,3 = เจ้าหน้าที่
        /// </summary>
        /// 
        [Display(Name = "ประเภทผู้ใช้งาน")]
        public int? UserType { get; set; }

        [Display(Name = "เบอร์โทรศัพท์")]
        [Required(ErrorMessage = "กรุณากรอกเบอร์โทรศัพท์")]
        public string Tel { get; set; }

        [Display(Name = "อีเมล")]
        [Required(ErrorMessage = "กรุณากรอกอีเมล")]
        public string Email { get; set; }

        public Guid? RoleId { get; set; }
        /// <summary>
        /// รหัสผู้ออกแบบ
        /// </summary>
        public Guid UserDesignerId { get; set; }
        /// <summary>
        /// เลขประจำตัวประชาชน
        /// </summary>
        [MinLength(13, ErrorMessage = "กรุณากรอกเลขประจำตัวประชาชนให้ถูกต้อง")]
        public string PersonalId { get; set; }
        /// <summary>
        /// รหัสธนาคาร
        /// </summary>
        public int? BankId { get; set; }
        /// <summary>
        /// เลขที่บัญชี
        /// </summary>
        public string AccountNumber { get; set; }
        /// <summary>
        /// ประเภทบัญชี: 1=ออมทรัพย์, 2=ประจำ, 3=กระแสรายวัน
        /// </summary>
        public int? AccountType { get; set; }
        public string Address { get; set; }
        /// <summary>
        /// code ตำบล
        /// </summary>
        public int? SubDistrictId { get; set; }
        /// <summary>
        /// code อำเภอ
        /// </summary>
        public int? DistrictId { get; set; }
        /// <summary>
        /// code จังหวัด
        /// </summary>
        public int? ProvinceId { get; set; }
        /// <summary>
        /// รหัสไปรษณีย์
        /// </summary>
        public string PostCode { get; set; }
        public string UserTypeDesc { get; set; }
        public string ProvinceDesc { get; set; }
        public string DistrictDesc { get; set; }
        public string SubDistrictDesc { get; set; }
        public string BankName { get; set; }
        public string AccountTypeDesc { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime DeletedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Guid? DeletedBy { get; set; }

        public string Code { get; set; }
        public int? Status { get; set; }
        public string StatusDesc { get; set; }
        public string Remark { get; set; }
    }
}
