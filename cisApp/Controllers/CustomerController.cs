using cisApp.Common;
using cisApp.Function;
using cisApp.library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.Controllers
{
    [Authorize]
    [CustomActionExecute("DC03CFBD-60DA-424E-9D6D-0406C3D8B7F4")]
    public class CustomerController : BaseController
    {
        private List<UserModel> _model = new List<UserModel>();
        private Guid _PermissionMenuId;
        private int _PermissionManage;
        private readonly IWebHostEnvironment _HostingEnvironment;
        public CustomerController(IWebHostEnvironment environment)
        {
            _PermissionMenuId = Guid.Parse("DC03CFBD-60DA-424E-9D6D-0406C3D8B7F4");
            _PermissionManage = 2;// สิทธิ์ผู้ใช้งาน
            _HostingEnvironment = environment;
        }

        public IActionResult Index(int pageIndex = 1)
        {
            LogActivityEvent(LogCommon.LogMode.CUSTOMER);
            return View(new PaginatedList<UserModel>(_model, _model.Count, pageIndex, 2));
        }

        [HttpPost]
        public PartialViewResult ItemList(SearchModel model)
        {
            model.type = 1;
            List<UserModel> _model = GetUser.Get.GetUserModels(model);
            int count = GetUser.Get.GetUserModelsTotal(model);

            LogActivityEvent(LogCommon.LogMode.SEARCH);
            return PartialView("PT/_itemlist", new PaginatedList<UserModel>(_model, count, model.currentPage.Value, model.pageSize.Value));
        }

        [HttpPost]
        public JsonResult Active(Guid id, bool active)
        {
            try
            {
                var user = GetUser.Manage.Active(id, active, _UserId().Value);

                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "Customer")));
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
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "Customer")));
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

                var sendMailResult = SendMail.SendMailResetPassword(user.Email, user.Fname + " " + user.Lname, newPassword, _HostingEnvironment.WebRootPath);

                if (sendMailResult == false)
                {
                    LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.SaveFail);
                    return Json(new ResponseModel().ResponseError(MessageCommon.AdminSendMailPasswordFail));
                }
                    

                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.AdminSendMailPasswordSuccess, Url.Action("Index", "Customer")));
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                return Json(new ResponseModel().ResponseError(MessageCommon.AdminSendMailPasswordFail));
            }
        }
    }
}
