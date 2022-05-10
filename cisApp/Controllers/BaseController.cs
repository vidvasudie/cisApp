using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.library;
using cisApp.Function;
using cisApp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace cisApp.Controllers
{
    public class BaseController : Controller
    {
        readonly static IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json")
                      .Build();
        public Guid? _UserId()
        {
            var userId = HttpContext.Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
            if (!String.IsNullOrEmpty(userId))
            {
                return Guid.Parse(userId);
            }
            else
            {
                return null;
            }
        }
        //public static Guid? _UserId {get; set;}
        public static int? _UserType { get; set; }
        public static Guid? _RoleId { get; set; }
        public static string _UserName { get; set; }
        public static string _FullName { get; set; }
        public static int? _RoleMenuType { get; set; }

        public class CustomActionExecuteAttribute : ActionFilterAttribute
        {
            private readonly Guid _menuid;
            public CustomActionExecuteAttribute(string menuId)
            {
                // _menuid  คือ  MenuId ของตารางเมนู 
                _menuid = Guid.Parse(menuId); 
            }
            public override void OnActionExecuting(ActionExecutingContext context)
            { 
                _FullName = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "FullName")?.Value;
                var userId = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
                if (!String.IsNullOrEmpty(userId))
                {
                    //_UserId = Guid.Parse(userId);
                } 
                var roleId = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "RoleId")?.Value;
                if (!String.IsNullOrEmpty(roleId))
                {
                    _RoleId = Guid.Parse(roleId);
                }
                var userType = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
                if (!String.IsNullOrEmpty(userType))
                {
                    _UserType = int.Parse(userType);
                }
                _UserName = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value;
                var roleMenu = GetRoleMenu.Get.GetByRoleIdAndMenuId(_RoleId.Value, _menuid);
                _RoleMenuType = roleMenu.Type;


                base.OnActionExecuting(context);
            }

            public override void OnActionExecuted(ActionExecutedContext context)
            {
                base.OnActionExecuted(context);
            }
        }

        

        [HttpGet]
        public JsonResult GetValidateIdentity(string pid)
        {
            return Json(new { status = Utility.IsValidIdentityFormat(pid) });
        }
        [HttpGet]
        public JsonResult GetDistrict(int provinceId)
        {
            List<TmDistrict> dataList = new List<TmDistrict>();
            try
            {
                dataList = GetTmDistrict.Get.GetByProvinceId(provinceId);
                return Json(new { status = true, data = dataList });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, data = dataList });
            }
        }
        [HttpGet]
        public JsonResult GetSubDistrict(int districtId)
        {
            List<TmSubdistrict> dataList = new List<TmSubdistrict>();
            try
            {
                dataList = GetTmSubDistrict.Get.GetByDistrictId(districtId);
                return Json(new { status = true, data = dataList });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, data = dataList });
            }
        }
        [HttpGet]
        public JsonResult GetPostCode(int subdistrictId)
        {
            TmSubdistrict data = new TmSubdistrict();
            try
            {
                data = GetTmSubDistrict.Get.GetById(subdistrictId);
                return Json(new { status = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, data = data });
            }
        }

        public ActionResult UploadFile(IFormFile upload_file)
        {
            return Ok(new { ok=true });
        }
        [HttpPost]
        public PartialViewResult UploadPreview(FileAttachModel file)
        {
            return PartialView("~/Views/Shared/Common/_ImageItem.cshtml", file);
        }
        [HttpPost]
        public PartialViewResult PreviewImage(UploadFilesModel model)
        {
            if (model.AlbumId != null && model.AlbumId > 0)
            {
                var albs = GetAlbum.Get.GetAttachFileByAlbumId(model.AlbumId.Value, config.GetSection("WebConfig:AdminWebStie").Value);
                if (albs != null && albs.Count > 0)
                {
                    model.files = new List<FileAttachModel>();
                    int idx = 0;
                    foreach (var f in albs)
                    {
                        model.files.Add(new FileAttachModel { NextImgSelected= idx, NextImg= idx, FileName=f.FileName, AttachFileId=f.AttachFileId });
                        idx++;
                    }
                    var fa = model.files.Where(o => o.AttachFileId == model.AttachFileId).FirstOrDefault();
                    if (fa != null)
                    {
                        foreach (var f in model.files)
                        {
                            f.NextImgSelected = fa.NextImg; 
                        } 
                    }
                }

            }
            return PartialView("~/Views/Shared/Album/_ImageItems.cshtml", model.files);
        }
    }
}