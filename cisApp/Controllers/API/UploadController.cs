using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cisApp.Controllers.API
{
    
    [ApiController]
    public class UploadController : ControllerBase
    {
        [Route("api/upload")]
        [HttpPost]
        public IActionResult Upload([FromBody] UploadAPIModel value)
        { 
            try
            {
                if (Guid.Empty == value.UserId)
                {
                    return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
                }

                var athFile = GetAttachFile.Manage.UploadFile(value.FileBase64, value.FileName, value.Size, null, value.UserId);
                if (athFile == null)
                {
                    return Ok(resultJson.errors("อัพโหลดไฟล์ไม่สำเร็จ", "fail", null));
                }
                string UrlPath = athFile.UrlPath;

                return Ok(resultJson.success("อัพโหลดไฟล์สำเร็จ", "success", new { athFile.AttachFileId, athFile.FileName, UrlPath }));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("อัพโหลดไฟล์ไม่สำเร็จ", "fail", ex));
            }
        }
    }
}