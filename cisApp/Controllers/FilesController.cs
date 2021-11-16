using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.Controllers
{
    public class FilesController : Controller
    {
        public IActionResult Index(string Mode)
        {
            return View("Index", Mode);
        }
    }
}
