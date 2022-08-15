using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using cisApp.Function;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using cisApp.library;
using Microsoft.AspNetCore.Hosting;
using cisApp.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using cisApp.Common;

namespace cisApp.Controllers
{
    public class ResetPasswordController : BaseController
    {
        private readonly IWebHostEnvironment _HostingEnvironment;

        readonly static IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json")
                      .Build();

        public ResetPasswordController(IWebHostEnvironment environment)
        {
            _HostingEnvironment = environment;
        }
        [HttpGet]
        public IActionResult Index(string token)
        {
            try
            {
                var userResetPassword = GetUserResetPassword.Get.GetByToken(token);

                string mobileLink = config.GetSection("WebConfig:MobileLink").Value;
                string mobileLinkAndroid = config.GetSection("WebConfig:MobileLinkAndroid").Value;

                ViewData["MobileLink"] = mobileLink + "ChangePassword/" + token;
                ViewData["MobileLinkAndroid"] = mobileLinkAndroid + "ChangePassword/" + token;

                if (userResetPassword != null)
                {
                    ChangePasswordModel model = new ChangePasswordModel()
                    {
                        Token = token
                    };

                    LogActivityEvent(LogCommon.LogMode.RESETPASSWD);
                    return View(model);
                }

                LogActivityEvent(LogCommon.LogMode.RESETPASSWD, MessageCommon.TXT_OPERATE_ERROR);
                return RedirectToAction("Error", "ResetPassword");
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.RESETPASSWD, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                return RedirectToAction("Error", "ResetPassword");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ChangePasswordModel clientModel)
        {
            try
            {
                
                var user = GetUserResetPassword.Manage.ResetPassword(clientModel.Token, clientModel.Password);

                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess("เปลี่ยนรหัสผ่านสำเร็จ", Url.Action("Success", "ResetPassword")));
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                return Json(new ResponseModel().ResponseError(MessageCommon.SaveFail));
            }
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

    }
}