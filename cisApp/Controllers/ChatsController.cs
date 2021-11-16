using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.Controllers
{
    public class ChatsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Private()
        {
            return View();
        }
        public IActionResult Group()
        {
            return View();
        }
    }
}
