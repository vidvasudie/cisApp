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

namespace cisApp.Controllers
{
    [Authorize]
    [CustomActionExecute("CA06E5CC-691E-4BE7-A8EF-3F9EE8400B1A")]
    public class UserManagementController : BaseController
    {
        private List<UserModel> _model = new List<UserModel>();
        private Guid _PermissionMenuId;
        private int _PermissionManage;
        public UserManagementController()
        {
            _PermissionMenuId = Guid.Parse("CA06E5CC-691E-4BE7-A8EF-3F9EE8400B1A");
            _PermissionManage = 2;// สิทธิ์ผู้ใช้งาน
        }

        public IActionResult Index(int pageIndex = 1)
        {
            return View(new PaginatedList<UserModel>(_model, _model.Count, pageIndex, 2));
        }

        [HttpPost]
        public PartialViewResult ItemList(SearchModel model)
        {
            List<UserModel> _model = GetUser.Get.GetUserModels(model);
            int count = GetUser.Get.GetUserModelsTotal(model);

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
            
            return View(data);
        }


        [HttpPost]
        public JsonResult Manage(UserModel data)
        {
            try
            {
                var user = GetUser.Manage.Update(data, _UserId.Value);

                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "userManagement")));
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel().ResponseError());
            }
        }

        [HttpPost]
        public JsonResult Active(Guid id, bool active)
        {
            try
            {
                var user = GetUser.Manage.Active(id, active, _UserId.Value);

                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "userManagement")));
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel().ResponseError());
            }
        }

        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            try
            {
                var user = GetUser.Manage.Delete(id, _UserId.Value);

                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "userManagement")));
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel().ResponseError());
            }
        }
    }


}
