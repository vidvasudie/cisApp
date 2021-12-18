using cisApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cisApp.API.Controllers
{

    [ApiController]
    public class JobsFuncController : BaseController
    {
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

        [Route("api/jobs/canceljob")]
        [HttpDelete]
        public IActionResult CancelJob(Guid jobId, Guid userId, string ip)
        {
            try
            {
                if (jobId == Guid.Empty || userId == Guid.Empty)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }

                var pm = GetJobPayment.Get.GetByJobId(jobId);
                if(pm.Where(o => o.PayStatus > 1).Count() > 0) //1=รอชำระเงิน
                {
                    //ถ้าจ่ายแล้ว ยกเลิกไม่ได้
                    return Ok(resultJson.errors("ไม่สามารถยกเลิกได้ เมื่อมีการชำระเงินแล้ว", "fail", null));
                }
                 
                var job = GetJobs.Manage.CancelJob(jobId, userId, ip);
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

        [Route("api/jobs/getcandidatelist")]
        [HttpGet]
        public IActionResult GetCandidateList(Guid jobId)
        {
            try
            {
                if (jobId == Guid.Empty)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }

                var data = GetJobsCandidate.Get.GetByJobId(new SearchModel() { gId= jobId });
                if(data == null || data.Count == 0)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", null));
                }
                
                return Ok(resultJson.success("สำเร็จ", "success", data.Select(o => new { o.UserId, o.UserFullName, o.IsLike, o.PriceRate, o.UserRate }).ToList()));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

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
    
    
    }
}
