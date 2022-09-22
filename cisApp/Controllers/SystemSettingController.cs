using cisApp.Common;
using cisApp.Core;
using cisApp.Function;
using cisApp.library;
using Microsoft.AspNetCore.Authorization;
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
    [CustomActionExecute("8386A3FD-A964-4089-B102-9487519896D4")]
    public class SystemSettingController : BaseController
    {
        readonly static IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json")
                      .Build();
        private List<Settings> _model = new List<Settings>();

        private Guid _PermissionMenuId;
        private int _PermissionManage;
        public SystemSettingController()
        {
            _PermissionMenuId = Guid.Parse("8386A3FD-A964-4089-B102-9487519896D4");
            _PermissionManage = 2;// สิทธิ์ผู้ใช้งาน
        }
        public IActionResult Email()
        {
            var systemSetting = GetSystemSetting.Get.GetByKeyword("Email");
            LogActivityEvent(LogCommon.LogMode.SETTING_DATA);
            return View(systemSetting);
        }
        

        [HttpPost]
        public JsonResult Manage(SystemSetting data)
        {
            try
            {
                var result = GetSystemSetting.Manage.Update(data, _UserId().Value);

                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action(result.Keyword, "SystemSetting")));
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                return Json(new ResponseModel().ResponseError());
            }
        }
        
    }
}
