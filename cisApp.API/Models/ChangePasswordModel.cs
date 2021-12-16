using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.API.Models
{
    public class ChangePasswordModel
    {
        [MinLength(8, ErrorMessage = "มีขนาดไม่น้อยกว่า 8 ตัว")]
        [MaxLength(15, ErrorMessage = "มีขนาดไม่เกิน 15 ตัว")]
        [RegularExpression(@"^[A-Za-z0-9!@#$%^&*()_+=:;?><{}]{8,15}$", ErrorMessage = "กรุณากรอกข้อมูลให้ถูกต้อง(ข้อมูลที่กรอกได้ ตัวเลข ตัวอักษรภาษาอังกฤษ เครื่องหมายหรืออักขระพิเศษ) ความยาว 8-15 ตัวอักษร")]
        public string Password { get; set; }

        [MinLength(8, ErrorMessage = "มีขนาดไม่น้อยกว่า 8 ตัว")]
        [MaxLength(15, ErrorMessage = "มีขนาดไม่เกิน 15 ตัว")]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
        [RegularExpression(@"^[A-Za-z0-9!@#$%^&*()_+=:;?><{}]{8,15}$", ErrorMessage = "กรุณากรอกข้อมูลให้ถูกต้อง(ข้อมูลที่กรอกได้ ตัวเลข ตัวอักษรภาษาอังกฤษ เครื่องหมายหรืออักขระพิเศษ) ความยาว 8-15 ตัวอักษร")]
        public string PasswordConfirm { get; set; }

        public string Token { get; set; }
        public string RefCode { get; set; }
    }
}
