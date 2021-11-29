using cisApp.Core;
using cisApp.Function;
using cisApp.library;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.Controllers
{
    [CustomActionExecute("434737C0-D543-476A-A7CE-FBD6C4E155EC")]
    public class JobsController : BaseController
    {
        private Guid _PermissionMenuId;
        private int _PermissionManage;
        public JobsController()
        {
            _PermissionMenuId = Guid.Parse("434737C0-D543-476A-A7CE-FBD6C4E155EC");
            _PermissionManage = 2;// สิทธิ์ผู้ใช้งาน
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult ItemList(SearchModel model)
        {
            List<JobModel> _model = GetJobs.Get.GetJobs(model);
            int count = GetJobs.Get.GetJobsTotal(model);

            return PartialView("PT/_itemlist", new PaginatedList<JobModel>(_model, count, model.currentPage.Value, model.pageSize.Value));
        }

        public IActionResult Manage(SearchModel model)
        {
            List<JobModel> _model = GetJobs.Get.GetJobs(model);
            if (_model != null && _model.Count > 0)
                return View(_model.FirstOrDefault());

            //for (var l=1; l<= 5;l++ )
            //{
            //    var data = new JobModel()
            //    {
            //        UserId = Guid.Parse("D01758CF-3F30-432B-8361-5754CDE8CA13"),
            //        JobTypeId = 1,
            //        JobStatus = l,
            //        JobDescription = "test job status = "+l,
            //        JobAreaSize = 180+l,
            //        JobPrice = 123456+(l+10),
            //        JobPricePerSqM = 500+l,
            //        JobBeginDate = DateTime.Now.AddMonths(l-1),
            //        JobEndDate = DateTime.Now.AddMonths(l),
            //    };
            //    GetJobs.Manage.Update(data); 
            //} 

            return View(new JobModel());
        }

        #region Candidate

        [HttpPost]
        public JsonResult AddNewCandidate(JobCandidateModel model)
        {
            try
            {
                GetJobsCandidate.Manage.UpdateNewCandidate(model.userCandidates, _UserId.Value);
                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess));
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel().ResponseError());
            }

        }

        [HttpPost]
        public JsonResult CandidateDelete(int id)
        {
            try
            {
                var user = GetJobsCandidate.Manage.Delete(id, _UserId.Value);

                return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess));
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel().ResponseError());
            }
        }

        [HttpPost]
        public PartialViewResult AddCandidateUser(JobCandidateModel model)
        {
            return PartialView("~/Views/Shared/Jobs/_listSelectedDesigner.cshtml", model);
        }
        [HttpPost]
        public PartialViewResult CandidateUserList(JobCandidateModel model)
        {
            return PartialView("~/Views/Shared/Jobs/_listUserDesigner.cshtml", model);
        }

        [HttpPost]
        public PartialViewResult CardCandidateList(JobModel model)
        {
            return PartialView("~/Views/Jobs/PT/_CardJobCandidate.cshtml", model);
        }

        #endregion

        public IActionResult Detail(string Mode)
        {
            return View("Detail", Mode);
        }
        public IActionResult Payment()
        {
            return View();
        }

        public IActionResult CreatePayment()
        {
            return View();
        }
        public IActionResult Submitwork()
        {
            return View();
        }
    }


}
