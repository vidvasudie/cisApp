using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cisApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtpController : ControllerBase
    {
     
        [HttpPost]
        public object Post([FromBody]OtpModel value)
        {
            if(value.ActivityID == null || string.IsNullOrEmpty( value.mobile))
            {
                return NotFound(resultJson.errors("ข้อมูลไม่ถูกต้อง", "Error" ,null)); 
            }
            else
            {
                return Ok(resultJson.success(null, null, null, null)); 
            } 
        }
         
    }
}
