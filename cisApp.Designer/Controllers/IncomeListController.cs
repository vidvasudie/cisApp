using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace cisApp.Designer.Controllers
{
    [CustomActionExecute]
    public class IncomeListController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}