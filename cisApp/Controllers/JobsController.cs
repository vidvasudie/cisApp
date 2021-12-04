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
        public IActionResult Index(SearchModel model)
        {
            return View(model);
        }

        [HttpPost]
        public PartialViewResult ItemList(SearchModel model)
        {
            List<JobModel> _model = GetJobs.Get.GetJobs(model);
            int count = GetJobs.Get.GetJobsTotal(model);

            return PartialView("PT/_itemlist", new PaginatedList<JobModel>(_model, count, model.currentPage.Value, model.pageSize.Value));
        }

        public IActionResult Detail(SearchModel model)
        {
            List<JobModel> _model = GetJobs.Get.GetJobs(model);
            if (_model != null && _model.Count > 0)
                return View(_model.FirstOrDefault());

            return View(new JobModel());
        }

        public IActionResult Manage(SearchModel model)
        {
            if(model != null && model.gId != null && model.gId != Guid.Empty)
            {
                List<JobModel> _model = GetJobs.Get.GetJobs(model);
                if (_model != null && _model.Count > 0)
                    return View(_model.FirstOrDefault());
            } 
            return View(new JobModel() { UserId= model.gId.Value });
        }

        [HttpPost]
        public JsonResult Manage(JobModel data)
        {
            try
            {
                if(data.JobId == Guid.Empty)
                {
                    //new create job 1=ร่าง/2=รอ
                    data.JobStatus = 2; 
                }
                data.JobStatus = data.IsDraft ? 1 : data.JobStatus;
                data.UserId = Guid.Parse("50BA6DDD-0D68-475B-83E4-EC83C0499BFE");
                data.JobPrice = data.JobPricePerSqM * data.JobAreaSize;
                data.JobProceedRatio = GetTmVatratio.Get.GetFirst().Ratio;
                data.JobPriceProceed = data.JobPrice * data.JobProceedRatio;
                data.JobVatratio = GetTmProceedRatio.Get.GetFirst().Ratio;
                data.JobFinalPrice = data.JobPriceProceed * data.JobVatratio; 
                data.UpdatedBy = _UserId.Value;
                data.CreatedBy = _UserId.Value;

                var result = GetJobs.Manage.Update(data, Request.HttpContext.Connection.RemoteIpAddress.ToString());
                if (result != null)
                {
                    return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "Jobs")));
                }
                else
                {
                    return Json(new ResponseModel().ResponseError());
                } 
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel().ResponseError());
            }   
             
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

        

        //public IActionResult Detail2(string Mode)
        //{
        //    return View("Detail", Mode);
        //}
        public IActionResult Payment()
        {
            return View();
        }

        public IActionResult CreatePayment()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Submitwork(Guid id, Guid userId, string AlbumType = "1")
        {
            try
            {
                AlbumModel model = new AlbumModel()
                {
                    JobId = id,
                    UserId = userId,
                    AlbumType = AlbumType
                };
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        [HttpPost]
        public JsonResult Submitwork(AlbumModel data)
        {
            try
            {                
                var result = GetAlbum.Manage.Update(data, _UserId.Value);
                if (result != null)
                {
                    return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "Jobs")));
                }
                else
                {
                    return Json(new ResponseModel().ResponseError());
                }
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel().ResponseError());
            }
        }
    }


}
