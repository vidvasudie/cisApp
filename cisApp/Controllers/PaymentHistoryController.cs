using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Common;
using cisApp.Core;
using cisApp.Function;
using cisApp.library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cisApp.Controllers
{
    [Authorize]
    [CustomActionExecute("0E12C6B9-14B1-4ABA-B220-49D7496EDBD7")]
    public class PaymentHistoryController : BaseController
    {
        private List<PaymentHistoryModel> _model = new List<PaymentHistoryModel>();
        private Guid _PermissionMenuId;
        private int _PermissionManage;
        public PaymentHistoryController()
        {
            _PermissionMenuId = Guid.Parse("0E12C6B9-14B1-4ABA-B220-49D7496EDBD7");
            _PermissionManage = 2;// สิทธิ์ผู้ใช้งาน
        }
        public IActionResult Index(SearchModel model)
        {
            LogActivityEvent(LogCommon.LogMode.PAYMENT_HIST);
            return View(model);
        }

        [HttpPost]
        public PartialViewResult ItemList(SearchModel model)
        {
            List<PaymentHistoryModel> _model = GetPaymentHistory.Get.GetBySearch(model);
            int count = GetPaymentHistory.Get.GetBySearchTotal(model);

            LogActivityEvent(LogCommon.LogMode.SEARCH);
            return PartialView("PT/_itemlist", new PaginatedList<PaymentHistoryModel>(_model, count, model.currentPage.Value, model.pageSize.Value));
        }

        // id ในเคสแก้ไข ถ้าเพิ่มใหม่ใช่ jobId มาว่าจะเพิ่มให้ใบงานไหน
        [HttpGet]
        public IActionResult Manage(Guid? id)
        {
            try
            {
                PaymentHistory data = new PaymentHistory();
                if (id != null)
                {
                    data = GetPaymentHistory.Get.GetById(id.Value);
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
        public JsonResult Manage(PaymentHistory data)
        {
            try
            {
                var user = GetPaymentHistory.Manage.Update(data, _UserId().Value);

                LogActivityEvent(LogCommon.LogMode.MANAGE, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "PaymentHistory")));
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                return Json(new ResponseModel().ResponseError());
            }
        }

        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            try
            {
                GetPaymentHistory.Manage.Delete(id, _UserId().Value);

                LogActivityEvent(LogCommon.LogMode.DELETE, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "PaymentHistory")));
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.DELETE, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                return Json(new ResponseModel().ResponseError());
            }
        }

        [HttpGet]
        public IActionResult ManageDate()
        {
            try
            {
                PaymentHistoryDate data = GetPaymentHistoryDate.Get.GetDefault();

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
        public JsonResult ManageDate(PaymentHistoryDate data)
        {
            try
            {
                var user = GetPaymentHistoryDate.Manage.Update(data.Day.Value, _UserId().Value);

                LogActivityEvent(LogCommon.LogMode.MANAGE, MessageCommon.SaveSuccess);
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "PaymentHistory")));
            }
            catch (Exception ex)
            {
                LogActivityEvent(LogCommon.LogMode.UPDATE, MessageCommon.TXT_OPERATE_ERROR, ex.ToString());
                return Json(new ResponseModel().ResponseError());
            }
        }


        [HttpPost]
        public PartialViewResult Export(SearchModel model)
        {
            var dt = GetPaymentHistory.Get.GetExportPaymentHistory(model);

            return PartialView("~/Views/Shared/Export/_TableDetail.cshtml", dt);
        }

    }

    

}
    
