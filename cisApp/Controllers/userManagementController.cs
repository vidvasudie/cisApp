using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;
using cisApp.Models;
using cisApp.library;
using cisApp.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using cisApp.Common;

namespace cisApp.Controllers
{
    [Authorize]
    [CustomActionExecute("CA06E5CC-691E-4BE7-A8EF-3F9EE8400B1A")]
    public class UserManagementController : BaseController
    {
        private List<UserModel> _model = new List<UserModel>();
        private Guid _PermissionMenuId;
        private int _PermissionManage;
        private readonly IWebHostEnvironment _HostingEnvironment;
        public UserManagementController(IWebHostEnvironment environment)
        {
            _PermissionMenuId = Guid.Parse("CA06E5CC-691E-4BE7-A8EF-3F9EE8400B1A");
            _PermissionManage = 2;// สิทธิ์ผู้ใช้งาน
            _HostingEnvironment = environment;
        }

        public IActionResult Index(int pageIndex = 1)
        {
            LogActivityEvent(LogCommon.LogMode.USER_MGNT);
            return View(new PaginatedList<UserModel>(_model, _model.Count, pageIndex, 2));
            
        }

        public IActionResult IndexV1(int pageIndex = 1)
        {
            //return View(new PaginatedList<UserModel>(_model, _model.Count, pageIndex, 2));
            return View("V1/Index");
        }

        [HttpPost]
        public PartialViewResult ItemList(SearchModel model)
        {
            List<UserModel> _model = GetUser.Get.GetUserModels(model);
            int count = GetUser.Get.GetUserModelsTotal(model);

            LogActivityEvent(LogCommon.LogMode.SEARCH);
            return PartialView("PT/_itemlist", new PaginatedList<UserModel>(_model, count, model.currentPage.Value, model.pageSize.Value));
        }

        public IActionResult Manage(Guid? id, int type = 1)
        {
            UserModel data = new UserModel();
            if (id != null) {
                data = GetUser.Get.GetById(id.Value);
            }
            else
            {
                data.UserType = type;
            }

            LogActivityEvent(LogCommon.LogMode.MANAGE);
            return View(data);
        }


        [HttpPost]
        public JsonResult Manage(UserModel data)
        {
            try
            {
                var user = GetUser.Manage.Update(data, _UserId().Value);

                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "userManagement")));
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                return Json(new ResponseModel().ResponseError());
            }
        }

        [HttpPost]
        public JsonResult Active(Guid id, bool active)
        {
            try
            {
                var user = GetUser.Manage.Active(id, active, _UserId().Value);

                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "userManagement")));
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                return Json(new ResponseModel().ResponseError());
            }
        }

        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            try
            {
                var user = GetUser.Manage.Delete(id, _UserId().Value);

                LogActivityEvent(LogCommon.LogMode.DELETE, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "userManagement")));
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.DELETE, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                return Json(new ResponseModel().ResponseError());
            }
        }

        [HttpPost]
        public JsonResult ResetPassword(Guid id)
        {
            try
            {
                var user = GetUser.Get.GetById(id);

                string newPassword = Utility.RandomPassword();

                var result = GetUser.Manage.ResetPassWord(id, Encryption.Encrypt(newPassword));

                var sendMailResult = SendMail.SendMailResetPassword(user.Email, user.Fname + " " + user.Lname, newPassword
                    , _HostingEnvironment.WebRootPath, GetSystemSetting.Get.GetEmailSettingModel());

                if (sendMailResult == false)
                {
                    LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.AdminSendMailPasswordFail);
                    return Json(new ResponseModel().ResponseError(MessageCommon.AdminSendMailPasswordFail));
                }

                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.AdminSendMailPasswordSuccess, Url.Action("Index", "userManagement")));
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.AdminSendMailPasswordFail, ex.ToString());
                return Json(new ResponseModel().ResponseError(MessageCommon.AdminSendMailPasswordFail));
            }
        }

        [HttpPost]
        public JsonResult Downgrade(Guid id)
        {
            try
            {
                var user = GetUser.Manage.Downgrade(id, _UserId().Value);

                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess("ลดขั้นผู้ใข้งานสำเร็จ", Url.Action("Index", "userManagement")));
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.AdminSendMailPasswordFail, ex.ToString());
                return Json(new ResponseModel().ResponseError());
            }
        }
    }


}
