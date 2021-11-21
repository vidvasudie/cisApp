using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace cisApp.Function
{
    public class RoleModel
    {
        public Guid RoleId { get; set; }
        [Display(Name = "ชื่อกลุ่มสิทธิ์")]
        public string RoleName { get; set; }
        [Display(Name = "สถานะ")]
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid RoleMenuId { get; set; } 
        public Guid MenuId { get; set; }
        public int Type { get; set; }
        public string TypeDesc { get; set; }
        public string MenuName { get; set; }


    }
}
