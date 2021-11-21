﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.library;
using cisApp.Function;
using cisApp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace cisApp.Controllers
{
    public class BaseController : Controller
    {

        public static Guid? _UserId {get; set;}
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
                    _UserId = Guid.Parse(userId);
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
    }
}