using cisApp.Core;
using cisApp.Function;
using cisApp.library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.Controllers
{
    [Authorize]
    [CustomActionExecute("8386A3FD-A964-4089-B102-9487519896D4")]
    public class SettingController : BaseController
    {

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
            return View(new PaginatedList<Settings>(_model, _model.Count, pageIndex, 2));
        }

        [HttpPost]
        public PartialViewResult ItemList()
        {
            List<Settings> _model = GetSettings.Get.GetAll();
            int count = _model.Count;

            return PartialView("PT/_itemlist", new PaginatedList<Settings>(_model, count, 1, 10));
        }

        public IActionResult Manage(Guid? id)
        {
            try
            {
                Settings data = new Settings();
                
                if (id != null)
                {
                    data = GetSettings.Get.GetById(id.Value);

                    if (data == null)
                    {
                        throw new Exception("Wrong Url Exception");
                    }
                }
                else
                {
                    throw new Exception("Wrong Url Exception");
                }

                return View(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }


        [HttpPost]
        public JsonResult Manage(Settings data)
        {
            try
            {
                var user = GetSettings.Manage.Update(data, _UserId().Value);

                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "Setting")));
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel().ResponseError());
            }
        }
    }
}
