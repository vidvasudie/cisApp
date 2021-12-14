using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;
using cisApp.API.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cisApp.API.Controllers
{
    
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// สมัครสมาชิก
        /// </summary>
        /// <param name="value"></param>
        [Route("api/register")]
        [HttpPost]
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
                        var _result = GetUser.Manage.Update(new UserModel() { Fname = value.Fname, Lname = value.Lname, Tel = value.Tel, Email = value.Email, UserType = 1, IsActive = true }, null, value.NewPassword);
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
        [Route("api/resetpass")]
        [HttpPost]
        public void resetpass([FromBody] string value)
        {
        }

        [Route("api/validatethirdpartyuser")]
        [HttpPost]
        public IActionResult ValidateThirdPartyUser([FromBody] UserModel model)
        {
            try
            {
                if (String.IsNullOrEmpty(model.Fname) || String.IsNullOrEmpty(model.Lname) || String.IsNullOrEmpty(model.Email))
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }

                var user = GetUser.Get.GetByThirdPartyInfo(model.Fname, model.Lname, model.Email); 
                if (user == null)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", new { model.Fname, model.Lname, model.Email }));
                }
               
                return Ok(resultJson.success("สำเร็จ", "success", new LoginResult { uSID = user.UserId.Value, Fname = user.Fname, Lname = user.Lname, isDesigner = (user.UserType == 1) ? false : true }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ค้นหาข้อมูลไม่สำเร็จ", "fail", new { model.Fname, model.Lname, model.Email }));
            }
        }


    }
}
