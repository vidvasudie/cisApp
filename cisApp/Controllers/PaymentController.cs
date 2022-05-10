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
    [CustomActionExecute("0E12C6B9-14B1-4ABA-B220-49D7496EDBD7")]
    public class PaymentController : BaseController
    {
        private List<UserModel> _model = new List<UserModel>();
        private Guid _PermissionMenuId;
        private int _PermissionManage;
        public PaymentController()
        {
            _PermissionMenuId = Guid.Parse("0E12C6B9-14B1-4ABA-B220-49D7496EDBD7");
            _PermissionManage = 2;// สิทธิ์ผู้ใช้งาน
        }
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
        [HttpGet]
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
                var user = GetJobPayment.Manage.Update(data, _UserId().Value);
                
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
                var user = GetJobPayment.Manage.Status(data.JobPayId.Value, data.PayStatus.Value, data.Comment, _UserId().Value);
                if (data.PayStatus.Value == 3)
                {
                    //Noti ลูกค้า เมื่อการ ยืนยัน ชำระเงินสำเร็จ
                    var job = GetJobs.Get.GetById(user.JobId.Value);
                    new MobileNotfication().Forcustomer(MobileNotfication.Modecustomer.paymentconfirm, job.UserId, user.JobId.Value);

                    //Noti นักออกแบบปรับสถานะเป็น ประกวดงาน เมื่อการชำระเงินสำเร็จ
                    var jc = GetJobsCandidate.Get.GetByJobId(new SearchModel { gId = user.JobId });
                    if (jc != null && jc.Count > 0)
                    {
                        foreach (var u in jc.Where(o => o.CaStatusId == 2))
                        //เฉพาะผู้ประกวดผลงาน
                        {
                            new MobileNotfication().Fordesigner(MobileNotfication.ModeDesigner.contest, u.UserId.Value, user.JobId.Value);
                        }
                    }
                }
                    
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "Payment")));
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel().ResponseError());
            }
        }
    }
}
