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
                var user = GetUser.Manage.Update(data, _UserId.Value);
                data.UserId = user.UserId;
                string code = Utility.GenerateRequestCode(GetUserDesignerRequest.Get.GetLastNumber());
                data.Code = code;
                data.Status = 1;//1=รอดำเนินการ
                var ureq = GetUserDesignerRequest.Manage.Update(data);

                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "userApprove")));
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel().ResponseError());
            }
        }

        public IActionResult ManageRequest()
        {
            return View();
        }

        
    }
}