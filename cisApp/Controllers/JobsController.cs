using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.Controllers
{
    public class JobsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail(string Mode)
        {
            return View("Detail", Mode);
        }
        public IActionResult Payment()
        {
            return View();
        }

        public IActionResult CreatePayment()
        {
            return View();
        }
        public IActionResult Submitwork()
        {
            return View();
        }
    }


}
