using cisApp.Common;
using cisApp.Function;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cisApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : BaseController
    { 
        static string _DefaultProfile = "assets/media/users/100_1.jpg";

        // GET: api/<HomeController>
        [HttpPost]
        public object Post( string tags, string categories , string orderby = "", int? page = 1, int limit = 10, List<Guid> imgs = null)
        {
            try
            {
                string webAdmin = _config.GetSection("WebConfig:AdminWebStie").Value;
                SearchModel model = new SearchModel()
                {
                    Tags = tags,
                    Categories = categories,
                    Orderby = orderby,
                    currentPage = page,
                    pageSize = limit,
                    Imgs = imgs
                };
                List<AlbumImageModel> Obj = new List<AlbumImageModel>();

                Obj = GetAlbum.Get.GetAlbumFeed(model, webAdmin);

                if (Obj.Count > 0)
                {
                    return Ok(resultJson.success(null, null, Obj.Select(o => new { o.AttachFileId, o.FileName, o.FullUrlPath, o.UserId, o.JobID, o.AlbumName, o.Category, o.Tags, o.AlbumRefId }).ToList(), null, null, page, page + 1));
                }
                else
                {
                    return Unauthorized(resultJson.errors("ไม่พบข้อมูล", "ไม่พบข้อมูล", null));
                }
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
            

        }

        [HttpGet]
        public object Get(string tags, string categories, string orderby = "", int? page = 1, int limit = 10)
        {
            try
            {
                LogActivityEvent(LogCommon.LogMode.HOME, _UserId());

                string webAdmin = _config.GetSection("WebConfig:AdminWebStie").Value;
                SearchModel model = new SearchModel()
                {
                    Tags = tags,
                    Categories = categories,
                    Orderby = orderby,
                    currentPage = page,
                    pageSize = limit
                };
                List<AlbumImageModel> Obj = new List<AlbumImageModel>();

                Obj = GetAlbum.Get.GetAlbumFeed(model, webAdmin);

                if (Obj.Count > 0)
                { 
                    return Ok(resultJson.success(null, null, Obj.Select(o => new { o.AttachFileId, o.FileName, o.FullUrlPath, o.UserId, o.JobID, o.AlbumName, o.Category, o.Tags, o.AlbumRefId }).ToList(), null, null, page, page + 1));
                }
                else
                {
                    return Unauthorized(resultJson.errors("ไม่พบข้อมูล", "ไม่พบข้อมูล", null));
                }
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }


        }


        [HttpGet("GetByAttachId")]
        public object GetByAttachId(Guid? attachId, Guid? userId)
        {

            try
            {
                string webAdmin = _config.GetSection("WebConfig:AdminWebStie").Value;

                var Obj = GetAlbum.Get.GetAlbumImageByAttachId(webAdmin, attachId.Value);

                var Liked = GetPostLike.Get.GetByUserIdAndRefId(userId.Value, Obj.AttachFileId);

                var comment = GetPostComment.Get.GetByRefId(Obj.AttachFileId);

                var user = GetUser.Get.GetById(Obj.UserId.Value);

                if (Obj != null)
                {
                    return Ok(resultJson.success(null, null,
                        new 
                        {
                            AttachFileId = Obj.AttachFileId,
                            FileName = Obj.FileName,
                            FullUrlPath = Obj.FullUrlPath,
                            UserId = Obj.UserId,
                            JobID = Obj.JobID,
                            AlbumName = Obj.AlbumName,
                            Category = Obj.Category,
                            Tags = Obj.Tags,
                            AlbumRefId = Obj.AlbumRefId,
                            LikeCount = Obj.LikeCount != null ? Obj.LikeCount : 0,
                            IsLiked = Liked == null ? false : true,
                            DesignerName = user.Fname + " " + user.Lname,
                            ProfilePath = user.AttachFileImage != null ? webAdmin + user.AttachFileImage.UrlPathAPI : null,
                            CreateDate = Obj.CreatedDate,
                            CommentCount = comment.Count
                        }
                       ));
                }
                else
                {
                    return Unauthorized(resultJson.errors("ไม่พบข้อมูล", "ไม่พบข้อมูล", null));
                }
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }
    }
}
