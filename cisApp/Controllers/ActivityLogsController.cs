using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.Controllers
{
    public class ActivityLogsController : BaseController
    {
        public IActionResult Index()
        {
            if (_UserId() != null && _UserId() != Guid.Empty)
            {
                LogActivityEvent(Common.LogCommon.LogMode.LOG);
            }
            return View();
        }
    }
}
