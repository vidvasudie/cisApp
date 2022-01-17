using cisApp.Function;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cisApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilController : ControllerBase
    {
        readonly static IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json")
                      .Build();

        // GET: api/<HomeController>
        [HttpPost("PostLike")]
        public object PostLike(Guid? refId, Guid? userId, bool isActive = true)
        {
            try
            {
                var obj = GetPostLike.Manage.Update(userId.Value, refId.Value, isActive);

                return Ok(resultJson.success("บันทึกข้อมูลสำเร็จ", "success", obj));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [HttpPost("PostComment")]
        public object PostComment(Guid? refId, Guid? userId, string comment)
        {
            try
            {
                var obj = GetPostComment.Manage.Add(userId.Value, refId.Value, comment);

                return Ok(resultJson.success("บันทึกข้อมูลสำเร็จ", "success", obj));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [HttpGet("GetComment")]
        public object GetComment(Guid? refId, int page = 1, int limit = 10)
        {
            try
            {
                string webAdmin = config.GetSection("WebConfig:AdminWebStie").Value;

                var obj = GetPostComment.Get.GetByRefId(refId.Value, page, limit);

                foreach (var item in obj)
                {
                    var attch = GetUser.Get.GetUserProfileImg(item.UserId);

                    item.FullUrlPath = webAdmin + attch.UrlPathAPI;
                }

                return Ok(resultJson.success("บันทึกข้อมูลสำเร็จ", "success", obj));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [HttpGet("GetFaq")]
        public object GetQFaq(int page = 1, int limit = 10)
        {
            try
            {
                var faq = GetFaq.Get.GetActive(); 
                 
                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", faq.Select(o => new { 
                    o.Qorder,
                    o.Question,
                    o.Answer
                }).Skip(page - 1).Take(limit).ToList()));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [HttpPost("PostHelp")]
        public object PostUserHelp(string tel, string email, string message)
        {
            try
            {
                var obj = GetUserHelp.Manage.Add(tel, email, message);

                return Ok(resultJson.success("บันทึกข้อมูลสำเร็จ", "success", new { obj.Id }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [HttpGet("GetFavorite")]
        public object GetFavoriteList(Guid userId, int page = 1, int limit = 10)
        {
            try
            {
                var fav = GetUserFavoriteDesigner.Get.GetFavoriteList(userId, page, limit);
                string Host = config.GetSection("WebConfig:AdminWebStie").Value;
                bool removeLast = Host.Last() == '/';
                if (removeLast)
                {
                    Host = Host.Remove(Host.Length - 1);
                }
                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", fav.Select(o => new {
                    userId,
                    Fullname = o.UserFullName,
                    UrlPath = o.UrlPath == null ? null : o.UrlPath.Replace("~", Host),
                }).Skip(page - 1).Take(limit).ToList()));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

    }
}
