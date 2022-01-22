using cisApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;
using cisApp.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cisApp.API.Controllers
{

    [ApiController]
    public class JobsFuncController : BaseController
    {
        /// <summary>
        /// สร้างใบงาน
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("api/jobs/createjob")]
        [HttpPost]
        public object Post([FromBody] JobAPIModels value)
        {
            try
            {
                if(value.UserId == Guid.Empty)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }

                JobModel job = new JobModel()
                {
                    IsApi = true,
                    UserId = value.UserId,
                    JobTypeId = value.JobTypeId,
                    JobDescription = value.JobDescription,
                    JobAreaSize = value.JobAreaSize,
                    JobPrice = value.JobPrice,
                    JobPriceProceed = value.JobPriceProceed,
                    JobFinalPrice = value.JobFinalPrice,
                    JobProceedRatio = value.JobProceedRatio,
                    JobVatratio = value.JobVatRatio,
                    JobPricePerSqM = value.JobPricePerSqM,
                    JobStatus = value.IsDraft ? 1 : 2, //1=ร่าง, 2=รอ 
                    IsInvRequired = value.IsInvRequired,
                    InvAddress = value.InvAddress,
                    InvPersonalId = value.InvPersonalId,
                    Invname = value.Invname,
                    files = value.FileList.Select(o => new FileAttachModel { AttachFileId= Guid.Parse(o.FileId), FileName=o.FileName, TypeId=o.Type }).ToList()
                };

                var result = GetJobs.Manage.Update(job, value.Ip);

                return Ok(resultJson.success("สร้างใบงานสำเร็จ", "success", new { result.JobId, result.JobNo }));

            }
            catch(Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            } 

            
        }

        /// <summary>
        /// แสดงข้อมูลประเภทใบงาน
        /// </summary>
        /// <returns></returns>
        [Route("api/jobs/getjobtype")]
        [HttpGet]
        public object GetJobType()
        {
            try
            {
                var obj = GetJobsType.Get.GetActive().Select(o => new { id=o.JobTypeId, name=o.Name }).ToList();
                if(obj == null || obj.Count == 0)
                {
                    return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", null));
                }
                return Ok(resultJson.success("สำเร็จ", "success", obj));
            }
            catch(Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        /// <summary>
        /// ข้อมูลตั้งต้นของการสร้างใบงาน
        /// </summary>
        /// <returns></returns>
        [Route("api/jobs/getjobinfo")]
        [HttpGet]
        public object GetJobDefaultInfo()
        {
            try
            {
                var proceedRatio = GetTmProceedRatio.Get.GetFirst();
                var vatRatio = GetTmVatratio.Get.GetFirst();
                var priceSQM = 250;
                return Ok(resultJson.success("สำเร็จ", "success", new { ProceedRatio= proceedRatio.Ratio, VatRatio= vatRatio.Ratio, PriceSQM= priceSQM }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        /// <summary>
        /// แสดงรายการใบงานของลูกค้า แสดงเฉพาะใบงานที่ยังไม่ยุติ / สิ้นสุด
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("api/jobs/getcustomerjoblist")]
        [HttpGet]
        public IActionResult GetCustomerJobList(Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }

                var data = GetJobs.Get.GetCustomerJobList(userId);
                if (data == null)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", null));
                }

                return Ok(resultJson.success("สำเร็จ", "success", data));

            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        /// <summary>
        /// แสดงรายการใบงานของลูกค้า แสดงเฉพาะใบงานที่ ยุติ / สิ้นสุด
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("api/jobs/getcustomerhistorylist")]
        [HttpGet]
        public IActionResult GetCustomerHistoryJobList(Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }

                var data = GetJobs.Get.GetCustomerHistoryJobList(userId);
                if (data == null)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", null));
                }
                string Host = _config.GetSection("WebConfig:AdminWebStie").Value;
                bool removeLast = Host.Last() == '/'; 
                if (removeLast)
                {
                    Host = Host.Remove(Host.Length - 1);
                }

                return Ok(resultJson.success("สำเร็จ", "success", data.Select(o => new { 
                    o.JobID,
                    o.JobType,
                    o.JobAreaSize,
                    o.JobDescription,
                    o.JobStatusDesc,
                    o.AttachFileID,
                    o.FileName,
                    ImgUrlPath = o.UrlPath.Replace("~", Host),
                    o.Fullname
                })));

            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        /// <summary>
        /// ยกเลิกใบงาน และปรับสถานะ cadiadte เป้น 6=ใบงานถูกยกเลิก
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="userId"></param>
        /// <param name="cancelId"></param>
        /// <param name="cancelMsg"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        [Route("api/jobs/canceljob")]
        [HttpDelete]
        public IActionResult CancelJob(Guid jobId, Guid userId, int cancelId, string cancelMsg, string ip)
        {
            try
            {
                if (jobId == Guid.Empty || userId == Guid.Empty)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }

                var pm = GetJobPayment.Get.GetByJobId(jobId);
                if(pm.Where(o => o.PayStatus != 1 || o.PayStatus != 4).Count() > 0) //1=รอชำระเงิน, 4=ไม่อนุมัติ/คืนเงิน
                {
                    //ถ้าจ่ายแล้ว ยกเลิกไม่ได้
                    return Ok(resultJson.errors("ไม่สามารถยกเลิกได้ เมื่อมีการชำระเงินแล้ว", "fail", null));
                }
                 
                var job = GetJobs.Manage.CancelJob(jobId, userId, cancelId, cancelMsg, ip);
                if (job == null)
                {
                    return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", null));
                }
                return Ok(resultJson.success("สำเร็จ", "success", new { job.JobId, job.JobNo }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        /// <summary>
        /// ดึงรายการผู้สมัครในใบงานนั้นๆ 
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Route("api/jobs/getcandidatelist")]
        [HttpGet]
        public IActionResult GetCandidateList(Guid jobId, int status = 5)
        {
            try
            {
                if (jobId == Guid.Empty)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }
                var job = GetJobs.Get.GetById(jobId);
                if (job == null)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", null));
                }
                var data = GetJobsCandidate.Get.GetByJobId(new SearchModel() { gId= jobId, statusStr = status.ToString(), statusOpt= status == 5 ? "less":"" });
                if(data == null || data.Count == 0)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", null));
                }
                
                return Ok(resultJson.success("สำเร็จ", "success", new { jobId= job.JobId, job.JobFinalPrice, candidates = data.Select(o => new { o.UserId, o.UserFullName, o.IsLike, o.PriceRate, o.UserRate, UrlPath = o.UrlPathAPI }).ToList() } ));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        /// <summary>
        /// ปฎิเสธนักออกแบบ
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="userId"></param>
        /// <param name="caUserId"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        [Route("api/jobs/rejectcandidate")]
        [HttpDelete]
        public IActionResult RejectCandidate(Guid jobId, Guid userId, Guid caUserId, string ip)
        {
            try
            {
                if (jobId == Guid.Empty || userId == Guid.Empty)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                } 

                var jobCa = GetJobsCandidate.Manage.Reject(jobId, userId, caUserId, ip);
                if (jobCa == null)
                {
                    return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", null));
                }
                return Ok(resultJson.success("สำเร็จ", "success", new { jobCa.JobId }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        /// <summary>
        /// แสดงรายชื่อ ผู้เข้าประกวด พร้อม list รายการรูปที่จัดส่ง เวลาที่จัดส่ง
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [Route("api/jobs/getworksubmitlist")]
        [HttpGet]
        public IActionResult GetWorkSubmitList(Guid jobId)
        {
            try
            {
                if (jobId == Guid.Empty)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }

                var data = GetJobs.Get.GetWorkSubmitList(jobId);
                if (data == null || data.Count() == 0)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", null));
                }

                string Host = _config.GetSection("WebConfig:AdminWebStie").Value;
                bool removeLast = Host.Last() == '/'; 
                if (removeLast)
                {
                    Host = Host.Remove(Host.Length - 1);
                } 

                return Ok(resultJson.success("สำเร็จ", "success", data.Select(o => new
                {
                    o.JobId,
                    o.CaUserId,
                    CaUrlPath = o.CaUrlPath == null ? null : o.CaUrlPath.Replace("~", Host),
                    o.Fullname,
                    o.LastLogin,
                    o.UpdatedDateStr,
                    o.AlbumName,
                    works = data.Where(d => d.CaUserId == o.CaUserId && d.AlbumName != null).Select(s => new { s.AlbumAttachFileID, s.AlbumFileName, WorkUrlPath = s.AlbumAttachFileID == null ? null : s.WorkUrlPath.Replace("~", Host) })
                })));
                

            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [Route("api/jobs/submitwork")]
        [HttpPost]
        public IActionResult SubmitWork([FromBody]SubmitworkModel value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string AlbumType = "1";

                    var job = GetJobs.Get.GetById(value.JobId.Value);

                    if (job.JobStatus < 5) // ถ้า job status ไม่เท่ากับ 5 จะเป็นการประกวดทั้งหมด
                    {
                        AlbumType = "1";
                        
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
                        return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "ใบงานดังกล่าวได้ส่งแก้ไขงานครบจำนวนที่ได้ตั้งไว้แล้ว", null));;
                    }

                    AlbumModel model = new AlbumModel()
                    {
                        JobId = value.JobId.Value,
                        UserId = value.UserId,
                        Category = value.Category,
                        Tags = value.Tags,
                        AlbumName = value.AlbumName,
                        Url = value.Url,
                        AlbumType = AlbumType,
                        apiFiles = value.imgs
                    };

                    

                    var result = GetAlbum.Manage.Update(model, value.UserId.Value);

                    // update edit count
                    if (job.EditSubmitCount == 0) // ส่งผลงานครั้งแรก
                    {
                        GetJobs.Manage.UpdateEditCount(value.JobId.Value, 1);

                    }
                    else if (job.EditSubmitCount == 1) // แก้ครั้งแรก
                    {
                        GetJobs.Manage.UpdateEditCount(value.JobId.Value, 2);
                    }
                    else if (job.EditSubmitCount == 2) // แก้ครั้งที่ 2 ครั้งสุดท้ายแล้ว
                    {
                        GetJobs.Manage.UpdateEditCount(value.JobId.Value, 3);
                    }
                    return Ok(resultJson.success("สำเร็จ", "success", new { result.JobId }));
                }
                return Ok(resultJson.errors("ข้อมูลไม่ถูกต้อง ModelState Not Valid", "fail", null));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        /// <summary>
        /// บันทึกนักแบบที่ถูกคัดเลือก 
        /// job_status=4(ประกาศ)
        /// ca_job_status=3(คัดเลือก)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("api/jobs/candidateselect")]
        [HttpPost]
        public IActionResult CandidateSelect([FromBody]CandidateSelectModel value)
        {
            try
            { 
                if (value.JobId == Guid.Empty || value.CaUserId == Guid.Empty)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }

                var job = GetJobs.Manage.UpdateCandidate(value);
                if(job == null)
                {
                    return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", null));
                } 

                return Ok(resultJson.success("สำเร็จ", "success", new { JobId=job.JobId, CaUserId=job.JobCaUserId }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        /// <summary>
        /// ดึงข้อมูลสาเหตุการยกเลิอกใบงาน
        /// </summary>
        /// <returns></returns>
        [Route("api/jobs/getcausecancel")]
        [HttpGet]
        public IActionResult GetCauseCancelList()
        {
            try
            { 
                var list = GetTmCauseCancel.Get.GetByActive();
                if (list == null)
                {
                    return Ok(resultJson.success("ไม่พบข้อมูล", "Data not found.", null));
                }

                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", list.Select(o => new { id = o.Id, name = o.Description })));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        /// <summary>
        /// แสดงรายละเอียดการส่งงานของนักออกแบบที่ได้รับเลือก (ตรวจสอบผลงาน)
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [Route("api/jobs/getapprovedetail")]
        [HttpGet]
        public IActionResult GetApproveDetail(Guid jobId)
        {
            try
            {
                var data = GetJobs.Get.GetApproveDetail(jobId);
                if (data == null || data.Count == 0)
                {
                    return Ok(resultJson.success("ไม่พบข้อมูล", "Data not found.", null));
                }

                string Host = _config.GetSection("WebConfig:AdminWebStie").Value;
                bool removeLast = Host.Last() == '/';
                string PicUrlPath = data.First().PicUrlPath;
                if (removeLast)
                {
                    Host = Host.Remove(Host.Length - 1);
                }
                PicUrlPath = PicUrlPath.Replace("~", Host);
                
                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", new { 
                    data.First().JobId,
                    data.First().JobCaUserId,
                    PicUrlPath,
                    data.First().Fullname,
                    data.First().Tel,
                    data.First().IsCanEdit,
                    data.First().IsConfirmApprove,
                    data.First().IsCusFavorite,
                    albums = data.Select(o => new { o.AlbumName, o.AlbumType, o.AlbumTypeDesc, o.Url, ImgUrlPath=o.ImgUrlPath.Replace("~", Host) }),
                } ));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        /// <summary>
        /// บันทึก review 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("api/jobs/submitjobreview")]
        [HttpPost]
        public IActionResult SubmitReview([FromBody] JobDesignerReview value)
        {
            try
            {
                var data = GetJobDesignerReview.Manage.Add(value); 

                return Ok(resultJson.success("สำเร็จ", "success", new { data.JobId }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        /// <summary>
        /// ดึงข้อมูลรายละเอียดใบงาน
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [Route("api/jobs/getjobdetail")]
        [HttpGet]
        public IActionResult GetJobDetail(Guid jobId)
        {
            try
            {
                if (jobId == Guid.Empty)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }
                var job = GetJobs.Get.GetJobDetail(jobId).FirstOrDefault();
                if (job == null)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", null));
                }
                var data = GetJobsCandidate.Get.GetByJobId(new SearchModel() { gId = jobId, statusStr = "2,3,4", statusOpt="in" });
                if (data == null || data.Count == 0)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", null));
                }

                return Ok(resultJson.success("สำเร็จ", "success", new { 
                    jobId = job.JobID,
                    job.JobNo,
                    job.JobDescription,
                    job.JobTypeName,
                    job.JobAreaSize,
                    job.JobPricePerSqM,
                    job.JobStatus,
                    job.JobPrice,
                    job.JobPriceProceed,
                    job.JobFinalPrice,
                    job.IsInvRequired,
                    job.InvAddress,
                    job.InvPersonalID,
                    job.CreatedDateStr,
                    job.UserID,
                    job.FileName,
                    job.UrlPathUserImage,
                    job.RecruitedPrice,
                    job.ContestPrice, 
                    candidates = data.Select(o => new { o.UserId, o.UserFullName, o.IsLike, o.PriceRate, o.UserRate }).ToList() }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }




    }
}
