using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using cisApp.Core;
using cisApp.library;
using static cisApp.library.DateTimeUtil;

namespace cisApp.Function
{
    public class UserModel: UserModelCommon
    {
        public Guid? UserId { get; set; }

         
        /// <summary>
        /// 1 = ผู้ใข้งาน,
        /// 2 = นักออกแบบ
        /// ,3 = เจ้าหน้าที่
        /// </summary>
        /// 
        [Display(Name = "ประเภทผู้ใช้งาน")]
        public int? UserType { get; set; }

        
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
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateStr
        {
            get
            {
                return CreatedDate.ToStringFormat(DateTimeFormat.FULL);
            }
            set
            {
                CreatedDate = value.ToDateTimeFormat();
            }
        }
        public DateTime UpdatedDate { get; set; }
        public DateTime DeletedDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Guid? DeletedBy { get; set; }

        //public int Id { get; set; }
        public string Code { get; set; }
        public int? Status { get; set; }
        public string StatusDesc { get; set; }
        public string Remark { get; set; }
        public int JobWaitingStatusTotal { get; set; }
        public int JobProcessStatusTotal { get; set; }
        public int JobTotal { get; set; }
        public int LikeOtherCount { get; set; }
        public int OtherLikedCount { get; set; }


        [NotMapped]
        public List<FileAttachModel> files { get; set; }

        [NotMapped]
        public AttachFile AttachFileImage { get; set; }
        [NotMapped]
        public Guid AttachFileId { get; set; }

        [NotMapped]
        public bool FileRemove { get; set; }

        [NotMapped]
        public string FileBase64 { get; set; }

        [NotMapped]
        public string FileName { get; set; }

        [NotMapped]
        public string FileSize { get; set; }
        [NotMapped]
        public string UrlPath
        {
            get
            {
                return "~/Uploads" + "/" + this.AttachFileId + "/" + this.FileName;
            }
        }

        [NotMapped]
        public Guid? ApiUserImg { get; set; }
        [NotMapped]
        public List<Guid> ApiAttachFileImg { get; set; } = new List<Guid>();
        [NotMapped]
        public int ReqId { get; set; }
    }
}
