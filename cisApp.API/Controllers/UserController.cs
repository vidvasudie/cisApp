﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;
using cisApp.API.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cisApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
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
        [HttpPost("resetpass")]
        public object resetpass([FromBody] string value)
        {
            try
            {                
                var Obj = GetUser.Get.GetByEmail(value);

                if (Obj == null)
                {
                    return Unauthorized(resultJson.errors("ไม่พบข้อมูล", "ไม่พบข้อมูล", null));
                }

                var userResetPassword = GetUserResetPassword.Manage.Add(Obj.UserId.Value);
                return Ok(resultJson.success(null, null, new { Status = true, Message = "ระบบได้ทำการส่งลิงก์รีเซ็ตรหัสผ่านไปยังอีเมลของท่านแล้ว" }, null, null, null, null));
            }
            catch (Exception ex)
            {
                return Unauthorized(resultJson.errors("ไม่พบข้อมูล", "ไม่พบข้อมูล", null));
            }
        }

        /// <summary>
        /// ดึงข้อมูล Profile
        /// </summary>
        /// <param name="value"></param>
        [HttpGet]
        public object Get(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return Unauthorized(resultJson.errors("ไม่พบข้อมูล", "ไม่พบข้อมูล", null));
                }
                var Obj = GetUser.Get.GetById(id.Value);
                return Ok(resultJson.success(null, null, new { Obj.Fname, Obj.Lname, Obj.Tel, Obj.Email  }, null, null, null, null));
            }
            catch (Exception ex)
            {
                return Unauthorized(resultJson.errors("ไม่พบข้อมูล", "ไม่พบข้อมูล", null));
            }
        }

        [HttpPut]
        public object Put([FromBody]UserModelCommon value)
        {
            try
            {                
                var Obj = GetUser.Get.GetById(value.Id.Value);
                Obj.Fname = value.Fname;
                Obj.Lname = value.Lname;
                Obj.Tel = value.Tel;
                Obj.Email = value.Email;

                var Result = GetUser.Manage.Update(Obj, value.Id.Value);


                return Ok(resultJson.success(null, null, new { Result.Fname, Result.Lname, Result.Tel, Result.Email }, null, null, null, null));
            }
            catch (Exception ex)
            {
                return Unauthorized(resultJson.errors("ไม่พบข้อมูล", "ไม่พบข้อมูล", null));
            }
        }

        [HttpPut("profile")]
        public object Profile([FromBody]UploadAPIModel value)
        {
            if (ModelState.IsValid)
            {
                if (Guid.Empty == value.UserId)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }
                try
                {
                    var athFile = GetAttachFile.Manage.UploadFile(value.FileBase64, value.FileName, value.Size, null, value.UserId);
                    if (athFile == null)
                    {
                        return Ok(resultJson.errors("อัพโหลดไฟล์ไม่สำเร็จ", "fail", null));
                    }

                    var user = GetUser.Manage.UpdateProfile(athFile, value.UserId, value.UserId);

                    string Host = _config.GetSection("WebConfig:AdminWebStie").Value;
                    bool removeLast = Host.Last() == '/';
                    string UrlPath = athFile.UrlPath;
                    if (removeLast)
                    {
                        UrlPath = UrlPath.Remove(UrlPath.Length - 1);
                    }
                    UrlPath = UrlPath.Replace("~", Host);

                    return Ok(resultJson.success("อัพโหลดไฟล์สำเร็จ", "success", new { athFile.AttachFileId, athFile.FileName, UrlPath }));
                }
                catch (Exception ex)
                {
                    return Ok(resultJson.errors("อัพโหลดไฟล์ไม่สำเร็จ", "fail", ex));
                }
            }
            return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
        }

    }
}
