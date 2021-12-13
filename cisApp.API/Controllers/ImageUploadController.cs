using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using cisApp.API.Models;
using cisApp.Function;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cisApp.API.Controllers
{ 
    [ApiController]
    public class ImageUploadController : BaseController
    {
        [Route("api/image/upload")]
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
                    //var athFile = GetAttachFile.Manage.UploadFile(value.FileBase64, value.FileName, value.Size, null, value.UserId);
                    using (var client = new HttpClient())
                    {
                        //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        //System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
                        client.BaseAddress = new Uri(_config.GetSection("WebConfig:AdminWebStie").Value);
                        var response = client.PostAsJsonAsync(_config.GetSection("WebConfig:UploadAPI").Value, value).Result;
                        if (response.IsSuccessStatusCode)
                        { 

                            Console.Write("Success");
                        }
                        else
                            Console.Write("Error");
                    }



                    //if (athFile == null)
                    //{
                    //    return Ok(resultJson.errors("อัพโหลดไฟล์ไม่สำเร็จ", "fail", null));
                    //}
                    //string Host = _config.GetSection("WebConfig:AdminWebStie").Value;
                    //bool removeLast = Host.Last() == '/';
                    //string UrlPath = athFile.UrlPath;
                    //if (removeLast)
                    //{
                    //    Host = Host.Remove(Host.Length - 1); 
                    //}
                    //UrlPath = UrlPath.Replace("~", Host);

                    //return Ok(resultJson.success("อัพโหลดไฟล์สำเร็จ", "success", new { athFile.AttachFileId, athFile.FileName, UrlPath }));
                    return Ok();
                }
                catch (Exception ex)
                {
                    return Ok(resultJson.errors("อัพโหลดไฟล์ไม่สำเร็จ", "fail", ex));
                }                
            }
            return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid Request.", null));
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