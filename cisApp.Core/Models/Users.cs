﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cisApp.Core
{
    public partial class Users
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
        public int? UserType { get; set; } = 1;

        [Display(Name = "เบอร์โทรศัพท์")]
        [Required(ErrorMessage = "กรุณากรอกเบอร์โทรศัพท์")]
        public string Tel { get; set; }

        [Display(Name = "อีเมล")]
        [Required(ErrorMessage = "กรุณากรอกอีเมล")]
        public string Email { get; set; }

        public Guid? RoleId { get; set; }
        public Guid? PasswordId { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}