using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;
using cisApp.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cisApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtpController : ControllerBase
    {
     
        [HttpPost]
        public object Post([FromBody]OtpModel value)
        {
            if(value.ActivityID == null || string.IsNullOrEmpty( value.mobile))
            {
                return NotFound(resultJson.errors("ข้อมูลไม่ถูกต้อง", "Error" ,null)); 
            }
            else
            {
                return Ok(resultJson.success(null, null, null, null)); 
            } 
        }

        [HttpPost("RequestOtp")]
        public IActionResult RequestOtp(Guid? userId, string sendTo, int? otpRef)
        {
            try
            {
                var otp = GetOtp.Manage.Add(userId, "sms", sendTo, otpRef);

                return Ok(resultJson.success("บันทึกข้อมูลสำเร็จ", "success", new
                {
                    OtpId = otp.OtpId
                }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [HttpPost("ValidateOtp")]
        public IActionResult ValidateOtp(string sendTo, string otpMessage)
        {
            try
            {
                var result = GetOtp.Get.ValidateOtp(sendTo, otpMessage);

                if (result == true)
                {
                    return Ok(resultJson.success("บันทึกข้อมูลสำเร็จ", "success", new { result = true }));
                }
                return Ok(resultJson.success("รหัส otp ไม่ถูกต้อง", "fail", new
                {
                    result = false
                }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

    }
}
