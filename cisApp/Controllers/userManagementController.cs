using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Function;

namespace cisApp.Controllers
{
    public class userManagementController : Controller
    {
        public IActionResult Index()
        {
            //var users = GetUser.Get.GetAll();
            return View();
        }
        public IActionResult manage(string Mode)
        {
            return View("manage", Mode);
        }
    }


}
