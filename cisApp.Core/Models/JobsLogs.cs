﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace cisApp.Core
{
    public partial class JobsLogs
    {
        public int JoblogId { get; set; }
        public int? JobId { get; set; }
        /// <summary>
        /// ข้อความอธิบายสถานะ
        /// </summary>
        public string Desctiption { get; set; }
        public string Ipaddress { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}