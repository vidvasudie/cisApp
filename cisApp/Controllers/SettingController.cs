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
    public class SettingController : BaseController
    {
        readonly static IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json")
                      .Build();
        private List<Settings> _model = new List<Settings>();

        private Guid _PermissionMenuId;
        private int _PermissionManage;
        public SettingController()
        {
            _PermissionMenuId = Guid.Parse("8386A3FD-A964-4089-B102-9487519896D4");
            _PermissionManage = 2;// สิทธิ์ผู้ใช้งาน
        }
        public IActionResult Index(int pageIndex = 1)
        {
            LogActivityEvent(LogCommon.LogMode.SETTING_DATA);
            return View(new PaginatedList<Settings>(_model, _model.Count, pageIndex, 2));
        }

        [HttpPost]
        public PartialViewResult ItemList()
        {
            string webAdmin = config.GetSection("WebConfig:AdminWebStie").Value;
            List<Settings> _model = GetSettings.Get.GetAll(webAdmin);
            int count = _model.Count;

            LogActivityEvent(LogCommon.LogMode.SEARCH);
            return PartialView("PT/_itemlist", new PaginatedList<Settings>(_model, count, 1, 100));
        }

        public IActionResult Manage(Guid? id)
        {
            try
            {
                
                Settings data = new Settings();
                string webAdmin = config.GetSection("WebConfig:AdminWebStie").Value;

                if (id != null)
                {
                    data = GetSettings.Get.GetById(id.Value, webAdmin);

                    if (data == null)
                    {
                        LogActivityEvent(LogCommon.LogMode.MANAGE, MessageCommon.TXT_OPERATE_ERROR);
                        throw new Exception("Wrong Url Exception");
                    }
                }
                else
                {
                    LogActivityEvent(LogCommon.LogMode.MANAGE, MessageCommon.TXT_OPERATE_ERROR);
                    throw new Exception("Wrong Url Exception");
                }
                LogActivityEvent(LogCommon.LogMode.MANAGE);
                return View(data);
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.MANAGE, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                throw ex;
            }
            
        }


        [HttpPost]
        public JsonResult Manage(Settings data)
        {
            try
            {
                var user = GetSettings.Manage.Update(data, _UserId().Value);

                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "Setting")));
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                return Json(new ResponseModel().ResponseError());
            }
        }
    }
}
