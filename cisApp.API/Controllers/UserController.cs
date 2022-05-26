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

            try
            {
                if (string.IsNullOrEmpty(value.OTP))
                {
                    return Unauthorized(resultJson.errors("OTP ไม่ถูกต้อง", "Incorrect otp ", null));

                }
                else
                {
                    //var otpValidate = GetOtp.Get.ValidateOtp(value.Tel, value.OTP);
                    // เช็ค otp
                    if (true)
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
            catch (Exception ex)
            {
                return Unauthorized(resultJson.errors(ex.Message, "ไม่พบข้อมูล", null));
            }
        }


        /// <summary>
        /// แก้ไขรหัสผ่าน
        /// </summary>
        /// <param name="value"></param>
        [HttpPost("resetpass")]
        public object resetpass(string value)
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
        /// หลังจากได้ token
        /// </summary>
        /// <param name="value"></param>
        [HttpPost("resetpassToken")]
        public object resetpassToken([FromBody] string value)
        {
            try
            {
                var userResetPassword = GetUserResetPassword.Get.GetByToken(value);

                if (userResetPassword != null)
                {
                    return Ok(resultJson.success(null, null, new { userResetPassword.Token, userResetPassword.RefCode }, null, null, null, null));
                }

                return Unauthorized(resultJson.errors("ไม่พบข้อมูล", "ไม่พบข้อมูล", null));
            }
            catch (Exception ex)
            {
                return Unauthorized(resultJson.errors("ไม่พบข้อมูล", "ไม่พบข้อมูล", null));
            }

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
        /// แก้ไขรหัสผ่าน submit
        /// </summary>
        /// <param name="value"></param>
        [HttpPost("resetpassSubmit")]
        public object resetpassToken([FromBody]ChangePasswordModel value )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = GetUserResetPassword.Manage.ResetPassword(value.Token, value.Password);

                    return Ok(resultJson.success(null, null, new { Status = true, Message = "เปลี่ยนรหัสผ่านสำเร็จ" }, null, null, null, null));
                }

                return Unauthorized(resultJson.errors("กรุณาใส่รหัส่ผ่านได้ถูกต้อง", "กรุณาใส่รหัส่ผ่านได้ถูกต้อง", null));
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
                return Ok(resultJson.success(null, null, new { Obj.Fname, Obj.Lname, Obj.Tel, Obj.Email, Obj.LikeOtherCount, Obj.OtherLikedCount  }, null, null, null, null));
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
                return Unauthorized(resultJson.errors(ex.Message, "ไม่พบข้อมูล", null));
            }
        }

        [HttpPut("profile")]
        public object Profile([FromBody]UploadProfileModel value)
        {
            if (ModelState.IsValid)
            {
                if (Guid.Empty == value.UserId)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }
                try
                {                    
        
                    var user = GetUser.Manage.UpdateProfile(value.AttachId.Value, value.UserId.Value, value.UserId);



                    return Ok(resultJson.success("อัพโหลดไฟล์สำเร็จ", "success", new { user.UserId }));
                }
                catch (Exception ex)
                {
                    return Ok(resultJson.errors("อัพโหลดไฟล์ไม่สำเร็จ", "fail", ex));
                }
            }
            return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
        }

        [HttpPost("validatethirdpartyuser")]
        public IActionResult ValidateThirdPartyUser([FromBody] ThirdPartyModel model)
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

                GetUserClientId.Manage.Add(user.UserId.Value, model.ClientId);

                return Ok(resultJson.success("สำเร็จ", "success", new LoginResult { uSID = user.UserId.Value, Fname = user.Fname, Lname = user.Lname, isDesigner = (user.UserType == 1) ? false : true }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ค้นหาข้อมูลไม่สำเร็จ", "fail", new { model.Fname, model.Lname, model.Email }));
            }
        }

        [HttpPost("RequestUpgrade")]
        public IActionResult RequestUpgrade([FromBody] UserModel model)
        {
            try
            {
                if (model.ApiAttachFileImg.Count == 0)
                {
                    return Ok(resultJson.errors("กรุณาอัพโหลดหลักฐาน", "fail", null));
                }

                model.Status = 1;
                model.CreatedBy = model.UserId.Value;
                model.UpdatedBy = model.UserId.Value;

                var userId = model.UserId.Value;

                GetUserDesignerRequest.Manage.AddNewRequest(model);

                if (model.ApiUserImg != null)
                {
                    GetUser.Manage.UpdateProfile(model.ApiUserImg.Value, userId, userId);
                }

                return Ok(resultJson.success("สำเร็จ", "success", null));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ค้นหาข้อมูลไม่สำเร็จ", "fail", null));
            }
        }

        /// <summary>
        /// แก้ไขเบอร์โทร
        /// </summary>
        /// <param name="value"></param> 
        [HttpPost("updatetel")]
        public object EditTel(string tel, Guid id)
        {
            try
            {
                var user = GetUser.Manage.UpdateTel(tel, id);
                return Ok(resultJson.success("บันทึกข้อมูลสำเร็จ", "success", new { user.UserId }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }

        }

        [HttpPost("registerstatus")]
        public object registerstatus(Guid? userId)
        {
            try
            {
                var Obj = GetUser.Get.GetById(userId.Value);

                if (Obj == null)
                {
                    return Unauthorized(resultJson.errors("ไม่พบข้อมูล", "ไม่พบข้อมูล", null));
                }

                int registerStatus = 0;

                if (Obj.UserType == 2 || Obj.UserType == 3)
                {
                    registerStatus = 2;
                }
                else
                {
                    var lastestRequest = GetUserDesignerRequest.Get.GetLasted(userId.Value);
                    if (lastestRequest == null)
                    {
                        registerStatus = 0;
                    }
                    else
                    {                        

                        if (lastestRequest.Status == 3)
                        {
                            registerStatus = 3;
                        }
                        else
                        {
                            registerStatus = 1;
                        }
                        
                    }
                }

                

                //var userResetPassword = GetUserResetPassword.Manage.Add(Obj.UserId.Value);
                return Ok(resultJson.success("บันทึกข้อมูลสำเร็จ", "success", new { registerStatus }));
            }
            catch (Exception ex)
            {
                return Unauthorized(resultJson.errors("ไม่พบข้อมูล", "ไม่พบข้อมูล", null));
            }
        }

        [HttpGet("RegisterData")]
        public object RegisterData(Guid? userId)
        {
            try
            {
                var userRequest = GetUserDesignerRequest.Get.GetLasted(userId.Value);

                if (userRequest != null)
                {
                    if (userRequest.ProvinceId != null)
                    {
                        var province = GetTmProvince.Get.GetById(userRequest.ProvinceId.Value);
                        userRequest.provinceName = province.NameTh;
                    }

                    if (userRequest.DistrictId != null)
                    {
                        var district = GetTmDistrict.Get.GetById(userRequest.DistrictId.Value);
                        userRequest.districtName = district.NameTh;
                    }

                    if (userRequest.SubDistrictId != null)
                    {
                        var subDistrict = GetTmSubDistrict.Get.GetById(userRequest.SubDistrictId.Value);
                        userRequest.subDistrictName = subDistrict.NameTh;
                    }

                }

                //var userResetPassword = GetUserResetPassword.Manage.Add(Obj.UserId.Value);
                return Ok(resultJson.success("บันทึกข้อมูลสำเร็จ", "success", userRequest));
            }
            catch (Exception ex)
            {
                return Unauthorized(resultJson.errors("ไม่พบข้อมูล", "ไม่พบข้อมูล", null));
            }
        }

    }
}
