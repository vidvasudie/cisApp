using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using cisApp.Common;
using cisApp.Function;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace cisApp.API.Controllers
{ 
    [ApiController]
    public class BaseController : ControllerBase
    {
        public IConfigurationRoot _config = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .Build();

        public async void LogActivityEvent(LogCommon.LogMode log, Guid userId, string msg = "", string exception = "")
        {
            var user = GetUser.Get.GetById(userId);
            string name = user != null && !String.IsNullOrEmpty(user.Fname) && !String.IsNullOrEmpty(user.Lname) ? user.Fname + " " + user.Lname : "ไม่ระบุ";
            await GetLogActivity.Manage.AddAsync(Request, userId, name, log, msg, exception);
        }

        public Guid _UserId()
        {
            try
            {
                if (HttpContext.Request.Headers.ContainsKey("userId"))
                {
                    HttpContext.Request.Headers.TryGetValue("userId", out var gid);
                    return Guid.Parse(gid);
                }
            }
            catch (Exception ex)
            {

            }
            return Guid.Empty;
        }



    }
}