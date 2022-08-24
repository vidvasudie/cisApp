using cisApp.Common;
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
    public class FaqController : BaseController
    {

        private List<Faq> _model = new List<Faq>();

        private Guid _PermissionMenuId;
        private int _PermissionManage;
        public FaqController()
        {
            _PermissionMenuId = Guid.Parse("8386A3FD-A964-4089-B102-9487519896D4");
            _PermissionManage = 2;// สิทธิ์ผู้ใช้งาน
        }
        public IActionResult Index(int pageIndex = 1)
        {
            LogActivityEvent(LogCommon.LogMode.FAQ);
            return View(new PaginatedList<Faq>(_model, _model.Count, pageIndex, 2));
        }

        [HttpPost]
        public PartialViewResult ItemList()
        {
            List<Faq> _model = GetFaq.Get.GetAll();
            int count = _model.Count;

            LogActivityEvent(LogCommon.LogMode.SEARCH);
            return PartialView("PT/_itemlist", new PaginatedList<Faq>(_model, count, 1, 10));
        }

        public IActionResult Manage(int? id)
        {
            try
            {
                
                Faq data = new Faq();
                
                if (id != null)
                {
                    data = GetFaq.Get.GetById(id.Value);

                    if (data == null)
                    {
                        LogActivityEvent(LogCommon.LogMode.MANAGE, MessageCommon.TXT_OPERATE_ERROR);
                        throw new Exception("Wrong Url Exception");
                    }
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
        public JsonResult Manage(Faq data)
        {
            try
            {
                var user = GetFaq.Manage.Update(data, _UserId().Value);

                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "Faq")));
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                return Json(new ResponseModel().ResponseError());
            }
        }

        [HttpPost]
        public JsonResult Order(int faqId, int order)
        {
            try
            {
                if (order == 1)
                {
                    GetFaq.Manage.OrderUp(faqId);
                }
                else
                {
                    GetFaq.Manage.OrderDown(faqId);
                }                

                LogActivityEvent(LogCommon.LogMode.MANAGE, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "Faq")));
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.DELETE, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                return Json(new ResponseModel().ResponseError());
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var faq = GetFaq.Manage.Delete(id, _UserId().Value);

                LogActivityEvent(LogCommon.LogMode.DELETE, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "Faq")));
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.DELETE, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                return Json(new ResponseModel().ResponseError());
            }
        }
    }
}
