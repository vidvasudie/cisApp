using System;
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

        public class Authorized : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext context)
            {

                Guid fixGuid = new Guid("F71758CF-3F30-432B-8361-5754CDE8BF26");

                _UserId = fixGuid;

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