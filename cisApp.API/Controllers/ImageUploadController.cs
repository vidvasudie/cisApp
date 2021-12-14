using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using cisApp.API.Models;
using cisApp.Core;
using cisApp.Function;
using DIGITAL_ID.library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cisApp.API.Controllers
{ 
    [ApiController]
    public class ImageUploadController : BaseController
    {
        [Route("api/image/upload")]
        [HttpPost]
        public IActionResult Upload([FromBody] List<UploadAPIModel> value)
        {

            if (value == null || value.Count == 0)
            {
                return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
            }
            List<object> listFiles = new List<object>();
            foreach (var img in value)
            {
                try
                {
                    if (Guid.Empty == img.UserId)
                    {
                        return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                    }

                    var athFile = GetAttachFile.Manage.UploadFile(img.FileBase64, img.FileName, img.Size, null, img.UserId);
                    if (athFile == null)
                    {
                        return Ok(resultJson.errors("อัพโหลดไฟล์ไม่สำเร็จ", "fail", null));
                    }
                    string Host = _config.GetSection("WebConfig:AdminWebStie").Value;
                    bool removeLast = Host.Last() == '/';
                    string UrlPath = athFile.UrlPath;
                    if (removeLast)
                    {
                        Host = Host.Remove(Host.Length - 1);
                    }
                    UrlPath = UrlPath.Replace("~", Host);

                    listFiles.Add(new { athFile.AttachFileId, athFile.FileName, UrlPath });
                }
                catch (Exception ex)
                {
                    return Ok(resultJson.errors("อัพโหลดไฟล์ไม่สำเร็จ", "fail", ex));
                }
            }
            return Ok(resultJson.success("อัพโหลดไฟล์สำเร็จ", "success", listFiles));
        }

        [Route("api/image/profile")]
        [HttpGet]
        public IActionResult ProfilePreview(Guid userId)
        {
            try
            {
                if (Guid.Empty == userId)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }
                var athFile = GetAttachFile.Get.GetByUserId(userId);
                if (athFile == null)
                {
                    return Ok(resultJson.errors("ไม่พบข้อมูล", "Data not found.", null));
                }
                string Host = _config.GetSection("WebConfig:AdminWebStie").Value;
                bool removeLast = Host.Last() == '/';
                string UrlPath = athFile.UrlPath;
                if (removeLast)
                {
                    Host = Host.Remove(Host.Length - 1);
                }
                UrlPath = UrlPath.Replace("~", Host);

                return Ok(resultJson.success("สำเร็จ", "success", new { athFile.AttachFileId, athFile.FileName, UrlPath }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("ค้นหาไม่สำเร็จ", "fail", ex));
            }
        }
    }
}