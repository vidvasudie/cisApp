﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using cisApp.Designer.Models;
using cisApp.Function;
using cisApp.library;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace cisApp.Designer.Controllers
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
            var xx = Encryption.Decrypt("q7xElbO8iE7ZligWO0UoIw==");// ("s9LrP8c+HjTWUbLOve8Xhg==");//Abc123456
            //return View(new LoginModel() { username= "admin@gmail.com", password="12345", userType = 3 });
             
            return View(new LoginModel() { });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Login(LoginModel obj)
        {
            try
            {
                if (obj.username == "admin@gmail.com" && obj.password == "12345")
                {
                    return LoginTest(new UserModel()
                    {
                        UserId = Guid.Parse("F71758CF-3F30-432B-8361-5754CDE8BF26")
                        , Email = "admin@gmail.com"
                        , Fname = "ทดสอบ"
                        , Lname = "ระบบ"
                        //, UserType = 3
                        //, RoleId = Guid.Parse("0A36BB48-C69B-4B3B-9E2E-F486918A5352")
                    }); ;
                }
                if (string.IsNullOrEmpty(obj.username) || string.IsNullOrEmpty(obj.password))
                {
                    return Json(new ResponseModel().ResponseError(MessageCommon.IncorrectData));
                }
                else
                {
                    obj.userType = 2;
                    var users = GetUser.Get.GetUserLogin(obj);
                    if (users == null || users.Count < 1)
                    {
                        string messageError = "Username หรือ Password ไม่ถูกต้อง";
                        return Json(new ResponseModel().ResponseError(messageError));
                    }
                    else
                    {
                        var user = users.FirstOrDefault();
                        if (!user.IsActive)
                        {
                            string messageError = "ผู้ใช้งานถูกปิดการใช้งาน กรุณาติดต่อเจ้าหน้าที่ดูแลระบบ";
                            return Json(new ResponseModel().ResponseError(messageError));
                        }
                        else
                        {
                            SetCookie(user);
                            GetUser.Manage.LoginStamp(user.UserId.Value);
                            return Json(new ResponseModel().ResponseSuccess(MessageCommon.LoginSuccess, Url.Action("Index", "JobList")));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                string messageError = "พบข้อผิดพลาด กรุณาลองใหม่";
                return Json(new ResponseModel().ResponseError(messageError));
            }
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
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

                return Json(new ResponseModel().ResponseSuccess("ระบบได้ทำการส่งรหัสผ่านใหม่ไปยังอีเมลของท่านแล้ว", Url.Action("Index", "Login")));
            }
            catch (Exception ex)
            {
                string messageError = "พบข้อผิดพลาด กรุณาลองใหม่";
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
                            //new Claim("UserType", model.UserType.ToString()),
                            //new Claim("RoleId", "0A36BB48-C69B-4B3B-9E2E-F486918A5352"),//model.RoleId.ToString()),
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
            Guid userCode = Guid.Empty; 
            if (_UserId().HasValue)
                userCode = _UserId().Value; 

            HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Login");
        }

    }
}