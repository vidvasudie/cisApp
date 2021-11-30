using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace cisApp.Function
{
 public   class UserModelCommon
    {
        [Display(Name = "ชื่อ")]
        [Required(ErrorMessage = "กรุณากรอกชื่อ")]
        public string Fname { get; set; }

        [Display(Name = "นามสกุล")]
        [Required(ErrorMessage = "กรุณากรอกนามสกุล")]
        public string Lname { get; set; }

        [Display(Name = "อีเมล")]
        [Required(ErrorMessage = "กรุณากรอกอีเมล")]
        public string Email { get; set; }


        [Display(Name = "เบอร์โทรศัพท์")]
        [Required(ErrorMessage = "กรุณากรอกเบอร์โทรศัพท์")]
        public string Tel { get; set; }



        public string NewPassword { get; set; } 
        public string ConfirmPassword { get; set; }


        public string OTP { get; set; }

    }
}
