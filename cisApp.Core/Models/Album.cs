﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace cisApp.Core
{
    public partial class Album
    {
        public int? AlbumId { get; set; }
        public Guid JobId { get; set; }
        public Guid? UserId { get; set; }
        public string Category { get; set; }
        public string Tags { get; set; }
        public string AlbumName { get; set; }
        public string Url { get; set; }
        /// <summary>
        /// แบ่งเป็นเป็น 1=ประกวด,2=ส่งงานงวดที่1,3=ส่งงานงวดที่2,4=ส่งงานงวดที่3
        /// </summary>
        public string AlbumType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid? DeletedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }

    }
}