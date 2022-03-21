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
    public class HistoryListController : BaseController
    {
        private readonly ILogger<HistoryListController> _logger;

        public HistoryListController(ILogger<HistoryListController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
         
    }
}
