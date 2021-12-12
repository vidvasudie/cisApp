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

    }
}
