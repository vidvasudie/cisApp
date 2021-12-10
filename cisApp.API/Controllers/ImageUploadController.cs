using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.API.Models;
using cisApp.Function;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cisApp.API.Controllers
{
    [Route("api/imageupload")]
    [ApiController]
    public class ImageUploadController : BaseController
    {
        [HttpPost]
        public IActionResult Upload([FromBody] UploadAPIModel value)
        {
            
            if (ModelState.IsValid)
            {
                if(Guid.Empty == value.UserId)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }
                try
                {
                    var athFile = GetAttachFile.Manage.UploadFile(value.FileBase64, value.FileName, value.Size, null, value.UserId);
                    if (athFile == null)
                    {
                        return Ok(resultJson.errors("อัพโหลดไฟล์ไม่สำเร็จ", "fail", null));
                    }
                    string Host = _config.GetSection("AttachDomain:Host").Value;
                    bool removeLast = Host.Last() == '/';
                    string UrlPath = athFile.UrlPath;
                    if (removeLast)
                    {
                        UrlPath = UrlPath.Remove(UrlPath.Length - 1); 
                    }
                    UrlPath = UrlPath.Replace("~", Host);

                    return Ok(resultJson.success("อัพโหลดไฟล์สำเร็จ", "success", new { athFile.AttachFileId, athFile.FileName, UrlPath }));
                }
                catch (Exception ex)
                {
                    return Ok(resultJson.errors("อัพโหลดไฟล์ไม่สำเร็จ", "fail", ex));
                }                
            }
            return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
        }
    }
}