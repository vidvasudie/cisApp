﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace cisApp.Core
{
    public partial class Users
    {
        public Guid UserId { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        /// <summary>
        /// 1 = ผู้ใข้งาน,
        /// 2 = นักออกแบบ
        /// ,3 = เจ้าหน้าที่
        /// </summary>
        public int? UserType { get; set; }
    }
}