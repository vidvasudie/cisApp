﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace cisApp.Core
{
    public partial class UserFavoriteDesigner
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        /// <summary>
        /// รหัสผู้ใข้งานของนักออกแบบ
        /// </summary>
        public Guid? UserDesignerId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}