﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace cisApp.Core
{
    public partial class PostComment
    {
        public Guid? PostCommentId { get; set; }
        public Guid RefId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public Guid UpdatedBy { get; set; }
        public string Comment { get; set; }
        public Guid? ImgId { get; set; }
    }
}