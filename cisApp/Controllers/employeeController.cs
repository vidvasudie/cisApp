using cisApp.Function;
using cisApp.library;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.Controllers
{ 
    public class EmployeeController : BaseController
    {
        static int _EmployeeType = 3;

        private List<UserModel> _model = new List<UserModel>();
        public IActionResult Index(int pageIndex = 1)
        {
            return View(new PaginatedList<UserModel>(_model, _model.Count, pageIndex, 2));
        }

        [HttpPost]
        public PartialViewResult ItemList(SearchModel model)
        {
            model.userType = 3;
            List<UserModel> _model = GetUser.Get.GetUserModels(model);
            int count = GetUser.Get.GetUserModelsTotal(model);

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

                return View(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }


        [HttpPost]
        public JsonResult Manage(UserModel data)
        {
            try
            {
                var user = GetUser.Manage.Update(data, _UserId.Value);

                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "Employee")));
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

                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "Employee")));
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

                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "Employee")));
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel().ResponseError());
            }
        }
    }
}
