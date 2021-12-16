using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cisApp.API.Controllers
{ 
    [ApiController]
    public class DesignerFuncController : BaseController
    {
        [Route("api/designer/joblist")]
        [HttpGet]
        public IActionResult JobList(Guid userId, string text, int? skip=0, int? take=10)
        {
            try
            {
                var model = new DesignerJobListSearch() { userId= userId, text= text, skip= skip, take= take };
                //get list of jobs designer can sign
                var jobs = GetUserDesigner.Get.GetJobListSearch(model);
                if (jobs == null || jobs.Count() == 0)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", null));
                }


                foreach (var j in jobs)
                {
                    //get candidate with status 2:อยู่ระหว่างประกวด
                    j.jobCandidates = GetJobsCandidate.Get.GetByJobId(new SearchModel() { gId = j.JobID, status = 2 });

                    //get jobeximage 
                    j.jobsExamImages = GetJobsExamImage.Get.GetImageByJobId(j.JobID);
                }

                return Ok(resultJson.success("สร้างใบงานสำเร็จ", "success", jobs, model.take, jobs.Count()));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

    }
}