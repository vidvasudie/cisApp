using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cisApp.library;
using cisApp.Core;
using cisApp.Function; 

namespace cisApp.Controllers
{
    [Authorized]
    public class userApproveController : BaseController
    { 
        public IActionResult Index()
        { 

            return View();
        }
        [HttpPost]
        public PartialViewResult ItemList(SearchModel model)
        {
            model.status = 1;//ค้นหาเอาเฉพาะ สถานะรอดำเนินการ = 1
            model.mode = 0;
            List<UserModel> _model = GetUserDesignerRequest.Get.GetUserDesignerRequestModel(model);
            int count = GetUserDesignerRequest.Get.GetUserDesignerRequestModelTotal(model);

            return PartialView("PT/_itemlist", new PaginatedList<UserModel>(_model, count, model.currentPage.Value, model.pageSize.Value)); 
        }

        public IActionResult Manage(SearchModel model)
        {
            List<UserModel> _model = GetUserDesignerRequest.Get.GetUserDesignerRequestModel(model);
            if (_model != null)
                return View(_model.FirstOrDefault());
            return View(new UserModel() { UserType=2 });
        }

        [HttpPost]
        public JsonResult Manage(UserModel data)
        {
            try
            {
                data.UserType = 1; 
                data.Status = 1;//1=รอดำเนินการ 
                data.CreatedBy = _UserId.Value;
                data.UpdatedBy = _UserId.Value;
                int result = GetUserDesignerRequest.Manage.AddNewRequest(data);
                if(result > 0)
                {
                    return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "userApprove")));
                }
                else
                {
                    return Json(new ResponseModel().ResponseError());
                }
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel().ResponseError());
            }
        }

        public IActionResult ManageRequest(SearchModel model)
        { 
            List<UserModel> _model = GetUserDesignerRequest.Get.GetUserDesignerRequestModel(model);
            if(_model != null)
                return View(_model.FirstOrDefault());
            return View(new UserModel()); 
        }

        [HttpPost]
        public JsonResult ManageRequest(UserModel data)
        {
            try
            {
                data.UserType = 2;
                data.CreatedBy = _UserId.Value;
                data.UpdatedBy = _UserId.Value;
                var result = GetUserDesignerRequest.Manage.UpdateRequestStatus(data);
                if(result > 0)
                {
                    return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "userApprove")));
                }
                else
                {
                    return Json(new ResponseModel().ResponseError());
                }
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel().ResponseError());
            }
        }


    }
}