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
    public class userApproveController : Controller
    { 
        public IActionResult Index()
        { 

            return View();
        }
        [HttpPost]
        public PartialViewResult ItemList(SearchModel model)
        {
            model.status = 1;//ค้นหาเอาเฉพาะ สถานะรอดำเนินการ = 1
            List<UserModel> _model = GetUserDesignerRequest.Get.GetUserDesignerRequestModel(model);
            int count = GetUserDesignerRequest.Get.GetUserDesignerRequestModelTotal(model);

            return PartialView("PT/_itemlist", new PaginatedList<UserModel>(_model, count, model.currentPage.Value, model.pageSize.Value)); 
        }

        public IActionResult Manage()
        {
            return View(new UserModel() { UserType=2 });
        }

        [HttpPost]
        public JsonResult Manage(UserModel data)
        {
            try
            {
                data.UserType = 1;
                //var user = GetUser.Manage.Update(data);
                //data.UserId = user.UserId;
                //string code = Utility.GenerateRequestCode(GetUserDesignerRequest.Get.GetLastNumber()+1);
                //data.Code = code;
                data.Status = 1;//1=รอดำเนินการ
                //var ureq = GetUserDesignerRequest.Manage.Update(data);
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
                //var user = GetUser.Manage.Update(data); 
                //var ureq = GetUserDesignerRequest.Manage.Active(data);
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