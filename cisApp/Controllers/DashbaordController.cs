using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisApp.Common;
using Microsoft.AspNetCore.Mvc;

namespace cisApp.Controllers
{
    public class DashbaordController : BaseController
    {
        public IActionResult Index()
        {
            LogActivityEvent(LogCommon.LogMode.DASHBOARD);
            return View();
        }
    }
}