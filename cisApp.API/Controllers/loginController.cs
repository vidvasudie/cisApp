﻿using cisApp.API.Models;
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
    public class loginController : ControllerBase
    {
        //// GET: api/<loginController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<loginController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<loginController>
        /// <summary>
        /// เข้าสู่ระบบ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public object Post([FromBody] LoginModels value)
        {
             
            var users = GetUser.Get.GetUserLogin(new LoginModel() { username = value.email, password = value.password, userType = null });
            if (users.Count > 0)
            { 
                return Ok(resultJson.success(null,null, new LoginResult { uSID = users.FirstOrDefault().UserId.Value , Fname = users.FirstOrDefault().Fname , Lname = users.FirstOrDefault().Lname, isDesigner = (users.FirstOrDefault().UserType == 1)?false:true }));
            }
            else
            {
                return Unauthorized(resultJson.errors("ไม่พบข้อมูล", "ไม่พบข้อมูล",null)); 
            }  
        }






        //// PUT api/<loginController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<loginController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
