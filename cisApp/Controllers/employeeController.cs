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
    [CustomActionExecute("1BBF28A4-A88C-4A48-B2BF-C9D59C996902")]
    public class EmployeeController : BaseController
    {
        static int _EmployeeType = 3;

        private List<UserModel> _model = new List<UserModel>();
        private Guid _PermissionMenuId;
        private int _PermissionManage;
        private readonly IWebHostEnvironment _HostingEnvironment;
        public EmployeeController(IWebHostEnvironment environment)
        {
            _PermissionMenuId = Guid.Parse("1BBF28A4-A88C-4A48-B2BF-C9D59C996902");
            _PermissionManage = 2;// สิทธิ์ผู้ใช้งาน
            _HostingEnvironment = environment;
        }

        public IActionResult Index(int pageIndex = 1)
        {
            LogActivityEvent(LogCommon.LogMode.OFFICER_MGNT);
            return View(new PaginatedList<UserModel>(_model, _model.Count, pageIndex, 2));
        }

        [HttpPost]
        public PartialViewResult ItemList(SearchModel model)
        {
            model.type = 3;
            List<UserModel> _model = GetUser.Get.GetUserModels(model);
            int count = GetUser.Get.GetUserModelsTotal(model);

            LogActivityEvent(LogCommon.LogMode.SEARCH);
            return PartialView("PT/_itemlist", new PaginatedList<UserModel>(_model, count, model.currentPage.Value, model.pageSize.Value));
        }

        public IActionResult Manage(Guid? id)
        {
            try
            {
                UserModel data = new UserModel();
                
                if (id != null)
                {
                    data = GetUser.Get.GetById(id.Value);

                    if (data.UserType != _EmployeeType) throw new Exception("Wrong UserType");
                }
                else
                {
                    data.UserType = _EmployeeType;
                }

                LogActivityEvent(LogCommon.LogMode.MANAGE);
                return View(data);
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.MANAGE, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                throw ex;
            }
            
        }


        [HttpPost]
        public JsonResult Manage(UserModel data)
        {
            try
            {
                var user = GetUser.Manage.Update(data, _UserId().Value);

                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "Employee")));
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
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "Employee")));
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
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "Employee")));
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
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.AdminSendMailPasswordSuccess, Url.Action("Index", "Employee")));
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                return Json(new ResponseModel().ResponseError(MessageCommon.AdminSendMailPasswordFail));
            }
        }
    }
}
