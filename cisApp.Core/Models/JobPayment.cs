﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace cisApp.Core
{
    public partial class JobPayment
    {
        public int? JobPayId { get; set; }
        public Guid? JobId { get; set; }
        public DateTime? PayDate { get; set; }
        /// <summary>
        /// รอชำระเงิน =1 , อยู่ระหว่างตรวจสอบ =2 , สำเร็จ  = 3 , ไม่อนุมัติ/คืนเงิน  = 4 
        /// </summary>
        public int? PayStatus { get; set; } = 1;
        public string Comment { get; set; }
        public string OrderId { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Guid? DeletedBy { get; set; }

        [NotMapped]
        public AttachFile AttachFileImage { get; set; }

        [NotMapped]
        public bool FileRemove { get; set; }

        [NotMapped]
        public string FileBase64 { get; set; }

        [NotMapped]
        public string FileName { get; set; }

        [NotMapped]
        public string FileSize { get; set; }

        [NotMapped]
        public decimal JobPrice { get; set; }

        [NotMapped]
        public int JobTypeId { get; set; }

        [NotMapped]
        public string JobTypeDesc { get; set; }

        [NotMapped]
        public int JobStatus { get; set; }

        [NotMapped]
        public string JobStatusDesc { get; set; }


        [NotMapped]
        public string JobOwner { get; set; }

        [NotMapped]
        public decimal JobAreaSize { get; set; }
        
        [NotMapped]
        public string JobDescription { get; set; }

        [NotMapped]
        public string JobNo { get; set; }
    }
}