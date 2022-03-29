using cisApp.Designer.Models;
using cisApp.Function;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.Designer.Controllers
{
    [CustomActionExecute]
    public class AlbumController : BaseController
    {

        private readonly ILogger<AlbumController> _logger;
        public AlbumController(ILogger<AlbumController> logger)
        {
            _logger = logger;
        } 


        [HttpGet]
        public IActionResult Submitwork(Guid id, Guid userId, string AlbumType = "1")
        {
            try
            {
                AlbumType = "1";

                var job = GetJobs.Get.GetById(id);

                if (job.JobStatus < 4) // ถ้า job status ไม่เท่ากับ 4 จะเป็นการประกวดทั้งหมด
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
                    if (job.JobStatus == 9)
                    {
                        AlbumType = "3";
                    }
                    else
                    {
                        AlbumType = "2";
                    }
                }
                else if (job.EditSubmitCount == 2) // แก้ครั้งที่ 2 ครั้งสุดท้ายแล้ว
                {
                    if (job.JobStatus == 9)
                    {
                        AlbumType = "4";
                    }
                    else
                    {
                        AlbumType = "3";
                    }
                }
                else // เกินกว่านี้ เตะออก
                {
                    //return Json(new ResponseModel().ResponseError("ไม่สามารถส่งงานดังกล่าวได้เนื่องจากสถานะงาน ไม่ได้อยู่ในสถานะที่ส่งงานได้"));
                }

                var album = GetAlbum.Get.GetByJobIdWithStatus(id, userId, AlbumType);

                AlbumModel model = new AlbumModel()
                {
                    JobId = id,
                    UserId = userId,
                    AlbumType = AlbumType
                };

                if (album != null)
                {
                    model = GetAlbum.Utils.ToAlbumModel(album);

                    model.files = new List<FileAttachModel>();

                    var imgs = GetAlbum.Get.GetAttachFileByAlbumId(album.AlbumId.Value, "");

                    foreach (var item in imgs)
                    {
                        model.files.Add(GetAttachFile.Utils.ToFileAttachModel(item));
                    }
                }

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

                if (job.JobStatus >= 4 && data.UserId != job.JobCaUserId)
                {
                    return Json(new ResponseModel().ResponseError("บันทึกข้อมูลไม่สำเร็จ เฉพาะผู้ผ่านการประกวดเท่านั้นที่สามารถส่งงานได้")); ;
                }

                if (job.JobStatus < 4) // ถ้า job status ไม่เท่ากับ 4 จะเป็นการประกวดทั้งหมด
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
                    if (job.JobStatus == 9)
                    {
                        AlbumType = "3";
                    }
                    else
                    {
                        AlbumType = "2";
                    }
                }
                else if (job.EditSubmitCount == 2) // แก้ครั้งที่ 2 ครั้งสุดท้ายแล้ว
                {
                    if (job.JobStatus == 9)
                    {
                        AlbumType = "4";
                    }
                    else
                    {
                        AlbumType = "3";
                    }
                }
                else // เกินกว่านี้ เตะออก
                {
                    return Json(new ResponseModel().ResponseError("ไม่สามารถส่งงานดังกล่าวได้เนื่องจากสถานะงาน ไม่ได้อยู่ในสถานะที่ส่งงานได้"));
                }

                data.AlbumType = AlbumType;

                var result = GetAlbum.Manage.Update(data, _UserId().Value, false);

                if (job.JobStatus < 4)
                {

                }
                // update edit count
                else if (job.EditSubmitCount == 0) // ส่งผลงานครั้งแรก
                {
                    GetJobs.Manage.UpdateEditCount(data.JobId, 1);
                }
                else if (job.JobStatus == 9 && job.EditSubmitCount == 1) // แก้ครั้งแรก
                {
                    GetJobs.Manage.UpdateEditCount(data.JobId, 2);
                    GetJobs.Manage.UpdateJobStatus(job.JobId, 5);
                }
                else if (job.JobStatus == 9 && job.EditSubmitCount == 2) // แก้ครั้งที่ 2 ครั้งสุดท้ายแล้ว
                {
                    GetJobs.Manage.UpdateEditCount(data.JobId, 3);
                    GetJobs.Manage.UpdateJobStatus(job.JobId, 5);
                }

                if (job.JobStatus == 4)
                {
                    job.JobStatus = 5;
                    GetJobs.Manage.UpdateJobStatus(job.JobId, 5);
                }

                if (result != null)
                {
                    return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "JobList")));
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

        [HttpGet]
        public IActionResult Editwork(int? albumId)
        {
            try
            {

                if (albumId == null)
                {
                    throw new Exception("albumId null Exception");
                }

                var album = GetAlbum.Get.GetById(albumId.Value);

                AlbumModel model = null;
                
                if (album != null)
                {
                    model = GetAlbum.Utils.ToAlbumModel(album);

                    model.files = new List<FileAttachModel>();

                    var imgs = GetAlbum.Get.GetAttachFileByAlbumId(album.AlbumId.Value, "");

                    foreach (var item in imgs)
                    {
                        model.files.Add(GetAttachFile.Utils.ToFileAttachModel(item));
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult Editwork(AlbumModel data)
        {
            try
            {
                var result = GetAlbum.Manage.Update(data, _UserId().Value, false);

                if (result != null)
                {
                    return Json(new ResponseModel().ResponseSuccess(MessageCommon.SaveSuccess, Url.Action("Index", "JobList")));
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
