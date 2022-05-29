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
        public IActionResult LogDetail(JobModel data)
        { 
            return View(data);
        }
        [HttpPost]
        public JsonResult JobStatusUpdate(JobModel data)
        {
            try
            {
                var job = GetJobs.Get.GetById(data.JobId);
                if(job == null)
                {
                    return Json(new { success = false, message = "ไม่พบข้อมูล", redirectUrl = "" });
                }
                if (job.JobStatus == 3 && data.JobStatus == 4)
                {
                    var jobca = GetJobsCandidate.Get.GetByJobId(new SearchModel { gId=data.JobId, statusStr="3", statusOpt= "equal" });
                    if(jobca == null || jobca.Count == 0)
                    {
                        return Json(new { success = false, message = "ต้องเลือกผู้ชนะก่อน", data=new { status= job.JobStatus }, redirectUrl = "" });
                    }
                }

                var res = GetJobs.Manage.UpdateJobStatus(data.JobId, data.JobStatus, _UserId(), Request.HttpContext.Connection.RemoteIpAddress.ToString());
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
         
        public IActionResult JobUpdateCandidateSelect(Guid jobId, Guid caUserId)
        {
            try
            {  
                var job = GetJobs.Manage.UpdateCandidate(new CandidateSelectModel { JobId = jobId, CaUserId = caUserId, UserId = _UserId().Value, ip = Request.HttpContext.Connection.RemoteIpAddress.ToString() });
                if (job == null)
                {
                    return BadRequest("บันทึกข้อมูลไม่สำเร็จ");
                }

                return RedirectToAction("Detail", new { jobId= job.JobId });
            }
            catch(Exception ex)
            {
                return BadRequest();
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

        public IActionResult ManageImgExam(SearchModel model)
        {
            JobModel job = new JobModel();
            job.JobId = model.gId.Value;

            var files = GetJobsExamImage.Get.GetImageByJobId(model.gId.Value);
            if(files != null && files.Count > 0)
            {
                job.files = files.Select(o => new FileAttachModel { 
                    TypeId=o.JobsExTypeId.Value, 
                    Description = o.JobsExTypeDesc,
                    AttachFileId=o.AttachFileId,
                    FileName=o.FileName,
                    Size=o.Size
                }).ToList();
            }
            return View(job);
        }

        [HttpPost]
        public JsonResult JobUpdateImgExam(JobModel data)
        {
            try
            {
                var result = GetJobs.Manage.UpdateImageExam(data.files, new Jobs { JobId=data.JobId, UpdatedDate = DateTime.Now, UpdatedBy = _UserId().Value, CreatedDate = DateTime.Now, CreatedBy = _UserId().Value });
                if(result == 1)
                {
                    return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Detail", "Jobs", new { gId = data.JobId })));
                }
                return Json(new ResponseModel().ResponseError("บันทึกข้อมูลไม่สำเร็จ"));
            }
            catch(Exception ex)
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

                #region job candidate lock
                var jobLock = GetJobCadidateLock.Get.GetLockByJobId(model.JobId.Value);
                if (jobLock != null) //ต้องไม่อยู่ในช่วงจายเงิน 
                {
                    return Json(new ResponseModel().ResponseError("ไม่สามารถยกสมัครได้ เนื่องจากอยู่ในระหว่่างการตรวจสอบการชำระเงิน", null));
                }

                var jpm = GetJobPayment.Get.GetByJobId(model.JobId.Value); //order by วันล่าสุดมาแล้ว 
                if (jpm.Count > 0 && jpm.First().PayStatus == 2) //ต้องไม่อยู่ในช่วงตรวจสอบการชำระเงิน
                {
                    return Json(new ResponseModel().ResponseError("ไม่สามารถยกสมัครได้ เนื่องจากอยู่ในระหว่่างการตรวจสอบการชำระเงิน", null));
                }
                #endregion

                if (isCanProcess)
                {
                    //ให้ทำการเพิ่มผู้สมัครได้ 
                    if ((job.JobStatus == 4 || job.JobStatus == 5 || job.JobStatus == 7 || job.JobStatus == 9) && (model.userCandidates != null && model.userCandidates.Count() == 1))
                    {
                        //จัดการผู้ชนะเท่านั้น และเพิ่มได้ คนเดียวเท่านั้น
                        var jca = jobCa.Where(o => o.CaStatusId == 3).FirstOrDefault();
                        if (jca != null)
                        {
                            //ปฎิเสธ ผู้ชนะคนเดิม ก่อน และคืน slot งาน
                            GetJobsCandidate.Manage.StatusUpdate(job.JobId, jca.UserId.Value, _UserId().Value, 5, Request.HttpContext.Connection.RemoteIpAddress.ToString());
                            
                        }
                        //เพิ่มผู้สมัคร
                        GetJobsCandidate.Manage.UpdateNewCandidate(model.userCandidates, _UserId().Value, Request.HttpContext.Connection.RemoteIpAddress.ToString());
                        //ปรับสถานะ ให้เป็นผู้ชนะ และอัพเดตสถานะใบงาน กลับไปเป็นสถานะ ประกวด = 4
                        GetJobs.Manage.UpdateCandidate(new CandidateSelectModel { JobId = job.JobId, CaUserId = model.userCandidates.First().UserId.Value, UserId = _UserId().Value, ip = Request.HttpContext.Connection.RemoteIpAddress.ToString() });
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
                #region job candidate lock
                var jobLock = GetJobCadidateLock.Get.GetLockByCaId(id); 
                if (jobLock != null) //ต้องไม่อยู่ในช่วงจายเงิน 
                {    
                    return Json(new ResponseModel().ResponseError("ไม่สามารถยกสมัครได้ เนื่องจากอยู่ในระหว่่างการตรวจสอบการชำระเงิน", null));
                }

                var jpm = GetJobPayment.Get.GetByCandidateId(id); //order by วันล่าสุดมาแล้ว 
                if (jpm.Count > 0 && jpm.First().PayStatus == 2) //ต้องไม่อยู่ในช่วงตรวจสอบการชำระเงิน
                {
                    return Json(new ResponseModel().ResponseError("ไม่สามารถยกสมัครได้ เนื่องจากอยู่ในระหว่่างการตรวจสอบการชำระเงิน", null));
                }
                #endregion

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
    }


}
