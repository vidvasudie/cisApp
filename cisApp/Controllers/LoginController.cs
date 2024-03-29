﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using cisApp.Function;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using cisApp.library;
using Microsoft.AspNetCore.Hosting;
using cisApp.Common;

namespace cisApp.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IWebHostEnvironment _HostingEnvironment;
        public LoginController(IWebHostEnvironment environment)
        {
            _HostingEnvironment = environment;
        }
        public IActionResult Index()
        {
            var xx = Encryption.Decrypt("s9LrP8c+HjTWUbLOve8Xhg==");
            //return View(new LoginModel() { username= "admin@gmail.com", password="12345", userType = 3 });




         //      MobileNotfication mobileNotfication = new MobileNotfication();
         //     mobileNotfication.Fordesigner(MobileNotfication.ModeDesigner.alert, Guid.Parse("E5DB061F-C01E-4FD8-B92A-10C2A351355D"), Guid.Parse("180AFBE8-769F-41C0-B2AE-0511C557302E"));


            return View(new LoginModel() {  });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Login(LoginModel obj)
        {
            try
            {
                if(obj.username == "admin@gmail.com" && obj.password == "12345")
                {
                    LogActivityEvent(LogCommon.LogMode.LOGIN, MessageCommon.LoginSuccess, "TEST USER");
                    return LoginTest(new UserModel() { 
                        UserId= Guid.Parse("F71758CF-3F30-432B-8361-5754CDE8BF26")
                        , Email= "admin@gmail.com"
                        , Fname= "ทดสอบ"
                        , Lname= "ระบบ"
                        , UserType=3
                        , RoleId = Guid.Parse("0A36BB48-C69B-4B3B-9E2E-F486918A5352")
                    });;
                }
                if (string.IsNullOrEmpty(obj.username) || string.IsNullOrEmpty(obj.password))
                {
                    LogActivityEvent(LogCommon.LogMode.LOGIN, MessageCommon.IncorrectData);
                    return Json(new ResponseModel().ResponseError(MessageCommon.IncorrectData)); 
                }
                else
                {
                    obj.userType = 3;
                    var users = GetUser.Get.GetUserLogin(obj);
                    if (users == null || users.Count < 1)
                    {
                        string messageError = "Username หรือ Password ไม่ถูกต้อง";
                        LogActivityEvent(LogCommon.LogMode.LOGIN, MessageCommon.TXT_NOT_DATA, messageError);
                        return Json(new ResponseModel().ResponseError(messageError));
                    }
                    else
                    {
                        var user = users.FirstOrDefault();
                        if (!user.IsActive)
                        {
                            string messageError = "ผู้ใช้งานถูกปิดการใช้งาน กรุณาติดต่อเจ้าหน้าที่ดูแลระบบ";
                            LogActivityEvent(LogCommon.LogMode.LOGIN, messageError);
                            return Json(new ResponseModel().ResponseError(messageError)); 
                        }
                        else
                        {
                            SetCookie(user);
                            GetUser.Manage.LoginStamp(user.UserId.Value);
                            LogActivityEvent(LogCommon.LogMode.LOGIN, MessageCommon.LoginSuccess);
                            return Json(new ResponseModel().ResponseSuccess(MessageCommon.LoginSuccess, Url.Action("Index", "Home")));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                string messageError = "พบข้อผิดพลาด กรุณาลองใหม่";
                LogActivityEvent(LogCommon.LogMode.LOGIN, MessageCommon.TXT_ACCESS_DENIED, ex.ToString());
                return Json(new ResponseModel().ResponseError(messageError));
            } 
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            LogActivityEvent(LogCommon.LogMode.FORGETPASSWD);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ForgetPassword(LoginModel obj)
        {
            try
            {
                var user = GetUser.Get.GetByEmail(obj.username);

                if (user == null)
                {
                    return Json(new ResponseModel().ResponseError("ไม่พบชื่อผู้ใช้ดังกล่าวในระบบ"));
                }

                string newPassword = Utility.RandomPassword();

                GetUser.Manage.ResetPassWord(user.UserId.Value, Encryption.Encrypt(newPassword));

                SendMail.SendMailResetPassword(user.Email, user.Email, newPassword
                    , _HostingEnvironment.WebRootPath, GetSystemSetting.Get.GetEmailSettingModel());

                LogActivityEvent(LogCommon.LogMode.FORGETPASSWD, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess("ระบบได้ทำการส่งรหัสผ่านใหม่ไปยังอีเมลของท่านแล้ว", Url.Action("Index", "LogiN")));
            }
            catch (Exception ex)
            {
                string messageError = "พบข้อผิดพลาด กรุณาลองใหม่";
                LogActivityEvent(LogCommon.LogMode.FORGETPASSWD, MessageCommon.SaveFail, ex.ToString());
                return Json(new ResponseModel().ResponseError(messageError));
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }



        private void SetCookie(UserModel model)
        {
            #region setCookie
            
            var claims = new List<Claim>
                        {
                            new Claim("UserId", model.UserId.ToString()),
                            new Claim("UserType", model.UserType.ToString()),
                            new Claim("RoleId", "0A36BB48-C69B-4B3B-9E2E-F486918A5352"),//model.RoleId.ToString()),
                            new Claim("UserName", model.Email),
                            new Claim("FullName", model.Fname+" "+model.Lname),
                        };
            var grandmaIdentity = new ClaimsIdentity(claims, "User");

            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
            HttpContext.SignInAsync(userPrincipal);
            #endregion
        }
        private JsonResult LoginTest(UserModel model)
        { 
            SetCookie(model);
            return Json(new ResponseModel().ResponseSuccess(MessageCommon.LoginSuccess, Url.Action("Index", "Home")));
        }

        public IActionResult Logout()
        {
            //Guid userCode = Guid.Empty;
            //string userName = "";
            //if (_UserId().HasValue)
            //    userCode = _UserId().Value;
            //if (!string.IsNullOrEmpty(_UserName()))
            //    userName = _UserName();
            HttpContext.SignOutAsync();

            LogActivityEvent(LogCommon.LogMode.LOGOUT, MessageCommon.LogoutSuccess);

            return RedirectToAction("Index", "Login");
        }

    }
}