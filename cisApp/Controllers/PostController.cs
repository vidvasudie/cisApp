using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using cisApp.Function;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using cisApp.library;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace cisApp.Controllers
{
    public class PostController : BaseController
    {
        private readonly IWebHostEnvironment _HostingEnvironment;

        readonly static IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json")
                      .Build();

        public PostController(IWebHostEnvironment environment)
        {
            _HostingEnvironment = environment;
        }
        public IActionResult Index(Guid id)
        {
            string mobileLink = config.GetSection("WebConfig:MobileLink").Value;
            string mobileLinkAndroid = config.GetSection("WebConfig:MobileLinkAndroid").Value;

            ViewData["MobileLink"] = mobileLink + "PhotoGrid/" + id.ToString();
            ViewData["MobileLinkAndroid"] = mobileLinkAndroid + "PhotoGrid/" + id.ToString();
            return View();
        }
    }
}