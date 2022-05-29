using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.API.Models;
using cisApp.Function;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using cisApp.Core;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace cisApp.API.Controllers
{ 
    [ApiController]
    [Route("api/[controller]")]
    public class UserBookmarkController : BaseController
    {

        public static IConfigurationRoot _config = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .Build();

        [HttpPost("Add")]
        public object Add(UserBookMarkImageInputModel value)
        {
            try
            {
                GetUserBookmarkImage.Manage.Add(value.UserId, value.RefId);

                return Ok(resultJson.success(null, null, null, null, null, null, null));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        [HttpPost("Delete")]
        public object Delete(UserBookMarkImageInputModel value)
        {
            try
            {
                GetUserBookmarkImage.Manage.Delete(value.UserId, value.RefId);

                return Ok(resultJson.success(null, null, null, null, null, null, null));
            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }

        
        [HttpGet("Search")]
        public object Get(Guid? userId, int? page = 1, int? limit = 10)
        {

            try
            {

                var data = GetUserBookmarkImage.Get.Search(userId.Value, page.Value, limit.Value);

                var total = GetUserBookmarkImage.Get.Total(userId.Value);
                
                return Ok(resultJson.success(null, null, data, null, total, null, null));

            }
            catch (Exception ex)
            {
                return Ok(resultJson.errors("บันทึกข้อมูลไม่สำเร็จ", "fail", ex));
            }
        }


        public class UserBookMarkImageInputModel
        {
            public Guid UserId { get; set; }
            public Guid RefId { get; set; }
        }
        
        

        
    }
}