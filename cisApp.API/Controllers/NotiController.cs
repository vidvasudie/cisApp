using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;
using cisApp.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cisApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotiController : ControllerBase
    {
     
        // GET api/<NotiController>/5
        [HttpGet("{id}")]
        public object Get(Guid userId)
        {
            var result = GetNotification.Get.GetbyUserID(userId);
            return Ok(resultJson.success("บันทึกข้อมูลสำเร็จ", "success", new
            {
                result
            }));
        }
       
        // PUT api/<NotiController>/5
        [HttpPut("{id}")]
        public object Put(Guid userId , [FromBody]NotiModel value)
        {

            GetNotification.Manage.Read(value.ID, userId);
            return Ok(resultJson.success("บันทึกข้อมูลสำเร็จ", "success", new
            {
                 
            }));

        }
        public class NotiModel
        { 
            public int ID { get; set; }


        }

    }
}
