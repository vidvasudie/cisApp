using cisApp.Function;
using cisApp.library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Core;

namespace cisApp.Controllers
{
    [Authorize]
    [CustomActionExecute("CA06E5CC-691E-4BE7-A8EF-3F9EE8400B1A")]
    public class PaymentController : BaseController
    {
        private List<JobPayment> _model = new List<JobPayment>();
        private Guid _PermissionMenuId;
        private int _PermissionManage;
        public IActionResult Index(SearchModel model)
        {
            return View(model);
        }

        public IActionResult Detail()
        {
            return View();
        }

        public IActionResult History()
        {
            return View();
        }

        public IActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult ItemList(SearchModel model)
        {
            List<JobPayment> _model = GetJobPayment.Get.GetBySearch(model);
            int count = GetJobPayment.Get.GetBySearchTotal(model);

            return PartialView("PT/_itemlist", new PaginatedList<JobPayment>(_model, count, model.currentPage.Value, model.pageSize.Value));
        }


        // id ในเคสแก้ไข ถ้าเพิ่มใหม่ใช่ jobId มาว่าจะเพิ่มให้ใบงานไหน
        public IActionResult Manage(int? id, Guid? jobId)
        {
            try
            {
                JobPayment data = new JobPayment();
                if (id != null)
                {
                    data = GetJobPayment.Get.GetById(id.Value);
                }
                else if (jobId != null)
                {
                    data.JobId = jobId;
                }
                else
                {
                    // if id and jobId null need to throw exeption
                    throw new Exception("Error");
                }

                return View(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }


        [HttpPost]
        public JsonResult Manage(JobPayment data)
        {
            try
            {
                var user = GetJobPayment.Manage.Update(data, _UserId.Value);

                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "Payment")));
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel().ResponseError());
            }
        }

        public IActionResult ManageStatus(int? id)
        {
            try
            {
                JobPayment data = new JobPayment();
                if (id != null)
                {
                    data = GetJobPayment.Get.GetById(id.Value);
                }
                else
                {
                    // if id and jobId null need to throw exeption
                    throw new Exception("Error");
                }

                return View(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult ManageStatus(JobPayment data)
        {
            try
            {
                var user = GetJobPayment.Manage.Status(data.JobPayId.Value, data.PayStatus.Value, data.Comment, _UserId.Value);

                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "Payment")));
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel().ResponseError());
            }
        }
    }
}
