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
    public class UserController : ControllerBase
    {
        /// <summary>
        /// สมัครสมาชิก
        /// </summary>
        /// <param name="value"></param>
        [HttpPost("register")]
        public object register([FromBody] UserModelCommon value)
        {

            var _result = GetUser.Manage.Update(new UserModel() { Fname= value.Fname, Lname = value.Lname, Tel = value.Tel, Email =value.Tel,UserType = 1 , IsActive = true});
           
            
            return Ok(resultJson.success("บันทึกข้อมูลสำเร็จ", _result));

        }


        /// <summary>
        /// แก้ไขรหัสผ่าน
        /// </summary>
        /// <param name="value"></param>
        [HttpPost("resetpass")]
        public void resetpass([FromBody] string value)
        {
        }




    }
}
