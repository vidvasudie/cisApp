using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cisApp.library;
using cisApp.Core;
using cisApp.Function;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace cisApp.Controllers
{ 
    [CustomActionExecute("33A0E193-2812-4783-9A7C-480762AC5A54")]
    public class UserApproveController : BaseController
    { 
        private Guid _PermissionMenuId;
        private int _PermissionManage;
        public UserApproveController()
        { 
            _PermissionMenuId = Guid.Parse("33A0E193-2812-4783-9A7C-480762AC5A54"); 
            _PermissionManage = 2;// สิทธิ์ผู้ใช้งาน
        }

        readonly static IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json")
                      .Build();

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
            List<UserModel> _model = GetUserDesignerRequest.Get.GetUserDesignerRequestModel(model); //ใช้ code ในการหารายการข้อมูล
            if (_model != null && _model.Count > 0)
            {
                var data = _model.FirstOrDefault();
                //get data image profile 
                var userImg = GetUser.Get.GetUserImgs(data.UserId.Value);

                if (userImg.Count > 0)
                {
                    data.AttachFileImage = GetAttachFile.Get.GetByRefId(userImg.FirstOrDefault().UserImgId.Value);
                }
                //get upgrade file attach
                var fatchs = GetUserDesignerRequest.Get.GetUserDesignerRequestFiles(data.ReqId);
                data.files = fatchs;

                return View(data);
            }
                
            if(model.Id != null)
            {
                var user = GetUser.Get.GetById(model.Id.Value);
                user.UserType = 2;
                //get data image profile 
                var userImg = GetUser.Get.GetUserImgs(user.UserId.Value);

                if (userImg.Count > 0)
                {
                    user.AttachFileImage = GetAttachFile.Get.GetByRefId(userImg.FirstOrDefault().UserImgId.Value);
                }

                return View(user);
            }
            else
            {
                return View(new UserModel() { UserType = 2 });
            }
            
        }

        [HttpPost]
        public JsonResult Manage(UserModel data)
        {
            try
            {
                data.UserType = 1; 
                data.Status = 1;//1=รอดำเนินการ 
                data.CreatedBy = _UserId().Value;
                data.UpdatedBy = _UserId().Value;
                int result = GetUserDesignerRequest.Manage.AddNewRequest(data);
                if(result > 0)
                {
                    return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "UserApprove")));
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
                var fatchs = GetUserDesignerRequest.Get.GetUserDesignerRequestFiles(data.ReqId);
                data.files = fatchs;

                try
                {
                    string webAdmin = config.GetSection("WebConfig:AdminWebStie").Value;

                    var album = GetAlbum.Get.GetUserExampleAlbum(data.UserId.Value);
                    SearchModel searchModel = new SearchModel()
                    {
                        AlbumId = album.AlbumId.Value,
                        pageSize = 999,
                        currentPage = 1
                    };
                    data.Images = GetAlbum.Get.GetAlbumImageByAlbumId(searchModel, webAdmin);
                }
                catch (Exception ex) { }

                return View(data);
            }
            
            return View(new UserModel()); 
        }

        [HttpPost]
        public JsonResult ManageRequest(UserModel data)
        {
            try
            {
                //data.UserType = 2;
                if (data.Status == 2)
                {
                    data.UserType = 2;
                }
                data.CreatedBy = _UserId().Value;
                data.UpdatedBy = _UserId().Value;
                var result = GetUserDesignerRequest.Manage.UpdateRequestStatus(data);
                if(result > 0)
                {
                    if (data.Status == 3)
                    {
                        GetAlbum.Manage.DeleteAlbumExample(data.UserId.Value);
                    }
                    return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "UserApprove")));
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
        public IActionResult History()
        {
            return View("V1/History");
        }
        public IActionResult History2()
        {
            return View("V1/Index");
        }
    }
}