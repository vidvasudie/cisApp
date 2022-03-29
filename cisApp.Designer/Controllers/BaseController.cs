using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;
using Microsoft.AspNetCore.Http;
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

        public string _UserName()
        {
            var UserName = HttpContext.Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value;
            return UserName;
        }

        public string _FullName()
        {
            var FullName = HttpContext.Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "FullName")?.Value;
            return FullName;
        }

        public class CustomActionExecuteAttribute : ActionFilterAttribute
        { 
            public static string _Fullname="";
            public static Guid? _UserId = null;
            public override void OnActionExecuting(ActionExecutingContext context)
            {
                var userId = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
                if (String.IsNullOrEmpty(userId) && context.HttpContext.Request.Path == "")
                {
                    context.Result = new RedirectToActionResult("Logout", "Login", null);
                }
                else
                {
                    _UserId = Guid.Parse(userId);
                }
                _Fullname = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "FullName")?.Value;

                base.OnActionExecuting(context);
            }

            public override void OnActionExecuted(ActionExecutedContext context)
            {
                base.OnActionExecuted(context);
            }
        }



        public ActionResult UploadFile(IFormFile upload_file)
        {
            return Ok(new { ok = true });
        }
        [HttpPost]
        public PartialViewResult UploadPreview(FileAttachModel file)
        {
            return PartialView("~/Views/Shared/Common/_ImageItem.cshtml", file);
        }
        [HttpPost]
        public PartialViewResult PreviewImage(UploadFilesModel model)
        {
            return PartialView("~/Views/Shared/Album/_ImageItems.cshtml", model.files);
        }

    }
}