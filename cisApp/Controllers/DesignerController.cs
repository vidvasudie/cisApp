using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.Controllers
{
    public class DesignerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
