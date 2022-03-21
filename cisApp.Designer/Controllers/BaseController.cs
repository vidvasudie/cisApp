using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace cisApp.Designer.Controllers
{
    public class BaseController : Controller
    {
        public IConfigurationRoot _config = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .Build();
        public Guid? _UserId()
        {
            var userId = HttpContext.Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
            if (!String.IsNullOrEmpty(userId))
            {
                return Guid.Parse(userId);
            }
            else
            {
                return null;
            }
        }

        public class CustomActionExecuteAttribute : ActionFilterAttribute
        { 
            public override void OnActionExecuting(ActionExecutingContext context)
            { 


                base.OnActionExecuting(context);
            }

            public override void OnActionExecuted(ActionExecutedContext context)
            {
                base.OnActionExecuted(context);
            }
        }

    }
}