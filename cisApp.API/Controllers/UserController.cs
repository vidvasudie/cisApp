using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cisApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// สมัครสมาชิก
        /// </summary>
        /// <param name="value"></param>
        [HttpPost("register")]
        public object register([FromBody] UserModelCommon value)
        {

            if (string.IsNullOrEmpty(value.OTP))
            {
                return Unauthorized(resultJson.errors("OTP ไม่ถูกต้อง", "Incorrect otp ", null));

            }
            else
            {
                // เช็ค otp
                if (value.OTP == "123456")
                { 
                    if ((value.NewPassword != value.ConfirmPassword) && value.NewPassword.Length < 8)
                    {
                        return Unauthorized(resultJson.errors("รหัสผ่านไม่ถูกต้อง", "Incorrect password", null));
                    }
                    else
                    {
                        var _result = GetUser.Manage.Update(new UserModel() { Fname = value.Fname, Lname = value.Lname, Tel = value.Tel, Email = value.Tel, UserType = 1, IsActive = true }, null, value.NewPassword);
                        return Ok(resultJson.success("บันทึกข้อมูลสำเร็จ", "success", new { ActivityID = _result.UserId, time = DateTime.Now, expire = DateTime.Now.AddDays(1) }));
                    }
                }
                else
                { 
                    return Unauthorized(resultJson.errors("OTP ไม่ถูกต้อง", "Incorrect otp ", null));

                }
            } 
        }


        /// <summary>
        /// แก้ไขรหัสผ่าน
        /// </summary>
        /// <param name="value"></param>
        [HttpPost("resetpass")]
        public void resetpass([FromBody] string value)
        {
        }




    }
}
