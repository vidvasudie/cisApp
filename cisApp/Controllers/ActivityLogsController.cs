using cisApp.Function;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.Controllers
{
    [CustomActionExecute("BF5D2740-6400-4D0F-9E75-8554B0E2D374")]
    public class ActivityLogsController : BaseController
    {
        private Guid _PermissionMenuId;
        private int _PermissionManage;
        public ActivityLogsController()
        {
            _PermissionMenuId = Guid.Parse("BF5D2740-6400-4D0F-9E75-8554B0E2D374");
            _PermissionManage = 2;// สิทธิ์ผู้ใช้งาน
        } 

        public IActionResult Index(SearchModel model)
        {
            if (_UserId() != null && _UserId() != Guid.Empty)
            {
                LogActivityEvent(Common.LogCommon.LogMode.LOG);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult Search(SearchModel model)
        {
            return Json(new ResponseModel().ResponseSuccess());
        }

        public PartialViewResult GetTable()
        {  
            return PartialView("PT/_itemlist");
        }

    }
}
