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
    public class JobsFuncController : ControllerBase
    {
        [Route("api/[controller]")]
        [HttpPost]
        public object Post([FromBody] JobAPIModels value)
        { 
           var result=  GetJobs.Manage.Update(new JobModel
            {
                IsDraft = value.IsDraft,
                JobTypeId = value.JobTypeId, 
                JobDescription = value.JobDescription,
                JobAreaSize = value.JobAreaSize,
                JobPrice = value.JobPrice,
                JobPricePerSqM = value.JobPricePerSqM
            });

            return Ok(resultJson.success("สร้างใบงานสำเร็จ","success", new { result.JobId })); 
        }


        [Route("api/[controller]/GetType")]
        [HttpGet]
        public object GetType()
        {

            var obj = GetJobsType.Get.GetActive().Select(o => new { o.JobTypeId, o.Name }).ToList();
            return Ok(resultJson.success( "", "", new { obj }));
        }



        //// PUT api/<JobsFuncController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<JobsFuncController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
