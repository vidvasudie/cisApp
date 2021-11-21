using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using cisApp.Function;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace cisApp.Controllers
{
    public class LoginController : BaseController
    {
        public IActionResult Index()
        {
            return View(new LoginModel() { username= "admin@gmail.com", password="12345" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Login(LoginModel obj)
        {
            try
            {
                if(obj.username == "admin@gmail.com" && obj.password == "12345")
                {
                    return LoginTest(new UserModel() { UserId= Guid.Parse("F71758CF-3F30-432B-8361-5754CDE8BF26"), Email= "admin@gmail.com", Fname= "ทดสอบ", Lname= "ระบบ", UserType=3 });
                }
                if (string.IsNullOrEmpty(obj.username) || string.IsNullOrEmpty(obj.password))
                {
                    return Json(new ResponseModel().ResponseError(MessageCommon.IncorrectData)); 
                }
                else
                { 
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
                            return Json(new ResponseModel().ResponseSuccess(MessageCommon.LoginSuccess, Url.Action("Index", "Home")));
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
            Guid userCode = Guid.Empty;
            string userName = "";
            if (_UserId.HasValue)
                userCode = _UserId.Value;
            if (!string.IsNullOrEmpty(_UserName))
                userName = _UserName;
            HttpContext.SignOutAsync();
           
            return RedirectToAction("Index", "Login");
        }

    }
}