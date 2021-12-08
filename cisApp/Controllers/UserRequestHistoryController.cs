using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;
using cisApp.library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cisApp.Controllers
{ 
    [CustomActionExecute("C95C682B-5FDC-4D45-8808-8151BE5A6EDF")]
    public class UserRequestHistoryController : BaseController
    {
        private Guid _PermissionMenuId;
        private int _PermissionManage;
        public UserRequestHistoryController()
        {
            _PermissionMenuId = Guid.Parse("C95C682B-5FDC-4D45-8808-8151BE5A6EDF");
            _PermissionManage = 2;// สิทธิ์ผู้ใช้งาน
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult ItemList(SearchModel model)
        { 
            model.mode = 1;//ค้นหาเอาเฉพาะ สถานะ != 1
            List<UserModel> _model = GetUserDesignerRequest.Get.GetUserDesignerRequestModel(model);
            int count = GetUserDesignerRequest.Get.GetUserDesignerRequestModelTotal(model);

            return PartialView("PT/_itemlist", new PaginatedList<UserModel>(_model, count, model.currentPage.Value, model.pageSize.Value));
        }

        public IActionResult Manage(SearchModel model)
        {
            if (!String.IsNullOrEmpty(model.goBack))
            {
                ViewBag.goBack = model.goBack;
                ViewBag.TitleList = "รายการคำขอสมัครนักออกแบบ";
            }
            else
            {
                ViewBag.goBack = Url.Action("Index", "UserRequestHistory");
                ViewBag.TitleList = "รายการประวัติคำขอสมัครนักออกแบบ";
            }
             
            List<UserModel> _model = GetUserDesignerRequest.Get.GetUserDesignerRequestModel(model); //ใช้ code ในการหารายการข้อมูล
            if (_model != null && _model.Count > 0)
            {
                var data = _model.FirstOrDefault();
                //get data image profile
                // get payment_img id
                var userImg = GetUser.Get.GetUserImgs(data.UserId.Value);

                if (userImg.Count > 0)
                {
                    data.AttachFileImage = GetAttachFile.Get.GetByRefId(userImg.FirstOrDefault().UserImgId.Value);
                }
                //get upgrade file attach
                var fatchs = GetUserDesignerRequest.Get.GetUserDesignerRequestFiles(data.Id);
                data.files = fatchs;

                return View(data);
            }
            return BadRequest();
            //return View(new UserModel());
        }

    }
}