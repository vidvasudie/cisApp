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
    }
}
