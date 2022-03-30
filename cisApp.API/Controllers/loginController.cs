using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.API.Models;
using cisApp.Function;
using cisApp.library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cisApp.API.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // POST api/login
        /// <summary>
        /// เข้าสู่ระบบ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public object Post([FromBody] LoginModels value)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var users = GetUser.Get.GetUserLogin(new LoginModel() { username = value.email, password = value.password, userType = -1 });
                    if (users.Count > 0)
                    {
                        return Ok(resultJson.success(null, null, new LoginResult { uSID = users.FirstOrDefault().UserId.Value, Fname = users.FirstOrDefault().Fname, Lname = users.FirstOrDefault().Lname, isDesigner = (users.FirstOrDefault().UserType == 1) ? false : true }));
                    }
                    else
                    {
                        return Unauthorized(resultJson.errors("ไม่พบข้อมูล", "ไม่พบข้อมูล", null));
                    }
                }
                catch (Exception ex)
                {
                    return Unauthorized(resultJson.errors("ไม่พบข้อมูล", "Internal server error.", ex));
                }
            }
            return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid request.", null));
        }

        [HttpPost("PostClientId")]
        public object PostClientId([FromBody] PostClientIdModel value)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var clientId = GetUserClientId.Manage.Add(value.UserId, value.ClientId);

                    return Ok(resultJson.success(null, null, clientId ));

                }
                catch (Exception ex)
                {
                    return Unauthorized(resultJson.errors("ไม่พบข้อมูล", "Internal server error.", ex));
                }
            }
            return BadRequest(resultJson.errors("parameter ไม่ถูกต้อง", "Invalid request.", null));
        }

        //[HttpGet]
        //public object Get() 
        //{
        //    MobileNotfication mobileNotfication = new MobileNotfication();
        //    mobileNotfication.Fordesigner(MobileNotfication.ModeDesigner.favorite, "cBH85wFyTwe-eewKlMuUhv:APA91bFG1nNvhI3my-Y9GC_6T8Vng5HtrSN8MTIgW36iY0-SSHGg6Si_LfJKNQ5cQR50de_-ldGLbGEVGWiAnUTDJVMz9sJX8yKNWu-zkU-Zl0zmE1VHTdFfjlod2HKh8PPRzmy3wQ_h");
        //    return Ok();


        //}
    
    
    }
}