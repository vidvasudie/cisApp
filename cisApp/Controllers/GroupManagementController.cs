using cisApp.Core;
using cisApp.Function;
using cisApp.library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.Controllers
{
    
    [CustomActionExecute("6A966CD8-5F38-4D67-92D2-8D8D65C35B5A")]
    public class GroupManagementController : BaseController
    {
        private Guid _PermissionMenuId;
        private int _PermissionManage;
        public GroupManagementController()
        {
            _PermissionMenuId = Guid.Parse("6A966CD8-5F38-4D67-92D2-8D8D65C35B5A");
            _PermissionManage = 2;// สิทธิ์ผู้ใช้งาน
        }
        public IActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public PartialViewResult ItemList(SearchModel model)
        { 
            List<Role> _model = GetRole.Get.GetRole(model);
            int count = GetRole.Get.GetRoleTotal(model);

            return PartialView("PT/_itemlist", new PaginatedList<Role>(_model, count, model.currentPage.Value, model.pageSize.Value));
        }

        public IActionResult Manage(SearchModel model)
        {
            if (model.gId == null || model.gId == Guid.Empty)
            {
                return View(new Role() { IsActive = true });
            }
            Role _model = GetRole.Get.GetById(model.gId.Value);
            if (_model != null)
                return View(_model);
            return View(new Role() { IsActive = true });
        }

        [HttpPost]
        public IActionResult Manage(Role data, List<RoleManageModel> menulist)
        {
            try
            {

                data.CreatedBy = _UserId().Value;
                data.UpdatedBy = _UserId().Value;
                GetRole.Manage.Update(data, menulist);

                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "GroupManagement")));
                 
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
                GetRole.Manage.Active(id, active, _UserId().Value);

                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "GroupManagement")));
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
                GetRole.Manage.Delete(id, _UserId().Value);

                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "GroupManagement")));
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel().ResponseError());
            }
        }

    }
}
