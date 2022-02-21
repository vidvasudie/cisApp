using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Core;
using cisApp.Function;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cisApp.API.Controllers
{ 
    [ApiController]
    public class DesignerFuncController : BaseController
    {
        /// <summary>
        /// แสดงรายการงานที่สามารถสมัครได้ ตามเงื่อนไข
        /// - slot งานที่เหลืออยู่
        /// - rate งานที่รับได้
        /// - ลูกค้ากด Favorite
        /// - วันหมดอายุการรับสมัคร
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="text"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
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
                //jobs = jobs.Where(o => o.UserID != userId).ToList();
                List<DesignerJobListModel> tmp = new List<DesignerJobListModel>();
                int maxDayWait = int.Parse(_config.GetSection("JobProcess:WaitCaSubmit").Value);
                foreach (var j in jobs)
                {
                    if (j.StatusDate.Value.AddDays(maxDayWait).Ticks > DateTime.Now.Ticks)
                    {
                        //get candidate with status 1:สมัคร
                        j.jobCandidates = GetJobsCandidate.Get.GetByJobId(new SearchModel() { gId = j.JobID, statusStr = "1" });

                        //get jobeximage 
                        j.jobsExamImages = GetJobsExamImage.Get.GetImageByJobId(j.JobID);

                        tmp.Add(j);
                    }
                    
                } 

                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", tmp.Select(o => new {
                    o.IsCusFavorite,
                    o.JobID,
                    o.JobNo,
                    o.JobTypeName,
                    o.JobDescription,
                    o.JobAreaSize,
                    //o.JobPrice,
                    //o.JobPriceProceed,
                    //o.JobFinalPrice,
                    //o.JobPricePerSqM,
                    //o.JobStatus,
                    //o.IsInvRequired,
                    //o.InvAddress,
                    //o.InvPersonalID,
                    o.CreatedDate,
                    o.CreatedDateStr,
                    o.UserID,
                    o.Fullname,
                    o.FileName,
                    o.UrlPathUserImage,
                    o.RecruitedPrice,
                    o.ContestPrice,
                    JobCandidates = o.jobCandidates.Select(s => new { caUserId = s.UserId, caFullname = s.UserFullName, s.UrlPathAPI }),
                    JobsExamImages = o.jobsExamImages.Select(s => new { s.UrlPathAPI })
                }).ToList(), model.take, jobs.Count()));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        /// <summary>
        /// บันทึกข้อมูลการยกเลิกการสมัคร เข้าประกวดงาน
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jobId"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
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

        /// <summary>
        /// หน้าแสดงรายการงานทั้งกมดที่สมัครไปแล้ว ยังอยู่ในสถานะรอ 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="text"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
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
                    j.jobCandidates = GetJobsCandidate.Get.GetByJobId(new SearchModel() { gId = j.JobID, statusStr = "1" });

                    //get jobeximage 
                    j.jobsExamImages = GetJobsExamImage.Get.GetImageByJobId(j.JobID);
                }

                return Ok(resultJson.success("สร้างใบงานสำเร็จ", "success", jobs.OrderByDescending(o => o.CreatedDate), model.take, jobs.Count()));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        /// <summary>
        /// แสดงรายละเอียดงานท่ี่ได้สมัครไว้ 
        /// - คำนวณ ระยะเวลา การส่งงาน 10 ตรม ต่อ 2.5 วัน
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [Route("api/designer/jobsubmitdetail")]
        [HttpGet]
        public IActionResult GetSubmitJobDetail(Guid userId, Guid jobId)
        {
            try
            { 
                //get list of jobs designer can sign with validate massage
                var jobs = GetUserDesigner.Get.GetJobDetailValid(userId, jobId);
                if (jobs == null || jobs.Count() == 0)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", null));
                } 
                var j = jobs.FirstOrDefault();  

                //get candidate with status 2:อยู่ระหว่างประกวด  
                j.jobCandidates = GetJobsCandidate.Get.GetByJobId(new SearchModel() { gId = jobId, statusStr = "1" });

                //get jobeximage  
                j.jobsExamImages = GetJobsExamImage.Get.GetImageByJobId(jobId);

                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", new List<object>() { 
                    new {
                        j.IsCusFavorite,
                        j.JobID,
                        j.JobNo,
                        j.JobTypeName,
                        j.JobDescription,
                        j.JobAreaSize,
                        j.JobPrice,
                        j.JobPriceProceed,
                        j.JobFinalPrice,
                        j.JobPricePerSqM,
                        j.JobStatus, 
                        j.JobEndDate,
                        j.IsInvRequired,
                        j.InvAddress,
                        j.InvPersonalID,
                        j.CreatedDate,
                        j.CreatedDateStr,
                        j.UserID,
                        j.Fullname,
                        j.FileName,
                        j.UrlPathUserImage,
                        j.RecruitedPrice,
                        j.ContestPrice,
                        j.ValidMassage,
                        IsCanSubmit = String.IsNullOrEmpty(j.ValidMassage),
                        JobCandidates = j.jobCandidates.Select(s => new { caUserId = s.UserId, caFullname = s.UserFullName, s.UrlPathAPI }),
                        JobsExamImages = j.jobsExamImages.Select(s => new { s.UrlPathAPI, s.JobsExTypeDesc })
                    }
                })); 
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        /// <summary>
        /// บันทึกข้อมูลการสมัครเข้าประกวดงาน
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("api/designer/submitjob")]
        [HttpPost]
        public IActionResult SubmitJob([FromBody]JobCandidateModel value)
        {
            try
            {
                if (value.JobId == Guid.Empty || value.UserId == Guid.Empty)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }

                var jobs = GetUserDesigner.Get.GetJobDetailValid(value.UserId.Value, value.JobId.Value);
                if (!String.IsNullOrEmpty(jobs.FirstOrDefault().ValidMassage))
                {
                    return Ok(resultJson.errors(jobs.FirstOrDefault().ValidMassage, "Can not submit job.", null));
                }

                var job = GetJobsCandidate.Manage.Add(value.JobId.Value, value.UserId.Value, value.Ip);
                if (job == null)
                {
                    return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ นักออกแบบไม่สามารถสมัครงานเดิมได้เกิน 2 ครั้ง", "fail", null));
                }
                
                return Ok(resultJson.success("สำเร็จ", "success", new { job.JobId }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        /// <summary>
        /// แสดงรายการงานของนักออกแบบ ที่สถานะอยู่ระหว่างประกวด
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
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
                    var albs = GetAlbum.Get.GetByJobId(j.JobID);
                    int jobAlbCount = 0;
                    if(albs != null && albs.Count > 0)
                    {
                        jobAlbCount = albs.Where(o => o.AlbumType == "1").Select(o => o.UserId).GroupBy(g => g.Value).Count();
                    }
                    j.jobCandidates = (from jc in j.jobCandidates
                                       join alb in albs.Where(o => o.AlbumType == "1") on jc.UserId equals alb.UserId
                                       select jc).ToList();
                    j.JobUserSubmitCount = jobAlbCount;
                }

                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", jobs, model.take, jobs.Count()));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }

        }

        /// <summary>
        /// แสดงข้อมูล review ของนักออกแบบตามใบงานหรือทังหมด
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jobId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
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
                    reviews.FirstOrDefault().PositionName,
                    PicUrlPath,
                    reviews = reviews.Select(o => new { o.UserId, o.Fullname, o.Rate, o.RateAll, o.Message })
                }, model.take, reviews.Count()));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }

        }

        /// <summary>
        /// แสดงข้อมูลประวัติการประกวด แยกตามประเภทใบงาน
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// บันทึกการกด favorite นักออกแบบ (toggle)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// บันทึกการกด bookmark นักออกแบบ (toggle)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("api/designer/bookmark")]
        [HttpPost]
        public IActionResult UserBookmark([FromBody]FavoriteModel value)
        {
            try
            {
                if (value.UserId == Guid.Empty || value.CaUserId == Guid.Empty)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }

                var fav = GetUserBookmarkDesigner.Manage.Update(value);
                if (fav == null)
                {
                    return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", null));
                }

                return Ok(resultJson.success("สำเร็จ", "success", new { caUserId = fav.UserDesignerId }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        /// <summary>
        /// แสดงรายการประวัติงานที่เคยประกวด 
        /// - ไม่ีผ่านการประกวด
        /// - ผ่านการประกวด และสิ้นสุดแล้ว
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
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
                var aLImg = GetAlbum.Get.GetRandomAlbumImage(Host + "/", userId, 1);
                  
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
                    IsWin=!String.IsNullOrEmpty(o.WinText),
                    o.WinText,
                    ImgCoverUrl = aLImg.First().FullUrlPath,
                    PicUrlPath = o.UrlPath.Replace("~", Host)
                }) ));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }

        }

        /// <summary>
        /// แสดงข้อมูลนักออกแบบ
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [Route("api/designer/profile")]
        [HttpGet]
        public IActionResult GetProfile(Guid userDesignerId, Guid userId, int? skip = 0, int? take = 100)
        {
            try
            { 
                var dpf = GetUserDesigner.Get.GetDesignerProfile(userDesignerId, userId);
                if (dpf == null)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", null));
                }
                var model = new DesignerJobListSearch() { userId = userDesignerId, skip = skip, take = take };
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
                    dpf.PositionName,
                    dpf.IsFavorite,
                    dpf.Caption,
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