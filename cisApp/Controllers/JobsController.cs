﻿using cisApp.Core;
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
        public IActionResult LogDetail(JobModel data)
        { 
            return View(data);
        }
        [HttpPost]
        public JsonResult JobStatusUpdate(JobModel data)
        {
            try
            {
                var res = GetJobs.Manage.UpdateJobStatus(data.JobId, data.JobStatus, _UserId());
                if(res != null)
                {
                    return Json(new { success = true, message = "success", redirectUrl = "" });
                }
                return Json(new { success = false, message = "fail", redirectUrl = "" });
            }
            catch (Exception ex)
            {
                return Json(new { success=false, message= ex.ToString(), redirectUrl="" });
            }
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
                //data.UserId = Guid.Parse("50BA6DDD-0D68-475B-83E4-EC83C0499BFE");
                data.JobPrice = data.JobPricePerSqM * data.JobAreaSize;
                data.JobProceedRatio = GetTmVatratio.Get.GetFirst().Ratio;
                data.JobPriceProceed = data.JobPrice * data.JobProceedRatio;
                data.JobVatratio = GetTmProceedRatio.Get.GetFirst().Ratio;
                data.JobFinalPrice = data.JobPriceProceed * data.JobVatratio; 
                data.UpdatedBy = _UserId().Value;
                data.CreatedBy = _UserId().Value;

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
                bool isCanProcess = true;
                var jobCa = GetJobsCandidate.Get.GetByJobId(new SearchModel { gId= model.JobId, statusStr = "1,2,3", statusOpt="in" });
                var job = GetJobs.Get.GetById(model.JobId.Value);
                if (jobCa != null && jobCa.Where(o => o.CaStatusId != 3).Count() > 0 && job != null)
                {
                    if(job.JobStatus == 2 || job.JobStatus == 3)
                    {
                        if ((model.userCandidates != null && (model.userCandidates.Count() + jobCa.Where(o => o.CaStatusId != 3).Count()) > 3) || jobCa.Count() >= 3)
                        {
                            //กรณีใบงาน ยังอยู่ในสถานะ รอ หรือประกวด
                            //ถ้าจำนวนผู้สมัครครบ 3 คน จะเพิ่มไม่ได้อีก
                            //*** นอกเหนือจากนั้น ไม่ต้องนับจำนวนคน เพราะจะจัดการผู้ชนะเท่านั้น
                            isCanProcess = false; 
                        }
                    }
                    if ((job.JobStatus == 4 || job.JobStatus == 5 || job.JobStatus == 7 || job.JobStatus == 9) && (model.userCandidates != null && model.userCandidates.Count() > 1))
                    {
                        return Json(new ResponseModel().ResponseError("ไม่สามารถเพิ่มผู้ชนะประกวดได้มากกว่า 1 คน"));
                    }
                }
                if (isCanProcess)
                {
                    //ให้ทำการเพิ่มผู้สมัครได้ 
                    if ((job.JobStatus == 4 || job.JobStatus == 5 || job.JobStatus == 7 || job.JobStatus == 9) && (model.userCandidates != null && model.userCandidates.Count() > 1))
                    {
                        //จัดการผู้ชนะเท่านั้น และเพิ่มได้ คนเดียวเท่านั้น
                        var jca = jobCa.Where(o => o.CaStatusId == 3).FirstOrDefault();
                        if (jca != null)
                        {
                            //ปฎิเสธ ผู้ชนะคนเดิม ก่อน และคืน slot งาน
                            GetJobsCandidate.Manage.StatusUpdate(job.JobId, jca.UserId.Value, _UserId().Value, 5);
                            
                        }
                        //เพิ่มผู้สมัคร
                        GetJobsCandidate.Manage.UpdateNewCandidate(model.userCandidates, _UserId().Value, Request.HttpContext.Connection.RemoteIpAddress.ToString());
                        //ปรับสถานะ ให้เป็นผู้ชนะ และอัพเดตสถานะใบงาน กลับไปเป็นสถานะ ประกวด = 4
                        GetJobs.Manage.UpdateCandidate(new CandidateSelectModel { JobId = job.JobId, CaUserId = jca.UserId.Value, UserId = _UserId().Value, ip = Request.HttpContext.Connection.RemoteIpAddress.ToString() });
                    }
                    else
                    {
                        if(model.userCandidates != null && model.userCandidates.Count <= 3)
                        {
                            //เพิ่มผู้สมัคร และlock slot งานนักออกแบบ
                            GetJobsCandidate.Manage.UpdateNewCandidate(model.userCandidates, _UserId().Value, Request.HttpContext.Connection.RemoteIpAddress.ToString());
                        }
                        else
                        {
                            return Json(new ResponseModel().ResponseError("ไม่สามารถเพิ่มผู้สมัคร หรือผู้ร่วมประกวดได้มากกว่า 3 คน"));
                        }
                    }
                }
                else
                {
                    return Json(new ResponseModel().ResponseError("ไม่สามารถเพิ่มผู้สมัคร หรือผู้ร่วมประกวดได้มากกว่า 3 คน"));
                }
                
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
                //ปฎิเสธ และคืน slot งาน
                var user = GetJobsCandidate.Manage.Delete(id, _UserId().Value, Request.HttpContext.Connection.RemoteIpAddress.ToString()); 

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
                AlbumType = "1";

                //var job = GetJobs.Get.GetById(id);

                //if (job.JobStatus < 5) // ถ้า job status ไม่เท่ากับ 5 จะเป็นการประกวดทั้งหมด
                //{
                //    AlbumType = "1";

                //}
                //else if (job.JobStatus == 7) // 7 คือ ขอพิมพ์เขียวให้ type เป็น 5 คือพิมพ์เขียว
                //{
                //    AlbumType = "5";
                //}
                //else if (job.EditSubmitCount == 0) // ส่งผลงานครั้งแรก
                //{
                //    AlbumType = "2";

                //}
                //else if (job.EditSubmitCount == 1) // แก้ครั้งแรก
                //{
                //    AlbumType = "3";
                //}
                //else if (job.EditSubmitCount == 2) // แก้ครั้งที่ 2 ครั้งสุดท้ายแล้ว
                //{
                //    AlbumType = "4";
                //}
                //else // เกินกว่านี้ เตะออก
                //{
                    
                //}

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
                string AlbumType = "1";
                var job = GetJobs.Get.GetById(data.JobId);

                if (job.JobStatus < 5) // ถ้า job status ไม่เท่ากับ 5 จะเป็นการประกวดทั้งหมด
                {
                    AlbumType = "1";

                }
                else if (job.JobStatus == 7) // 7 คือ ขอพิมพ์เขียวให้ type เป็น 5 คือพิมพ์เขียว
                {
                    AlbumType = "5";
                }
                else if (job.EditSubmitCount == 0) // ส่งผลงานครั้งแรก
                {
                    AlbumType = "2";

                }
                else if (job.EditSubmitCount == 1) // แก้ครั้งแรก
                {
                    AlbumType = "3";
                }
                else if (job.EditSubmitCount == 2) // แก้ครั้งที่ 2 ครั้งสุดท้ายแล้ว
                {
                    AlbumType = "4";
                }
                else // เกินกว่านี้ เตะออก
                {
                    return Json(new ResponseModel().ResponseError("ไม่สามารถส่งงานดังกล่าวได้เนื่องจากสถานะงาน ไม่ได้อยู่ในสถานะที่ส่งงานได้"));
                }
                var result = GetAlbum.Manage.Update(data, _UserId().Value);

                // update edit count
                if (job.EditSubmitCount == 0) // ส่งผลงานครั้งแรก
                {
                    GetJobs.Manage.UpdateEditCount(data.JobId, 1);

                }
                else if (job.EditSubmitCount == 1) // แก้ครั้งแรก
                {
                    GetJobs.Manage.UpdateEditCount(data.JobId, 2);
                }
                else if (job.EditSubmitCount == 2) // แก้ครั้งที่ 2 ครั้งสุดท้ายแล้ว
                {
                    GetJobs.Manage.UpdateEditCount(data.JobId, 3);
                }

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
