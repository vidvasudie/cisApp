﻿using cisApp.Function;
using cisApp.library;
using cisApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.Controllers
{
    [Authorize]
    [CustomActionExecute("DC03CFBD-60DA-424E-9D6D-0406C3D8B7F4")]
    public class FilesController : BaseController
    {
        private List<AlbumImageModel> _model = new List<AlbumImageModel>();
        private Guid _PermissionMenuId;
        private int _PermissionManage;
        private readonly IWebHostEnvironment _HostingEnvironment;

        readonly static IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json")
                      .Build();
        public FilesController(IWebHostEnvironment environment)
        {
            _PermissionMenuId = Guid.Parse("DC03CFBD-60DA-424E-9D6D-0406C3D8B7F4");
            _PermissionManage = 2;// สิทธิ์ผู้ใช้งาน
            _HostingEnvironment = environment;
        }

        public IActionResult Index(string searchBy = "image", Guid? designer = null, string category = "", string tag = "", int pageIndex = 1)
        {
            FilePageModel model = new FilePageModel();
            model.SearchBy = searchBy;
            model.Designer = designer;
            model.Category = category;
            model.Tag = tag;
            model.ImageModel = new PaginatedList<AlbumImageModel>(_model, _model.Count, pageIndex, 2);
            return View(model);
        }

        [HttpPost]
        public PartialViewResult ItemList(SearchModel model)
        {
            string webAdmin = config.GetSection("WebConfig:AdminWebStie").Value;

            

            if (model == null) model = new SearchModel();

            if (model.pageSize == 10) model.pageSize = 12;

            model.Orderby = "LastedCreate";

            

            if (model.SearchBy == "album")
            {

                ViewData["FileMode"] = "album";

                List<AlbumImageModel> _model = GetAlbum.Get.GetAlbum(model, webAdmin);
                int count = GetAlbum.Get.GetAlbumTotal(model);

                var paginationModel = new PaginatedList<AlbumImageModel>(_model, count, model.currentPage.Value, model.pageSize.Value);
                paginationModel.PageList = new List<int>() { 12, 24, 48 };

                return PartialView("PT/_itemlist", paginationModel);
            }
            else
            {

                ViewData["FileMode"] = "image";

                List<AlbumImageModel> _model = GetAlbum.Get.GetAlbumImage(model, webAdmin);
                int count = GetAlbum.Get.GetAlbumImageTotal(model);

                var paginationModel = new PaginatedList<AlbumImageModel>(_model, count, model.currentPage.Value, model.pageSize.Value);
                paginationModel.PageList = new List<int>() { 12, 24, 48 };

                return PartialView("PT/_itemlist", paginationModel);
            }

            
        }

        [HttpPost]
        public JsonResult DeleteImage(Guid id)
        {
            try
            {
                GetAlbum.Manage.DeleteAttachFileImage(id, _UserId().Value);

                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "Files")));
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel().ResponseError());
            }
        }



        public IActionResult Index_bak(string Mode)
        {
            return View("Index", Mode);
        }
    }
}
