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

        [Route("api/designer/canceljob")]
        [HttpDelete]
        public IActionResult CancelJob(Guid userId, Guid jobId, string ip)
        {
            try
            {
                if (jobId == Guid.Empty || userId == Guid.Empty)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }

                var job = GetJobs.Get.GetById(jobId);
                if (job == null)
                { 
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "fail", null));
                }
                if(job.JobStatus != 2) //รอ
                {
                    return Ok(resultJson.errors("ไม่สามารถยกเลิกได้", "fail", null));
                }

                var jobCa = GetJobsCandidate.Manage.Reject(jobId, userId, userId, ip);
                if (job == null)
                {
                    return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", null));
                }
                return Ok(resultJson.success("สำเร็จ", "success", new { job.JobId }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [Route("api/designer/jobsubmitlist")]
        [HttpGet]
        public IActionResult GetJobSubmitList(Guid userId, string text, int? skip = 0, int? take = 10)
        {
            try
            {
                var model = new DesignerJobListSearch() { userId = userId, text = text, skip = skip, take = take };
                //get list of jobs designer can sign
                var jobs = GetUserDesigner.Get.GetJobListSearch(model, true);
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

        [Route("api/designer/jobsubmitdetail")]
        [HttpGet]
        public IActionResult GetSubmitJobDetail(Guid userId, Guid jobId)
        {
            try
            {
                var model = new DesignerJobListSearch() { userId = userId };
                //get list of jobs designer can sign
                var jobs = GetUserDesigner.Get.GetJobListSearch(model);
                if (jobs == null || jobs.Count() == 0)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", null));
                }
                var job = jobs.Where(o => o.JobID == jobId);
                if (job == null)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", null));
                }
                var j = job.FirstOrDefault();
                //get candidate with status 2:อยู่ระหว่างประกวด
                j.jobCandidates = GetJobsCandidate.Get.GetByJobId(new SearchModel() { gId = jobId });

                //get jobeximage 
                j.jobsExamImages = GetJobsExamImage.Get.GetImageByJobId(jobId);
                 
                return Ok(resultJson.success("สร้างใบงานสำเร็จ", "success", new List<DesignerJobListModel> { j }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [Route("api/designer/jobcontestlist")]
        [HttpGet]
        public IActionResult GetJobContestList(Guid userId, int? skip = 0, int? take = 10)
        {
            try
            {
                var model = new DesignerJobListSearch() { userId = userId, skip = skip, take = take };
                //get list of jobs designer contest now
                var jobs = GetUserDesigner.Get.GetJobContestList(model);
                if (jobs == null || jobs.Count() == 0)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", null));
                }

                foreach (var j in jobs)
                {
                    //get candidate with already submit work
                    j.jobCandidates = GetJobsCandidate.Get.GetDesignerJobSubmitList(new SearchModel() { gId = j.JobID });
                    j.JobUserSubmitCount = j.jobCandidates != null ? j.jobCandidates.Count() : 0;
                }

                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", jobs, model.take, jobs.Count()));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }

        }

        [Route("api/designer/review")]
        [HttpGet]
        public IActionResult GetReviewList(Guid userId, Guid jobId, int? skip = 0, int? take = 10)
        {
            try
            {
                var model = new DesignerJobListSearch() { userId = userId, jobId = jobId, skip = skip, take = take };
                //get list of  designer review
                var reviews = GetUserDesigner.Get.GetReview(model);
                if (reviews == null || reviews.Count() == 0)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", null));
                }

                string Host = _config.GetSection("WebConfig:AdminWebStie").Value;
                bool removeLast = Host.Last() == '/';
                string PicUrlPath = reviews.First().UrlPath;
                if (removeLast)
                {
                    Host = Host.Remove(Host.Length - 1);
                }
                PicUrlPath = PicUrlPath.Replace("~", Host);

                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", new { 
                    reviews.FirstOrDefault().DesignerUserId,
                    reviews.FirstOrDefault().DesignerFullname,
                    PicUrlPath,
                    reviews = reviews.Select(o => new { o.UserId, o.Fullname, o.Rate, o.RateAll, o.Message })
                }, model.take, reviews.Count()));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }

        }

        [Route("api/designer/contestsummary")]
        [HttpGet]
        public IActionResult GetContestSummary(Guid userId)
        {
            try
            {
                //get list of  designer review
                var summaryList = GetUserDesigner.Get.GetContestSummary(userId);
                if (summaryList == null || summaryList.Count == 0)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", null));
                }
                 
                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", new
                {
                    summaryList.FirstOrDefault().JobCaUserID,
                    summaryList.FirstOrDefault().SummaryAll,
                    summaryGroup = summaryList.Select(o => new { o.JobTypeDesc, o.Total })
                }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }

        }

        [Route("api/designer/favorite")]
        [HttpPost]
        public IActionResult UserFavorite([FromBody]FavoriteModel value)
        {
            try
            {
                if (value.UserId == Guid.Empty || value.CaUserId == Guid.Empty)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }

                var fav = GetUserFavoriteDesigner.Manage.Update(value);
                if (fav == null)
                {
                    return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", null));
                }

                return Ok(resultJson.success("สำเร็จ", "success", new { caUserId=fav.UserDesignerId }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [Route("api/designer/jobshistory")]
        [HttpGet]
        public IActionResult GetJobsHistory(Guid userId, int? skip = 0, int? take = 10)
        {
            try
            {
                var model = new DesignerJobListSearch() { userId = userId, skip = skip, take = take };
                //get list of job history
                var histList = GetUserDesigner.Get.GetJobsHistory(model);
                if (histList == null || histList.Count == 0)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", null));
                }

                string Host = _config.GetSection("WebConfig:AdminWebStie").Value;
                bool removeLast = Host.Last() == '/'; 
                if (removeLast)
                {
                    Host = Host.Remove(Host.Length - 1);
                } 

                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", histList.Select(o => new 
                { 
                    o.IsCusFavorite,
                    o.JobId,
                    o.JobNo,
                    o.JobDescription,
                    o.JobTypeName,
                    o.JobAreaSize,
                    o.CreatedDateStr,
                    o.UserId,
                    o.Rate,
                    o.Fullname,
                    PicUrlPath= o.UrlPath.Replace("~", Host)
                }) ));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }

        }

        [Route("api/designer/profile")]
        [HttpGet]
        public IActionResult GetProfile(Guid userId, int? skip = 0, int? take = 100)
        {
            try
            { 
                var dpf = GetUserDesigner.Get.GetDesignerProfile(userId);
                if (dpf == null)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", null));
                }
                var model = new DesignerJobListSearch() { userId = userId, skip = skip, take = take };
                var imgs = GetUserDesigner.Get.GetDesignerAlbumImage(model);
                if (imgs == null)
                {
                    imgs = new List<AlbumModel>();
                }
                 
                string Host = _config.GetSection("WebConfig:AdminWebStie").Value;
                bool removeLast = Host.Last() == '/';
                if (removeLast)
                {
                    Host = Host.Remove(Host.Length - 1);
                }

                var albs = imgs.Select(o => o.AlbumName).Distinct();


                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", new 
                { 
                    dpf.DesignerUserId,
                    dpf.Fullname,
                    picUrl = dpf.UrlPath.Replace("~", Host),
                    dpf.RateAll,
                    dpf.RateCount,
                    dpf.ContestWinTotal,
                    dpf.AreaSQMRate,
                    dpf.AreaSQMMax,
                    dpf.AreaSQMUsed,
                    dpf.AreaSQMRemain,
                    albums = imgs.Where(o => o.AttachFileName != null).Select(o => new { o.AlbumName, imageUrl = o.UrlPath.Replace("~", Host), o.AttachFileName })
                }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }

        }

    }
}