using cisApp.Common;
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
    public class UtilController : BaseController
    { 
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
                string webAdmin = _config.GetSection("WebConfig:AdminWebStie").Value;

                var obj = GetPostComment.Get.GetByRefId(refId.Value, page, limit);

                foreach (var item in obj)
                {
                    var attch = GetUser.Get.GetUserProfileImg(item.UserId);

                    if (attch != null)
                    {
                        item.FullUrlPath = webAdmin + attch.UrlPathAPI;
                    }
                    
                }

                return Ok(resultJson.success("บันทึกข้อมูลสำเร็จ", "success", obj.OrderBy(o => o.CreatedDate).ToList()));
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
                string Host = _config.GetSection("WebConfig:AdminWebStie").Value;
                bool removeLast = Host.Last() == '/';
                if (removeLast)
                {
                    Host = Host.Remove(Host.Length - 1);
                }
                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", fav.Select(o => new {
                    userId,
                    userDesignerId = o.UserId,
                    Fullname = o.UserFullName,
                    UrlPath = o.UrlPath == null ? null : o.UrlPath.Replace("~", Host),
                }).Skip(page - 1).Take(limit).ToList()));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [Route("GetProvince")]
        [HttpGet]
        public IActionResult GetProvince()
        {
            try
            {
                //var xx = Encryption.Decrypt("s9LrP8c+HjTWUbLOve8Xhg==");
                var province = GetTmProvince.Get.GetByActive();
                if (province == null)
                {
                    return Ok(resultJson.success("ไม่พบข้อมูล", "Data not found.", null));
                }

                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", province.Select(o => new { id = o.Id, name = o.NameTh, nameEN = o.NameEn })));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [Route("GetDistrict")]
        [HttpGet]
        public IActionResult GetDistrict(int id)
        {
            try
            {
                //var xx = Encryption.Decrypt("s9LrP8c+HjTWUbLOve8Xhg==");
                var district = GetTmDistrict.Get.GetByProvinceId(id);
                if (district == null)
                {
                    return Ok(resultJson.success("ไม่พบข้อมูล", "Data not found.", null));
                }

                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", district.Select(o => new { id = o.Id, name = o.NameTh, nameEN = o.NameEn })));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [Route("GetSubDistrict")]
        [HttpGet]
        public IActionResult GetSubDistrict(int id)
        {
            try
            {
                //var xx = Encryption.Decrypt("s9LrP8c+HjTWUbLOve8Xhg==");
                var subDistrict = GetTmSubDistrict.Get.GetByDistrictId(id);
                if (subDistrict == null)
                {
                    return Ok(resultJson.success("ไม่พบข้อมูล", "Data not found.", null));
                }

                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", subDistrict.Select(o => new { id = o.Id, name = o.NameTh, nameEN = o.NameEn, zipcode = o.PostCode })));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [Route("GetText")]
        [HttpGet]
        public IActionResult GetText(string key)
        {
            try
            {
                //var xx = Encryption.Decrypt("s9LrP8c+HjTWUbLOve8Xhg==");
                var data = GetSettings.Get.GetByKeyword(key);
                if (data == null)
                {
                    return Ok(resultJson.success("ไม่พบข้อมูล", "Data not found.", null));
                }

                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", data.Content));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [Route("GetTextWithImg")]
        [HttpGet]
        public IActionResult GetTextWithImg(string key)
        {
            try
            {
                //var xx = Encryption.Decrypt("s9LrP8c+HjTWUbLOve8Xhg==");
                string webAdmin = _config.GetSection("WebConfig:AdminWebStie").Value;
                var data = GetSettings.Get.GetByKeyword(key, webAdmin);
                if (data == null)
                {
                    return Ok(resultJson.success("ไม่พบข้อมูล", "Data not found.", null));
                }

                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", data));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [Route("GetWinner")]
        [HttpGet]
        public IActionResult GetWinnerList()
        {
            try
            {
                LogActivityEvent(LogCommon.LogMode.WINNER_LIST, _UserId());

                var wins = GetJobs.Get.GetWinnerSummary();
                string Host = _config.GetSection("WebConfig:AdminWebStie").Value;
                bool removeLast = Host.Last() == '/';
                if (removeLast)
                {
                    Host = Host.Remove(Host.Length - 1);
                }
                return Ok(resultJson.success("ดึงข้อมูลสำเร็จ", "success", wins.Select(o => new { 
                    o.Rownum,
                    o.UserId,
                    o.Fullname,
                    UrlPath = string.IsNullOrEmpty(o.AttachFileName) ? null : o.UrlPath.Replace("~", Host),
                    o.WinCount
                })));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ดึงข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }


    }
}
