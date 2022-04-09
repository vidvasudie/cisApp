﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace cisApp.Core
{
    public partial class Jobs
    {
        public Guid JobId { get; set; }
        /// <summary>
        /// customer
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// รหัสงาน
        /// </summary>
        public string JobNo { get; set; }
        /// <summary>
        /// designer
        /// </summary>
        public Guid? JobCaUserId { get; set; }
        /// <summary>
        /// รหัสประเภทใบงาน
        /// </summary>
        public int JobTypeId { get; set; }
        /// <summary>
        /// ขอบเขตงาน
        /// </summary>
        public string JobDescription { get; set; }
        /// <summary>
        /// ขนาดพื้นที่ 
        /// </summary>
        public decimal JobAreaSize { get; set; }
        /// <summary>
        /// ราคารวมค่างาน
        /// </summary>
        public decimal JobPrice { get; set; }
        /// <summary>
        /// ราคา/ตรม
        /// </summary>
        public decimal JobPricePerSqM { get; set; }
        /// <summary>
        /// สถานะปัจจุบัน
        /// </summary>
        public int JobStatus { get; set; }
        public DateTime? JobBeginDate { get; set; }
        public DateTime? JobEndDate { get; set; }
        /// <summary>
        /// ราคารวมค่าบริการ
        /// </summary>
        public decimal? JobPriceProceed { get; set; }
        /// <summary>
        /// ราคารวมค่าบริการและภาษี
        /// </summary>
        public decimal? JobFinalPrice { get; set; }
        /// <summary>
        /// %ค่าดำเนินการในใบงาน
        /// </summary>
        public int? JobProceedRatio { get; set; }
        /// <summary>
        /// %VAT ในใบงาน
        /// </summary>
        public int? JobVatratio { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsInvRequired { get; set; }
        public string InvAddress { get; set; }
        public string InvPersonalId { get; set; }
        /// <summary>
        /// รหัสสาเหตุยกเลิก
        /// </summary>
        public int? CancelId { get; set; }
        /// <summary>
        /// จำนวนการขอแก้ไข
        /// </summary>
        public int? EditSubmitCount { get; set; }
        /// <summary>
        /// ชื่อในเอกสารขอใบกำกับภาษี
        /// </summary>
        public string Invname { get; set; }
        public bool IsAdvice { get; set; }
    }
}