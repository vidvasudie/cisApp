using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using cisApp.Designer.Models;

namespace cisApp.Designer.Controllers
{
    [CustomActionExecute]
    public class JobListController : BaseController
    {
        private readonly ILogger<JobListController> _logger;

        public JobListController(ILogger<JobListController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
         
    }
}
